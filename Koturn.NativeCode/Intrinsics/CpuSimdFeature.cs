using System;
using System.Collections.Generic;
using Koturn.NativeCode.Enums;


namespace Koturn.NativeCode.Intrinsics
{
    /// <summary>
    /// Structure with information on SIMD instructions available on CPU.
    /// </summary>
    public struct CpuSimdFeature
        : ICloneable, IEquatable<CpuSimdFeature>
    {
        #region Private variables
        /// <summary>
        /// SIMD flag value.
        /// </summary>
        private CpuSimdFeatureFlag _simdFlagValue;
        #endregion

        #region Properties
        /// <summary>
        /// MMX is available or not.
        /// </summary>
        public bool HasMmx
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Mmx) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Mmx) : (_simdFlagValue & ~CpuSimdFeatureFlag.Mmx);
        }
        /// <summary>
        /// SSE is available or not.
        /// </summary>
        public bool HasSse
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Sse) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Sse) : (_simdFlagValue & ~CpuSimdFeatureFlag.Sse);
        }
        /// <summary>
        /// SSE2 is available or not.
        /// </summary>
        public bool HasSse2
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Sse2) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Sse2) : (_simdFlagValue & ~CpuSimdFeatureFlag.Sse2);
        }
        /// <summary>
        /// SSE3 is available or not.
        /// </summary>
        public bool HasSse3
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Sse3) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Sse3) : (_simdFlagValue & ~CpuSimdFeatureFlag.Sse3);
        }
        /// <summary>
        /// SSSE3 is available or not.
        /// </summary>
        public bool HasSsse3
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Ssse3) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Ssse3) : (_simdFlagValue & ~CpuSimdFeatureFlag.Ssse3);
        }
        /// <summary>
        /// SSE4.1 is available or not.
        /// </summary>
        public bool HasSse41
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Sse41) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Sse41) : (_simdFlagValue & ~CpuSimdFeatureFlag.Sse41);
        }
        /// <summary>
        /// SSE4.2 is available or not.
        /// </summary>
        public bool HasSse42
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Sse42) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Sse42) : (_simdFlagValue & ~CpuSimdFeatureFlag.Sse42);
        }
        /// <summary>
        /// AES is available or not.
        /// </summary>
        public bool HasAes
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Aes) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Aes) : (_simdFlagValue & ~CpuSimdFeatureFlag.Aes);
        }
        /// <summary>
        /// SSE4A is available or not.
        /// </summary>
        public bool HasSse4a
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Sse4a) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Sse4a) : (_simdFlagValue & ~CpuSimdFeatureFlag.Sse4a);
        }
        /// <summary>
        /// AVX is available or not.
        /// </summary>
        public bool HasAvx
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx);
        }
        /// <summary>
        /// AVX2 is available or not.
        /// </summary>
        public bool HasAvx2
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx2) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx2) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx2);
        }
        /// <summary>
        /// FMA is available or not.
        /// </summary>
        public bool HasFma
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Fma) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Fma) : (_simdFlagValue & ~CpuSimdFeatureFlag.Fma);
        }
        /// <summary>
        /// AVX-512F is available or not.
        /// </summary>
        public bool HasAvx512F
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512F) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512F) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512F);
        }
        /// <summary>
        /// AVX-512BW is available or not.
        /// </summary>
        public bool HasAvx512Bw
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Bw) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Bw) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Bw);
        }
        /// <summary>
        /// AVX-512CD is available or not.
        /// </summary>
        public bool HasAvx512Cd
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Cd) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Cd) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Cd);
        }
        /// <summary>
        /// AVX-512DQ is available or not.
        /// </summary>
        public bool HasAvx512Dq
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Dq) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Dq) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Dq);
        }
        /// <summary>
        /// AVX-512ER is available or not.
        /// </summary>
        public bool HasAvx512Er
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Er) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Er) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Er);
        }
        /// <summary>
        /// AVX-512IFMA52 is available or not.
        /// </summary>
        public bool HasAvx512Ifma52
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Ifma52) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Ifma52) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Ifma52);
        }
        /// <summary>
        /// AVX-512PF is available or not.
        /// </summary>
        public bool HasAvx512Pf
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Pf) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Pf) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Pf);
        }
        /// <summary>
        /// AVX-512VL is available or not.
        /// </summary>
        public bool HasAvx512Vl
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Vl) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Vl) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Vl);
        }
        /// <summary>
        /// AVX-512POPCNTDQ is available or not.
        /// </summary>
        public bool HasAvx512Vpopcntdq
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Vpopcntdq) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Vpopcntdq) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Vpopcntdq);
        }
        /// <summary>
        /// AVX-512_4FMAPS is available or not.
        /// </summary>
        public bool HasAvx512_4fmaps
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512_4fmaps) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512_4fmaps) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512_4fmaps);
        }
        /// <summary>
        /// AVX-512_4VNNIW is available or not.
        /// </summary>
        public bool HasAvx512_4vnniw
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512_4vnniw) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512_4vnniw) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512_4vnniw);
        }
        /// <summary>
        /// AVX-512_BITALG is available or not.
        /// </summary>
        public bool HasAvx512Bitalg
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Bitalg) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Bitalg) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Bitalg);
        }
        /// <summary>
        /// AVX-512_VBMI is available or not.
        /// </summary>
        public bool HasAvx512Vbmi
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Vbmi) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Vbmi) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Vbmi);
        }
        /// <summary>
        /// AVX-512_VBMI2 is available or not.
        /// </summary>
        public bool HasAvx512Vbmi2
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Vbmi2) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Vbmi2) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Vbmi2);
        }
        /// <summary>
        /// AVX-512_VNNI2 is available or not.
        /// </summary>
        public bool HasAvx512Vnni
        {
            readonly get => (_simdFlagValue & CpuSimdFeatureFlag.Avx512Vnni) != 0;
            internal set => _simdFlagValue = value ? (_simdFlagValue | CpuSimdFeatureFlag.Avx512Vnni) : (_simdFlagValue & ~CpuSimdFeatureFlag.Avx512Vnni);
        }
        #endregion

        #region public methods
        /// <summary>
        /// Get supported commma-separated SIMD instruction string.
        /// </summary>
        /// <returns>Supported commma-separated SIMD instruction string, ex) "MMX, SSE, SSE2".</returns>
        public override readonly string ToString()
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

        /// <summary>
        /// Check if given object is equivalent to this object.
        /// </summary>
        /// <param name="obj">An object to compare</param>
        /// <returns>Return <c>true</c> if <paramref name="obj"/> is an object equivalent to this object, otherwise <c>false</c>.</returns>
        public override readonly bool Equals(object? obj)
        {
            return obj != null && obj is CpuSimdFeature feature && Equals(feature);
        }

        /// <summary>
        /// Compure hash code of this object.
        /// </summary>
        /// <returns>The has code of this object.</returns>
        public override readonly int GetHashCode()
        {
            return _simdFlagValue.GetHashCode();
        }

        /// <summary>
        /// Check if given object is equivalent to this object.
        /// </summary>
        /// <param name="obj">An object to compare</param>
        /// <returns>Return <c>true</c> if <paramref name="obj"/> is an object equivalent to this object, otherwise <c>false</c>.</returns>
        public readonly bool Equals(CpuSimdFeature obj)
        {
            return _simdFlagValue == obj._simdFlagValue;
        }

        /// <summary>
        /// Clone this object.
        /// </summary>
        /// <returns>A clone of this object.</returns>
        public object Clone()
        {
            return new CpuSimdFeature()
            {
                _simdFlagValue = _simdFlagValue
            };
        }

        /// <summary>
        /// Check if LHS is equivalent to RHS.
        /// </summary>
        /// <param name="left">LHS.</param>
        /// <param name="right">RHS.</param>
        /// <returns>True if LHS is equivalent to RHS, otherwise false.</returns>
        public static bool operator ==(CpuSimdFeature left, CpuSimdFeature right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Check if LHS is not equivalent to RHS.
        /// </summary>
        /// <param name="left">LHS.</param>
        /// <param name="right">RHS.</param>
        /// <returns>True if LHS is not equivalent to RHS, otherwise false.</returns>
        public static bool operator !=(CpuSimdFeature left, CpuSimdFeature right)
        {
            return !(left == right);
        }
        #endregion
    }
}
