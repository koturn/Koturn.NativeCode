using System.Runtime.InteropServices;
using System.Security;


namespace NativeCodeSharp.Intrinsics
{
    /// <summary>
    /// Delegate for cpuid.
    /// </summary>
    /// <param name="cpuInfo">Ouput paramter of cpuid.</param>
    /// <param name="eax">4byte value for eax, an argument of cpuid</param>
    /// <param name="ecx">4byte value for ecx, an argument of cpuid</param>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    [SuppressUnmanagedCodeSecurity]
    public delegate void CpuIdDelegate(out CpuInfo cpuInfo, uint eax, uint ecx = 0);
}
