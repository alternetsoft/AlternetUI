using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxMemoryHandler : DisposableObject, IMemoryHandler
    {
        /// <inheritdoc/>
        public IntPtr Alloc(int size)
        {
            return Native.WxOtherFactory.MemoryAlloc((ulong)size);
        }

        /// <inheritdoc/>
        public IntPtr Realloc(IntPtr ptr, int newSize)
            => Native.WxOtherFactory.MemoryRealloc(ptr, (ulong)newSize);

        /// <inheritdoc/>
        public void Free(IntPtr ptr)
            => Native.WxOtherFactory.MemoryFree(ptr);

        /// <inheritdoc/>
        public IntPtr Copy(IntPtr dest, IntPtr src, int count)
            => Native.WxOtherFactory.MemoryCopy(dest, src, (ulong)count);

        /// <inheritdoc/>
        public IntPtr Move(IntPtr dest, IntPtr src, int count)
            => Native.WxOtherFactory.MemoryMove(dest, src, (ulong)count);

        /// <inheritdoc/>
        public IntPtr Fill(IntPtr dest, byte fillByte, int count) =>
            Native.WxOtherFactory.MemorySet(dest, fillByte, (ulong)count);

        /// <inheritdoc/>
        public IntPtr AllocLong(ulong size)
        {
            return Native.WxOtherFactory.MemoryAlloc(size);
        }

        /// <inheritdoc/>
        public IntPtr ReallocLong(IntPtr ptr, ulong newSize)
            => Native.WxOtherFactory.MemoryRealloc(ptr, newSize);

        /// <inheritdoc/>
        public IntPtr CopyLong(IntPtr dest, IntPtr src, ulong count)
            => Native.WxOtherFactory.MemoryCopy(dest, src, count);

        /// <inheritdoc/>
        public IntPtr MoveLong(IntPtr dest, IntPtr src, ulong count)
            => Native.WxOtherFactory.MemoryMove(dest, src, count);

        /// <inheritdoc/>
        public IntPtr FillLong(IntPtr dest, byte fillByte, ulong count) =>
            Native.WxOtherFactory.MemorySet(dest, fillByte, count);
    }
}
