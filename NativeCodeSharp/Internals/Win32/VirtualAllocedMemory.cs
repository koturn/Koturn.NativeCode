#if NET7_0_OR_GREATER
#    define SUPPORT_LIBRARY_IMPORT
#endif  // NET7_0_OR_GREATER

using System;
using System.Security;
using System.Runtime.InteropServices;
using NativeCodeSharp.Internals.Win32.Enums;


namespace NativeCodeSharp.Internals.Win32
{
    /// <summary>
    /// <see cref="SafeHandle"/> for a memory which allocated with VirtualAlloc.
    /// </summary>
#if SUPPORT_LIBRARY_IMPORT
    internal sealed partial class VirtualAllocedMemory : SafeHandle
#else
    internal sealed class VirtualAllocedMemory : SafeHandle
#endif  // SUPPORT_LIBRARY_IMPORT
    {
        /// <summary>
        /// Private ctor
        /// </summary>
#if NET6_0_OR_GREATER
        public VirtualAllocedMemory()
#else
        private VirtualAllocedMemory()
#endif  // NET8_0_OR_GREATER
            : base(IntPtr.Zero, true)
        {
        }

        /// <summary>
        /// True if memory is not allocated (null pointer), otherwise false.
        /// </summary>
        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }

        /// <summary>
        /// Free allocated memory.
        /// </summary>
        /// <returns>True if freeing is successful, otherwise false</returns>
        protected override bool ReleaseHandle()
        {
            return SafeNativeMethods.VirtualFree(handle, UIntPtr.Zero, VirtualFreeType.Release);
        }


        /// <summary>
        /// Provides native methods.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
#if SUPPORT_LIBRARY_IMPORT
        internal static partial class SafeNativeMethods
#else
        internal static class SafeNativeMethods
#endif  // SUPPORT_LIBRARY_IMPORT
        {
            /// <summary>
            /// Releases, decommits, or releases and decommits a region of pages within the virtual address space of the calling process.
            /// </summary>
            /// <param name="address">A pointer to the base address of the region of pages to be freed.</param>
            /// <param name="size">The size of the region of memory to be freed, in bytes.</param>
            /// <param name="allocType">The type of free operation.</param>
            /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is 0 (zero). To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = nameof(VirtualFree), SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool VirtualFree(IntPtr address, UIntPtr size, VirtualFreeType allocType);
#else
            [DllImport("kernel32.dll", EntryPoint = nameof(VirtualFree), ExactSpelling = true, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool VirtualFree(IntPtr address, UIntPtr size, VirtualFreeType allocType);
#endif  // SUPPORT_LIBRARY_IMPORT
        }
    }
}
