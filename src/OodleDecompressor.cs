using System;
using System.Runtime.InteropServices;

namespace UsmapNET
{
	internal unsafe class OodleDecompressor : IDisposable
	{
		private readonly IntPtr _handle;

#if NET5_0
		public readonly delegate* unmanaged[Cdecl]<byte[], long, byte[], long, int, int, int, long, long, long, long, long, long, int, long> Decompress;
#else
		public delegate long OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] result, long outputBufferSize, int a, int b, int c, long d, long e, long f, long g, long h, long i, int ThreadModule);
		public readonly OodleLZ_Decompress Decompress;
#endif

		public OodleDecompressor(string oodlePath)
		{
			_handle = NativeLibrary.Load(oodlePath);
			var address = NativeLibrary.GetExport(_handle, "OodleLZ_Decompress");

#if NET5_0
			Decompress = (delegate* unmanaged[Cdecl]<byte[], long, byte[], long, int, int, int, long, long, long, long, long, long, int, long>)address;
#else
			Decompress = Marshal.GetDelegateForFunctionPointer<OodleLZ_Decompress>(address);
#endif
		}

		public void Dispose()
		{
			if (_handle != IntPtr.Zero)
			{
				NativeLibrary.Free(_handle);
			}
		}
	}
}