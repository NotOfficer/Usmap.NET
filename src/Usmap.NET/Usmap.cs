using System.Buffers;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;

using GenericReader;

namespace UsmapDotNet;

public class Usmap
{
	public string[] Names { get; }
	public UsmapEnum[] Enums { get; }
	public UsmapSchema[] Schemas { get; }

	public Usmap(string filePath, UsmapOptions? options = null) : this(new GenericFileReader(filePath), options) { }
	public Usmap(Stream fileStream, UsmapOptions? options = null) : this(new GenericStreamReader(fileStream), options) { }
	public Usmap(byte[] fileBuffer, UsmapOptions? options = null) : this(new GenericBufferReader(fileBuffer), options) { }

	public Usmap(IGenericReader usmapReader, UsmapOptions? options, bool disposeReader = true)
	{
		IGenericReader? reader = null;
		byte[]? compressedBuffer = null;
		byte[]? uncompressedBuffer = null;

		try
		{
			var header = usmapReader.Read<UsmapHeader>();
			if (header.Magic != UsmapHeader.MagicValue)
				throw new FileLoadException("Invalid .usmap magic constant");
			if (header.Version > EUsmapVersion.Initial)
				throw new FileLoadException($"Invalid or unsupported .usmap version: {(int)header.Version}");
			if (header.CompressionMethod > EUsmapCompressionMethod.Max)
				throw new FileLoadException(
					$"Invalid or unsupported .usmap compression: {(int)header.CompressionMethod}");
			if (usmapReader.Length - usmapReader.Position < header.CompressedSize)
				throw new FileLoadException("There is not enough data in the .usmap file");

			options ??= new UsmapOptions();

			switch (header.CompressionMethod)
			{
				case EUsmapCompressionMethod.None:
				{
					if (header.CompressedSize != header.UncompressedSize)
						throw new FileLoadException(
							"No .usmap compression: Compression size must be equal to decompression size");

					reader = usmapReader;
					break;
				}
				case EUsmapCompressionMethod.Oodle:
				{
					if (options.Oodle is null)
						throw new FileLoadException("Undefined oodle instance");

					compressedBuffer = ArrayPool<byte>.Shared.Rent((int)header.CompressedSize);
					var compressedSpan = new Span<byte>(compressedBuffer, 0, (int)header.CompressedSize);
					usmapReader.Read(compressedSpan);

					uncompressedBuffer = ArrayPool<byte>.Shared.Rent((int)header.UncompressedSize);
					var uncompressedSpan = new Span<byte>(uncompressedBuffer, 0, (int)header.UncompressedSize);

					var result = (uint)options.Oodle.Decompress(compressedSpan, uncompressedSpan);
					if (result != header.UncompressedSize)
						throw new FileLoadException(
							$"Invalid oodle .usmap decompress result: {result} / {header.UncompressedSize}");

					reader = new GenericBufferReader(uncompressedBuffer, 0, (int)header.UncompressedSize);
					break;
				}
				case EUsmapCompressionMethod.Brotli:
				{
					compressedBuffer = ArrayPool<byte>.Shared.Rent((int)header.CompressedSize);
					var compressedSpan = new Span<byte>(compressedBuffer, 0, (int)header.CompressedSize);
					usmapReader.Read(compressedSpan);

					uncompressedBuffer = ArrayPool<byte>.Shared.Rent((int)header.UncompressedSize);
					var uncompressedSpan = new Span<byte>(uncompressedBuffer, 0, (int)header.UncompressedSize);

					using var decoder = new BrotliDecoder();
					var result = decoder.Decompress(compressedSpan, uncompressedSpan, out var bytesConsumed,
						out var bytesWritten);
					if (result != OperationStatus.Done)
						throw new FileLoadException(
							$"Invalid brotli .usmap decompress result: {result} | {bytesWritten} / {header.UncompressedSize} | {bytesConsumed} / {header.CompressedSize}");

					reader = new GenericBufferReader(uncompressedBuffer, 0, (int)header.UncompressedSize);
					break;
				}
				default:
					throw new UnreachableException();
			}

			string[] names;

			{
				var size = reader.Read<uint>();
				names = new string[size];

				for (var i = 0; i < size; ++i)
				{
					var nameSize = reader.Read<byte>();
					var name = reader.ReadString(nameSize, Encoding.UTF8);
					names[i] = name;
				}
			}

			{
				var size = reader.Read<uint>();
				Enums = new UsmapEnum[size];

				for (var i = 0; i < size; ++i)
				{
					var idx = reader.Read<uint>();
					var enumNamesSize = reader.Read<byte>();
					var enumNames = new string[enumNamesSize];

					for (var j = 0; j < enumNamesSize; ++j)
					{
						var nameIdx = reader.Read<uint>();
						enumNames[j] = names[nameIdx];
					}

					Enums[i] = new UsmapEnum(names[idx], enumNames);
				}
			}

			{
				var size = reader.Read<uint>();
				Schemas = new UsmapSchema[size];

				for (var i = 0; i < size; ++i)
				{
					var idx = reader.Read<uint>();
					var superIdx = reader.Read<uint>();
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

					Schemas[i] = new UsmapSchema(names[idx], superIdx == uint.MaxValue ? null : names[superIdx],
						propCount, props);
				}
			}

			Names = options.SaveNames ? names : [];
		}
		finally
		{
			if (disposeReader && reader is not null)
				reader.Dispose();
			if (compressedBuffer is not null)
				ArrayPool<byte>.Shared.Return(compressedBuffer);
			if (uncompressedBuffer is not null)
				ArrayPool<byte>.Shared.Return(uncompressedBuffer);
		}
	}

	public override string ToString()
	{
		return $"Schemas: {Schemas.Length} | Enums: {Enums.Length} | Names: {Names?.Length ?? 0}";
	}
}
