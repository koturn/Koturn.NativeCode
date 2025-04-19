using System;


namespace NativeCodeSharp.Internal.Win32.Enums
{
    /// <summary>
    /// Enum for third argument of <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>.
    /// </summary>
    [Flags]
    internal enum VirtualAllocType : uint
    {
        /// <summary>
        /// <para>Allocates memory charges (from the overall size of memory and the paging files on disk) for the specified reserved memory pages.
        /// The function also guarantees that when the caller later initially accesses the memory, the contents will be zero.
        /// Actual physical pages are not allocated unless/until the virtual addresses are actually accessed.</para>
        /// <para>To reserve and commit pages in one step, call <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> with <c><see cref="Commit"/> | <see cref="Reserve"/></c>.</para>
        /// <para>Attempting to commit a specific address range by specifying <see cref="Commit"/> without <see cref="Reserve"/> and a non-NULL lpAddress fails unless the entire range has already been reserved.
        /// The resulting error code is ERROR_INVALID_ADDRESS.</para>
        /// <para>An attempt to commit a page that is already committed does not cause the function to fail.
        /// This means that you can commit pages without first determining the current commitment state of each page.</para>
        /// <para>If lpAddress specifies an address within an enclave, flAllocationType must be <see cref="Commit"/>.</para>
        /// </summary>
        Commit = 0x1000,
        /// <summary>
        /// <para>Reserves a range of the process's virtual address space without allocating any actual physical storage in memory or in the paging file on disk.</para>
        /// <para>You can commit reserved pages in subsequent calls to the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> function.
        /// To reserve and commit pages in one step, call <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> with <c><see cref="Commit"/> | <see cref="Reserve"/></c>.</para>
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
        /// <para>When you specify <see cref="Reset"/>, <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> ignores the value of flProtect.
        /// However, you must still set flProtect to a valid protection value, such as <see cref="MemoryProtectionType.NoAccess"/>.</para>
        /// <para><see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> returns an error if you use <see cref="Reset"/> and the range of memory is mapped to a file.
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
        /// When you specify <see cref="Reset"/>, the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> function ignores the value of flProtect.
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
}
