using System.Runtime.InteropServices;
using System.Security;


namespace NativeCodeSharp.Intrinsics
{
    /// <summary>
    /// Delegate for cpuid support check.
    /// </summary>
    /// <returns>Return true if cpuid is supported, otherwise false.</returns>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    [SuppressUnmanagedCodeSecurity]
    public delegate bool IsCpuIdSupportedDelegate();
}
