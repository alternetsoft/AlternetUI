using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public interface ISkiaCanvasLock : IDisposable
    {
        int Width { get; }

        int Height { get; }

        SKSurface Surface { get; }

        SKCanvas Canvas { get; }
    }
}
