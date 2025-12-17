using NetEscapades.EnumGenerators;

namespace UsmapDotNet;

/// <summary/>
[EnumExtensions]
public enum EUsmapCompressionMethod : byte
{
    /// <summary/>
    None,
    /// <summary/>
    Oodle,
    /// <summary/>
    Brotli,
    /// <summary/>
    ZStandard,
    
    /// <summary/>
    MaxPlusOne,
    /// <summary/>
    Max = MaxPlusOne - 1,
    /// <summary/>
    Unknown = 0xFF
}
