using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IMemoryHandler : IDisposable
    {
        IntPtr Alloc(int size);

        IntPtr Realloc(IntPtr ptr, int newSize);

        void Free(IntPtr ptr);

        IntPtr Copy(IntPtr dest, IntPtr src, int count);

        IntPtr Move(IntPtr dest, IntPtr src, int count);

        IntPtr Fill(IntPtr dest, byte fillByte, int count);

        IntPtr AllocLong(ulong size);

        IntPtr ReallocLong(IntPtr ptr, ulong newSize);

        IntPtr CopyLong(IntPtr dest, IntPtr src, ulong count);

        IntPtr MoveLong(IntPtr dest, IntPtr src, ulong count);

        IntPtr FillLong(IntPtr dest, byte fillByte, ulong count);
    }
}
