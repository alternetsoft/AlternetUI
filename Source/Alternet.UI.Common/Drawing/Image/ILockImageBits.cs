using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public interface ILockImageBits
    {
        int Width { get; }

        int Height { get; }

        IntPtr LockBits();

        void UnlockBits();

        int GetStride();

        ImageBitsFormatKind BitsFormat { get; }
    }
}
