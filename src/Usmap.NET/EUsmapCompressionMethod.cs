namespace UsmapDotNet;

public enum EUsmapCompressionMethod : byte
{
	None,
	Oodle,
	Brotli,

	MaxPlusOne,
	Max = MaxPlusOne - 1,
	Unknown = byte.MaxValue
}
