using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class NotImplementedPlatform : NativePlatform
    {
        public override IntPtr MemoryAlloc(int size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryAllocLong(ulong size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopy(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryCopyLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override void MemoryFree(IntPtr ptr)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMove(IntPtr dest, IntPtr src, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryMoveLong(IntPtr dest, IntPtr src, ulong count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryRealloc(IntPtr ptr, int newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemoryReallocLong(IntPtr ptr, ulong newSize)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySet(IntPtr dest, byte fillByte, int count)
        {
            throw new NotImplementedException();
        }

        public override IntPtr MemorySetLong(IntPtr dest, byte fillByte, ulong count)
        {
            throw new NotImplementedException();
        }
    }
}
