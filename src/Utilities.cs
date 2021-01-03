using System.Runtime.InteropServices;

namespace UsmapNET
{
	public static class Utilities
	{
		internal const string OODLE_DLL_NAME = "oo2core64";

		[DllImport(OODLE_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
		internal static extern long OodleLZ_Decompress(byte[] buffer, long bufferSize, byte[] result, long outputBufferSize, int a, int b, int c, long d, long e, long f, long g, long h, long i, int ThreadModule);
	}
}