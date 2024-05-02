using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class MauiPlatform
    {
        /// <inheritdoc/>
        public override nint MemoryAlloc(int size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemoryAllocLong(ulong size)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemoryCopy(nint dest, nint src, int count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemoryCopyLong(nint dest, nint src, ulong count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override void MemoryFree(nint ptr)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemoryMove(nint dest, nint src, int count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemoryMoveLong(nint dest, nint src, ulong count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemoryRealloc(nint ptr, int newSize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemoryReallocLong(nint ptr, ulong newSize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemorySet(nint dest, byte fillByte, int count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override nint MemorySetLong(nint dest, byte fillByte, ulong count)
        {
            throw new NotImplementedException();
        }
    }
}
