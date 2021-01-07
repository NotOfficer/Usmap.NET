using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Text;

using UsmapNET.Enums;

namespace UsmapNET.Classes
{
	public class Usmap
	{
		public const ushort MAGIC = 0x30C4;

		public string[] Names { get; }
		public UsmapEnum[] Enums { get; }
		public UsmapSchema[] Schemas { get; }

		public Usmap(string filePath, UsmapOptions options = null) : this(new GenericStreamReader(filePath), options) { }
		public Usmap(Stream fileStream, UsmapOptions options = null) : this(new GenericStreamReader(fileStream), options) { }
		public Usmap(byte[] fileBuffer, UsmapOptions options = null) : this(new GenericBufferReader(fileBuffer), options) { }

		internal Usmap(GenericReader fileReader, UsmapOptions options)
		{
			var magic = fileReader.Read<ushort>();

			if (magic != MAGIC)
			{
				throw new FileLoadException("Invalid .usmap magic constant");
			}

			var version = fileReader.Read<EUsmapVersion>();

			if (version != EUsmapVersion.LATEST)
			{
				throw new FileLoadException($"Invalid .usmap version: {(int)version}");
			}

			var compMethod = fileReader.Read<EUsmapCompressionMethod>();
			var compSize = fileReader.Read<uint>();
			var decompSize = fileReader.Read<uint>();

			if (fileReader.Size - fileReader.Position < compSize)
			{
				throw new FileLoadException("There is not enough data in the .usmap file");
			}

			GenericReader reader;

			switch (compMethod)
			{
				case EUsmapCompressionMethod.None:
				{
					if (compSize != decompSize)
					{
						throw new FileLoadException("No .usmap compression: Compression size must be equal to decompression size");
					}

					reader = fileReader;
					break;
				}
				case EUsmapCompressionMethod.Oodle:
				{
					if (options?.OodlePath == null)
					{
						throw new FileLoadException("Undefined oodle library path");
					}

					if (!File.Exists(options.OodlePath))
					{
						throw new FileLoadException($"Could not find oodle library at \"{options.OodlePath}\"");
					}

					var compData = fileReader.ReadBytes((int)compSize);
					fileReader.Dispose();
					var data = new byte[decompSize];
					using var decompressor = new OodleDecompressor(options.OodlePath);
					var result = decompressor.Decompress(compData, compSize, data, decompSize, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3);

					if (result != decompSize)
					{
						throw new FileLoadException($"Invalid oodle .usmap decompress result: {result} / {decompSize}");
					}

					reader = new GenericBufferReader(data);
					break;
				}
				case EUsmapCompressionMethod.Brotli:
				{
					var compData = fileReader.ReadBytes((int)compSize);
					fileReader.Dispose();
					var data = new byte[decompSize];
					using var decoder = new BrotliDecoder();
					var result = decoder.Decompress(compData, data, out var bytesConsumed, out var bytesWritten);

					if (result != OperationStatus.Done)
					{
						throw new FileLoadException($"Invalid brotli .usmap decompress result: {result} | {bytesWritten} / {decompSize} | {bytesConsumed} / {compSize}");
					}

					reader = new GenericBufferReader(data);
					break;
				}
				default:
					throw new FileLoadException($"Unknown .usmap compression method: {(int)compMethod}");
			}

			{
				var size = reader.Read<uint>();
				Names = new string[size];

				for (var i = 0; i < size; ++i)
				{
					var nameSize = reader.ReadByte();
					var name = reader.ReadString(nameSize, Encoding.UTF8);
					Names[i] = name;
				}
			}

			{
				var size = reader.Read<uint>();
				Enums = new UsmapEnum[size];

				for (var i = 0; i < size; ++i)
				{
					var idx = reader.Read<uint>();
					var enumNamesSize = reader.ReadByte();
					var enumNames = new string[enumNamesSize];

					for (var j = 0; j < enumNamesSize; ++j)
					{
						var nameIdx = reader.Read<uint>();
						enumNames[j] = Names[nameIdx];
					}

					Enums[i] = new UsmapEnum(Names[idx], enumNames);
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
					var props = new UsmapProperty[serializablePropCount];

					for (var j = 0; j < serializablePropCount; ++j)
					{
						var schemaIdx = reader.Read<ushort>();
						var arraySize = reader.Read<byte>();
						var nameIdx = reader.Read<uint>();
						var data = DeserializePropData(reader);
						props[j] = new UsmapProperty(Names[nameIdx], schemaIdx, arraySize, data);
					}

					Schemas[i] = new UsmapSchema(Names[idx], superIdx == uint.MaxValue ? null : Names[superIdx], propCount, props);
				}
			}

			reader.Dispose();
		}

		private UsmapPropertyData DeserializePropData(GenericReader reader)
		{
			var propType = reader.Read<EUsmapPropertyType>();
			var data = new UsmapPropertyData(propType);

			switch (propType)
			{
				case EUsmapPropertyType.EnumProperty:
				{
					data.InnerType = DeserializePropData(reader);
					var idx = reader.Read<uint>();
					data.EnumName = Names[idx];
					break;
				}
				case EUsmapPropertyType.StructProperty:
				{
					var idx = reader.Read<uint>();
					data.StructType = Names[idx];
					break;
				}
				case EUsmapPropertyType.SetProperty:
				case EUsmapPropertyType.ArrayProperty:
				{
					data.InnerType = DeserializePropData(reader);
					break;
				}
				case EUsmapPropertyType.MapProperty:
				{
					data.InnerType = DeserializePropData(reader);
					data.ValueType = DeserializePropData(reader);
					break;
				}
			}

			return data;
		}

		public override string ToString()
		{
			return $"Schemas: {Schemas.Length} | Enums: {Enums.Length}";
		}
	}
}