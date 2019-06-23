using System;
using System.Security;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NativeCodeSharp;


namespace NativeCodeSharpTest
{
    [TestClass]
    public class NativeMethodCreatorTest
    {
        /// <summary>
        /// Inner Product Example
        /// </summary>
        [TestMethod]
        public void Create01()
        {
            var a = new float[64];
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = i;
            }
            var b = new float[64];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = i;
            }

            var c = new float[64];

            using (var mh = CreateInnerProductMethodHandle())
            {
                unsafe
                {
                    fixed (float* pc = c)
                    fixed (float* pa = a)
                    fixed (float* pb = b)
                    {
                        mh.Method(pc, pa, pb, c.Length);
                    }
                }
            }

            foreach (var e in c)
            {
                Console.Write($"{e} ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Calculate inner product.
        /// </summary>
        /// <param name="dst">Destination array</param>
        /// <param name="src1">First source array</param>
        /// <param name="src2">Second source array</param>
        /// <param name="count">Size of <paramref name="dst"/>, <paramref name="src1"/> and <paramref name="src2"/>.</param>
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [SuppressUnmanagedCodeSecurity]
        private unsafe delegate void InnerProductDelegate(float* dst, float* src1, float* src2, int count);

        /// <summary>
        /// Create native method handle that calculate inner product.
        /// </summary>
        /// <returns>Native method handle of <see cref="InnerProductDelegate"/></returns>
        private static NativeMethodHandle<InnerProductDelegate> CreateInnerProductMethodHandle()
        {
            return NativeMethodCreator.CreateMethodHandle<InnerProductDelegate>(Environment.Is64BitProcess
                ? new byte[]
                {
                    0x48, 0x89, 0x5c, 0x24, 0x08,              // mov         qword ptr [rsp+8],rbx
                    0x48, 0x89, 0x7c, 0x24, 0x10,              // mov         qword ptr [rsp+10h],rdi
                    0x41, 0x8d, 0x41, 0xfc,                    // lea         eax,[r9-4]
                    0x4c, 0x8b, 0xd2,                          // mov         r10,rdx
                    0x48, 0x63, 0xd0,                          // movsxd      rdx,eax
                    0x49, 0x8b, 0xf8,                          // mov         rdi,r8
                    0x4c, 0x8b, 0xd9,                          // mov         r11,rcx
                    0x85, 0xc0,                                // test        eax,eax
                    0x78, 0x3b,                                // js          L2
                    0x49, 0x8b, 0xca,                          // mov         rcx,r10
                    0x49, 0x8b, 0xdb,                          // mov         rbx,r11
                    0x49, 0x2b, 0xc8,                          // sub         rcx,r8
                    0x49, 0x2b, 0xd8,                          // sub         rbx,r8
                    0x48, 0x83, 0xc2, 0x04,                    // add         rdx,4
                    0x49, 0x8b, 0xc0,                          // mov         rax,r8
                    0x48, 0xc1, 0xea, 0x02,                    // shr         rdx,2
                    0x66, 0x66, 0x66, 0x0f, 0x1f, 0x84,        // nop         word ptr [rax+rax]
                    0x00, 0x00, 0x00, 0x00, 0x00,
                    // :L1
                    0x0f, 0x10, 0x0c, 0x01,                    // movups      xmm1,xmmword ptr [rcx+rax]
                    0x0f, 0x10, 0x00,                          // movups      xmm0,xmmword ptr [rax]
                    0x48, 0x8d, 0x40, 0x10,                    // lea         rax,[rax+10h]
                    0x0f, 0x59, 0xc8,                          // mulps       xmm1,xmm0
                    0x0f, 0x11, 0x4c, 0x03, 0xf0,              // movups      xmmword ptr [rbx+rax-10h],xmm1
                    0x48, 0x83, 0xea, 0x01,                    // sub         rdx,1
                    0x75, 0xe7,                                // jne         L1
                    // :L2
                    0x49, 0x63, 0xc9,                          // movsxd      rcx,r9d
                    0x48, 0x8b, 0xc1,                          // mov         rax,rcx
                    0x83, 0xe0, 0x03,                          // and         eax,3
                    0x0f, 0x84, 0xc7, 0x00, 0x00, 0x00,        // je          L6
                    0x44, 0x2b, 0xc8,                          // sub         r9d,eax
                    0x4d, 0x63, 0xc9,                          // movsxd      r9,r9d
                    0x4c, 0x3b, 0xc9,                          // cmp         r9,rcx
                    0x0f, 0x8d, 0xb8, 0x00, 0x00, 0x00,        // jge         L6
                    0x48, 0x8b, 0xc1,                          // mov         rax,rcx
                    0x49, 0x2b, 0xc1,                          // sub         rax,r9
                    0x48, 0x83, 0xf8, 0x04,                    // cmp         rax,4
                    0x0f, 0x8c, 0x7b, 0x00, 0x00, 0x00,        // jl          L4
                    0x49, 0x8d, 0x40, 0x04,                    // lea         rax,[r8+4]
                    0x48, 0x8b, 0xd9,                          // mov         rbx,rcx
                    0x49, 0x2b, 0xd9,                          // sub         rbx,r9
                    0x4a, 0x8d, 0x04, 0x88,                    // lea         rax,[rax+r9*4]
                    0x48, 0x83, 0xeb, 0x04,                    // sub         rbx,4
                    0x49, 0x8b, 0xd2,                          // mov         rdx,r10
                    0x4d, 0x8b, 0xc3,                          // mov         r8,r11
                    0x48, 0xc1, 0xeb, 0x02,                    // shr         rbx,2
                    0x48, 0x2b, 0xd7,                          // sub         rdx,rdi
                    0x4c, 0x2b, 0xc7,                          // sub         r8,rdi
                    0x48, 0xff, 0xc3,                          // inc         rbx
                    0x4d, 0x8d, 0x0c, 0x99,                    // lea         r9,[r9+rbx*4]
                    // :L3
                    0xf3, 0x0f, 0x10, 0x44, 0x02, 0xfc,        // movss       xmm0,dword ptr [rdx+rax-4]
                    0xf3, 0x0f, 0x59, 0x40, 0xfc,              // mulss       xmm0,dword ptr [rax-4]
                    0x48, 0x8d, 0x40, 0x10,                    // lea         rax,[rax+10h]
                    0xf3, 0x41, 0x0f, 0x11, 0x44, 0x00, 0xec,  // movss       dword ptr [r8+rax-14h],xmm0
                    0xf3, 0x0f, 0x10, 0x4c, 0x10, 0xf0,        // movss       xmm1,dword ptr [rax+rdx-10h]
                    0xf3, 0x0f, 0x59, 0x48, 0xf0,              // mulss       xmm1,dword ptr [rax-10h]
                    0xf3, 0x42, 0x0f, 0x11, 0x4c, 0x00, 0xf0,  // movss       dword ptr [rax+r8-10h],xmm1
                    0xf3, 0x0f, 0x10, 0x44, 0x02, 0xf4,        // movss       xmm0,dword ptr [rdx+rax-0Ch]
                    0xf3, 0x0f, 0x59, 0x40, 0xf4,              // mulss       xmm0,dword ptr [rax-0Ch]
                    0xf3, 0x41, 0x0f, 0x11, 0x44, 0x00, 0xf4,  // movss       dword ptr [r8+rax-0Ch],xmm0
                    0xf3, 0x0f, 0x10, 0x4c, 0x02, 0xf8,        // movss       xmm1,dword ptr [rdx+rax-8]
                    0xf3, 0x0f, 0x59, 0x48, 0xf8,              // mulss       xmm1,dword ptr [rax-8]
                    0xf3, 0x41, 0x0f, 0x11, 0x4c, 0x00, 0xf8,  // movss       dword ptr [r8+rax-8],xmm1
                    0x48, 0x83, 0xeb, 0x01,                    // sub         rbx,1
                    0x75, 0xae,                                // jne         L3
                    // :L4
                    0x4c, 0x3b, 0xc9,                          // cmp         r9,rcx
                    0x7d, 0x28,                                // jge         L6
                    0x4c, 0x2b, 0xd7,                          // sub         r10,rdi
                    0x4a, 0x8d, 0x04, 0x8f,                    // lea         rax,[rdi+r9*4]
                    0x4c, 0x2b, 0xdf,                          // sub         r11,rdi
                    0x49, 0x2b, 0xc9,                          // sub         rcx,r9
                    // :L5
                    0xf3, 0x41, 0x0f, 0x10, 0x04, 0x02,        // movss       xmm0,dword ptr [r10+rax]
                    0xf3, 0x0f, 0x59, 0x00,                    // mulss       xmm0,dword ptr [rax]
                    0x48, 0x8d, 0x40, 0x04,                    // lea         rax,[rax+4]
                    0xf3, 0x41, 0x0f, 0x11, 0x44, 0x03, 0xfc,  // movss       dword ptr [r11+rax-4],xmm0
                    0x48, 0x83, 0xe9, 0x01,                    // sub         rcx,1
                    0x75, 0xe5,                                // jne         L5
                    // :L6
                    0x48, 0x8b, 0x5c, 0x24, 0x08,              // mov         rbx,qword ptr [rsp+8]
                    0x48, 0x8b, 0x7c, 0x24, 0x10,              // mov         rdi,qword ptr [rsp+10h]
                    0xc3,                                      // ret
                } : new byte[]
                {
                    0x53,                                      // push        ebx
                    0x8b, 0xdc,                                // mov         ebx,esp
                    0x83, 0xec, 0x08,                          // sub         esp,8
                    0x83, 0xe4, 0xf8,                          // and         esp,0FFFFFFF8h
                    0x83, 0xc4, 0x04,                          // add         esp,4
                    0x55,                                      // push        ebp
                    0x8b, 0x6b, 0x04,                          // mov         ebp,dword ptr [ebx+4]
                    0x89, 0x6c, 0x24, 0x04,                    // mov         dword ptr [esp+4],ebp
                    0x8b, 0xec,                                // mov         ebp,esp
                    0x83, 0xec, 0x10,                          // sub         esp,10h
                    0x56,                                      // push        esi
                    0x57,                                      // push        edi
                    0x8b, 0x7b, 0x0c,                          // mov         edi,dword ptr [ebx+0Ch]
                    0x8b, 0xf1,                                // mov         esi,ecx
                    0x89, 0x75, 0xf4,                          // mov         dword ptr [ebp-0Ch],esi
                    0x8d, 0x47, 0xfc,                          // lea         eax,[edi-4]
                    0x89, 0x45, 0xfc,                          // mov         dword ptr [ebp-4],eax
                    0x85, 0xc0,                                // test        eax,eax
                    0x78, 0x42,                                // js          L2
                    0x8b, 0x4b, 0x08,                          // mov         ecx,dword ptr [ebx+8]
                    0x8b, 0xfe,                                // mov         edi,esi
                    0x2b, 0xca,                                // sub         ecx,edx
                    0x2b, 0xfa,                                // sub         edi,edx
                    0x89, 0x4d, 0xf8,                          // mov         dword ptr [ebp-8],ecx
                    0x8b, 0xc2,                                // mov         eax,edx
                    0x8b, 0x4d, 0xfc,                          // mov         ecx,dword ptr [ebp-4]
                    0x8b, 0x75, 0xf8,                          // mov         esi,dword ptr [ebp-8]
                    0x83, 0xc1, 0x04,                          // add         ecx,4
                    0xc1, 0xe9, 0x02,                          // shr         ecx,2
                    0x66, 0x0f, 0x1f, 0x84, 0x00, 0x00,        // nop         word ptr [eax+eax]
                    0x00, 0x00, 0x00,
                    // :L1
                    0x8d, 0x40, 0x10,                          // lea         eax,[eax+10h]
                    0x0f, 0x10, 0x4c, 0x06, 0xf0,              // movups      xmm1,xmmword ptr [esi+eax-10h]
                    0x0f, 0x10, 0x40, 0xf0,                    // movups      xmm0,xmmword ptr [eax-10h]
                    0x0f, 0x59, 0xc8,                          // mulps       xmm1,xmm0
                    0x0f, 0x11, 0x4c, 0x07, 0xf0,              // movups      xmmword ptr [edi+eax-10h],xmm1
                    0x83, 0xe9, 0x01,                          // sub         ecx,1
                    0x75, 0xe7,                                // jne         L1
                    0x8b, 0x75, 0xf4,                          // mov         esi,dword ptr [ebp-0Ch]
                    0x8b, 0x7b, 0x0c,                          // mov         edi,dword ptr [ebx+0Ch]
                    // :L2
                    0x8b, 0xcf,                                // mov         ecx,edi
                    0x83, 0xe1, 0x03,                          // and         ecx,3
                    0x74, 0x32,                                // je          L4
                    0x8b, 0xc7,                                // mov         eax,edi
                    0x2b, 0xc1,                                // sub         eax,ecx
                    0x3b, 0xc7,                                // cmp         eax,edi
                    0x7d, 0x2a,                                // jge         L4
                    0x8b, 0x7b, 0x08,                          // mov         edi,dword ptr [ebx+8]
                    0x2b, 0xd7,                                // sub         edx,edi
                    0x2b, 0xf7,                                // sub         esi,edi
                    0x8d, 0x04, 0x87,                          // lea         eax,[edi+eax*4]
                    0x0f, 0x1f, 0x84, 0x00, 0x00, 0x00,        // nop         dword ptr [eax+eax]
                    0x00, 0x00,
                    // :L3
                    0xf3, 0x0f, 0x10, 0x04, 0x02,              // movss       xmm0,dword ptr [edx+eax]
                    0x8d, 0x40, 0x04,                          // lea         eax,[eax+4]
                    0xf3, 0x0f, 0x59, 0x40, 0xfc,              // mulss       xmm0,dword ptr [eax-4]
                    0xf3, 0x0f, 0x11, 0x44, 0x06, 0xfc,        // movss       dword ptr [esi+eax-4],xmm0
                    0x83, 0xe9, 0x01,                          // sub         ecx,1
                    0x75, 0xe8,                                // jne         L3
                    // :L4
                    0x5f,                                      // pop         edi
                    0x5e,                                      // pop         esi
                    0x8b, 0xe5,                                // mov         esp,ebp
                    0x5d,                                      // pop         ebp
                    0x8b, 0xe3,                                // mov         esp,ebx
                    0x5b,                                      // pop         ebx
                    0xc3,                                      // ret
                });
        }
    }
}
