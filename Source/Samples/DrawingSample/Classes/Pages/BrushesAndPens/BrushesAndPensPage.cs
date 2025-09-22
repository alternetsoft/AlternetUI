using Alternet.UI;
using System;
using System.Collections.Generic;
using Alternet.Drawing;
using System.Linq;

namespace DrawingSample
{
    internal sealed class BrushesAndPensPage : DrawingPage
    {
        private BrushType brush = BrushType.LinearGradient;

        private ShapeType[] includedShapeTypes;

        private int shapeCount = 5;

        private int brushColorHue = 1;

        private int penColorHue = 2;

        private int penWidth = 4;

        private BrushHatchStyle hatchStyle = BrushHatchStyle.DiagonalCross;

        private DashStyle penDashStyle = DashStyle.Dash;

        private LineJoin lineJoin = LineJoin.Miter;

        private LineCap lineCap = LineCap.Flat;

        private Shape[]? shapes;

        public BrushesAndPensPage()
        {
            includedShapeTypes =
                new ShapeType[] { AllShapeTypes.Rectangle, AllShapeTypes.Ellipse };
        }

        public enum BrushType
        {
            Solid,
            Hatch,
            LinearGradient,
            RadialGradient
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

        ShapeType[] IncludedShapes
        {
            get => includedShapeTypes;
            set
            {
                includedShapeTypes = value;
                Update();
            }
        }

        public bool RectanglesIncluded
        {
            get => IsShapeIncluded(AllShapeTypes.Rectangle);
            set => SetIncludedShape(AllShapeTypes.Rectangle, value);
        }

        public bool EllipsesIncluded
        {
            get => IsShapeIncluded(AllShapeTypes.Ellipse);
            set => SetIncludedShape(AllShapeTypes.Ellipse, value);
        }

        private void SetIncludedShape(ShapeType shape, bool value)
        {
            var shapes = IncludedShapes.ToList();

            if (value)
                shapes.Add(shape);
            else
                shapes.Remove(shape);

            IncludedShapes = shapes.ToArray();
        }

        private bool IsShapeIncluded(BrushesAndPensPage.ShapeType shape)
            => IncludedShapes.Contains(shape);

        public int ShapeCount
        {
            get => shapeCount;
            set
            {
                shapeCount = value;
                Update();
            }
        }

        public int BrushColorHue
        {
            get => brushColorHue;
            set
            {
                brushColorHue = value;
                Update();
            }
        }

        public int PenColorHue
        {
            get => penColorHue;
            set
            {
                penColorHue = value;
                Update();
            }
        }

        public int PenWidth
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

        public DashStyle PenDashStyle
        {
            get => penDashStyle;
            set
            {
                penDashStyle = value;
                Update();
            }
        }

        public LineJoin LineJoin
        {
            get => lineJoin;
            set
            {
                lineJoin = value;
                Update();
            }
        }

        public LineCap LineCap
        {
            get => lineCap;
            set
            {
                lineCap = value;
                Update();
            }
        }

        public override string Name => "Brushes and Pens";

        public static ShapeTypes AllShapeTypes { get; } = new ShapeTypes(
                new ShapeType(CreateRectangle),
                new ShapeType(CreateEllipse));

        public override void OnActivated()
        {
            Update();
        }

        public override void Draw(Graphics dc, RectD bounds)
        {
            if (Canvas == null)
                throw new Exception();

            if (bounds.IsEmpty)
                return;

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

        protected override AbstractControl CreateSettingsControl()
        {
            var control = new BrushesAndPensPageSettings();
            control.Initialize(this);
            return control;
        }

        private static RectangleShape CreateRectangle(Random random, RectD bounds, Brush fill, Pen stroke)
        {
            var rect = new RectD(
                random.Next(0, (int)bounds.Width / 2),
                random.Next(0, (int)bounds.Height / 2),
                random.Next((int)bounds.Width / 5, (int)bounds.Width / 3),
                random.Next((int)bounds.Height / 5, (int)bounds.Height / 3));
            return new RectangleShape(rect, fill, stroke);
        }

        private static EllipseShape CreateEllipse(Random random, RectD bounds, Brush fill, Pen stroke)
        {
            var rect = new RectD(
                random.Next(0, (int)bounds.Width / 2),
                random.Next(0, (int)bounds.Height / 2),
                random.Next((int)bounds.Width / 5, (int)bounds.Width / 3),
                random.Next((int)bounds.Height / 5, (int)bounds.Height / 3));
            return new EllipseShape(rect, fill, stroke);
        }

        private void Update()
        {
            shapes = null;
            Canvas?.Invalidate();
        }

        private IEnumerable<Shape> CreateShapes(
            Random random,
            RectD bounds,
            Brush fill,
            Pen stroke)
        {
            if (includedShapeTypes.Length == 0)
                yield break;

            for (int i = 0; i < shapeCount; i++)
                yield return includedShapeTypes[random.Next(includedShapeTypes.Length)].ShapeFactory(random, bounds, fill, stroke);
        }

        private Pen CreateStrokePen()
        {
            RGBValue c = new HSVValue(
                MathUtils.MapRanges(penColorHue, 0, 10, 0, 1),
                1,
                0.3);

            return new Pen(c, penWidth, penDashStyle, lineCap, lineJoin);
        }

        private Brush CreateFillBrush()
        {
            RGBValue c = new HSVValue(
                MathUtils.MapRanges(brushColorHue, 0, 10, 0, 1),
                1,
                0.7);

            RGBValue c1 = new HSVValue(
                MathUtils.MapRanges(brushColorHue, 0, 10, 0, 1) - 0.3,
                1,
                0.7);

            RGBValue c2 = new HSVValue(
                MathUtils.MapRanges(brushColorHue, 0, 10, 0, 1) - 0.8,
                1,
                0.7);

            return brush switch
            {
                BrushType.Solid => new SolidBrush(Color.FromArgb(c.R, c.G, c.B)),
                BrushType.Hatch => new HatchBrush(hatchStyle, Color.FromArgb(c.R, c.G, c.B)),
                
                BrushType.LinearGradient =>
                    new LinearGradientBrush(
                        new PointD(0,0),
                        new PointD(200, 200),
                        new[]
                        {
                            new GradientStop(Color.FromArgb(c.R, c.G, c.B), 0),
                            new GradientStop(Color.FromArgb(c1.R, c1.G, c1.B), 0.5f),
                            new GradientStop(Color.FromArgb(c2.R, c2.G, c2.B), 0.8f),
                        }),
                
                BrushType.RadialGradient =>
                    new RadialGradientBrush(
                        new PointD(200, 200),
                        200,
                        new PointD(200, 200),
                        new[]
                        {
                            new GradientStop(Color.FromArgb(c.R, c.G, c.B), 0),
                            new GradientStop(Color.FromArgb(c1.R, c1.G, c1.B), 0.5f),
                            new GradientStop(Color.FromArgb(c2.R, c2.G, c2.B), 0.8f),
                        }),
                
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

            public abstract void Draw(Graphics dc);
        }

        public class ShapeType
        {
            public ShapeType(Factory shapeFactory)
            {
                ShapeFactory = shapeFactory;
            }

            public delegate Shape Factory(Random random, RectD bounds, Brush fill, Pen stroke);

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
            private RectD rectangle;

            public RectangleShape(RectD rectangle, Brush fill, Pen stroke) : base(fill, stroke)
            {
                this.rectangle = rectangle;
            }

            public override void Draw(Graphics dc)
            {
                dc.FillRectangle(Fill, rectangle);
                dc.DrawRectangle(Stroke, rectangle);
            }
        }

        private class EllipseShape : Shape
        {
            private RectD bounds;

            public EllipseShape(RectD bounds, Brush fill, Pen stroke) : base(fill, stroke)
            {
                this.bounds = bounds;
            }

            public override void Draw(Graphics dc)
            {
                dc.FillEllipse(Fill, bounds);
                dc.DrawEllipse(Stroke, bounds);
            }
        }
    }
}