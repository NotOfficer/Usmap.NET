#if NET9_0_OR_GREATER
global using UsmapData = System.Span<byte>;
global using UsmapRoData = System.ReadOnlySpan<byte>;
global using UsmapReader = GenericReader.GenericSpanReader;
#else
global using UsmapData = System.Memory<byte>;
global using UsmapRoData = System.ReadOnlyMemory<byte>;
global using UsmapReader = GenericReader.GenericBufferReader;
#endif
