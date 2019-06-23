using System;
using System.Collections.Generic;
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
    public sealed class NativeMethodHandle<TDelegate> : IDisposable
        where TDelegate : Delegate
    {
        #region Properties
        /// <summary>
        /// Delegate for native code
        /// </summary>
        public TDelegate Method { get; private set; }
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
        internal NativeMethodHandle(VirtualAllocedMemory unmanagedMemory)
        {
            Method = Marshal.GetDelegateForFunctionPointer(
                unmanagedMemory.DangerousGetHandle(),
                typeof(TDelegate)) as TDelegate;
            IsDisposed = false;
            _unmanagedMemory = unmanagedMemory;
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
            if (code == null)
            {
                ThrowArgumentNullException("code");
            }
            var vam = Kernel32.VirtualAlloc(
                IntPtr.Zero,
                (UIntPtr)code.Length,
                VirtualAllocType.Commit,
                MemoryProtectionType.ReadWrite);
            if (vam.IsInvalid)
            {
                ThrowMemoryOperationException("Failed to allocate memory with VirtualAlloc.");
            }
            var addr = vam.DangerousGetHandle();
            Marshal.Copy(code, 0, addr, code.Length);

            // Give executable permission to unmanaged memroy.
            if (!Kernel32.VirtualProtect(
                addr,
                (UIntPtr)code.Length,
                MemoryProtectionType.Execute,
                out _))
            {
                vam.Dispose();
                ThrowMemoryOperationException("Failed to give executable permission with VirtualProtect.");
            }

            // GetCurrentProcess returns a pseudo handle.
            // You need not to free a pseudo handle by ClodeHandle.
            if (!Kernel32.FlushInstructionCache(
                Kernel32.GetCurrentProcess(),
                addr,
                (UIntPtr)code.Length))
            {
                vam.Dispose();
                ThrowMemoryOperationException("Failed to flush instruction code data with FlushInstructionCache.");
            }

            return new NativeMethodHandle<TDelegate>(vam);
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
            if (codeQuery == null)
            {
                ThrowArgumentNullException("codeQuery");
            }
            return Create<TDelegate>(codeQuery.ToArray());
        }

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <param name="paramName">A parameter name.</param>
        private static void ThrowArgumentNullException(string paramName)
        {
            throw new ArgumentNullException(paramName);
        }

        /// <summary>
        /// Throw <see cref="MemoryOperationException"/>.
        /// </summary>
        /// <param name="message">Exception message.</param>
        private static void ThrowMemoryOperationException(string message)
        {
            throw new MemoryOperationException(message);
        }
    }
}
