using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    internal interface IBitmapData : IDisposable
    {
        int Width { get; }

        int Height { get; }

        int Stride { get; }

        PixelFormat PixelFormat { get; }

        IntPtr Scan0 { get; }

        int Reserved { get; }
    }
}
