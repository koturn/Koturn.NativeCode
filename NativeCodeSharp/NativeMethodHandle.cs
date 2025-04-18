using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

using NativeCodeSharp.Exceptions;
using NativeCodeSharp.Internal.Win32;


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
            var vam = Kernel32.VirtualAlloc(
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
            Kernel32.CopyMemory(
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
    public static class NativeMethodHandle
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
            var vam = Kernel32.VirtualAlloc(
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
            return Create<TDelegate>(codeQuery.ToArray());
        }

        /// <summary>
        /// Change protection type of vartual allocated memory and flush cache.
        /// </summary>
        /// <param name="vam">Memory handle allocated by <see cref="Kernel32.VirtualAlloc"/>.</param>
        /// <param name="codeSize">Size of native code.</param>
        internal static void ChangeProtectionAndFlush(VirtualAllocedMemory vam, int codeSize)
        {
            var addr = vam.DangerousGetHandle();

            // Give executable permission to unmanaged memroy.
            if (!Kernel32.VirtualProtect(
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
            if (!Kernel32.FlushInstructionCache(
                Kernel32.GetCurrentProcess(),
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
    }
}
