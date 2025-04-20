Koturn.NativeCode
=================

[![Test status](https://ci.appveyor.com/api/projects/status/l60s1opgar23v9vq/branch/main?svg=true)](https://ci.appveyor.com/project/koturn/koturn-nativecode/branch/main)

Native code method generator for .NET / .NET Framework.


## Example

### Inner Product Calculation using SSE2

```cs
using System;
using System.Runtime.InteropServices;
using System.Security;

using Koturn.NativeCode;


namespace NativeCodeMain
{
    class Program
    {
        private const int ArraySize = 32;

        static void Main(string[] args)
        {
            var src1 = new float[ArraySize];
            var src2 = new float[ArraySize];
            for (int i = 0; i < ArraySize; i++)
            {
                src1[i] = i;
                src2[i] = i;
            }

            var dst = new float[ArraySize];

            using (var mh = CreateInnerProductMethodHandle())
            {
                mh.Method(dst, src1, src2, dst.Length);
            }

            foreach (var e in dst)
            {
                Console.Write($"{e} ");
            }
        }

        /// <summary>
        /// Calculate inner product of two <see cref="float"/> arrays.
        /// </summary>
        /// <param name="dst">Destination array</param>
        /// <param name="src1">First source array</param>
        /// <param name="src2">Second source array</param>
        /// <param name="count">Element size of <paramref name="dst"/>, <paramref name="src1"/> and <paramref name="src2"/></param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [SuppressUnmanagedCodeSecurity]
        private delegate void InnerProductDelegate(float[] dst, float[] src1, float[] src2, int count);

        /// <summary>
        /// Create native code method that calculate inner product of two <see cref="float"/> arrays.
        /// </summary>
        /// <returns>Native code method of inner product calculation</returns>
        private static NativeMethodHandle<InnerProductDelegate> CreateInnerProductMethodHandle()
        {
            // void __stdcall
            // inner_product(
            //     float* dst,
            //     const float* src1,
            //     const float* src2,
            //     int size) noexcept
            // {
            //     constexpr auto stride = static_cast<int>(sizeof(__m128) / sizeof(float));
            //
            //     for (int i = 0, im = size - stride; i <= im; i += stride) {
            //         _mm_storeu_ps(
            //             &dst[i],
            //             _mm_mul_ps(
            //                 _mm_loadu_ps(&src1[i]),
            //                 _mm_loadu_ps(&src2[i])));
            //     }
            //
            //     const auto remSize = size % stride;
            //     if (remSize == 0) {
            //         return;
            //     }
            //
            //     const auto offset = size - remSize;
            //     for (int i = offset; i < size; i++) {
            //         dst[i] = src1[i] * src2[i];
            //     }
            // }
            return NativeMethodHandle.Create<InnerProductDelegate>(Environment.Is64BitProcess ? new byte[]
                {
                    0x41, 0x83, 0xf9, 0x03,                    // cmp    r9d,0x3
                    0x7e, 0x33,                                // jle    L2
                    0x45, 0x8d, 0x51, 0xfc,                    // lea    r10d,[r9-0x4]
                    0x31, 0xc0,                                // xor    eax,eax
                    0x41, 0xc1, 0xea, 0x02,                    // shr    r10d,0x2
                    0x49, 0x83, 0xc2, 0x01,                    // add    r10,0x1
                    0x49, 0xc1, 0xe2, 0x04,                    // shl    r10,0x4
                    0x0f, 0x1f, 0x84, 0x00, 0x00, 0x00, 0x00,  // nop    DWORD PTR [rax+rax*1+0x0]
                    0x00,
                    // L1:
                    0x41, 0x0f, 0x10, 0x04, 0x00,              // movups xmm0,XMMWORD PTR [r8+rax*1]
                    0x0f, 0x10, 0x0c, 0x02,                    // movups xmm1,XMMWORD PTR [rdx+rax*1]
                    0x0f, 0x59, 0xc1,                          // mulps  xmm0,xmm1
                    0x0f, 0x11, 0x04, 0x01,                    // movups XMMWORD PTR [rcx+rax*1],xmm0
                    0x48, 0x83, 0xc0, 0x10,                    // add    rax,0x10
                    0x49, 0x39, 0xc2,                          // cmp    r10,rax
                    0x75, 0xe7,                                // jne    L1
                    // L2:
                    0x45, 0x89, 0xca,                          // mov    r10d,r9d
                    0x41, 0xc1, 0xfa, 0x1f,                    // sar    r10d,0x1f
                    0x41, 0xc1, 0xea, 0x1e,                    // shr    r10d,0x1e
                    0x43, 0x8d, 0x04, 0x11,                    // lea    eax,[r9+r10*1]
                    0x83, 0xe0, 0x03,                          // and    eax,0x3
                    0x44, 0x29, 0xd0,                          // sub    eax,r10d
                    0x74, 0x49,                                // je     L4
                    0x45, 0x89, 0xca,                          // mov    r10d,r9d
                    0x41, 0x29, 0xc2,                          // sub    r10d,eax
                    0x45, 0x39, 0xd1,                          // cmp    r9d,r10d
                    0x7e, 0x3e,                                // jle    L4
                    0x41, 0x83, 0xe9, 0x01,                    // sub    r9d,0x1
                    0x4d, 0x63, 0xda,                          // movsxd r11,r10d
                    0x45, 0x29, 0xd1,                          // sub    r9d,r10d
                    0x4a, 0x8d, 0x04, 0x9d, 0x00, 0x00, 0x00,  // lea    rax,[r11*4+0x0]
                    0x00,
                    0x4f, 0x8d, 0x4c, 0x0b, 0x01,              // lea    r9,[r11+r9*1+0x1]
                    0x49, 0xc1, 0xe1, 0x02,                    // shl    r9,0x2
                    0x66, 0x2e, 0x0f, 0x1f, 0x84, 0x00, 0x00,  // nop    WORD PTR cs:[rax+rax*1+0x0]
                    0x00, 0x00, 0x00,
                    // L3:
                    0xf3, 0x0f, 0x10, 0x04, 0x02,              // movss  xmm0,DWORD PTR [rdx+rax*1]
                    0xf3, 0x41, 0x0f, 0x59, 0x04, 0x00,        // mulss  xmm0,DWORD PTR [r8+rax*1]
                    0xf3, 0x0f, 0x11, 0x04, 0x01,              // movss  DWORD PTR [rcx+rax*1],xmm0
                    0x48, 0x83, 0xc0, 0x04,                    // add    rax,0x4
                    0x4c, 0x39, 0xc8,                          // cmp    rax,r9
                    0x75, 0xe7,                                // jne    L3
                    // L4:
                    0xc3,                                      // ret
                } : new byte[]
                {
                    0x55,                                      // push   ebp
                    0x89, 0xe5,                                // mov    ebp,esp
                    0x57,                                      // push   edi
                    0x56,                                      // push   esi
                    0x8b, 0x75, 0x14,                          // mov    esi,DWORD PTR [ebp+0x14]
                    0x53,                                      // push   ebx
                    0x8b, 0x55, 0x08,                          // mov    edx,DWORD PTR [ebp+0x8]
                    0x8b, 0x5d, 0x0c,                          // mov    ebx,DWORD PTR [ebp+0xc]
                    0x83, 0xe4, 0xf0,                          // and    esp,0xfffffff0
                    0x8b, 0x4d, 0x10,                          // mov    ecx,DWORD PTR [ebp+0x10]
                    0x83, 0xfe, 0x03,                          // cmp    esi,0x3
                    0x7e, 0x21,                                // jle    L2
                    0x8d, 0x7e, 0xfc,                          // lea    edi,[esi-0x4]
                    0x31, 0xc0,                                // xor    eax,eax
                    0x83, 0xe7, 0xfc,                          // and    edi,0xfffffffc
                    0x83, 0xc7, 0x04,                          // add    edi,0x4
                    // L1:
                    0x0f, 0x10, 0x04, 0x81,                    // movups xmm0,XMMWORD PTR [ecx+eax*4]
                    0x0f, 0x10, 0x0c, 0x83,                    // movups xmm1,XMMWORD PTR [ebx+eax*4]
                    0x0f, 0x59, 0xc1,                          // mulps  xmm0,xmm1
                    0x0f, 0x11, 0x04, 0x82,                    // movups XMMWORD PTR [edx+eax*4],xmm0
                    0x83, 0xc0, 0x04,                          // add    eax,0x4
                    0x39, 0xf8,                                // cmp    eax,edi
                    0x75, 0xea,                                // jne    L1
                    // L2:
                    0x89, 0xf7,                                // mov    edi,esi
                    0xc1, 0xff, 0x1f,                          // sar    edi,0x1f
                    0xc1, 0xef, 0x1e,                          // shr    edi,0x1e
                    0x8d, 0x04, 0x3e,                          // lea    eax,[esi+edi*1]
                    0x83, 0xe0, 0x03,                          // and    eax,0x3
                    0x29, 0xf8,                                // sub    eax,edi
                    0x74, 0x2a,                                // je     L4
                    0x89, 0xf7,                                // mov    edi,esi
                    0x29, 0xc7,                                // sub    edi,eax
                    0x39, 0xfe,                                // cmp    esi,edi
                    0x7e, 0x22,                                // jle    L4
                    0xc1, 0xe7, 0x02,                          // shl    edi,0x2
                    0x8d, 0x04, 0x3b,                          // lea    eax,[ebx+edi*1]
                    0x8d, 0x1c, 0xb3,                          // lea    ebx,[ebx+esi*4]
                    0x01, 0xf9,                                // add    ecx,edi
                    0x01, 0xfa,                                // add    edx,edi
                    // L3:
                    0xd9, 0x00,                                // fld    DWORD PTR [eax]
                    0x83, 0xc0, 0x04,                          // add    eax,0x4
                    0x83, 0xc1, 0x04,                          // add    ecx,0x4
                    0x83, 0xc2, 0x04,                          // add    edx,0x4
                    0xd8, 0x49, 0xfc,                          // fmul   DWORD PTR [ecx-0x4]
                    0xd9, 0x5a, 0xfc,                          // fstp   DWORD PTR [edx-0x4]
                    0x39, 0xd8,                                // cmp    eax,ebx
                    0x75, 0xeb,                                // jne    L3
                    // L4:
                    0x8d, 0x65, 0xf4,                          // lea    esp,[ebp-0xc]
                    0x5b,                                      // pop    ebx
                    0x5e,                                      // pop    esi
                    0x5f,                                      // pop    edi
                    0x5d,                                      // pop    ebp
                    0xc2, 0x10, 0x00,                          // ret    0x10
                });
        }
    }
}
```


## Build

```shell
> nmake restore
> nmake
```

## Deploy

```shell
> nmake deploy
```


## License

This software is released under the MIT License, see [LICENSE](LICENSE).
