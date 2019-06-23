using System;
using System.Runtime.InteropServices;

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
}
