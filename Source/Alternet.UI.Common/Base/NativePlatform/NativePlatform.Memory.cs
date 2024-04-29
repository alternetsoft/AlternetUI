using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class NativePlatform
    {
        /// <inheritdoc cref="BaseMemory.Alloc"/>
        public abstract IntPtr MemoryAlloc(int size);

        /// <inheritdoc cref="BaseMemory.Realloc"/>
        public abstract IntPtr MemoryRealloc(IntPtr ptr, int newSize);

        /// <inheritdoc cref="BaseMemory.FreeMem"/>
        public abstract void MemoryFree(IntPtr ptr);

        /// <inheritdoc cref="BaseMemory.Copy"/>
        public abstract IntPtr MemoryCopy(IntPtr dest, IntPtr src, int count);

        /// <inheritdoc cref="BaseMemory.Move"/>
        public abstract IntPtr MemoryMove(IntPtr dest, IntPtr src, int count);

        /// <inheritdoc cref="BaseMemory.Fill"/>
        public abstract IntPtr MemorySet(IntPtr dest, byte fillByte, int count);

        /// <inheritdoc cref="BaseMemory.AllocLong"/>
        public abstract IntPtr MemoryAllocLong(ulong size);

        /// <inheritdoc cref="BaseMemory.ReallocLong"/>
        public abstract IntPtr MemoryReallocLong(IntPtr ptr, ulong newSize);

        /// <inheritdoc cref="BaseMemory.CopyLong"/>
        public abstract IntPtr MemoryCopyLong(IntPtr dest, IntPtr src, ulong count);

        /// <inheritdoc cref="BaseMemory.MoveLong"/>
        public abstract IntPtr MemoryMoveLong(IntPtr dest, IntPtr src, ulong count);

        /// <inheritdoc cref="BaseMemory.FillLong"/>
        public abstract IntPtr MemorySetLong(IntPtr dest, byte fillByte, ulong count);
    }
}
