using GenericReader;

namespace UsmapDotNet;

/// <summary/>
public sealed class UsmapPropertyData
{
    /// <summary/>
    public EUsmapPropertyType Type { get; }
    /// <summary/>
    public string? EnumName { get; internal set; }
    /// <summary/>
    public string? StructType { get; internal set; }
    /// <summary/>
    public UsmapPropertyData? InnerType { get; internal set; }
    /// <summary/>
    public UsmapPropertyData? ValueType { get; internal set; }

    internal UsmapPropertyData(EUsmapPropertyType type)
    {
        Type = type;
    }

    internal static UsmapPropertyData Deserialize<TReader>(ref TReader reader, string[] names)
        where TReader : IGenericReader
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        EUsmapPropertyType propType = reader.Read<EUsmapPropertyType>();
        var data = new UsmapPropertyData(propType);

        switch (propType)
        {
            case EUsmapPropertyType.EnumProperty:
            {
                data.InnerType = Deserialize(ref reader, names);
                uint idx = reader.Read<uint>();
                data.EnumName = names[idx];
                break;
            }
            case EUsmapPropertyType.StructProperty:
            {
                uint idx = reader.Read<uint>();
                data.StructType = names[idx];
                break;
            }
            case EUsmapPropertyType.SetProperty:
            case EUsmapPropertyType.ArrayProperty:
            case EUsmapPropertyType.OptionalProperty:
            {
                data.InnerType = Deserialize(ref reader, names);
                break;
            }
            case EUsmapPropertyType.MapProperty:
            {
                data.InnerType = Deserialize(ref reader, names);
                data.ValueType = Deserialize(ref reader, names);
                break;
            }
        }

        return data;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Type.ToStringFast()} | {StructType ?? EnumName ?? "?"}";
    }
}
