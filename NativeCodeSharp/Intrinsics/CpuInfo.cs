using System.Runtime.InteropServices;


namespace NativeCodeSharp.Intrinsics
{
    /// <summary>
    /// Structure for cpuid
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct CpuInfo
    {
        /// <summary>
        /// Eax value which is Input/Output parameter for cpuid
        /// </summary>
        public uint Eax { get; set; }
        /// <summary>
        /// Eax value which is Input/Output parameter for cpuid
        /// </summary>
        public uint Ebx { get; set; }
        /// <summary>
        /// Ecx value which is Input/Output parameter for cpuid
        /// </summary>
        public uint Ecx { get; set; }
        /// <summary>
        /// Edx value which is Input/Output parameter for cpuid
        /// </summary>
        public uint Edx { get; set; }
        /// <summary>
        /// Stringify all member values.
        /// </summary>
        /// <returns>Stringified all member values.</returns>
        public override string ToString()
        {
            return string.Format("(eax, ebx, ecx, edx) = (0x{0:X8}, 0x{1:X8}, 0x{2:X8}, 0x{3:X8})", Eax, Ebx, Ecx, Edx);
        }
    }
}
