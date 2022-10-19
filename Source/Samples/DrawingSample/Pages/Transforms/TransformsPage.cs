using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    internal sealed class TransformsPage : DrawingPage
    {
        private static readonly Image logoImage = new Image(typeof(TransformsPage).Assembly.GetManifestResourceStream(
            "DrawingSample.Resources.Logo.png") ?? throw new Exception());

        private static readonly Pen frameShadowDarkPen = new Pen(Color.DarkGoldenrod, 3);

        private static readonly Pen frameShadowLightPen = new Pen(Color.Wheat, 3);

        private static readonly Pen canvasFramePen = new Pen(Color.MistyRose, 3);

        private static readonly Pen outerCirclePen = new Pen(Color.DarkRed, 3);

        private static readonly Font canvasTestFont = new Font(FontFamily.GenericMonospace, 10);

        private int translationX;

        private int translationY;

        private int scalefactorX;

        private int scalefactorY;

        private int rotation;

        public override string Name => "Transforms";

        public int TranslationX
        {
            get => translationX;

            set
            {
                translationX = value;
                Canvas?.Invalidate();
            }
        }

        public int TranslationY
        {
            get => translationY;

            set
            {
                translationY = value;
                Canvas?.Invalidate();
            }
        }

        public int ScaleFactorX
        {
            get => scalefactorX;

            set
            {
                scalefactorX = value;
                Canvas?.Invalidate();
            }
        }

        public int ScaleFactorY
        {
            get => scalefactorY;

            set
            {
                scalefactorY = value;
                Canvas?.Invalidate();
            }
        }

        public int Rotation
        {
            get => rotation;

            set
            {
                rotation = value;
                Canvas?.Invalidate();
            }
        }

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            dc.PushTransform(GetTransform());

            DrawScene(dc);
            DrawSubTransformedPart(dc);

            dc.Pop();
        }

        protected override Control CreateSettingsControl()
        {
            var control = new TransformsPageSettings();
            control.Initialize(this);
            return control;
        }

        private static void DrawScene(DrawingContext dc)
        {
            var outerFrame = new Rect(10, 10, 200, 300);
            var innerFrame = outerFrame.InflatedBy(-10, -10);

            void DrawFrame()
            {
                var frameShadows = outerFrame.InflatedBy(-5, -5);

                dc.FillRectangle(Brushes.Orange, outerFrame);
                dc.FillRectangle(Brushes.LightGray, innerFrame);

                dc.DrawLines(frameShadowDarkPen, new[] { frameShadows.BottomLeft, frameShadows.TopLeft, frameShadows.TopRight });
                dc.DrawLines(frameShadowLightPen, new[] { frameShadows.TopRight, frameShadows.BottomRight, frameShadows.BottomLeft });

                dc.DrawRectangle(canvasFramePen, innerFrame.InflatedBy(-5, -5));
            }

            void DrawFigure()
            {
                using var path = new GraphicsPath(dc);

                var center = innerFrame.Center;

                var pathRadius = 80;

                Point GetPointOnCircle(double r, double a)
                {
                    const double DegToRad = Math.PI / 180;
                    return new Point(center.X + r * Math.Cos(a * DegToRad), center.Y + r * Math.Sin(a * DegToRad));
                }

                path.StartFigure(GetPointOnCircle(pathRadius, 0));

                for (double a = 0; a < 360; a += 10)
                {
                    path.AddLineTo(GetPointOnCircle(pathRadius, a + 280));
                    path.AddLineTo(GetPointOnCircle(pathRadius, a));
                }

                path.CloseFigure();

                var ellipseRect = new Rect(center - new Size(pathRadius, pathRadius), new Size(pathRadius * 2, pathRadius * 2));
                dc.FillEllipse(Brushes.Tomato, ellipseRect);
                dc.DrawPath(Pens.Black, path);
                dc.DrawEllipse(outerCirclePen, ellipseRect);
            }

            DrawFrame();
            DrawFigure();

            //dc.DrawText(
            //    "AlterNET UI",
            //    canvasTestFont,
            //    Brushes.Blue,
            //    innerFrame.InflatedBy(-10, -10),
            //    new TextFormat { HorizontalAlignment = TextHorizontalAlignment.Center, VerticalAlignment = TextVerticalAlignment.Bottom });

            dc.DrawImage(logoImage, innerFrame.InflatedBy(-10, -10).TopLeft);
        }

        private static void DrawSubTransformedPart(DrawingContext dc)
        {
            dc.PushTransform(TransformMatrix.CreateTranslation(210, 50));
            dc.PushTransform(TransformMatrix.CreateScale(2, 2));
            dc.PushTransform(TransformMatrix.CreateRotation(10));

            var r = new Rect(10, 10, 100, 200);
            dc.DrawRectangle(Pens.Blue, r);
            //dc.DrawText(
            //    "This rectangle is drawn with additional transforms pushed on stack.",
            //    Control.DefaultFont,
            //    Brushes.Blue,
            //    r.InflatedBy(-10, -10),
            //    new TextFormat { Wrapping = TextWrapping.Word });

            dc.Pop();
            dc.Pop();
            dc.Pop();
        }

        private TransformMatrix GetTransform()
        {
            var matrix = new TransformMatrix();
            matrix.Rotate(Rotation);
            matrix.Scale(1 + ScaleFactorX / 100.0, 1 + ScaleFactorY / 100.0);
            matrix.Translate(TranslationX, TranslationY);
            return matrix;
        }
    }
}