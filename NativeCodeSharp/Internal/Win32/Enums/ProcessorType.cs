namespace NativeCodeSharp.Internal.Win32.Enums
{
    /// <summary>
    /// Enum which represents processor architecture value.
    /// </summary>
    internal enum ProcessorType : uint
    {
        /// <summary>
        /// Intel 386.
        /// </summary>
        Intel386 = 386,
        /// <summary>
        /// Intel 486.
        /// </summary>
        Intel486 = 486,
        /// <summary>
        /// Intel Pentium.
        /// </summary>
        IntelPentium = 586,
        /// <summary>
        /// Intel IA-64.
        /// </summary>
        IntelIa64 = 2200,
        /// <summary>
        /// AMD x86/x64.
        /// </summary>
        AmdX8664 = 8664,
    }
}
