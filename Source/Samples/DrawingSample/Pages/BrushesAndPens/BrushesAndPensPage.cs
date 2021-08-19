using Alternet.UI;
using System;
using System.Drawing;
using Brush = Alternet.UI.Brush;
using Pen = Alternet.UI.Pen;
using Pens = Alternet.UI.Pens;
using SolidBrush = Alternet.UI.SolidBrush;

namespace DrawingSample
{
    internal sealed class BrushesAndPensPage : DrawingPage
    {
        [Flags]
        public enum Shapes
        {
            None = 0,
            Rectangles = 1 << 0
        }

        public enum BrushType
        {
            Solid,
            Hatch
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

        int shapeCount = 5;

        public int ShapeCount
        {
            get => shapeCount;
            set
            {
                shapeCount = value;
                Update();
            }
        }

        double brushColorHue = 0.1;

        public double BrushColorHue
        {
            get => brushColorHue;
            set
            {
                brushColorHue = value;
                Update();
            }
        }

        double penColorHue = 0.25;

        public double PenColorHue
        {
            get => penColorHue;
            set
            {
                penColorHue = value;
                Update();
            }
        }

        float penWidth = 4;

        public float PenWidth
        {
            get => penWidth;
            set
            {
                penWidth = value;
                Update();
            }
        }

        BrushHatchStyle hatchStyle = BrushHatchStyle.DiagonalCross;

        public BrushHatchStyle HatchStyle
        {
            get => hatchStyle;
            set
            {
                hatchStyle = value;
                Update();
            }
        }

        PenDashStyle penDashStyle = PenDashStyle.Dash;

        public PenDashStyle PenDashStyle
        {
            get => penDashStyle;
            set
            {
                penDashStyle = value;
                Update();
            }
        }

        private void Update()
        {
            fillBrush = null;
            strokePen = null;
            Canvas?.Update();
        }

        public override string Name => "Brushes and Pens";

        Brush? fillBrush;
        Pen? strokePen;

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            if (Canvas == null)
                throw new Exception();

            var random = new Random(0);

            if (fillBrush == null)
            {
                var c = new Skybrud.Colors.HslColor(brushColorHue, 1, 0.7).ToRgb();

                switch (brush)
                {
                    case BrushType.Solid:
                        fillBrush = new SolidBrush(Color.FromArgb(c.R, c.G, c.B));
                        break;
                    case BrushType.Hatch:
                        fillBrush = new HatchBrush(hatchStyle, Color.FromArgb(c.R, c.G, c.B));
                        break;
                    default:
                        throw new Exception();
                }
            }

            if (strokePen == null)
            {
                var c = new Skybrud.Colors.HslColor(penColorHue, 1, 0.3).ToRgb();
                strokePen = new Pen(Color.FromArgb(c.R, c.G, c.B), penWidth, penDashStyle);
            }

            var b = Canvas.Handler.ClientRectangle;
            for (int i = 0; i < shapeCount; i++)
            {
                var rect = new RectangleF(
                    random.Next(0, (int)b.Width / 2),
                    random.Next(0, (int)b.Height / 2),
                    random.Next((int)b.Width / 5, (int)b.Width / 3),
                    random.Next((int)b.Height / 5, (int)b.Height / 3));

                dc.FillRectangle(fillBrush, rect);
                dc.DrawRectangle(strokePen, rect);
            }
        }

        protected override Control CreateSettingsControl() => new BrushesAndPensPageSettings(this);
    }
}