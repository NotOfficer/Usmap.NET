using System.Diagnostics;
using System.IO.Compression;
using System.Net;

using OodleDotNet;

namespace UsmapDotNet.Tests;

public static class Constants
{
    public const int ExpectedSchemas = 28697;
    public const int ExpectedEnums = 4391;
    public const int ExpectedNames = 141495;

    public const int ExpectedSchemasV3 = 29520;
    public const int ExpectedEnumsV3 = 4484;
    public const int ExpectedNamesV3 = 144915;

    public const int ExpectedSchemasV4 = 47836;
    public const int ExpectedEnumsV4 = 6478;
    public const int ExpectedNamesV4 = 192297;

    public static async Task<string> DownloadOodleAsync()
    {
        if (!OperatingSystem.IsWindows() && !OperatingSystem.IsLinux())
        {
            throw new PlatformNotSupportedException("this test is not supported on the current platform");
        }

        const string baseUrl = "https://github.com/WorkingRobot/OodleUE/releases/download/2025-07-31-1001/"; // 2.9.14
        string url;
        string entryName;

        if (OperatingSystem.IsWindows())
        {
            url = baseUrl + "msvc.zip";
            entryName = "bin/Release/oodle-data-shared.dll";
        }
        else if (OperatingSystem.IsLinux())
        {
            url = baseUrl + "gcc.zip";
            entryName = "lib/Release/liboodle-data-shared.so";
        }
        else
        {
            throw new UnreachableException();
        }

        using var client = new HttpClient(new SocketsHttpHandler
        {
            UseProxy = false,
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.All
        });
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var zip = new ZipArchive(responseStream, ZipArchiveMode.Read);
        var entry = zip.GetEntry(entryName);
        ArgumentNullException.ThrowIfNull(entry, "oodle entry in zip not found");
        await using var entryStream = entry.Open();
        string filePath = Path.GetTempFileName();
        await using var fs = File.Create(filePath);
        await entryStream.CopyToAsync(fs);
        return filePath;
    }
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

public class OodleCompressedTests : IUsmapTest, IAsyncLifetime
{
    private const string FilePath = "files/oo1.usmap";

    private string _oodleFilePath = null!;
    private Oodle _oodle = null!;
    private UsmapOptions _options = null!;

    public async Task InitializeAsync()
    {
        _oodleFilePath = await Constants.DownloadOodleAsync();
        _oodle = new Oodle(_oodleFilePath);
        _options = new UsmapOptions { Oodle = _oodle };
    }

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath, _options);
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath), _options);
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath), _options);
        Assert.Equal(Constants.ExpectedSchemas, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnums, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNames, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    public Task DisposeAsync()
    {
        _oodle.Dispose();
        File.Delete(_oodleFilePath);
        return Task.CompletedTask;
    }
}

public class OodleCompressedV3Tests : IUsmapTest, IAsyncLifetime
{
    private const string FilePath = "files/oo2.usmap";

    private string _oodleFilePath = null!;
    private Oodle _oodle = null!;
    private UsmapOptions _options = null!;

    public async Task InitializeAsync()
    {
        _oodleFilePath = await Constants.DownloadOodleAsync();
        _oodle = new Oodle(_oodleFilePath);
        _options = new UsmapOptions { Oodle = _oodle };
    }

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath, _options);
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath), _options);
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath), _options);
        Assert.Equal(Constants.ExpectedSchemasV3, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV3, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV3, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    public Task DisposeAsync()
    {
        _oodle.Dispose();
        File.Delete(_oodleFilePath);
        return Task.CompletedTask;
    }
}

public class ZstandardCompressedV4Tests : IUsmapTest
{
    private const string FilePath = "files/zs1.usmap";

    [Fact]
    public void ParseFromFile()
    {
        var usmap = Usmap.Parse(FilePath);
        Assert.Equal(Constants.ExpectedSchemasV4, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV4, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV4, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromStream()
    {
        var usmap = Usmap.Parse(File.OpenRead(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV4, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV4, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV4, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }

    [Fact]
    public void ParseFromBuffer()
    {
        var usmap = Usmap.Parse(File.ReadAllBytes(FilePath));
        Assert.Equal(Constants.ExpectedSchemasV4, usmap.Schemas.Count);
        Assert.Equal(Constants.ExpectedEnumsV4, usmap.Enums.Count);
        Assert.Equal(Constants.ExpectedNamesV4, usmap.Names.Count);
        Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
    }
}
