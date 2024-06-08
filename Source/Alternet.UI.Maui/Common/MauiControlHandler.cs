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
        private object? container;

        public MauiControlHandler()
        {
        }

        public object? Container
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
                skiaContainer.InvalidateSurface();
        }

        public override Graphics CreateDrawingContext()
        {
            SKBitmap bitmap = new();
            SKCanvas canvas = new(bitmap);
            return new SkiaGraphics(canvas);
        }
    }
}
