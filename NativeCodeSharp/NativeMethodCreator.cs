using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using NativeCodeSharp.Exceptions;
using NativeCodeSharp.Internal.Win32;


namespace NativeCodeSharp
{
    /// <summary>
    /// This class provides utility function to dynamic native code generating methods.
    /// </summary>
    public static class NativeMethodCreator
    {
        /// <summary>
        /// Create native code delegate and wrap it into <see cref="NativeMethodHandle{TDelegate}"/> from given machine code.
        /// </summary>
        /// <typeparam name="TDelegate">Method delegate type</typeparam>
        /// <param name="code">Native code</param>
        /// <returns><see cref="NativeMethodHandle{TDelegate}"/> instance that includes native code.</returns>
        /// <exception cref="ArgumentNullException">Throw when <paramref name="code"/> is null.</exception>
        /// <exception cref="MemoryOperationException">Throw when memory operation methods returns error.</exception>
        public static NativeMethodHandle<TDelegate> CreateMethodHandle<TDelegate>(byte[] code)
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
                new UIntPtr((uint)code.Length)))
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
        public static NativeMethodHandle<TDelegate> CreateMethodHandle<TDelegate>(IEnumerable<byte> codeQuery)
            where TDelegate : Delegate
        {
            if (codeQuery == null)
            {
                ThrowArgumentNullException("codeQuery");
            }
            return CreateMethodHandle<TDelegate>(codeQuery.ToArray());
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
