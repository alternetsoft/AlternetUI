using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawingSample
{
    internal sealed class TransformsPage : DrawingPage
    {
        public override string Name => "Transforms";

        public override void Draw(DrawingContext dc, Rect bounds)
        {
            dc.PushTransform(GetTransform());

            DrawScene(dc);
            DrawSubTransformedPart(dc);

            dc.Pop();
        }

        private static void DrawScene(DrawingContext dc)
        {
            var outerFrame = new Rect(10, 10, 200, 300);
            var innerFrame = outerFrame.InflatedBy(-10, -10);
            var frameShadows = outerFrame.InflatedBy(-5, -5);

            dc.FillRectangle(Brushes.Orange, outerFrame);
            dc.FillRectangle(Brushes.White, innerFrame);

            dc.DrawLines(new Pen(Color.DarkGoldenrod, 3), new[] { frameShadows.BottomLeft, frameShadows.TopLeft, frameShadows.TopRight });
            dc.DrawLines(new Pen(Color.Wheat, 3), new[] { frameShadows.TopRight, frameShadows.BottomRight, frameShadows.BottomLeft });

            dc.DrawRectangle(new Pen(Color.MistyRose, 3), innerFrame.InflatedBy(-5, -5));

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
            dc.DrawEllipse(new Pen(Color.DarkRed, 3), ellipseRect);
        }

        private static void DrawSubTransformedPart(DrawingContext dc)
        {
            dc.PushTransform(TransformMatrix.CreateTranslation(210, 50));
            dc.PushTransform(TransformMatrix.CreateScale(2, 2));
            dc.PushTransform(TransformMatrix.CreateRotation(10));
            
            var r = new Rect(10, 10, 100, 200);
            dc.DrawRectangle(Pens.Blue, r);
            dc.DrawText(
                "This rectangle is drawn with additional transforms pushed on stack.",
                Control.DefaultFont,
                Brushes.Blue,
                r.InflatedBy(-10, -10),
                new TextFormat { Wrapping = TextWrapping.Word });
            
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

        int translationX;
        public int TranslationX
        {
            get => translationX;

            set
            {
                translationX = value;
                Canvas?.Invalidate();
            }
        }

        int translationY;
        public int TranslationY
        {
            get => translationY;

            set
            {
                translationY = value;
                Canvas?.Invalidate();
            }
        }

        int scalefactorX;
        public int ScaleFactorX
        {
            get => scalefactorX;

            set
            {
                scalefactorX = value;
                Canvas?.Invalidate();
            }
        }

        int scalefactorY;
        public int ScaleFactorY
        {
            get => scalefactorY;

            set
            {
                scalefactorY = value;
                Canvas?.Invalidate();
            }
        }

        int rotation;
        public int Rotation
        {
            get => rotation;

            set
            {
                rotation = value;
                Canvas?.Invalidate();
            }
        }

        protected override Control CreateSettingsControl()
        {
            var control = new TransformsPageSettings();
            control.Initialize(this);
            return control;
        }
    }
}