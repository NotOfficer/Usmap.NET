using System;
using System.IO;

using OodleDotNet;

using Xunit;

namespace UsmapDotNet.Tests;

public class Tests
{
	private static readonly Oodle OodleInstance = new(Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
		@"Libraries\oo2core_9_win64.dll"));

	/*private const int ExpectedSchemas = 13890;
	private const int ExpectedEnums = 2367;
	private const int ExpectedNames = 74908;*/

	private const int ExpectedSchemas = 28697;
	private const int ExpectedEnums = 4391;
	private const int ExpectedNames = 141495;

	private const int ExpectedSchemasV3 = 29520;
	private const int ExpectedEnumsV3 = 4484;
	private const int ExpectedNamesV3 = 144915;

	private readonly string _uncompressedUsmapPath;
	private readonly string _brotliCompressedUsmapPath;
	private readonly string _oodleCompressedUsmapPath;

	private readonly string _uncompressedUsmapV3Path;
	private readonly string _brotliCompressedUsmapV3Path;
	private readonly string _oodleCompressedUsmapV3Path;

	public Tests()
	{
		var currentDir = Directory.GetCurrentDirectory();
		var testFilesDir = Path.Combine(currentDir, "TestFiles");

		_uncompressedUsmapPath = Path.Combine(testFilesDir, "xx1.usmap");
		_brotliCompressedUsmapPath = Path.Combine(testFilesDir, "br1.usmap");
		_oodleCompressedUsmapPath = Path.Combine(testFilesDir, "oo1.usmap");

		_uncompressedUsmapV3Path = Path.Combine(testFilesDir, "xx2.usmap");
		_brotliCompressedUsmapV3Path = Path.Combine(testFilesDir, "br2.usmap");
		_oodleCompressedUsmapV3Path = Path.Combine(testFilesDir, "oo2.usmap");
	}

	[Fact]
	public void ParseUncompressedFromFile()
	{
		var usmap = new Usmap(_uncompressedUsmapPath);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseUncompressedFromStream()
	{
		var usmap = new Usmap(File.OpenRead(_uncompressedUsmapPath));
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseUncompressedFromBuffer()
	{
		var buffer = File.ReadAllBytes(_uncompressedUsmapPath);
		var usmap = new Usmap(buffer);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseBrotliCompressedFromFile()
	{
		var usmap = new Usmap(_brotliCompressedUsmapPath);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseBrotliCompressedFromStream()
	{
		var usmap = new Usmap(File.OpenRead(_brotliCompressedUsmapPath));
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseBrotliCompressedFromBuffer()
	{
		var buffer = File.ReadAllBytes(_brotliCompressedUsmapPath);
		var usmap = new Usmap(buffer);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseOodleCompressedFromFile()
	{
		var options = new UsmapOptions
		{
			Oodle = OodleInstance
		};
		var usmap = new Usmap(_oodleCompressedUsmapPath, options);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseOodleCompressedFromStream()
	{
		var options = new UsmapOptions
		{
			Oodle = OodleInstance
		};
		var usmap = new Usmap(File.OpenRead(_oodleCompressedUsmapPath), options);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseOodleCompressedFromBuffer()
	{
		var buffer = File.ReadAllBytes(_oodleCompressedUsmapPath);
		var options = new UsmapOptions
		{
			Oodle = OodleInstance
		};
		var usmap = new Usmap(buffer, options);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	// v3

	[Fact]
	public void ParseUncompressedV3FromFile()
	{
		var usmap = new Usmap(_uncompressedUsmapV3Path);
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseUncompressedV3FromStream()
	{
		var usmap = new Usmap(File.OpenRead(_uncompressedUsmapV3Path));
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseUncompressedV3FromBuffer()
	{
		var buffer = File.ReadAllBytes(_uncompressedUsmapV3Path);
		var usmap = new Usmap(buffer);
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseBrotliCompressedV3FromFile()
	{
		var usmap = new Usmap(_brotliCompressedUsmapV3Path);
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseBrotliCompressedV3FromStream()
	{
		var usmap = new Usmap(File.OpenRead(_brotliCompressedUsmapV3Path));
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseBrotliCompressedV3FromBuffer()
	{
		var buffer = File.ReadAllBytes(_brotliCompressedUsmapV3Path);
		var usmap = new Usmap(buffer);
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseOodleCompressedV3FromFile()
	{
		var options = new UsmapOptions
		{
			Oodle = OodleInstance
		};
		var usmap = new Usmap(_oodleCompressedUsmapV3Path, options);
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseOodleCompressedV3FromStream()
	{
		var options = new UsmapOptions
		{
			Oodle = OodleInstance
		};
		var usmap = new Usmap(File.OpenRead(_oodleCompressedUsmapV3Path), options);
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}

	[Fact]
	public void ParseOodleCompressedV3FromBuffer()
	{
		var buffer = File.ReadAllBytes(_oodleCompressedUsmapV3Path);
		var options = new UsmapOptions
		{
			Oodle = OodleInstance
		};
		var usmap = new Usmap(buffer, options);
		Assert.Equal(ExpectedSchemasV3, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnumsV3, usmap.Enums.Length);
		Assert.Equal(ExpectedNamesV3, usmap.Names.Length);
		Assert.All(usmap.Names, x => Assert.False(string.IsNullOrEmpty(x)));
	}
}
