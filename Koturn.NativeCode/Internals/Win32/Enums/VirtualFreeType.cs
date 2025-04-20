using System;


namespace Koturn.NativeCode.Internals.Win32.Enums
{
    /// <summary>
    /// Enum for third argument of <see cref="VirtualAllocedMemory.SafeNativeMethods.VirtualFree(IntPtr, UIntPtr, VirtualFreeType)"/>.
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
        /// <para>If you specify this value, dwSize must be 0 (zero), and lpAddress must point to the base address returned
        /// by the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/> function
        /// when the region is reserved.
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
}
