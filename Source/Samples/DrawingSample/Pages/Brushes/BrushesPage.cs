using Alternet.UI;
using System;
using System.Drawing;
using Brush = Alternet.UI.Brush;
using SolidBrush = Alternet.UI.SolidBrush;

namespace DrawingSample
{
    internal sealed class BrushesPage : DrawingPage
    {
        [Flags]
        public enum Shapes
        {
            None = 0,
            Rectangles = 1 << 0
        }

        public enum BrushType
        {
            Solid
        }

        BrushType brush;

        public BrushType Brush
        {
            get => brush;
            set
            {
                brush = value;
                Update();
            }
        }

        Shapes includedShapes;

        public Shapes IncludedShapes
        {
            get => includedShapes;
            set
            {
                includedShapes = value;
                Update();
            }
        }

        int shapeCount = 10;

        public int ShapeCount
        {
            get => shapeCount;
            set
            {
                shapeCount = value;
                Update();
            }
        }

        double brushColorHue;

        public double BrushColorHue
        {
            get => brushColorHue;
            set
            {
                brushColorHue = value;
                Update();
            }
        }

        private void Update()
        {
            fillBrush = null;
            Canvas?.Update();
        }

        public override string Name => "Brushes";

        Brush? fillBrush;

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            if (Canvas == null)
                throw new Exception();

            var random = new Random(0);

            if (fillBrush == null)
            {
                var c = new Skybrud.Colors.HslColor(brushColorHue, 1, 0.9).ToRgb();
                fillBrush = new SolidBrush(Color.FromArgb(c.R, c.G, c.B));
            }

            var b = Canvas.Handler.ClientRectangle;
            for (int i = 0; i < shapeCount; i++)
            {
                var rect = new RectangleF(random.Next(0, (int)b.Width), random.Next(0, (int)b.Height), random.Next(0, (int)b.Width), random.Next(0, (int)b.Height));
                dc.FillRectangle(rect, fillBrush);
                dc.DrawRectangle(rect, Color.Blue);
            }
        }

        protected override Control CreateSettingsControl() => new BrushesPageSettings(this);
    }
}