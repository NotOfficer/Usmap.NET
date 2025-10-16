namespace UsmapDotNet;

/// <summary/>
public sealed class UsmapProperty
{
    /// <summary/>
    public string Name { get; }
    /// <summary/>
    public UsmapPropertyData Data { get; }
    /// <summary/>
    public ushort SchemaIdx { get; }
    /// <summary/>
    public byte ArraySize { get; }

    internal UsmapProperty(string name, ushort schemaIdx, byte arraySize, UsmapPropertyData data)
    {
        Name = name;
        SchemaIdx = schemaIdx;
        ArraySize = arraySize;
        Data = data;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Name} | {Data.Type}";
    }
}
