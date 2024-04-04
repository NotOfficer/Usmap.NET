namespace UsmapDotNet;

/// <summary/>
public class UsmapSchema
{
	/// <summary/>
	public string Name { get; }
	/// <summary/>
	public string? SuperType { get; }
	/// <summary/>
	public ushort PropCount { get; }
	/// <summary/>
	public UsmapProperty[] Properties { get; }

	internal UsmapSchema(string name, string? superType, ushort propCount, UsmapProperty[] properties)
	{
		Name = name;
		SuperType = superType;
		PropCount = propCount;
		Properties = properties;
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"{Name}{(SuperType == null ? null : $":{SuperType}")} | {Properties.Length} ({PropCount})";
	}
}
