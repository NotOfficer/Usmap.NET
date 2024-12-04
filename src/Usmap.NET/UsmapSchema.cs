namespace UsmapDotNet;

/// <summary/>
public sealed class UsmapSchema
{
	/// <summary/>
	public string Name { get; }
	/// <summary/>
	public string? SuperType { get; }
	/// <summary/>
	public ushort PropCount { get; }
	/// <summary/>
	public IReadOnlyList<UsmapProperty> Properties { get; }

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
		return $"{Name}{(SuperType is null ? null : $":{SuperType}")} | {Properties.Count} ({PropCount})";
	}
}
