namespace UsmapDotNet;

/// <summary/>
public class UsmapEnum
{
	/// <summary/>
	public string Name { get; }
	/// <summary/>
	public string[] Names { get; }

	internal UsmapEnum(string name, string[] names)
	{
		Name = name;
		Names = names;
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return $"{Name} | {Names.Length}";
	}
}
