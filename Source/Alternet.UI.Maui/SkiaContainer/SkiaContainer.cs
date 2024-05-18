using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using Microsoft.Maui.Graphics;

using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

/*
SkiaSharp.SKRect (float)
https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skrect?view=skiasharp-2.88

This is brush?
SKPaint https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skpaint?view=skiasharp-2.88
*/

namespace Alternet.UI
{
    public class SkiaContainer : SKCanvasView
    {
        private SkiaGraphics? graphics;
        private Alternet.UI.Control? control;

        public SkiaContainer()
        {
            EnableTouchEvents = true;
        }

        public Alternet.UI.Control? Control
        {
            get => control;

            set
            {
                if (control == value)
                    return;
                control = value;
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            if (graphics is null)
                graphics = new(this, e);
            else
            {
                graphics.Args = e;
            }

            RectD dirtyRect = RectD.Empty; // How to get this?

            control?.RaisePaint(new PaintEventArgs(graphics, dirtyRect));
        }

        protected override void OnTouch(SKTouchEventArgs e)
        {
            base.OnTouch(e);
        }
    }
}
