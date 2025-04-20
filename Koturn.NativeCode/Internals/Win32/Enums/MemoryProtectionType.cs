using System;


#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1069 // Enums values should not be duplicated


namespace Koturn.NativeCode.Internals.Win32.Enums
{
    /// <summary>
    /// Enum for <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>
    /// and <see cref="NativeMethodHandle.SafeNativeMethods.VirtualProtect(IntPtr, UIntPtr, MemoryProtectionType, out MemoryProtectionType)"/>.
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
        /// <para>This flag is not supported by the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>
        /// or <c>VirtualAllocEx</c> functions.</para>
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
        /// <para>This flag is not supported by the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>
        /// or <c>VirtualAllocEx</c> functions.
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
        /// <para>The <see cref="NoCache"/> flag can be used only when allocating private memory with
        /// the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>, <c>VirtualAllocEx</c>, or <c>VirtualAllocExNuma</c> functions.
        /// To enable non-cached memory access for shared memory, specify the SEC_NOCACHE flag when calling the <c>CreateFileMapping</c> function.</para>
        /// </summary>
        NoCache = 0x200,
        /// <summary>
        /// <para>Sets all pages to be write-combined.</para>
        /// <para>Applications should not use this attribute except when explicitly required for a device.
        /// Using the interlocked functions with memory that is mapped as write-combined can result in an EXCEPTION_ILLEGAL_INSTRUCTION exception.</para>
        /// <para>The <see cref="WriteCombine"/> flag cannot be specified with the <see cref="NoAccess"/>, <see cref="Guard"/>, and <see cref="NoCache"/> flags.</para>
        /// <para>The <see cref="WriteCombine"/> flag can be used only when allocating private memory with
        /// the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualAlloc(IntPtr, UIntPtr, VirtualAllocType, MemoryProtectionType)"/>, <c>VirtualAllocEx</c>, or <c>VirtualAllocExNuma</c> functions.
        /// To enable write-combined memory access for shared memory, specify the SEC_WRITECOMBINE flag when calling the <c>CreateFileMapping</c> function.</para>
        /// <para>Windows Server 2003 and Windows XP: This flag is not supported until Windows Server 2003 with SP1.</para>
        /// </summary>
        WriteCombine = 0x400,
        /// <summary>
        /// <para>Sets all locations in the pages as invalid targets for CFG.
        /// Used along with any execute page protection like <see cref="Execute"/>, <see cref="ExecuteRead"/>, <see cref="ExecuteReadWrite"/> and <see cref="ExecuteWriteCopy"/>.
        /// Any indirect call to locations in those pages will fail CFG checks and the process will be terminated.
        /// The default behavior for executable pages allocated is to be marked valid call targets for CFG.</para>
        /// <para>This flag is not supported by the <see cref="NativeMethodHandle.SafeNativeMethods.VirtualProtect(IntPtr, UIntPtr, MemoryProtectionType, out MemoryProtectionType)"/> or <c>CreateFileMapping</c> functions.</para>
        /// </summary>
        TargetsInvalid = 0x40000000,
        /// <summary>
        /// Pages in the region will not have their CFG information updated while the protection changes for <see cref="NativeMethodHandle.SafeNativeMethods.VirtualProtect(IntPtr, UIntPtr, MemoryProtectionType, out MemoryProtectionType)"/> .
        /// For example, if the pages in the region was allocated using <see cref="TargetsInvalid"/>, then the invalid information will be maintained while the page protection changes.
        /// This flag is only valid when the protection changes to an executable type like <see cref="Execute"/>, <see cref="ExecuteRead"/>, <see cref="ExecuteReadWrite"/> and <see cref="ExecuteWriteCopy"/>.
        /// The default behavior for <see cref="NativeMethodHandle.SafeNativeMethods.VirtualProtect(IntPtr, UIntPtr, MemoryProtectionType, out MemoryProtectionType)"/> protection
        /// change to executable is to mark all locations as valid call targets for CFG.
        /// </summary>
        TargetsNoUpdate = 0x40000000
    }
}
