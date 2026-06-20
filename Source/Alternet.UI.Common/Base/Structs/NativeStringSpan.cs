using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NativeStringSpan
    {
        public IntPtr Pointer;

        public int Length;

        public NativeStringSpan()
        {
        }

        public NativeStringSpan(IntPtr pointer, int length)
        {
            Pointer = pointer;
            Length = length;
        }
    }
}
