namespace UsmapDotNet;

/// <summary/>
public enum EUsmapVersion : byte
{
	/// <summary/>
	Initial,
	/// <summary>
	/// Adds package versioning to aid with compatibility
	/// </summary>
	PackageVersioning,
	/// <summary>
	/// Adds support for 16-bit wide name-lengths (ushort/uint16)
	/// </summary>
	LongFName,
	/// <summary>
	/// Adds support for enums with more than 255 values
	/// </summary>
	LargeEnums,
    /// <summary>
    /// Adds support for explicit enum values
    /// </summary>
	ExplicitEnumValues,

	/// <summary/>
	LatestPlusOne,
	/// <summary/>
	Latest = LatestPlusOne - 1
}
