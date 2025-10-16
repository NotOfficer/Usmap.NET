namespace UsmapDotNet;

/// <summary/>
public sealed class UsmapEnum
{
	/// <summary/>
	public string Name { get; }
	/// <summary/>
	public IReadOnlyList<UsmapEnumMember> Members { get; }

	internal UsmapEnum(string name, UsmapEnumMember[] members)
	{
		Name = name;
		Members = members;
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"{Name} | {Members.Count}";
	}
}

/// <summary/>
public readonly struct UsmapEnumMember
{
	/// <summary/>
	public string Name { get; }
	/// <summary/>
	public long Value { get; }

	internal UsmapEnumMember(string name, long value)
	{
		Name = name;
		Value = value;
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"{Name} ({Value})";
	}
}
