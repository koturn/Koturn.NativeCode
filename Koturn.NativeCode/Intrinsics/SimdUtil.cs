namespace Koturn.NativeCode.Intrinsics
{
    /// <summary>
    /// Utility class for SIMD intructions
    /// </summary>
    public static class SimdUtil
    {
        /// <summary>
        /// Get the structure of the list of SIMD instructions supported by the CPU.
        /// </summary>
        /// <returns>Structure of the list of SIMD instructions</returns>
        public static CpuSimdFeature GetCpuSimdFeature()
        {
            using (var cpuIdMethodHandle = Intrinsic.CreateCpuIdMethodHandle())
            {
                cpuIdMethodHandle.Method(out var cpuInfo, 1);

                var cpuFeature = default(CpuSimdFeature);
                cpuFeature.HasMmx = (cpuInfo.Edx & (1U << 23)) != 0U;
                cpuFeature.HasSse = (cpuInfo.Edx & (1U << 25)) != 0U;
                cpuFeature.HasSse2 = (cpuInfo.Edx & (1U << 26)) != 0U;
                cpuFeature.HasSse3 = (cpuInfo.Ecx & (1U << 0)) != 0U;
                cpuFeature.HasSsse3 = (cpuInfo.Ecx & (1U << 9)) != 0U;
                cpuFeature.HasFma = (cpuInfo.Ecx & (1U << 12)) != 0U;
                cpuFeature.HasSse41 = (cpuInfo.Ecx & (1U << 19)) != 0U;
                cpuFeature.HasSse42 = (cpuInfo.Ecx & (1U << 20)) != 0U;
                cpuFeature.HasAes = (cpuInfo.Ecx & (1U << 25)) != 0U;
                cpuFeature.HasAvx = (cpuInfo.Ecx & (1U << 28)) != 0U;

                cpuIdMethodHandle.Method(out cpuInfo, 7);
                cpuFeature.HasAvx2 = (cpuInfo.Ebx & (1U << 5)) != 0U;

                // AVX-512
                cpuFeature.HasAvx512F = (cpuInfo.Ebx & (1U << 16)) != 0U;
                cpuFeature.HasAvx512Bw = (cpuInfo.Ebx & (1U << 30)) != 0U;
                cpuFeature.HasAvx512Cd = (cpuInfo.Ebx & (1U << 28)) != 0U;
                cpuFeature.HasAvx512Dq = (cpuInfo.Ebx & (1U << 17)) != 0U;
                cpuFeature.HasAvx512Er = (cpuInfo.Ebx & (1U << 27)) != 0U;
                cpuFeature.HasAvx512Ifma52 = (cpuInfo.Ebx & (1U << 21)) != 0U;
                cpuFeature.HasAvx512Pf = (cpuInfo.Ebx & (1U << 26)) != 0U;
                cpuFeature.HasAvx512Vl = (cpuInfo.Ebx & (1U << 31)) != 0U;
                cpuFeature.HasAvx512_4fmaps = (cpuInfo.Edx & (1U << 2)) != 0U;
                cpuFeature.HasAvx512_4vnniw = (cpuInfo.Edx & (1U << 3)) != 0U;
                cpuFeature.HasAvx512Bitalg = (cpuInfo.Ecx & (1U << 12)) != 0U;
                cpuFeature.HasAvx512Vpopcntdq = (cpuInfo.Ecx & (1U << 14)) != 0U;
                cpuFeature.HasAvx512Vbmi = (cpuInfo.Ecx & (1U << 1)) != 0U;
                cpuFeature.HasAvx512Vbmi2 = (cpuInfo.Ecx & (1U << 6)) != 0U;
                cpuFeature.HasAvx512Vnni = (cpuInfo.Ecx & (1U << 11)) != 0U;

                cpuIdMethodHandle.Method(out cpuInfo, 0x80000000);
                if (cpuInfo.Eax >= 0x80000001U)
                {
                    cpuIdMethodHandle.Method(out cpuInfo, 0x80000001);
                    cpuFeature.HasSse4a = (cpuInfo.Ecx & (1U << 6)) != 0U;
                }

                return cpuFeature;
            }
        }
    }
}
