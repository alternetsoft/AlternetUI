using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Brush = Alternet.UI.Brush;
using Pen = Alternet.UI.Pen;
using SolidBrush = Alternet.UI.SolidBrush;

namespace DrawingSample
{
    internal sealed class BrushesAndPensPage : DrawingPage
    {
        private BrushType brush;

        private ShapeType[] includedShapeTypes;

        private int shapeCount = 5;

        private double brushColorHue = 0.1;

        private double penColorHue = 0.25;

        private float penWidth = 4;

        private BrushHatchStyle hatchStyle = BrushHatchStyle.DiagonalCross;

        private PenDashStyle penDashStyle = PenDashStyle.Dash;

        private Shape[]? shapes;

        public BrushesAndPensPage()
        {
            includedShapeTypes = new ShapeType[] { AllShapeTypes.Rectangle, AllShapeTypes.Ellipse };
        }

        public enum BrushType
        {
            Solid,
            Hatch
        }

        public BrushType Brush
        {
            get => brush;
            set
            {
                brush = value;
                Update();
            }
        }

        public ShapeType[] IncludedShapes
        {
            get => includedShapeTypes;
            set
            {
                includedShapeTypes = value;
                Update();
            }
        }

        public int ShapeCount
        {
            get => shapeCount;
            set
            {
                shapeCount = value;
                Update();
            }
        }

        public double BrushColorHue
        {
            get => brushColorHue;
            set
            {
                brushColorHue = value;
                Update();
            }
        }

        public double PenColorHue
        {
            get => penColorHue;
            set
            {
                penColorHue = value;
                Update();
            }
        }

        public float PenWidth
        {
            get => penWidth;
            set
            {
                penWidth = value;
                Update();
            }
        }

        public BrushHatchStyle HatchStyle
        {
            get => hatchStyle;
            set
            {
                hatchStyle = value;
                Update();
            }
        }

        public PenDashStyle PenDashStyle
        {
            get => penDashStyle;
            set
            {
                penDashStyle = value;
                Update();
            }
        }

        public override string Name => "Brushes and Pens";

        public static ShapeTypes AllShapeTypes { get; } = new ShapeTypes(
                new ShapeType(CreateRectangle),
                new ShapeType(CreateEllipse));

        public override void Draw(DrawingContext dc, RectangleF bounds)
        {
            if (Canvas == null)
                throw new Exception();

            if (shapes == null)
            {
                var random = new Random(0);
                var fill = CreateFillBrush();
                var stroke = CreateStrokePen();
                shapes = CreateShapes(random, bounds, fill, stroke).ToArray();
            }

            foreach (var shape in shapes)
                shape.Draw(dc);
        }

        protected override Control CreateSettingsControl() => new BrushesAndPensPageSettings(this);

        private static RectangleShape CreateRectangle(Random random, RectangleF bounds, Brush fill, Pen stroke)
        {
            var rect = new RectangleF(
                random.Next(0, (int)bounds.Width / 2),
                random.Next(0, (int)bounds.Height / 2),
                random.Next((int)bounds.Width / 5, (int)bounds.Width / 3),
                random.Next((int)bounds.Height / 5, (int)bounds.Height / 3));
            return new RectangleShape(rect, fill, stroke);
        }

        private static EllipseShape CreateEllipse(Random random, RectangleF bounds, Brush fill, Pen stroke)
        {
            var rect = new RectangleF(
                random.Next(0, (int)bounds.Width / 2),
                random.Next(0, (int)bounds.Height / 2),
                random.Next((int)bounds.Width / 5, (int)bounds.Width / 3),
                random.Next((int)bounds.Height / 5, (int)bounds.Height / 3));
            return new EllipseShape(rect, fill, stroke);
        }

        private void Update()
        {
            shapes = null;
            Canvas?.Update();
        }

        private IEnumerable<Shape> CreateShapes(Random random, RectangleF bounds, Brush fill, Pen stroke)
        {
            if (includedShapeTypes.Length == 0)
                yield break;

            for (int i = 0; i < shapeCount; i++)
                yield return includedShapeTypes[random.Next(includedShapeTypes.Length)].ShapeFactory(random, bounds, fill, stroke);
        }

        private Pen CreateStrokePen()
        {
            var c = new Skybrud.Colors.HslColor(penColorHue, 1, 0.3).ToRgb();
            return new Pen(Color.FromArgb(c.R, c.G, c.B), penWidth, penDashStyle);
        }

        private Brush CreateFillBrush()
        {
            var c = new Skybrud.Colors.HslColor(brushColorHue, 1, 0.7).ToRgb();

            return brush switch
            {
                BrushType.Solid => new SolidBrush(Color.FromArgb(c.R, c.G, c.B)),
                BrushType.Hatch => new HatchBrush(hatchStyle, Color.FromArgb(c.R, c.G, c.B)),
                _ => throw new Exception(),
            };
        }

        public abstract class Shape
        {
            protected Shape(Brush fill, Pen stroke)
            {
                Fill = fill;
                Stroke = stroke;
            }

            protected Brush Fill { get; }

            protected Pen Stroke { get; }

            public abstract void Draw(DrawingContext dc);
        }

        public class ShapeType
        {
            public ShapeType(Factory shapeFactory)
            {
                ShapeFactory = shapeFactory;
            }

            public delegate Shape Factory(Random random, RectangleF bounds, Brush fill, Pen stroke);

            public Factory ShapeFactory { get; }
        }

        public class ShapeTypes
        {
            public ShapeTypes(ShapeType rectangle, ShapeType ellipse)
            {
                Rectangle = rectangle;
                Ellipse = ellipse;
            }

            public ShapeType Rectangle { get; }

            public ShapeType Ellipse { get; }
        }

        private class RectangleShape : Shape
        {
            private RectangleF rectangle;

            public RectangleShape(RectangleF rectangle, Brush fill, Pen stroke) : base(fill, stroke)
            {
                this.rectangle = rectangle;
            }

            public override void Draw(DrawingContext dc)
            {
                dc.FillRectangle(Fill, rectangle);
                dc.DrawRectangle(Stroke, rectangle);
            }
        }

        private class EllipseShape : Shape
        {
            private RectangleF bounds;

            public EllipseShape(RectangleF bounds, Brush fill, Pen stroke) : base(fill, stroke)
            {
                this.bounds = bounds;
            }

            public override void Draw(DrawingContext dc)
            {
                dc.FillEllipse(Fill, bounds);
                dc.DrawEllipse(Stroke, bounds);
            }
        }
    }
}