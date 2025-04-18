using System.Runtime.InteropServices;


namespace NativeCodeSharp.Intrinsics
{
    /// <summary>
    /// Structure for cpuid
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = sizeof(uint))]
    public struct CpuInfo
    {
        /// <summary>
        /// Eax value which is output parameter for cpuid
        /// </summary>
        public uint Eax { get; }
        /// <summary>
        /// Eax value which is output parameter for cpuid
        /// </summary>
        public uint Ebx { get; }
        /// <summary>
        /// Ecx value which is putput parameter for cpuid
        /// </summary>
        public uint Ecx { get; }
        /// <summary>
        /// Edx value which is putput parameter for cpuid
        /// </summary>
        public uint Edx { get; }
        /// <summary>
        /// Stringify all member values.
        /// </summary>
        /// <returns>Stringified all member values.</returns>
        public override readonly string ToString()
        {
            return string.Format("(eax, ebx, ecx, edx) = (0x{0:X8}, 0x{1:X8}, 0x{2:X8}, 0x{3:X8})", Eax, Ebx, Ecx, Edx);
        }
    }
}
