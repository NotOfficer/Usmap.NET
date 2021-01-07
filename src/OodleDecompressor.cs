using System;
using System.Runtime.InteropServices;

namespace UsmapNET
{
	internal class OodleDecompressor : IDisposable
	{
		private readonly IntPtr _handle;

		public delegate long OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] result, long outputBufferSize, int a, int b, int c, long d, long e, long f, long g, long h, long i, int ThreadModule);
		public readonly OodleLZ_Decompress Decompress;

		public OodleDecompressor(string oodlePath)
		{
			_handle = NativeLibrary.Load(oodlePath);
			var address = NativeLibrary.GetExport(_handle, "OodleLZ_Decompress");
			Decompress = Marshal.GetDelegateForFunctionPointer<OodleLZ_Decompress>(address);
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