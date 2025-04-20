#if NET7_0_OR_GREATER
#    define SUPPORT_LIBRARY_IMPORT
#endif  // NET7_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using NativeCodeSharp.Exceptions;
using NativeCodeSharp.Internals.Win32;
using NativeCodeSharp.Internals.Win32.Enums;


namespace NativeCodeSharp
{
    /// <summary>
    /// Native method handle class
    /// </summary>
    /// <typeparam name="TDelegate">Handled method type</typeparam>
    public sealed class NativeMethodHandle<TDelegate> : ICloneable, IDisposable
        where TDelegate : Delegate
    {
        #region Properties
        /// <summary>
        /// Delegate for native code
        /// </summary>
        public TDelegate Method { get; }
        /// <summary>
        /// Size of native code
        /// </summary>
        public int CodeSize { get; }
        /// <summary>
        /// A flag property which indicates this instance is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; }
        #endregion

        #region private members
        /// <summary>
        /// Unmanaged memory handle
        /// </summary>
        private readonly VirtualAllocedMemory _unmanagedMemory;
        #endregion

        #region ctor
        /// <summary>
        /// Create native method dynamically
        /// </summary>
        /// <param name="unmanagedMemory">Unmanaged memory manager which contains natice code function</param>
        /// <param name="codeSize">Size of native code</param>
        internal NativeMethodHandle(VirtualAllocedMemory unmanagedMemory, int codeSize)
        {
            Method = (TDelegate)Marshal.GetDelegateForFunctionPointer(
                unmanagedMemory.DangerousGetHandle(),
                typeof(TDelegate));
            CodeSize = codeSize;
            IsDisposed = false;
            _unmanagedMemory = unmanagedMemory;
        }
        #endregion

        #region public methods
        /// <summary>
        /// <para>Create clone of this object.</para>
        /// <para>Owned unmanaged memory is copy to another new allocated unmanaged memory.</para>
        /// </summary>
        /// <returns>clone of this object.</returns>
        public object Clone()
        {
            var vam = NativeMethodHandle.SafeNativeMethods.VirtualAlloc(
                IntPtr.Zero,
                (UIntPtr)CodeSize,
                VirtualAllocType.Commit,
                MemoryProtectionType.ReadWrite);
            if (vam.IsInvalid)
            {
                NativeMethodHandle.ThrowMemoryOperationException(
                    Marshal.GetLastWin32Error(),
                    "Failed to allocate memory with VirtualAlloc.");
            }
            NativeMethodHandle.SafeNativeMethods.CopyMemory(
                vam.DangerousGetHandle(),
                _unmanagedMemory.DangerousGetHandle(),
                CodeSize);
            NativeMethodHandle.ChangeProtectionAndFlush(vam, CodeSize);

            return new NativeMethodHandle<TDelegate>(vam, CodeSize);
        }
        #endregion

        #region IDisposable Support
        /// <summary>
        /// Dispose this instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing">A flag that indicates this method is called from Dispose or finalizer</param>
        private void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }
            if (disposing)
            {
                _unmanagedMemory.Dispose();
            }
            IsDisposed = true;
        }
        #endregion
    }


    /// <summary>
    /// This class provides utility function to dynamic native code generating methods.
    /// </summary>
#if SUPPORT_LIBRARY_IMPORT
    public static partial class NativeMethodHandle
#else
    public static class NativeMethodHandle
#endif  // SUPPORT_LIBRARY_IMPORT
    {
        /// <summary>
        /// Create native code delegate and wrap it into <see cref="NativeMethodHandle{TDelegate}"/> from given machine code.
        /// </summary>
        /// <typeparam name="TDelegate">Method delegate type</typeparam>
        /// <param name="code">Native code</param>
        /// <returns><see cref="NativeMethodHandle{TDelegate}"/> instance that includes native code.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="code"/> is null.</exception>
        /// <exception cref="MemoryOperationException">Throw when memory operation methods returns error.</exception>
        public static NativeMethodHandle<TDelegate> Create<TDelegate>(byte[] code)
            where TDelegate : Delegate
        {
            if (code is null)
            {
                ThrowArgumentNullException(nameof(code));
            }
            var vam = SafeNativeMethods.VirtualAlloc(
                IntPtr.Zero,
                (UIntPtr)code.Length,
                VirtualAllocType.Commit,
                MemoryProtectionType.ReadWrite);
            if (vam.IsInvalid)
            {
                ThrowMemoryOperationException(Marshal.GetLastWin32Error(), "Failed to allocate memory with VirtualAlloc.");
            }
            Marshal.Copy(code, 0, vam.DangerousGetHandle(), code.Length);
            ChangeProtectionAndFlush(vam, code.Length);

            return new NativeMethodHandle<TDelegate>(vam, code.Length);
        }

        /// <summary>
        /// Create native code delegate and wrap it into <see cref="NativeMethodHandle{TDelegate}"/> from given machine code.
        /// </summary>
        /// <typeparam name="TDelegate">Method delegate type</typeparam>
        /// <param name="codeQuery">Native code data query</param>
        /// <returns><see cref="NativeMethodHandle{TDelegate}"/> instance that includes native code.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="codeQuery"/> is null.</exception>
        /// <exception cref="MemoryOperationException">Throw when memory operation methods returns error.</exception>
        public static NativeMethodHandle<TDelegate> Create<TDelegate>(IEnumerable<byte> codeQuery)
            where TDelegate : Delegate
        {
            if (codeQuery is null)
            {
                ThrowArgumentNullException(nameof(codeQuery));
            }
            return Create<TDelegate>([.. codeQuery]);
        }

        /// <summary>
        /// Change protection type of vartual allocated memory and flush cache.
        /// </summary>
        /// <param name="vam">Memory handle allocated by <see cref="SafeNativeMethods.VirtualAlloc"/>.</param>
        /// <param name="codeSize">Size of native code.</param>
        internal static void ChangeProtectionAndFlush(VirtualAllocedMemory vam, int codeSize)
        {
            var addr = vam.DangerousGetHandle();

            // Give executable permission to unmanaged memroy.
            if (!SafeNativeMethods.VirtualProtect(
                addr,
                (UIntPtr)codeSize,
                MemoryProtectionType.Execute,
                out _))
            {
                var error = Marshal.GetLastWin32Error();
                vam.Dispose();
                ThrowMemoryOperationException(error, "Failed to give executable permission with VirtualProtect.");
            }

            // GetCurrentProcess returns a pseudo handle.
            // You need not to free a pseudo handle by ClodeHandle.
            if (!SafeNativeMethods.FlushInstructionCache(
                SafeNativeMethods.GetCurrentProcess(),
                addr,
                (UIntPtr)codeSize))
            {
                var error = Marshal.GetLastWin32Error();
                vam.Dispose();
                ThrowMemoryOperationException(error, "Failed to flush instruction code data with FlushInstructionCache.");
            }
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="paramName">A parameter name.</param>
        [DoesNotReturn]
        private static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Throw <see cref="MemoryOperationException"/>.
        /// </summary>
        /// <param name="error">An error code obrained by <see cref="Marshal.GetLastWin32Error()"/>.</param>
        /// <param name="message">Exception message.</param>
        internal static void ThrowMemoryOperationException(int error, string message)
        {
            throw new MemoryOperationException(error, message);
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
            /// Gets a new Process component and associates it with the currently active process.
            /// </summary>
            /// <returns>A new Process component associated with the process resource that is running the calling application.</returns>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = nameof(GetCurrentProcess))]
            public static partial IntPtr GetCurrentProcess();
#else
            [DllImport("kernel32.dll", EntryPoint = nameof(GetCurrentProcess), ExactSpelling = true)]
            public static extern IntPtr GetCurrentProcess();
#endif  // SUPPORT_LIBRARY_IMPORT

            /// <summary>
            /// Flushes the instruction cache for the specified process.
            /// </summary>
            /// <param name="processHandle">A handle to a process whose instruction cache is to be flushed.</param>
            /// <param name="baseAddress">A pointer to the base of the region to be flushed. This parameter can be IntPtr.Zero.</param>
            /// <param name="regionSize">The size of the region to be flushed if the <paramref name="baseAddress"/> parameter is not IntPtr.Zero, in bytes.</param>
            /// <returns>If the function succeeds, the return value is nonzero. If the function fails,
            /// the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = nameof(FlushInstructionCache), SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool FlushInstructionCache(IntPtr processHandle, IntPtr baseAddress, UIntPtr regionSize);
#else
            [DllImport("kernel32.dll", EntryPoint = nameof(FlushInstructionCache), ExactSpelling = true, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FlushInstructionCache(IntPtr processHandle, IntPtr baseAddress, UIntPtr regionSize);
#endif  // SUPPORT_LIBRARY_IMPORT

            /// <summary>
            /// Changes the protection on a region of committed pages in the virtual address space of the calling process.
            /// </summary>
            /// <param name="address">A pointer an address that describes the starting page of the region of pages whose access protection attributes are to be changed.</param>
            /// <param name="size">The size of the region whose access protection attributes are to be changed, in bytes.</param>
            /// <param name="protectionType">The memory protection option.</param>
            /// <param name="oldProtectionType">A pointer to a variable that receives the previous access protection value of the first page in the specified region of pages. If this parameter is NULL or does not point to a valid variable, the function fails.</param>
            /// <returns>If the function succeeds, the return value is nonzero.  If the function fails, the return value is zero. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = nameof(VirtualProtect), SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool VirtualProtect(IntPtr address, UIntPtr size, MemoryProtectionType protectionType, out MemoryProtectionType oldProtectionType);
#else
            [DllImport("kernel32.dll", EntryPoint = nameof(VirtualProtect), ExactSpelling = true, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool VirtualProtect(IntPtr address, UIntPtr size, MemoryProtectionType protectionType, out MemoryProtectionType oldProtectionType);
#endif  // SUPPORT_LIBRARY_IMPORT

            /// <summary>
            /// <para>Reserves, commits, or changes the state of a region of pages in the virtual address space of the calling process.</para>
            /// <para>Memory allocated by this function is automatically initialized to zero.</para>
            /// </summary>
            /// <param name="address">The starting address of the region to allocate. </param>
            /// <param name="size">The size of the region, in bytes.</param>
            /// <param name="allocType">The type of memory allocation.</param>
            /// <param name="protectionType">The memory protection for the region of pages to be allocated.</param>
            /// <returns>If the function succeeds, the return value is <see cref="VirtualAllocedMemory"/> instance which is the wrapper of the base address of the allocated region of pages. If the function fails, the return value is null. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = nameof(VirtualAlloc), SetLastError = true)]
            public static partial VirtualAllocedMemory VirtualAlloc(IntPtr address, UIntPtr size, VirtualAllocType allocType, MemoryProtectionType protectionType);
#else
            [DllImport("kernel32.dll", EntryPoint = nameof(VirtualAlloc), ExactSpelling = true, SetLastError = true)]
            public static extern VirtualAllocedMemory VirtualAlloc(IntPtr address, UIntPtr size, VirtualAllocType allocType, MemoryProtectionType protectionType);
#endif  // SUPPORT_LIBRARY_IMPORT

            /// <summary>
            /// Copy from an unmanaged memory to another unmanaged memory.
            /// </summary>
            /// <param name="dst">Destination pointer.</param>
            /// <param name="src">Source pointer.</param>
            /// <param name="size">Size of copying.</param>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = "RtlCopyMemory")]
            public static partial void CopyMemory(IntPtr dst, IntPtr src, int size);
#else
            [DllImport("kernel32.dll", EntryPoint = "RtlCopyMemory", ExactSpelling = true)]
            public static extern void CopyMemory(IntPtr dst, IntPtr src, int size);
#endif  // SUPPORT_LIBRARY_IMPORT
        }
    }
}
