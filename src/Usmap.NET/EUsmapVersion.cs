namespace UsmapDotNet;

/// <summary/>
public enum EUsmapVersion : byte
{
	/// <summary/>
	Initial,
	
	/// <summary/>
	LatestPlusOne,
	/// <summary/>
	Latest = LatestPlusOne - 1
}
