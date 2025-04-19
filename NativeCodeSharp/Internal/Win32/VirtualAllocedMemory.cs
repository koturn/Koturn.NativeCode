using System;
using System.Security;
using System.Runtime.InteropServices;
using NativeCodeSharp.Internal.Win32.Enums;


namespace NativeCodeSharp.Internal.Win32
{
    /// <summary>
    /// <see cref="SafeHandle"/> for a memory which allocated with VirtualAlloc.
    /// </summary>
    internal sealed class VirtualAllocedMemory : SafeHandle
    {
        /// <summary>
        /// Private ctor
        /// </summary>
        private VirtualAllocedMemory()
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
        internal static class SafeNativeMethods
        {
            /// <summary>
            /// Releases, decommits, or releases and decommits a region of pages within the virtual address space of the calling process.
            /// </summary>
            /// <param name="address">A pointer to the base address of the region of pages to be freed.</param>
            /// <param name="size">The size of the region of memory to be freed, in bytes.</param>
            /// <param name="allocType">The type of free operation.</param>
            /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is 0 (zero). To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
            [DllImport("kernel32.dll", EntryPoint = nameof(VirtualFree), ExactSpelling = true, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool VirtualFree(IntPtr address, UIntPtr size, VirtualFreeType allocType);
        }
    }
}
