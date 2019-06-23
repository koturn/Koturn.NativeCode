using System.Collections.Generic;


namespace NativeCodeSharp.Intrinsics
{
    /// <summary>
    /// Structure with information on SIMD instructions available on CPU.
    /// </summary>
    public struct CpuSimdFeature
    {
        /// <summary>
        /// MMX is available or not.
        /// </summary>
        public bool HasMmx { get; internal set; }
        /// <summary>
        /// SSE is available or not.
        /// </summary>
        public bool HasSse { get; internal set; }
        /// <summary>
        /// SSE2 is available or not.
        /// </summary>
        public bool HasSse2 { get; internal set; }
        /// <summary>
        /// SSE3 is available or not.
        /// </summary>
        public bool HasSse3 { get; internal set; }
        /// <summary>
        /// SSSE3 is available or not.
        /// </summary>
        public bool HasSsse3 { get; internal set; }
        /// <summary>
        /// SSE4.1 is available or not.
        /// </summary>
        public bool HasSse41 { get; internal set; }
        /// <summary>
        /// SSE4.2 is available or not.
        /// </summary>
        public bool HasSse42 { get; internal set; }
        /// <summary>
        /// AES is available or not.
        /// </summary>
        public bool HasAes { get; internal set; }
        /// <summary>
        /// SSE4A is available or not.
        /// </summary>
        public bool HasSse4a { get; internal set; }
        /// <summary>
        /// AVX is available or not.
        /// </summary>
        public bool HasAvx { get; internal set; }
        /// <summary>
        /// AVX2 is available or not.
        /// </summary>
        public bool HasAvx2 { get; internal set; }
        /// <summary>
        /// FMA is available or not.
        /// </summary>
        public bool HasFma { get; internal set; }
        /// <summary>
        /// AVX-512F is available or not.
        /// </summary>
        public bool HasAvx512F { get; internal set; }
        /// <summary>
        /// AVX-512BW is available or not.
        /// </summary>
        public bool HasAvx512Bw { get; internal set; }
        /// <summary>
        /// AVX-512CD is available or not.
        /// </summary>
        public bool HasAvx512Cd { get; internal set; }
        /// <summary>
        /// AVX-512DQ is available or not.
        /// </summary>
        public bool HasAvx512Dq { get; internal set; }
        /// <summary>
        /// AVX-512ER is available or not.
        /// </summary>
        public bool HasAvx512Er { get; internal set; }
        /// <summary>
        /// AVX-512IFMA52 is available or not.
        /// </summary>
        public bool HasAvx512Ifma52 { get; internal set; }
        /// <summary>
        /// AVX-512PF is available or not.
        /// </summary>
        public bool HasAvx512Pf { get; internal set; }
        /// <summary>
        /// AVX-512VL is available or not.
        /// </summary>
        public bool HasAvx512Vl { get; internal set; }
        /// <summary>
        /// AVX-512POPCNTDQ is available or not.
        /// </summary>
        public bool HasAvx512Vpopcntdq { get; internal set; }
        /// <summary>
        /// AVX-512_4FMAPS is available or not.
        /// </summary>
        public bool HasAvx512_4fmaps { get; internal set; }
        /// <summary>
        /// AVX-512_4VNNIW is available or not.
        /// </summary>
        public bool HasAvx512_4vnniw { get; internal set; }
        /// <summary>
        /// AVX-512_BITALG is available or not.
        /// </summary>
        public bool HasAvx512Bitalg { get; internal set; }
        /// <summary>
        /// AVX-512_VBMI is available or not.
        /// </summary>
        public bool HasAvx512Vbmi { get; internal set; }
        /// <summary>
        /// AVX-512_VBMI2 is available or not.
        /// </summary>
        public bool HasAvx512Vbmi2 { get; internal set; }
        /// <summary>
        /// AVX-512_VNNI2 is available or not.
        /// </summary>
        public bool HasAvx512Vnni { get; internal set; }

        /// <summary>
        /// Get supported commma-separated SIMD instruction string.
        /// </summary>
        /// <returns>Supported commma-separated SIMD instruction string, ex) "MMX, SSE, SSE2".</returns>
        public override string ToString()
        {
            var list = new List<string>(27);

            if (HasMmx) list.Add("MMX");
            if (HasSse) list.Add("SSE");
            if (HasSse2) list.Add("SSE2");
            if (HasSse3) list.Add("SSE3");
            if (HasSsse3) list.Add("SSSE3");
            if (HasAes) list.Add("AES");
            if (HasSse41) list.Add("SSE4.1");
            if (HasSse42) list.Add("SSE4.2");
            if (HasSse4a) list.Add("SSE4A");
            if (HasAvx) list.Add("AVX");
            if (HasAvx2) list.Add("AVX2");
            if (HasFma) list.Add("FMA");
            if (HasAvx512F) list.Add("AVX-512F");
            if (HasAvx512Bw) list.Add("AVX-512BW");
            if (HasAvx512Cd) list.Add("AVX-512CD");
            if (HasAvx512Dq) list.Add("AVX-512DQ");
            if (HasAvx512Er) list.Add("AVX-512ER");
            if (HasAvx512Ifma52) list.Add("AVX-512IFMA52");
            if (HasAvx512Pf) list.Add("AVX-512PF");
            if (HasAvx512Vl) list.Add("AVX-512VL");
            if (HasAvx512Vpopcntdq) list.Add("AVX-512VPOPCNTDQ");
            if (HasAvx512_4fmaps) list.Add("AVX-512_4FMAPS");
            if (HasAvx512_4vnniw) list.Add("AVX-512_4VNNIW");
            if (HasAvx512Bitalg) list.Add("AVX-512_BITALG");
            if (HasAvx512Vbmi) list.Add("AVX-512_VBMI");
            if (HasAvx512Vbmi2) list.Add("AVX-512_VBMI2");
            if (HasAvx512Vnni) list.Add("AVX-512_VNNI");

            return string.Join(", ", list);
        }
    }
}
