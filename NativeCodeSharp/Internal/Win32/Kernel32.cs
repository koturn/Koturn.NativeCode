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
        /// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
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
        /// <returns>If the function succeeds, the return value is <see cref="VirtualAllocedMemory"/> instance which is the wrapper of the base address of the allocated region of pages. If the function fails, the return value is null. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern VirtualAllocedMemory VirtualAlloc(IntPtr address, UIntPtr size, VirtualAllocType allocType, MemoryProtectionType protectionType);

        /// <summary>
        /// Releases, decommits, or releases and decommits a region of pages within the virtual address space of the calling process.
        /// </summary>
        /// <param name="address">A pointer to the base address of the region of pages to be freed.</param>
        /// <param name="size">The size of the region of memory to be freed, in bytes.</param>
        /// <param name="allocType">The type of free operation.</param>
        /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is 0 (zero). To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VirtualFree(IntPtr address, UIntPtr size, VirtualFreeType allocType);

        /// <summary>
        /// Changes the protection on a region of committed pages in the virtual address space of the calling process.
        /// </summary>
        /// <param name="address">A pointer an address that describes the starting page of the region of pages whose access protection attributes are to be changed.</param>
        /// <param name="size">The size of the region whose access protection attributes are to be changed, in bytes.</param>
        /// <param name="protectionType">The memory protection option.</param>
        /// <param name="oldProtectionType">A pointer to a variable that receives the previous access protection value of the first page in the specified region of pages. If this parameter is NULL or does not point to a valid variable, the function fails.</param>
        /// <returns>If the function succeeds, the return value is nonzero.  If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
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
        /// <summary>
        /// x64 (AMD or Intel).
        /// </summary>
        Amd64 = 9,
        /// <summary>
        /// ARM
        /// </summary>
        Arm = 5,
        /// <summary>
        /// ARM64
        /// </summary>
        Arm64 = 12,
        /// <summary>
        /// Intel Itanium-based.
        /// </summary>
        Ia64 = 6,
        /// <summary>
        /// x86
        /// </summary>
        Intel = 0,
        /// <summary>
        /// Unknown architecture.
        /// </summary>
        Unknown = 0xffff
    }

    /// <summary>
    /// Enum which represents processor architecture value.
    /// </summary>
    internal enum ProcessorType : uint
    {
        /// <summary>
        /// Intel 386.
        /// </summary>
        Intel386 = 386,
        /// <summary>
        /// Intel 486.
        /// </summary>
        Intel486 = 486,
        /// <summary>
        /// Intel Pentium.
        /// </summary>
        IntelPentium = 586,
        /// <summary>
        /// Intel IA-64.
        /// </summary>
        IntelIa64 = 2200,
        /// <summary>
        /// AMD x86/x64.
        /// </summary>
        AmdX8664 = 8664,
    }

    /// <summary>
    /// Enum for third argument of <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>.
    /// </summary>
    [Flags]
    internal enum VirtualAllocType : uint
    {
        /// <summary>
        /// <para>Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages.
        /// The function also guarantees that when the caller later initially accesses the memory, the contents will be zero.
        /// Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.</para>
        /// <para>To reserve and commit pages in one step, call <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> with <c><see cref="Commit"/> | <see cref="Reserve"/></c>.</para>
        /// <para>Attempting to commit a specific address range by specifying <see cref="Commit"/> without <see cref="Reserve"/> and a non-NULL lpAddress fails unless the entire range has already been reserved.
        /// The resulting error code is ERROR_INVALID_ADDRESS.</para>
        /// <para>An attempt to commit a page that is already committed does not cause the function to fail.
        /// This means that you can commit pages without first determining the current commitment state of each page.</para>
        /// <para>If lpAddress specifies an address within an enclave, flAllocationType must be <see cref="Commit"/>.</para>
        /// </summary>
        Commit = 0x1000,
        /// <summary>
        /// <para>Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on disk.</para>
        /// <para>You can commit reserved pages in subsequent calls to the <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> function.
        /// To reserve and commit pages in one step, call <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> with <c><see cref="Commit"/> | <see cref="Reserve"/></c>.</para>
        /// <para>Other memory allocation functions, such as malloc and <c>LocalAlloc</c>, cannot use a reserved range of memory until it is released.</para>
        /// </summary>
        Reserve = 0x2000,
        /// <summary>
        /// <para>Indicates that data in the memory range specified by lpAddress and dwSize is no longer of interest.
        /// The pages should not be read from or written to the paging file.
        /// However, the memory block will be used again later, so it should not be decommitted.
        /// This value cannot be used with any other value.</para>
        /// <para>Using this value does not guarantee that the range operated on with <see cref="Reset"/> will contain zeros.
        /// If you want the range to contain zeros, decommit the memory and then recommit it.</para>
        /// <para>When you specify <see cref="Reset"/>, <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> ignores the value of flProtect.
        /// However, you must still set flProtect to a valid protection value, such as <see cref="MemoryProtectionType.NoAccess"/>.</para>
        /// <para><see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> returns an error if you use <see cref="Reset"/> and the range of memory is mapped to a file.
        /// A shared view is only acceptable if it is mapped to a paging file.</para>
        /// </summary>
        Reset = 0x80000,
        /// <summary>
        /// <para><see cref="ResetUndo"/> should only be called on an address range to which <see cref="Reset"/> was successfully applied earlier.
        /// It indicates that the data in the specified memory range specified by lpAddress and dwSize is of interest to the caller and attempts to reverse the effects of <see cref="Reset"/>.
        /// If the function succeeds, that means all data in the specified address range is intact.
        /// If the function fails, at least some of the data in the address range has been replaced with zeroes.</para>
        /// <para>This value cannot be used with any other value.
        /// If <see cref="ResetUndo"/> is called on an address range which was not <see cref="Reset"/> earlier, the behavior is undefined.
        /// When you specify <see cref="Reset"/>, the <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> function ignores the value of flProtect.
        /// However, you must still set flProtect to a valid protection value, such as PAGE_NOACCESS.</para>
        /// <para>Windows Server 2008 R2, Windows 7, Windows Server 2008, Windows Vista, Windows Server 2003 and Windows XP: <see cref="ResetUndo"/> flag is not supported until Windows 8 and Windows Server 2012.</para>
        /// </summary>
        ResetUndo = 0x1000000,
        /// <summary>
        /// <para>Allocates memory using large page support.</para>
        /// <para>The size and alignment must be a multiple of the large-page minimum.
        /// To obtain this value, use the <c>GetLargePageMinimum</c> function.</para>
        /// <para>If you specify this value, you must also specify <see cref="Reserve"/> and <see cref="Commit"/>.</para>
        /// </summary>
        LargePages = 0x20000000,
        /// <summary>
        /// <para>Reserves an address range that can be used to map Address Windowing Extensions (AWE) pages.</para>
        /// <para>This value must be used with <see cref="Reserve"/> and no other values.</para>
        /// </summary>
        Physical = 0x400000,
        /// <summary>
        /// Allocates memory at the highest possible address.
        /// This can be slower than regular allocations, especially when there are many allocations.
        /// </summary>
        TopDown = 0x100000,
        /// <summary>
        /// <para>Causes the system to track pages that are written to in the allocated region.
        /// If you specify this value, you must also specify MEM_RESERVE.</para>
        /// <para>To retrieve the addresses of the pages that have been written to since the region was allocated or the write-tracking state was reset, call the GetWriteWatch function.
        /// To reset the write-tracking state, call GetWriteWatch or ResetWriteWatch.
        /// The write-tracking feature remains enabled for the memory region until the region is freed.</para>
        /// </summary>
        WriteWatch = 0x200000
    }

    /// <summary>
    /// Enum for third argument of <see cref="Kernel32.VirtualFree(IntPtr, UIntPtr, VirtualFreeType)"/>.
    /// </summary>
    [Flags]
    internal enum VirtualFreeType : uint
    {
        /// <summary>
        /// <para>Decommits the specified region of committed pages.
        /// After the operation, the pages are in the reserved state.</para>
        /// <para>The function does not fail if you attempt to decommit an uncommitted page.
        /// This means that you can decommit a range of pages without first determining the current commitment state.</para>
        /// <para>The <see cref="Decommit"/> value is not supported when the lpAddress parameter provides the base address for an enclave.</para>
        /// </summary>
        Decommit = 0x4000,
        /// <summary>
        /// <para>Releases the specified region of pages, or placeholder (for a placeholder, the address space is released and available for other allocations).
        /// After this operation, the pages are in the free state.</para>
        /// <para>If you specify this value, dwSize must be 0 (zero), and lpAddress must point to the base address returned by the <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> function when the region is reserved.
        /// The function fails if either of these conditions is not met.</para>
        /// <para>If any pages in the region are committed currently, the function first decommits, and then releases them.</para>
        /// <para>The function does not fail if you attempt to release pages that are in different states, some reserved and some committed.
        /// This means that you can release a range of pages without first determining the current commitment state.</para>
        /// </summary>
        Release = 0x8000,
        /// <summary>
        /// To coalesce two adjacent placeholders, specify <c><see cref="Release"/> | <see cref="CoalescePlaceholders"/></c>.
        /// When you coalesce placeholders, lpAddress and dwSize must exactly match those of the placeholder.
        /// </summary>
        CoalescePlaceholders = 0x00000001,
        /// <summary>
        /// <para>Frees an allocation back to a placeholder (after you've replaced a placeholder with a private allocation using <c>VirtualAlloc2</c> or <c>Virtual2AllocFromApp</c>).</para>
        /// <para>To split a placeholder into two placeholders, specify <c><see cref="Release"/> | <see cref="PreservePlaceholder"/></c>.</para>
        /// </summary>
        PreservePlaceholder = 0x00000002
    }

    /// <summary>
    /// Enum for <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>
    /// and <see cref="Kernel32.VirtualProtect(IntPtr, UIntPtr, MemoryProtectionType, out MemoryProtectionType)"/>.
    /// </summary>
    [Flags]
    internal enum MemoryProtectionType : uint
    {
        /// <summary>
        /// <para>Disables all access to the committed region of pages.
        /// An attempt to read from, write to, or execute the committed region results in an access violation.</para>
        /// <para>This flag is not supported by the <c>CreateFileMapping</c> function.</para>
        /// </summary>
        NoAccess = 0x01,
        /// <summary>
        /// Enables read-only access to the committed region of pages.
        /// An attempt to write to the committed region results in an access violation.
        /// If Data Execution Prevention is enabled, an attempt to execute code in the committed region results in an access violation.
        /// </summary>
        ReadOnly = 0x02,
        /// <summary>
        /// Enables read-only or read/write access to the committed region of pages.
        /// If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.
        /// </summary>
        ReadWrite = 0x04,
        /// <summary>
        /// <para>Enables read-only or copy-on-write access to a mapped view of a file mapping object.
        /// An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process.
        /// The private page is marked as <see cref="ReadWrite"/>, and the change is written to the new page.
        /// If Data Execution Prevention is enabled, attempting to execute code in the committed region results in an access violation.</para>
        /// <para>This flag is not supported by the <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> or <c>VirtualAllocEx</c> functions.</para>
        /// </summary>
        WriteCopy = 0x08,
        /// <summary>
        /// <para>Enables execute access to the committed region of pages.
        /// An attempt to write to the committed region results in an access violation.</para>
        /// <para>This flag is not supported by the <c>CreateFileMapping</c> function.</para>
        /// </summary>
        Execute = 0x10,
        /// <summary>
        /// <para>Enables execute or read-only access to the committed region of pages.
        /// An attempt to write to the committed region results in an access violation.</para>
        /// <para>Windows Server 2003 and Windows XP: This attribute is not supported by the <c>CreateFileMapping</c> function until Windows XP with SP2 and Windows Server 2003 with SP1.</para>
        /// </summary>
        ExecuteRead = 0x20,
        /// <summary>
        /// <para>Enables execute, read-only, or read/write access to the committed region of pages.</para>
        /// <para>Windows Server 2003 and Windows XP: This attribute is not supported by the <c>CreateFileMapping</c> function until Windows XP with SP2 and Windows Server 2003 with SP1.</para>
        /// </summary>
        ExecuteReadWrite = 0x40,
        /// <summary>
        /// <para>Enables execute, read-only, or copy-on-write access to a mapped view of a file mapping object.
        /// An attempt to write to a committed copy-on-write page results in a private copy of the page being made for the process.
        /// The private page is marked as <see cref="ExecuteReadWrite"/>, and the change is written to the new page.</para>
        /// <para>This flag is not supported by the <see cref="Kernel32.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> or <c>VirtualAllocEx</c> functions.
        /// Windows Vista, Windows Server 2003 and Windows XP: This attribute is not supported by the <c>CreateFileMapping</c> function until Windows Vista with SP1 and Windows Server 2008.</para>
        /// </summary>
        ExecuteWriteCopy = 0x80,
        /// <summary>
        /// <para>Pages in the region become guard pages.
        /// Any attempt to access a guard page causes the system to raise a STATUS_GUARD_PAGE_VIOLATION exception and turn off the guard page status.
        /// Guard pages thus act as a one-time access alarm.
        /// For more information, see Creating Guard Pages.</para>
        /// <para>When an access attempt leads the system to turn off guard page status, the underlying page protection takes over.</para>
        /// <para>If a guard page exception occurs during a system service, the service typically returns a failure status indicator.</para>
        /// <para>This value cannot be used with <see cref="NoAccess"/>.</para>
        /// <para>This flag is not supported by the <c>CreateFileMapping</c> function.</para>
        /// </summary>
        Guard = 0x100,
        /// <summary>
        /// <para>Sets all pages to be non-cachable.
        /// Applications should not use this attribute except when explicitly required for a device.
        /// Using the interlocked functions with memory that is mapped with SEC_NOCACHE can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.</para>
        /// <para>The <see cref="NoCache"/> flag cannot be used with the <see cref="Guard"/>, <see cref="NoAccess"/>, or <see cref="WriteCombine"/> flags.</para>
        /// <para>The <see cref="NoCache"/> flag can be used only when allocating private memory with the <see cref="Kernel32.VirtualAlloc"/>, <c>VirtualAllocEx</c>, or <c>VirtualAllocExNuma</c> functions.
        /// To enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the <c>CreateFileMapping</c> function.</para>
        /// </summary>
        NoCache = 0x200,
        /// <summary>
        /// <para>Sets all pages to be write-combined.</para>
        /// <para>Applications should not use this attribute except when explicitly required for a device.
        /// Using the interlocked functions with memory that is mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.</para>
        /// <para>The <see cref="WriteCombine"/> flag cannot be specified with the <see cref="NoAccess"/>, <see cref="Guard"/>, and <see cref="NoCache"/> flags.</para>
        /// <para>The <see cref="WriteCombine"/> flag can be used only when allocating private memory with the <see cref="Kernel32.VirtualAlloc"/>, <c>VirtualAllocEx</c>, or <c>VirtualAllocExNuma</c> functions.
        /// To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the <c>CreateFileMapping</c> function.</para>
        /// <para>Windows Server 2003 and Windows XP: This flag is not supported until Windows Server 2003 with SP1.</para>
        /// </summary>
        WriteCombine = 0x400,
        /// <summary>
        /// <para>Sets all locations in the pages as invalid targets for CFG.
        /// Used along with any execute page protection like <see cref="Execute"/>, <see cref="ExecuteRead"/>, <see cref="ExecuteReadWrite"/> and <see cref="ExecuteWriteCopy"/>.
        /// Any indirect call to locations in those pages will fail CFG checks and the process will be terminated.
        /// The default behavior for executable pages allocated is to be marked valid call targets for CFG.</para>
        /// <para>This flag is not supported by the <see cref="Kernel32.VirtualProtect(IntPtr, UIntPtr, MemoryProtectionType, out MemoryProtectionType)"/> or <c>CreateFileMapping</c> functions.</para>
        /// </summary>
        TargetsInvalid = 0x40000000,
        /// <summary>
        /// Pages in the region will not have their CFG information updated while the protection changes for <see cref="Kernel32.VirtualProtect"/> .
        /// For example, if the pages in the region was allocated using <see cref="TargetsInvalid"/>, then the invalid information will be maintained while the page protection changes.
        /// This flag is only valid when the protection changes to an executable type like <see cref="Execute"/>, <see cref="ExecuteRead"/>, <see cref="ExecuteReadWrite"/> and <see cref="ExecuteWriteCopy"/>.
        /// The default behavior for <see cref="Kernel32.VirtualProtect(IntPtr, UIntPtr, MemoryProtectionType, out MemoryProtectionType)"/> protection change to executable is to mark all locations as valid call targets for CFG.
        /// </summary>
        TargetsNoUpdate = 0x40000000
    }
}
