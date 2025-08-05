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
        public override void ClipRegion(Region region)
        {
            throw new NotImplementedException();
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
    }
}
