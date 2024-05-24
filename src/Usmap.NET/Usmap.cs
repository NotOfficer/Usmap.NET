using System.Buffers;
using System.IO.Compression;
using System.Text;

using GenericReader;

namespace UsmapDotNet;

/// <summary/>
public class Usmap
{
	private const ushort MagicValue = 0x30C4;

	/// <summary/>
	public string[] Names { get; }
	/// <summary/>
	public UsmapEnum[] Enums { get; }
	/// <summary/>
	public UsmapSchema[] Schemas { get; }

	/// <inheritdoc />
	/// <param name="filePath">File path to parse from</param>
	/// <param name="options">Options/Configuration to parse</param>
	public Usmap(string filePath, UsmapOptions? options = null) : this(new GenericFileReader(filePath), options) { }

	/// <inheritdoc />
	/// <param name="fileStream"><see cref="Stream"/> to parse from</param>
	/// <param name="options">Options/Configuration to parse</param>
	/// <param name="leaveOpen">Whether or not to leave the <paramref name="fileStream"/> open</param>
	public Usmap(Stream fileStream, UsmapOptions? options = null, bool leaveOpen = false) : this(new GenericStreamReader(fileStream), options, leaveOpen) { }

	/// <inheritdoc />
	/// <param name="fileBuffer">File buffer to parse from</param>
	/// <param name="options">Options/Configuration to parse</param>
	public Usmap(byte[] fileBuffer, UsmapOptions? options = null) : this(new GenericBufferReader(fileBuffer), options) { }

	/// <inheritdoc />
	/// <param name="fileMemory">File memory to parse from</param>
	/// <param name="options">Options/Configuration to parse</param>
	public Usmap(ReadOnlyMemory<byte> fileMemory, UsmapOptions? options = null) : this(new GenericBufferReader(fileMemory), options) { }

	/// <summary>
	/// Parses an usmap
	/// </summary>
	/// <param name="usmapReader">The reader used to parse</param>
	/// <param name="options">Options/Configuration to parse</param>
	/// <param name="leaveOpen">Whether or not to leave the <paramref name="usmapReader"/> open</param>
	/// <exception cref="FileLoadException">Error while parsing</exception>
	/// <exception cref="InvalidOperationException">Data is compressed and oodle instance was <see langword="null"/></exception>
	public Usmap(IGenericReader usmapReader, UsmapOptions? options, bool leaveOpen = false)
	{
		byte[]? compressionBuffer = null;

		try
		{
			var magic = usmapReader.Read<ushort>();
			if (magic != MagicValue)
				throw new FileLoadException($"Invalid .usmap magic constant: 0x{magic:X4}, expected: 0x{MagicValue:X4}");
			var version = usmapReader.Read<EUsmapVersion>();
			if (version > EUsmapVersion.Latest)
				throw new FileLoadException($"Invalid or unsupported .usmap version: {(int)version}");

			var bHasVersioning = version >= EUsmapVersion.PackageVersioning && usmapReader.ReadBoolean();
			if (bHasVersioning)
			{
				//usmapReader.Read<FPackageFileVersion>();
				usmapReader.Position += sizeof(int) * 2;

				//usmapReader.Read<FCustomVersionContainer>();
				var versionsLength = usmapReader.Read<int>();
				usmapReader.Position += versionsLength * (16 /* FGuid */ + sizeof(int));
			}

			var compressionMethod = usmapReader.Read<EUsmapCompressionMethod>();
			var compressedSize = usmapReader.Read<uint>();
			var uncompressedSize = usmapReader.Read<uint>();

			if (compressionMethod > EUsmapCompressionMethod.Max)
				throw new FileLoadException($"Invalid or unsupported .usmap compression: {(int)compressionMethod}");
			if (usmapReader.Length - usmapReader.Position < compressedSize)
				throw new FileLoadException("There is not enough data in the .usmap file");

			options ??= new UsmapOptions();
			IGenericReader reader;

			if (compressionMethod == EUsmapCompressionMethod.None)
			{
				if (compressedSize != uncompressedSize)
					throw new FileLoadException("No .usmap compression: Compression size must be equal to decompression size");

				reader = usmapReader;
			}
			else
			{
				compressionBuffer = ArrayPool<byte>.Shared.Rent((int)(compressedSize + uncompressedSize));
				var compressedSpan = new Span<byte>(compressionBuffer, 0, (int)compressedSize);
				usmapReader.Read(compressedSpan);
				var uncompressedMemory = new Memory<byte>(compressionBuffer, (int)compressedSize, (int)uncompressedSize);

				switch (compressionMethod)
				{
					case EUsmapCompressionMethod.Oodle:
					{
						if (options.Oodle is null)
							throw new InvalidOperationException("Data is compressed and oodle instance was null");

						var result = (uint)options.Oodle.Decompress(compressedSpan, uncompressedMemory.Span);
						if (result != uncompressedSize)
							throw new FileLoadException($"Invalid oodle .usmap decompress result: {result} / {uncompressedSize}");
						break;
					}
					case EUsmapCompressionMethod.Brotli:
					{
						using var decoder = new BrotliDecoder();
						var result = decoder.Decompress(compressedSpan, uncompressedMemory.Span, out var bytesConsumed, out var bytesWritten);
						if (result != OperationStatus.Done)
							throw new FileLoadException($"Invalid brotli .usmap decompress result: {result} | {bytesWritten} / {uncompressedSize} | {bytesConsumed} / {compressedSize}");
						break;
					}
					case EUsmapCompressionMethod.ZStandard:
					{
						throw new FileLoadException($"Unsupported .usmap compression: {(int)EUsmapCompressionMethod.ZStandard} (Zstandard)");
					}
					default:
						throw new FileLoadException($"Invalid or unsupported .usmap compression: {(int)compressionMethod}");
				}

				reader = new GenericBufferReader(uncompressedMemory);
			}

			string[] names;

			{
				var size = reader.Read<uint>();
				names = new string[size];

				for (var i = 0; i < size; ++i)
				{
					var nameLength = (int)(version >= EUsmapVersion.LongFName
						? reader.Read<ushort>()
						: reader.Read<byte>());
					var name = reader.ReadString(nameLength, Encoding.UTF8);
					names[i] = name;
				}
			}

			{
				var size = reader.Read<uint>();
				Enums = new UsmapEnum[size];

				for (var i = 0; i < size; ++i)
				{
					var idx = reader.Read<uint>();
					var enumName = names[idx];
					var enumNamesSize = (int)(version >= EUsmapVersion.LargeEnums
						? reader.Read<ushort>()
						: reader.Read<byte>());
					var enumNames = new string[enumNamesSize];

					for (var j = 0; j < enumNamesSize; ++j)
					{
						var nameIdx = reader.Read<uint>();
						enumNames[j] = names[nameIdx];
					}

					Enums[i] = new UsmapEnum(enumName, enumNames);
				}
			}

			{
				var size = reader.Read<uint>();
				Schemas = new UsmapSchema[size];

				for (var i = 0; i < size; ++i)
				{
					var idx = reader.Read<uint>();
					var schemaName = names[idx];
					var superIdx = reader.Read<uint>();
					var schemaSuperType = superIdx == uint.MaxValue
						? null
						: names[superIdx];
					var propCount = reader.Read<ushort>();

					var serializablePropCount = reader.Read<ushort>();
					UsmapProperty[] props;

					if (serializablePropCount == 0)
					{
						props = [];
					}
					else
					{
						props = new UsmapProperty[serializablePropCount];

						for (var j = 0; j < serializablePropCount; ++j)
						{
							var schemaIdx = reader.Read<ushort>();
							var arraySize = reader.Read<byte>();
							var nameIdx = reader.Read<uint>();
							var data = UsmapPropertyData.Deserialize(reader, names);
							props[j] = new UsmapProperty(names[nameIdx], schemaIdx, arraySize, data);
						}
					}

					Schemas[i] = new UsmapSchema(schemaName, schemaSuperType, propCount, props);
				}
			}

			Names = options.SaveNames ? names : [];
		}
		finally
		{
			if (!leaveOpen)
				usmapReader.Dispose();
			if (compressionBuffer is not null)
				ArrayPool<byte>.Shared.Return(compressionBuffer);
		}
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"Schemas: {Schemas.Length} | Enums: {Enums.Length} | Names: {Names?.Length ?? 0}";
	}
}
