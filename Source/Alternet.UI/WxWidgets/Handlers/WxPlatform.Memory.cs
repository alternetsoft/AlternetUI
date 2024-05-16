using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal partial class WxPlatform
    {
        /// <inheritdoc/>
        public override IntPtr MemoryAlloc(int size)
        {
            return Native.WxOtherFactory.MemoryAlloc((ulong)size);
        }

        /// <inheritdoc/>
        public override IntPtr MemoryRealloc(IntPtr ptr, int newSize)
            => Native.WxOtherFactory.MemoryRealloc(ptr, (ulong)newSize);

        /// <inheritdoc/>
        public override void MemoryFree(IntPtr ptr)
            => Native.WxOtherFactory.MemoryFree(ptr);

        /// <inheritdoc/>
        public override IntPtr MemoryCopy(IntPtr dest, IntPtr src, int count)
            => Native.WxOtherFactory.MemoryCopy(dest, src, (ulong)count);

        /// <inheritdoc/>
        public override IntPtr MemoryMove(IntPtr dest, IntPtr src, int count)
            => Native.WxOtherFactory.MemoryMove(dest, src, (ulong)count);

        /// <inheritdoc/>
        public override IntPtr MemorySet(IntPtr dest, byte fillByte, int count) =>
            Native.WxOtherFactory.MemorySet(dest, fillByte, (ulong)count);

        /// <inheritdoc/>
        public override IntPtr MemoryAllocLong(ulong size)
        {
            return Native.WxOtherFactory.MemoryAlloc(size);
        }

        /// <inheritdoc/>
        public override IntPtr MemoryReallocLong(IntPtr ptr, ulong newSize)
            => Native.WxOtherFactory.MemoryRealloc(ptr, newSize);

        /// <inheritdoc/>
        public override IntPtr MemoryCopyLong(IntPtr dest, IntPtr src, ulong count)
            => Native.WxOtherFactory.MemoryCopy(dest, src, count);

        /// <inheritdoc/>
        public override IntPtr MemoryMoveLong(IntPtr dest, IntPtr src, ulong count)
            => Native.WxOtherFactory.MemoryMove(dest, src, count);

        /// <inheritdoc/>
        public override IntPtr MemorySetLong(IntPtr dest, byte fillByte, ulong count) =>
            Native.WxOtherFactory.MemorySet(dest, fillByte, count);
    }
}
