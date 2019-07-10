using System;
using System.Runtime.InteropServices;
using System.Security;


namespace NativeCodeSharp.Internal.Win32
{
    /// <summary>
    /// Wrapper class for kernel32.dll
    /// </summary>
    internal static class Kernel32
    {
        /// <summary>
        /// Gets a new Process component and associates it with the currently active process.
        /// </summary>
        /// <returns>A new Process component associated with the process resource that is running the calling application.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetCurrentProcess();

        /// <summary>
        /// Flushes the instruction cache for the specified process.
        /// </summary>
        /// <param name="processHandle">A handle to a process whose instruction cache is to be flushed.</param>
        /// <param name="baseAddress">A pointer to the base of the region to be flushed. This parameter can be IntPtr.Zero.</param>
        /// <param name="regionSize">The size of the region to be flushed if the <paramref name="baseAddress"/> parameter is not IntPtr.Zero, in bytes.</param>
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool FlushInstructionCache(IntPtr processHandle, IntPtr baseAddress, UIntPtr regionSize);

        /// <summary>
        /// Retrieves information about the current system.
        /// </summary>
        /// <param name="info">A reference to a <see cref="SystemInfo"/> structure that receives the information.</param>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void GetSystemInfo(out SystemInfo info);

        /// <summary>
        /// <para>Reserves, commits, or changes the state of a region of pages in the virtual address space of the calling process.</para>
        /// <para>Memory allocated by this function is automatically initialized to zero.</para>
        /// </summary>
        /// <param name="address">The starting address of the region to allocate. </param>
        /// <param name="size">The size of the region, in bytes.</param>
        /// <param name="allocType">The type of memory allocation.</param>
        /// <param name="protectionType">The memory protection for the region of pages to be allocated.</param>
        /// <returns>If the function succeeds, the return value is <see cref="VirtualAllocedMemory"/> instance which is the wrapper of the base address of the allocated region of pages. If the function fails, the return value is null. To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VirtualAllocedMemory VirtualAlloc(IntPtr address, UIntPtr size, VirtualAllocType allocType, MemoryProtectionType protectionType);

        /// <summary>
        /// Releases, decommits, or releases and decommits a region of pages within the virtual address space of the calling process.
        /// </summary>
        /// <param name="address">A pointer to the base address of the region of pages to be freed.</param>
        /// <param name="size">The size of the region of memory to be freed, in bytes.</param>
        /// <param name="allocType">The type of free operation.</param>
        /// <returns>If the function succeeds, the return value is true.  If the function fails, the return value is 0 (zero). To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool VirtualFree(IntPtr address, UIntPtr size, VirtualFreeType allocType);

        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of the calling process.
        /// </summary>
        /// <param name="address">A pointer an address that describes the starting page of the region of pages whose access protection attributes are to be changed.</param>
        /// <param name="size">The size of the region whose access protection attributes are to be changed, in bytes.</param>
        /// <param name="protectionType">The memory protection option.</param>
        /// <param name="oldProtectionType">A pointer to a variable that receives the previous access protection value of the first page in the specified region of pages. If this parameter is NULL or does not point to a valid variable, the function fails.</param>
        /// <returns>If the function succeeds, the return value is nonzero.  If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [SuppressUnmanagedCodeSecurity]
        public static extern bool VirtualProtect(IntPtr address, UIntPtr size, MemoryProtectionType protectionType, out MemoryProtectionType oldProtectionType);

        /// <summary>
        /// Copy from an unmanaged memory to another unmanaged memory.
        /// </summary>
        /// <param name="dst">Destination pointer.</param>
        /// <param name="src">Source pointer.</param>
        /// <param name="size">Size of copying.</param>
        [DllImport("kernel32.dll", SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void CopyMemory(IntPtr dst, IntPtr src, int size);
    }

    /// <summary>
    /// Enum which represents processor architecture.
    /// </summary>
    internal enum ProcessorArchitecture : ushort
    {
        Amd64 = 9,
        Ia64 = 6,
        Intel = 0,
        Unknown = 0xffff,
    }

    /// <summary>
    /// Enum which represents processor architecture value.
    /// </summary>
    internal enum ProcessorType : uint
    {
        Intel386 = 386,
        Intel486 = 486,
        IntelPentium = 586,
        IntelIa64 = 2200,
        AmdX8664 = 8664,
    }

    /// <summary>
    /// Enum for third argument of <see cref="VirtualAllocedMemory"/>.
    /// </summary>
    [Flags]
    internal enum VirtualAllocType : uint
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Reset = 0x80000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        Physical = 0x400000,
        LargePages = 0x20000000,
    }

    /// <summary>
    /// Enum for third argument of <see cref="Kernel32.VirtualFree"/>.
    /// </summary>
    [Flags]
    internal enum VirtualFreeType : uint
    {
        Decommit = 0x4000,
        Release = 0x8000,
    }

    /// <summary>
    /// Enum for <see cref="VirtualAllocedMemory"/> and <see cref="Kernel32.VirtualProtect"/>.
    /// </summary>
    [Flags]
    internal enum MemoryProtectionType : uint
    {
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        Guard = 0x100,
        NoCache = 0x200,
        WriteCombine = 0x400
    }
}
