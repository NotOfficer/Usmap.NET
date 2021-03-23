using System.IO;

using Xunit;

namespace Usmap.NET.Tests
{
	public class UsmapTests
	{
		private const string _oodlePath = @"C:\Games\Fortnite\FortniteGame\Binaries\Win64\oo2core_5_win64.dll";

		private const int _expectedSchemas = 13890;
		private const int _expectedEnums = 2367;
		private const int _expectedNames = 74908;

		private readonly string _uncompressedUsmapPath;
		private readonly string _brotliCompressedUsmapPath;
		private readonly string _oodleCompressedUsmapPath;

		public UsmapTests()
		{
			var currentDir = Directory.GetCurrentDirectory();
			var testFilesDir = Path.Combine(currentDir, "TestFiles");

			_uncompressedUsmapPath = Path.Combine(testFilesDir, "xx.usmap");
			_brotliCompressedUsmapPath = Path.Combine(testFilesDir, "br.usmap");
			_oodleCompressedUsmapPath = Path.Combine(testFilesDir, "oo.usmap");
		}

		[Fact]
		public void ParseUncompressedUsmapFromStream()
		{
			var usmap = new Usmap(_uncompressedUsmapPath);
			Assert.Equal(_expectedSchemas, usmap.Schemas.Length);
			Assert.Equal(_expectedEnums, usmap.Enums.Length);
			Assert.Equal(_expectedNames, usmap.Names.Length);
		}

		[Fact]
		public void ParseUncompressedUsmapFromBuffer()
		{
			var buffer = File.ReadAllBytes(_uncompressedUsmapPath);
			var usmap = new Usmap(buffer);
			Assert.Equal(_expectedSchemas, usmap.Schemas.Length);
			Assert.Equal(_expectedEnums, usmap.Enums.Length);
			Assert.Equal(_expectedNames, usmap.Names.Length);
		}

		[Fact]
		public void ParseBrotliCompressedFromStream()
		{
			var usmap = new Usmap(_brotliCompressedUsmapPath);
			Assert.Equal(_expectedSchemas, usmap.Schemas.Length);
			Assert.Equal(_expectedEnums, usmap.Enums.Length);
			Assert.Equal(_expectedNames, usmap.Names.Length);
		}

		[Fact]
		public void ParseBrotliCompressedFromBuffer()
		{
			var buffer = File.ReadAllBytes(_brotliCompressedUsmapPath);
			var usmap = new Usmap(buffer);
			Assert.Equal(_expectedSchemas, usmap.Schemas.Length);
			Assert.Equal(_expectedEnums, usmap.Enums.Length);
			Assert.Equal(_expectedNames, usmap.Names.Length);
		}

		[Fact]
		public void ParseOodleCompressedFromStream()
		{
			var options = new UsmapOptions
			{
				OodlePath = _oodlePath
			};
			var usmap = new Usmap(_oodleCompressedUsmapPath, options);
			Assert.Equal(_expectedSchemas, usmap.Schemas.Length);
			Assert.Equal(_expectedEnums, usmap.Enums.Length);
			Assert.Equal(_expectedNames, usmap.Names.Length);
		}

		[Fact]
		public void ParseOodleCompressedFromBuffer()
		{
			var buffer = File.ReadAllBytes(_oodleCompressedUsmapPath);
			var options = new UsmapOptions
			{
				OodlePath = _oodlePath
			};
			var usmap = new Usmap(buffer, options);
			Assert.Equal(_expectedSchemas, usmap.Schemas.Length);
			Assert.Equal(_expectedEnums, usmap.Enums.Length);
			Assert.Equal(_expectedNames, usmap.Names.Length);
		}
	}
}