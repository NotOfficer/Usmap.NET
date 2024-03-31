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

	private readonly string _uncompressedUsmapPath;
	private readonly string _brotliCompressedUsmapPath;
	private readonly string _oodleCompressedUsmapPath;

	public Tests()
	{
		var currentDir = Directory.GetCurrentDirectory();
		var testFilesDir = Path.Combine(currentDir, "TestFiles");

		_uncompressedUsmapPath = Path.Combine(testFilesDir, "xx1.usmap");
		_brotliCompressedUsmapPath = Path.Combine(testFilesDir, "br1.usmap");
		_oodleCompressedUsmapPath = Path.Combine(testFilesDir, "oo1.usmap");
	}

	[Fact]
	public void ParseUncompressedUsmapFromFile()
	{
		var usmap = new Usmap(_uncompressedUsmapPath);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
	}

	[Fact]
	public void ParseUncompressedUsmapFromStream()
	{
		var usmap = new Usmap(File.OpenRead(_uncompressedUsmapPath));
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
	}

	[Fact]
	public void ParseUncompressedUsmapFromBuffer()
	{
		var buffer = File.ReadAllBytes(_uncompressedUsmapPath);
		var usmap = new Usmap(buffer);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
	}

	[Fact]
	public void ParseBrotliCompressedFromFile()
	{
		var usmap = new Usmap(_brotliCompressedUsmapPath);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
	}

	[Fact]
	public void ParseBrotliCompressedFromStream()
	{
		var usmap = new Usmap(File.OpenRead(_brotliCompressedUsmapPath));
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
	}

	[Fact]
	public void ParseBrotliCompressedFromBuffer()
	{
		var buffer = File.ReadAllBytes(_brotliCompressedUsmapPath);
		var usmap = new Usmap(buffer);
		Assert.Equal(ExpectedSchemas, usmap.Schemas.Length);
		Assert.Equal(ExpectedEnums, usmap.Enums.Length);
		Assert.Equal(ExpectedNames, usmap.Names.Length);
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
	}
}
