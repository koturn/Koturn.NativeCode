using System;
using System.Security;
using System.Runtime.InteropServices;
using NativeCodeSharp.Internal.Win32;
using NativeCodeSharp.Internal.Win32.Enums;


namespace NativeCodeSharp.Intrinsics
{
    /// <summary>
    /// Utility class for intrinsic functions.
    /// </summary>
    public static class Intrinsic
    {
        #region check cpuid
        /// <summary>
        /// Check cpuid is supported or not.
        /// </summary>
        /// <returns>Return true if cpuid is supported, otherwise false.</returns>
        public static bool IsCpuIdSupported()
        {
            // All 64-bit intel processors support cpuid
            // https://software.intel.com/en-us/articles/using-cpuid-to-detect-the-presence-of-sse-41-and-sse-42-instruction-sets/
            return Environment.Is64BitProcess || IsCpuIdSupportedForX86();
        }

        /// <summary>
        /// <para>Check cpuid is supported or not.</para>
        /// <para>This function is for x86 because all 64 bit processor support cpuid.</para>
        /// </summary>
        /// <returns>Return true if cpuid is supported, otherwise false.</returns>
        public static bool IsCpuIdSupportedForX86()
        {
            using (var mh = CreateIsCpuIdSupportedMethodHandle())
            {
                return mh.Method();
            }
        }

        /// <summary>
        /// Create native code of cpuid checking function and wrap it into <see cref="NativeMethodHandle{TDelegate}"/>.
        /// </summary>
        /// <returns>cpuid support checking method cache.</returns>
        /// <exception cref="PlatformNotSupportedException">Throw when processor architecture is not x64 nor x86.</exception>
        public static NativeMethodHandle<IsCpuIdSupportedDelegate> CreateIsCpuIdSupportedMethodHandle()
        {
            if (!IsSupportedArchitecture())
            {
                ThrowPlatformNotSupportedException("cpuid support checking function is not supported on this architecture");
            }
            return NativeMethodHandle.Create<IsCpuIdSupportedDelegate>(Environment.Is64BitProcess
                // bool __stdcall isCpuIdSupported(void);  // The return value type may be 64bit value such as long long int
                ?
                [
                    0x9c,                                // pushfq
                    0x9c,                                // pushfq
                    0x58,                                // pop     rax
                    0x48, 0x89, 0xc1,                    // mov     rcx,rax
                    0x48, 0x35, 0x00, 0x00, 0x20, 0x00,  // xor     rax,200000h
                    0x50,                                // push    rax
                    0x9d,                                // popfq
                    0x9c,                                // pushfq
                    0x58,                                // pop     rax
                    0x48, 0x31, 0xc8,                    // xor     rax,rcx
                    0x48, 0xc1, 0xe8, 0x15,              // shr     rax,21
                    0x9d,                                // popfq
                    0xc3                                 // ret
                ]
                // bool __stdcall isCpuIdSupported(void);  // The return value type may be 32bit value such as int
                :
                [
                    0x9c,                          // pushf
                    0x9c,                          // pushf
                    0x58,                          // pop    eax
                    0x89, 0xc1,                    // mov    ecx,eax
                    0x35, 0x00, 0x00, 0x20, 0x00,  // xor    eax,200000h
                    0x50,                          // push   eax
                    0x9d,                          // popf
                    0x9c,                          // pushf
                    0x58,                          // pop    eax
                    0x31, 0xc8,                    // xor    eax,ecx
                    0xc1, 0xe8, 0x15,              // shr    eax,21
                    0x9d,                          // popf
                    0xc3,                          // ret
                ]);
        }
        #endregion

        #region cpuid
        /// <summary>
        /// cpuid method handle cache
        /// </summary>
        private static NativeMethodHandle<CpuIdDelegate>? _cpuIdHandle;

        /// <summary>
        /// Execute cpuid instruction.
        /// </summary>
        /// <param name="eax">Value for register, eax.</param>
        /// <param name="ecx">Value for register, ecx.</param>
        /// <returns>Result of cpuid</returns>
        public static CpuInfo CpuId(uint eax, uint ecx = 0)
        {
            _cpuIdHandle ??= CreateCpuIdMethodHandle();
            _cpuIdHandle.Method(out var cpuInfo, eax, ecx);
            return cpuInfo;
        }

        /// <summary>
        /// Create native code of cpuid and wrap it into <see cref="NativeMethodHandle{TDelegate}"/>.
        /// </summary>
        /// <returns>Method handle of cpuid</returns>
        /// <exception cref="PlatformNotSupportedException">Throw when processor architecture is not x64 nor x86.</exception>
        public static NativeMethodHandle<CpuIdDelegate> CreateCpuIdMethodHandle()
        {
            return CreateCpuIdMethodHandle<CpuIdDelegate>();
        }

        /// <summary>
        /// Check if environment processor architecture is Intel or AMD.
        /// </summary>
        /// <returns>True if your system is supported, otherwise false.</returns>
        private static bool IsSupportedArchitecture()
        {
            SafeNativeMethods.GetSystemInfo(out var systemInfo);
            return systemInfo.ProcessorArchitecture == ProcessorArchitecture.Intel
                || systemInfo.ProcessorArchitecture == ProcessorArchitecture.Amd64;
        }

        /// <summary>
        /// Throw <see cref="PlatformNotSupportedException"/>
        /// </summary>
        /// <param name="message">Exception message</param>
        private static void ThrowPlatformNotSupportedException(string message)
        {
            throw new PlatformNotSupportedException(message);
        }
        #endregion

        #region cpuid related methods
        /// <summary>
        /// Get CPU vendor ID.
        /// </summary>
        /// <returns>CPU vendor ID</returns>
        public static unsafe string GetCpuVendorId()
        {
            using (var mh = CreateCpuIdMethodHandle<UnsafeCpuIdDelegate>())
            {
                var cpuInfo = stackalloc uint[4];
                mh.Method(cpuInfo, 0);
                cpuInfo[0] = cpuInfo[1];
                cpuInfo[1] = cpuInfo[3];
                cpuInfo[3] = 0;  // for null-terminate
                return new string((sbyte*)cpuInfo);
            }
        }

        /// <summary>
        /// Get processor brand string.
        /// </summary>
        /// <returns>Processor brand string</returns>
        public static unsafe string GetCpuBrandString()
        {
            using (var mh = CreateCpuIdMethodHandle<UnsafeCpuIdDelegate>())
            {
                var cpuInfoArray = stackalloc uint[16];
                mh.Method(cpuInfoArray, 0x80000000);
                if (cpuInfoArray[0] < 0x80000004U)
                {
                    return string.Empty;
                }

                mh.Method(&cpuInfoArray[0], 0x80000002);
                mh.Method(&cpuInfoArray[4], 0x80000003);
                mh.Method(&cpuInfoArray[8], 0x80000004);

                return new string((sbyte*)cpuInfoArray);
            }
        }

        /// <summary>
        /// Get cache size and cache line size of CPU.
        /// </summary>
        /// <param name="cacheSize">Cache size of CPU in bytes.</param>
        /// <param name="cacheLineSize">Cache line size of CPU in bytes.</param>
        public static unsafe void
        GetCpuCacheSize(out int cacheSize, out int cacheLineSize)
        {
            using (var mh = CreateCpuIdMethodHandle<UnsafeCpuIdDelegate>())
            {
                var cpuInfo = stackalloc uint[4];
                mh.Method(cpuInfo, 0x80000000U);
                if (cpuInfo[0] < 0x80000006U)
                {
                    cacheSize = -1;
                    cacheLineSize = -1;
                    return;
                }
                mh.Method(cpuInfo, 0x80000006U);
                cacheSize = (int)((cpuInfo[2] & 0xffff0000U) >> 6);
                cacheLineSize = (int)(cpuInfo[2] & 0xffU);
            }
        }

        /// <summary>
        /// Create native code of cpuid and wrap it into <see cref="NativeMethodHandle{TDelegate}"/>.
        /// </summary>
        /// <typeparam name="TDelegate">Delegate for native method of cpuid.</typeparam>
        /// <returns>Method handle of cpuid.</returns>
        /// <exception cref="PlatformNotSupportedException">Throw when processor architecture is not x64 nor x86.</exception>
        private static NativeMethodHandle<TDelegate> CreateCpuIdMethodHandle<TDelegate>()
            where TDelegate : Delegate
        {
            if (!IsSupportedArchitecture())
            {
                ThrowPlatformNotSupportedException("cpuid is not supported on this architecture");
            }
            return NativeMethodHandle.Create<TDelegate>(Environment.Is64BitProcess
                // void __stdcall cpuid(int* cpuInfo, int eax, int ecx);
                ?
                [
                    0x53,                    // push   rbx
                    0x49, 0x89, 0xc9,        // mov    r9,rcx
                    0x89, 0xd0,              // mov    eax,edx
                    0x44, 0x89, 0xc1,        // mov    ecx,r8d
                    0x0f, 0xa2,              // cpuid
                    0x41, 0x89, 0x01,        // mov    dword ptr [r9],eax
                    0x41, 0x89, 0x59, 0x04,  // mov    dword ptr [r9 + 04h],ebx
                    0x41, 0x89, 0x49, 0x08,  // mov    dword ptr [r9 + 08h],ecx
                    0x41, 0x89, 0x51, 0x0c,  // mov    dword ptr [r9 + 0ch],edx
                    0x5b,                    // pop    rbx
                    0xc3                     // ret
                ]
                // void __stdcall cpuid(int* cpuInfo, int eax, int ecx);
                :
                [
                    0x56,                    // push   esi
                    0x53,                    // push   ebx
                    0x8b, 0x74, 0x24, 0x0c,  // mov    esi,dword ptr [esp + 0Ch]
                    0x8b, 0x44, 0x24, 0x10,  // mov    eax,dword ptr [esp + 10h]
                    0x8b, 0x4c, 0x24, 0x14,  // mov    ecx,dword ptr [esp + 14h]
                    0x0f, 0xa2,              // cpuid
                    0x89, 0x06,              // mov    dword ptr [esi],eax
                    0x89, 0x5e, 0x04,        // mov    dword ptr [esi + 04h],ebx
                    0x89, 0x4e, 0x08,        // mov    dword ptr [esi + 08h],ecx
                    0x89, 0x56, 0x0c,        // mov    dword ptr [esi + 0Ch],edx
                    0x5b,                    // pop    ebx
                    0x5e,                    // pop    esi
                    0xc2, 0x0c, 0x00         // ret    0Ch
                ]);
        }

        /// <summary>
        /// Delegate for cpuid (unsafe version).
        /// </summary>
        /// <param name="cpuInfo">Ouput paramter of cpuid.</param>
        /// <param name="eax">4byte value for eax, an argument of cpuid</param>
        /// <param name="ecx">4byte value for ecx, an argument of cpuid</param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [SuppressUnmanagedCodeSecurity]
        private unsafe delegate void UnsafeCpuIdDelegate(uint* cpuInfo, uint eax, uint ecx = 0);
        #endregion


        /// <summary>
        /// Provides native methods.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            /// <summary>
            /// Retrieves information about the current system.
            /// </summary>
            /// <param name="info">A reference to a <see cref="SystemInfo"/> structure that receives the information.</param>
            [DllImport("kernel32.dll", EntryPoint = nameof(GetSystemInfo), ExactSpelling = true, SetLastError = false)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void GetSystemInfo(out SystemInfo info);
        }
    }

    /// <summary>
    /// Delegate for cpuid.
    /// </summary>
    /// <param name="cpuInfo">Ouput paramter of cpuid.</param>
    /// <param name="eax">4byte value for eax, an argument of cpuid</param>
    /// <param name="ecx">4byte value for ecx, an argument of cpuid</param>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    [SuppressUnmanagedCodeSecurity]
    public delegate void CpuIdDelegate(out CpuInfo cpuInfo, uint eax, uint ecx = 0);

    /// <summary>
    /// Delegate for cpuid support check.
    /// </summary>
    /// <returns>Return true if cpuid is supported, otherwise false.</returns>
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    [SuppressUnmanagedCodeSecurity]
    public delegate bool IsCpuIdSupportedDelegate();
}
