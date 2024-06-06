using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

namespace Alternet.Drawing
{
    public interface ISkiaBitmapData : IBitmapData
    {
        SKColorType ColorType { get; }

        SKAlphaType AlphaType { get; }

        Image Image { get; }

        SKSurface Surface { get; }

        SKCanvas Canvas { get; }
        
        bool IsOk { get; }
    }
}
