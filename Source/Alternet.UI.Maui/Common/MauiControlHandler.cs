using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI
{
    internal class MauiControlHandler : PlessControlHandler, IControlHandler
    {
        public MauiControlHandler()
        {
        }

        public override Graphics CreateDrawingContext()
        {
            SKBitmap bitmap = new();
            SKCanvas canvas = new(bitmap);
            return new SkiaGraphics(canvas);
        }
    }
}
