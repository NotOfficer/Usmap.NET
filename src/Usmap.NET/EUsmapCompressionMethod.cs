namespace UsmapDotNet;

/// <summary/>
public enum EUsmapCompressionMethod : byte
{
	/// <summary/>
	None,
	/// <summary/>
	Oodle,
	/// <summary/>
	Brotli,
	
	/// <summary/>
	MaxPlusOne,
	/// <summary/>
	Max = MaxPlusOne - 1,
	/// <summary/>
	Unknown = byte.MaxValue
}
