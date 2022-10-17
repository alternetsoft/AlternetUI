using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample
{
    internal sealed class GraphicsPathPage : DrawingPage
    {
        public override string Name => "Graphics Path";

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            DrawInitialPath(dc);
        }

        private void DrawInitialPath(DrawingContext dc)
        {
            using var path = new GraphicsPath();
            path.AddLines(new[] { new Point(10, 10), new Point(100, 100), new Point(200, 10) });

            dc.DrawPath(Pens.Fuchsia, path);
        }

        protected override Control CreateSettingsControl()
        {
            var control = new GraphicsPathPageSettings();
            control.Initialize(this);
            return control;
        }
    }
}