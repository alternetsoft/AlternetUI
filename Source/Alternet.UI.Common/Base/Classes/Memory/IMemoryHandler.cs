using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which allow to perform operations with memory buffers.
    /// </summary>
    public interface IMemoryHandler : IDisposable
    {
        /// <inheritdoc cref="BaseMemory.Alloc(int)"/>
        IntPtr Alloc(int size);

        /// <inheritdoc cref="BaseMemory.Realloc"/>
        IntPtr Realloc(IntPtr ptr, int newSize);

        /// <inheritdoc cref="BaseMemory.FreeMem(IntPtr)"/>
        void Free(IntPtr ptr);

        /// <inheritdoc cref="BaseMemory.Copy(IntPtr, IntPtr, int)"/>
        IntPtr Copy(IntPtr dest, IntPtr src, int count);

        /// <inheritdoc cref="BaseMemory.Move"/>
        IntPtr Move(IntPtr dest, IntPtr src, int count);

        /// <inheritdoc cref="BaseMemory.Fill"/>
        IntPtr Fill(IntPtr dest, byte fillByte, int count);

        /// <inheritdoc cref="BaseMemory.AllocLong(ulong)"/>
        IntPtr AllocLong(ulong size);

        /// <inheritdoc cref="BaseMemory.ReallocLong"/>
        IntPtr ReallocLong(IntPtr ptr, ulong newSize);

        /// <inheritdoc cref="BaseMemory.CopyLong(IntPtr, IntPtr, ulong)"/>
        IntPtr CopyLong(IntPtr dest, IntPtr src, ulong count);

        /// <inheritdoc cref="BaseMemory.MoveLong"/>
        IntPtr MoveLong(IntPtr dest, IntPtr src, ulong count);

        /// <inheritdoc cref="BaseMemory.FillLong"/>
        IntPtr FillLong(IntPtr dest, byte fillByte, ulong count);
    }
}
