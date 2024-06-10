using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Controls;

using SkiaSharp;

namespace Alternet.UI
{
    internal class MauiControlHandler : PlessControlHandler, IControlHandler
    {
        private SkiaContainer? container;

        public MauiControlHandler()
        {
        }

        public SkiaContainer? Container
        {
            get => container;

            set => container = value;
        }

        public override void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            Invalidate();
        }

        public override void Update()
        {
            Invalidate();
        }

        public override void Invalidate()
        {
            if (container is SkiaContainer skiaContainer)
                skiaContainer.CanvasView.InvalidateSurface();
        }

        public override Graphics CreateDrawingContext()
        {
            SKBitmap bitmap = new();
            SKCanvas canvas = new(bitmap);
            return new SkiaGraphics(canvas);
        }
    }
}
