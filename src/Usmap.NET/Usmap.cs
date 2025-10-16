using System.Buffers;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;

using GenericReader;

using Microsoft.Win32.SafeHandles;

using ZstdSharp;

namespace UsmapDotNet;

/// <summary/>
public sealed class Usmap
{
    /// <summary/>
    public const ushort Magic = 0x30C4;

    /// <summary/>
    public IReadOnlyList<string> Names { get; }
    /// <summary/>
    public IReadOnlyList<UsmapEnum> Enums { get; }
    /// <summary/>
    public IReadOnlyList<UsmapSchema> Schemas { get; }

    internal Usmap(IReadOnlyList<string> names, IReadOnlyList<UsmapEnum> enums, IReadOnlyList<UsmapSchema> schemas)
    {
        Names = names;
        Enums = enums;
        Schemas = schemas;
    }

    /// <inheritdoc cref="Parse{TReader}(ref TReader,UsmapOptions?,bool)" />
    /// <param name="filePath">File path to parse from</param>
    /// <param name="options">Options/Configuration to parse</param>
    public static Usmap Parse(string filePath, UsmapOptions? options = null)
    {
        var reader = new GenericFileReader(filePath);
        return Parse(ref reader, options);
    }

    /// <inheritdoc cref="Parse{TReader}(ref TReader,UsmapOptions?,bool)" />
    /// <param name="fileHandle">File handle to parse from</param>
    /// <param name="options">Options/Configuration to parse</param>
    /// <param name="leaveOpen">Whether or not to leave the <paramref name="fileHandle"/> open</param>
    public static Usmap Parse(SafeFileHandle fileHandle, UsmapOptions? options = null, bool leaveOpen = false)
    {
        var reader = new GenericFileReader(fileHandle);
        return Parse(ref reader, options, leaveOpen);
    }

    /// <inheritdoc cref="Parse{TReader}(ref TReader,UsmapOptions?,bool)" />
    /// <param name="fileStream"><see cref="Stream"/> to parse from</param>
    /// <param name="options">Options/Configuration to parse</param>
    /// <param name="leaveOpen">Whether or not to leave the <paramref name="fileStream"/> open</param>
    public static Usmap Parse(Stream fileStream, UsmapOptions? options = null, bool leaveOpen = false)
    {
        var reader = new GenericStreamReader(fileStream);
        return Parse(ref reader, options, leaveOpen);
    }

    /// <inheritdoc cref="Parse{TReader}(ref TReader,UsmapOptions?,bool)" />
    /// <param name="fileBuffer">File buffer to parse from</param>
    /// <param name="options">Options/Configuration to parse</param>
    public static Usmap Parse(byte[] fileBuffer, UsmapOptions? options = null)
    {
        var reader = new UsmapReader(fileBuffer);
        return Parse(ref reader, options);
    }

    /// <inheritdoc cref="Parse{TReader}(ref TReader,UsmapOptions?,bool)" />
    /// <param name="fileSpan">File spam to parse from</param>
    /// <param name="options">Options/Configuration to parse</param>
    public static Usmap Parse(UsmapRoData fileSpan, UsmapOptions? options = null)
    {
        var reader = new UsmapReader(fileSpan);
        return Parse(ref reader, options);
    }

    /// <summary>
    /// Parses an usmap
    /// </summary>
    /// <param name="usmapReader">The reader used to parse</param>
    /// <param name="options">Options/Configuration to parse</param>
    /// <param name="leaveOpen">Whether or not to leave the <paramref name="usmapReader"/> open</param>
    /// <exception cref="FileLoadException">Error while parsing</exception>
    /// <exception cref="InvalidOperationException">Data is compressed and oodle instance was <see langword="null"/></exception>
    public static Usmap Parse<TReader>(ref TReader usmapReader, UsmapOptions? options, bool leaveOpen = false)
        where TReader : IGenericReader
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        byte[]? compressionBuffer = null;

        try
        {
            ushort magic = usmapReader.Read<ushort>();
            if (magic != Magic)
                throw new FileLoadException($"Invalid .usmap magic constant: 0x{magic:X4}, expected: 0x{Magic:X4}");
            EUsmapVersion version = usmapReader.Read<EUsmapVersion>();
            if (version > EUsmapVersion.Latest)
                throw new FileLoadException($"Invalid or unsupported .usmap version: {(int)version}");

            bool bHasVersioning = version >= EUsmapVersion.PackageVersioning && usmapReader.ReadBoolean();
            if (bHasVersioning)
            {
                //usmapReader.Read<FPackageFileVersion>();
                usmapReader.Position += sizeof(int) * 2;

                //usmapReader.Read<FCustomVersionContainer>();
                int versionsLength = usmapReader.Read<int>();
                usmapReader.Position += versionsLength * (16 /* FGuid */ + sizeof(int));
            }

            EUsmapCompressionMethod compressionMethod = usmapReader.Read<EUsmapCompressionMethod>();
            int compressedSize = usmapReader.Read<int>();
            int uncompressedSize = usmapReader.Read<int>();

            if (compressionMethod > EUsmapCompressionMethod.Max)
                throw new FileLoadException($"Invalid or unsupported .usmap compression: {(int)compressionMethod}");
            if (usmapReader.Length - usmapReader.Position < compressedSize)
                throw new FileLoadException("There is not enough data in the .usmap file");

            options ??= new UsmapOptions();
            bool longFName = version >= EUsmapVersion.LongFName;
            bool largeEnums = version >= EUsmapVersion.LargeEnums;
            bool explicitEnumValues = version >= EUsmapVersion.ExplicitEnumValues;

            if (compressionMethod == EUsmapCompressionMethod.None)
            {
                if (compressedSize != uncompressedSize)
                    throw new FileLoadException("No .usmap compression: Compression size must be equal to decompression size");

                return ParseInternal(ref usmapReader, options, longFName, largeEnums, explicitEnumValues);
            }
            else
            {
                compressionBuffer = ArrayPool<byte>.Shared.Rent(compressedSize + uncompressedSize);
                var compressedSpan = new Span<byte>(compressionBuffer, 0, compressedSize);
                usmapReader.Read(compressedSpan);
                var uncompressedData = new UsmapData(compressionBuffer, compressedSize, uncompressedSize);

                switch (compressionMethod)
                {
                    case EUsmapCompressionMethod.Oodle:
                    {
                        if (options.Oodle is null)
                            throw new InvalidOperationException(".usmap data is compressed and oodle instance was null");

                        int result = (int)options.Oodle.Decompress(compressedSpan, uncompressedData
#if !NET9_0_OR_GREATER
                            .Span
#endif
                        );

                        if (result != uncompressedSize)
                            throw new FileLoadException($"Invalid oodle .usmap data decompress result: {result} / {uncompressedSize}");

                        break;
                    }
                    case EUsmapCompressionMethod.Brotli:
                    {
                        if (!BrotliDecoder.TryDecompress(compressedSpan, uncompressedData
#if !NET9_0_OR_GREATER
                            .Span
#endif
                            , out int bytesWritten))
                        {
                            throw new FileLoadException($"Failed to decompress brotli .usmap data: {bytesWritten} / {uncompressedSize}");
                        }

                        break;
                    }
                    case EUsmapCompressionMethod.ZStandard:
                    {
                        using var decompressor = new Decompressor();
                        if (!decompressor.TryUnwrap(compressedSpan, uncompressedData
#if !NET9_0_OR_GREATER
                            .Span
#endif
                            , out int bytesWritten))
                        {
                            throw new FileLoadException($"Failed to decompress zstd .usmap data: {bytesWritten} / {uncompressedSize}");
                        }

                        break;
                    }
                    default:
                        throw new UnreachableException();
                }

                var reader = new UsmapReader(uncompressedData);
                return ParseInternal(ref reader, options, longFName, largeEnums, explicitEnumValues);
            }
        }
        finally
        {
            if (!leaveOpen)
                usmapReader.Dispose();
            if (compressionBuffer is not null)
                ArrayPool<byte>.Shared.Return(compressionBuffer);
        }
    }

    private static Usmap ParseInternal<TReader>(ref TReader reader, UsmapOptions options, bool longFName, bool largeEnums, bool explicitEnumValues)
        where TReader : IGenericReader
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        string[] names;
        UsmapEnum[] enums;
        UsmapSchema[] schemas;

        {
            uint size = reader.Read<uint>();
            names = new string[size];

            for (int i = 0; i < size; ++i)
            {
                int nameLength = longFName
                    ? reader.Read<ushort>()
                    : reader.Read<byte>();
                string name = reader.ReadString(nameLength, Encoding.UTF8);
                names[i] = name;
            }
        }

        {
            uint size = reader.Read<uint>();
            enums = new UsmapEnum[size];

            for (int i = 0; i < size; ++i)
            {
                uint idx = reader.Read<uint>();
                string enumName = names[idx];
                int enumMembersSize = largeEnums
                    ? reader.Read<ushort>()
                    : reader.Read<byte>();
                var enumMembers = new UsmapEnumMember[enumMembersSize];

                for (int j = 0; j < enumMembersSize; ++j)
                {
                    long memberValue = explicitEnumValues
                        ? reader.Read<long>()
                        : j;
                    uint memberNameIdx = reader.Read<uint>();
                    string memberName = names[memberNameIdx];
                    enumMembers[j] = new UsmapEnumMember(memberName, memberValue);
                }

                enums[i] = new UsmapEnum(enumName, enumMembers);
            }
        }

        {
            uint size = reader.Read<uint>();
            schemas = new UsmapSchema[size];

            for (int i = 0; i < size; ++i)
            {
                uint idx = reader.Read<uint>();
                string schemaName = names[idx];
                uint superIdx = reader.Read<uint>();
                string? schemaSuperType = superIdx == uint.MaxValue
                    ? null
                    : names[superIdx];
                ushort propCount = reader.Read<ushort>();

                ushort serializablePropCount = reader.Read<ushort>();
                UsmapProperty[] props;

                if (serializablePropCount == 0)
                {
                    props = [];
                }
                else
                {
                    props = new UsmapProperty[serializablePropCount];

                    for (int j = 0; j < serializablePropCount; ++j)
                    {
                        ushort schemaIdx = reader.Read<ushort>();
                        byte arraySize = reader.Read<byte>();
                        uint nameIdx = reader.Read<uint>();
                        var data = UsmapPropertyData.Deserialize(ref reader, names);
                        string name = names[nameIdx];
                        props[j] = new UsmapProperty(name, schemaIdx, arraySize, data);
                    }
                }

                schemas[i] = new UsmapSchema(schemaName, schemaSuperType, propCount, props);
            }
        }

        return new Usmap(options.SaveNames ? names : [], enums, schemas);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Schemas: {Schemas.Count} | Enums: {Enums.Count} | Names: {Names.Count}";
    }
}
