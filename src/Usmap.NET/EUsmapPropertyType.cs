using NetEscapades.EnumGenerators;

namespace UsmapDotNet;

/// <summary/>
[EnumExtensions]
public enum EUsmapPropertyType : byte
{
    /// <summary/>
    ByteProperty,
    /// <summary/>
    BoolProperty,
    /// <summary/>
    IntProperty,
    /// <summary/>
    FloatProperty,
    /// <summary/>
    ObjectProperty,
    /// <summary/>
    NameProperty,
    /// <summary/>
    DelegateProperty,
    /// <summary/>
    DoubleProperty,
    /// <summary/>
    ArrayProperty,
    /// <summary/>
    StructProperty,
    /// <summary/>
    StrProperty,
    /// <summary/>
    TextProperty,
    /// <summary/>
    InterfaceProperty,
    /// <summary/>
    MulticastDelegateProperty,
    /// <summary/>
    WeakObjectProperty,
    /// <summary/>
    LazyObjectProperty,
    /// <summary/>
    AssetObjectProperty,
    /// <summary/>
    SoftObjectProperty,
    /// <summary/>
    UInt64Property,
    /// <summary/>
    UInt32Property,
    /// <summary/>
    UInt16Property,
    /// <summary/>
    Int64Property,
    /// <summary/>
    Int16Property,
    /// <summary/>
    Int8Property,
    /// <summary/>
    MapProperty,
    /// <summary/>
    SetProperty,
    /// <summary/>
    EnumProperty,
    /// <summary/>
    FieldPathProperty,
    /// <summary/>
    OptionalProperty,
    /// <summary/>
    Utf8StrProperty,
    /// <summary/>
    AnsiStrProperty,
    /// <summary/>
    ClassProperty,
    /// <summary/>
    MulticastInlineDelegateProperty,
    /// <summary/>
    SoftClassProperty,
    /// <summary/>
    VerseStringProperty,
    /// <summary/>
    VerseDynamicProperty,
    /// <summary/>
    VerseFunctionProperty,

    //CustomProperty_FD = 0xFD,
    //CustomProperty_FE = 0xFE,

    /// <summary/>
    Unknown = byte.MaxValue
}
