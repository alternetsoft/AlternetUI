using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;
using Alternet.UI.Extensions;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaGraphics
    {
        /// <inheritdoc/>
        public override Region? Clip
        {
            get
            {
                return null;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc/>
        public override void DrawPath(Pen pen, GraphicsPath path)
        {
            DebugPenAssert(pen);
            throw new NotImplementedException();
            /*
            var paint = GetStrokePaint(pen);
            */
        }

        /// <inheritdoc/>
        public override void FillPath(Brush brush, GraphicsPath path)
        {
            DebugBrushAssert(brush);
            throw new NotImplementedException();
            /*
            var paint = GetFillPaint(brush);
            */
        }

        /// <inheritdoc/>
        public override void DrawText(string text, Font font, Brush brush, RectD bounds)
        {
            DebugFontAssert(font);
            DebugBrushAssert(brush);
            throw new NotImplementedException();
        }
    }
}
