using GenericReader;

namespace UsmapDotNet;

public class UsmapPropertyData
{
	public EUsmapPropertyType Type { get; }
	public string? EnumName { get; internal set; }
	public string? StructType { get; internal set; }
	public UsmapPropertyData? InnerType { get; internal set; }
	public UsmapPropertyData? ValueType { get; internal set; }

	internal UsmapPropertyData(EUsmapPropertyType type)
	{
		Type = type;
	}

	internal static UsmapPropertyData Deserialize(IGenericReader reader, string[] names)
	{
		var propType = reader.Read<EUsmapPropertyType>();
		var data = new UsmapPropertyData(propType);

		switch (propType)
		{
			case EUsmapPropertyType.EnumProperty:
			{
				data.InnerType = Deserialize(reader, names);
				var idx = reader.Read<uint>();
				data.EnumName = names[idx];
				break;
			}
			case EUsmapPropertyType.StructProperty:
			{
				var idx = reader.Read<uint>();
				data.StructType = names[idx];
				break;
			}
			case EUsmapPropertyType.SetProperty:
			case EUsmapPropertyType.ArrayProperty:
			case EUsmapPropertyType.OptionalProperty:
			{
				data.InnerType = Deserialize(reader, names);
				break;
			}
			case EUsmapPropertyType.MapProperty:
			{
				data.InnerType = Deserialize(reader, names);
				data.ValueType = Deserialize(reader, names);
				break;
			}
		}

		return data;
	}

	public override string ToString()
	{
		return $"{Type.ToStringFast()} | {(StructType ?? EnumName ?? "?")}";
	}
}
