using OffiUtils;

using ZstdSharp;

namespace UsmapDotNet.Tests;

public static class Constants
{
    public static bool TryDecompressZstd(
        ReadOnlySpan<byte> source,
        Span<byte> destination,
        out int bytesWritten)
    {
        using var decompressor = new Decompressor();
        return decompressor.TryUnwrap(source, destination, out bytesWritten);
    }

    public static UsmapOptions UsmapOptions { get; } = new()
    {
        Decompressor = DecompressorBuilder.DefaultWithOodlePort
            .Add(CompressionAlgorithm.Zstd, TryDecompressZstd)
            .Build()
    };

    public const int ExpectedSchemas = 28697;
    public const int ExpectedEnums = 4391;
    public const int ExpectedNames = 141495;

    public const int ExpectedSchemasV3 = 29520;
    public const int ExpectedEnumsV3 = 4484;
    public const int ExpectedNamesV3 = 144915;

    public const int ExpectedSchemasV4 = 47836;
    public const int ExpectedEnumsV4 = 6478;
    public const int ExpectedNamesV4 = 192297;
}

public interface IUsmapTest
{
    void ParseFromFile();
    void ParseFromStream();
    void ParseFromBuffer();
}

public class UncompressedTests : IUsmapTest
{
    private const string FilePath = "files/xx1.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath);
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath));
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath));
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}

public class UncompressedV3Tests : IUsmapTest
{
    private const string FilePath = "files/xx2.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath);
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}

public class BrotliCompressedTests : IUsmapTest
{
    private const string FilePath = "files/br1.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath);
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath));
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath));
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}

public class BrotliCompressedV3Tests : IUsmapTest
{
    private const string FilePath = "files/br2.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath);
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}

public class OodleCompressedTests : IUsmapTest
{
    private const string FilePath = "files/oo1.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath);
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath));
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath));
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}

public class OodleCompressedV3Tests : IUsmapTest
{
    private const string FilePath = "files/oo2.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath);
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}

public class ZstandardCompressedV4Tests : IUsmapTest
{
    private const string FilePath = "files/zs1.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath, Constants.UsmapOptions);
        Assert.Equal(Constants.ExpectedSchemasV4, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV4, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV4, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath), Constants.UsmapOptions);
        Assert.Equal(Constants.ExpectedSchemasV4, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV4, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV4, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath), Constants.UsmapOptions);
        Assert.Equal(Constants.ExpectedSchemasV4, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV4, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV4, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}
