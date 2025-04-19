namespace NativeCodeSharp.Internals.Win32.Enums
{
    /// <summary>
    /// Enum which represents processor architecture.
    /// </summary>
    internal enum ProcessorArchitecture : ushort
    {
        /// <summary>
        /// x64 (AMD or Intel).
        /// </summary>
        Amd64 = 9,
        /// <summary>
        /// ARM
        /// </summary>
        Arm = 5,
        /// <summary>
        /// ARM64
        /// </summary>
        Arm64 = 12,
        /// <summary>
        /// Intel Itanium-based.
        /// </summary>
        Ia64 = 6,
        /// <summary>
        /// x86
        /// </summary>
        Intel = 0,
        /// <summary>
        /// Unknown architecture.
        /// </summary>
        Unknown = 0xffff
    }
}
