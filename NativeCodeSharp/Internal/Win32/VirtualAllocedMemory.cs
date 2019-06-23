using System;
using System.Security.Permissions;
using System.Runtime.InteropServices;


namespace NativeCodeSharp.Internal.Win32
{
    /// <summary>
    /// <see cref="SafeHandle"/> for a memory which allocated with VirtualAlloc.
    /// </summary>
    [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
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
            return Kernel32.VirtualFree(handle, UIntPtr.Zero, VirtualFreeType.Release);
        }
    }
}
