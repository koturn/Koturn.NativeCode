using System;


namespace NativeCodeSharp.Enums
{
    /// <summary>
    /// Flag enum for SIMD features
    /// </summary>
    [Flags]
    public enum CpuSimdFeatureFlag : int
    {
        /// <summary>
        /// A flag bit which indicates MMX is available or not.
        /// </summary>
        Mmx = 0x00000001,
        /// <summary>
        /// A flag bit which indicates SSE is available or not.
        /// </summary>
        Sse = 0x00000002,
        /// <summary>
        /// A flag bit which indicates SSE2 is available or not.
        /// </summary>
        Sse2 = 0x00000004,
        /// <summary>
        /// A flag bit which indicates SSE3 is available or not.
        /// </summary>
        Sse3 = 0x00000008,
        /// <summary>
        /// A flag bit which indicates SSSE3 is available or not.
        /// </summary>
        Ssse3 = 0x00000010,
        /// <summary>
        /// A flag bit which indicates SSE4.1 is available or not.
        /// </summary>
        Sse41 = 0x00000020,
        /// <summary>
        /// A flag bit which indicates SSE4.2 is available or not.
        /// </summary>
        Sse42 = 0x00000040,
        /// <summary>
        /// A flag bit which indicates AES is available or not.
        /// </summary>
        Aes = 0x00000080,
        /// <summary>
        /// A flag bit which indicates SSE4A is available or not.
        /// </summary>
        Sse4a = 0x00000100,
        /// <summary>
        /// A flag bit which indicates AVX is available or not.
        /// </summary>
        Avx = 0x00000200,
        /// <summary>
        /// A flag bit which indicates AVX2 is available or not.
        /// </summary>
        Avx2 = 0x00000400,
        /// <summary>
        /// A flag bit which indicates FMA is available or not.
        /// </summary>
        Fma = 0x00000800,
        /// <summary>
        /// A flag bit which indicates AVX-512F is available or not.
        /// </summary>
        Avx512F = 0x00001000,
        /// <summary>
        /// A flag bit which indicates AVX-512BW is available or not.
        /// </summary>
        Avx512Bw = 0x00002000,
        /// <summary>
        /// A flag bit which indicates AVX-512CD is available or not.
        /// </summary>
        Avx512Cd = 0x00004000,
        /// <summary>
        /// A flag bit which indicates AVX-512DQ is available or not.
        /// </summary>
        Avx512Dq = 0x00008000,
        /// <summary>
        /// A flag bit which indicates AVX-512ER is available or not.
        /// </summary>
        Avx512Er = 0x00010000,
        /// <summary>
        /// A flag bit which indicates AVX-512IFMA52 is available or not.
        /// </summary>
        Avx512Ifma52 = 0x00020000,
        /// <summary>
        /// A flag bit which indicates AVX-512PF is available or not.
        /// </summary>
        Avx512Pf = 0x00040000,
        /// <summary>
        /// A flag bit which indicates AVX-512VL is available or not.
        /// </summary>
        Avx512Vl = 0x00080000,
        /// <summary>
        /// A flag bit which indicates AVX-512POPCNTDQ is available or not.
        /// </summary>
        Avx512Vpopcntdq = 0x00100000,
        /// <summary>
        /// A flag bit which indicates AVX-512_4FMAPS is available or not.
        /// </summary>
        Avx512_4fmaps = 0x00200000,
        /// <summary>
        /// A flag bit which indicates AVX-512_4VNNIW is available or not.
        /// </summary>
        Avx512_4vnniw = 0x00400000,
        /// <summary>
        /// A flag bit which indicates AVX-512_BITALG is available or not.
        /// </summary>
        Avx512Bitalg = 0x00800000,
        /// <summary>
        /// A flag bit which indicates AVX-512_VBMI is available or not.
        /// </summary>
        Avx512Vbmi = 0x01000000,
        /// <summary>
        /// A flag bit which indicates AVX-512_VBMI2 is available or not.
        /// </summary>
        Avx512Vbmi2 = 0x02000000,
        /// <summary>
        /// A flag bit which indicates AVX-512_VNNI2 is available or not.
        /// </summary>
        Avx512Vnni = 0x04000000
    }
}
