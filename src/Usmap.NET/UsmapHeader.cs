using System.Runtime.InteropServices;

namespace UsmapDotNet;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly struct UsmapHeader
{
	public const ushort MagicValue = 0x30C4;

	public readonly ushort Magic;
	public readonly EUsmapVersion Version;
	public readonly EUsmapCompressionMethod CompressionMethod;
	public readonly uint CompressedSize;
	public readonly uint UncompressedSize;
}
