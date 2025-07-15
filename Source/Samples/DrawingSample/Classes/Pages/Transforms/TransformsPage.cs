using Alternet.Drawing;
using Alternet.UI;

using System;

namespace DrawingSample
{
    internal sealed class TransformsPage : DrawingPage
    {
        private static readonly Pen frameShadowDarkPen = new(Color.DarkGoldenrod, 3);

        private static readonly Pen frameShadowLightPen = new(Color.Wheat, 3);

        private static readonly Pen canvasFramePen = new(Color.MistyRose, 3);

        private static readonly Pen outerCirclePen = new(Color.DarkRed, 3);

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

        public override void Draw(Graphics dc, RectD bounds)
        {
            dc.PushTransform(GetTransform());

            DrawScene(dc);
            DrawSubTransformedPart(dc);

            dc.Pop();
        }

        protected override AbstractControl CreateSettingsControl()
        {
            var control = new TransformsPageSettings();
            control.Initialize(this);
            return control;
        }

        private static void DrawScene(Graphics dc)
        {
            var outerFrame = new RectD(10, 10, 200, 300);
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

                PointD GetPointOnCircle(double r, double a) =>
                    DrawingUtils.GetPointOnCircle(center, r, a);

                path.StartFigure(GetPointOnCircle(pathRadius, 0));

                for (double a = 0; a < 360; a += 10)
                {
                    path.AddLineTo(GetPointOnCircle(pathRadius, a + 280));
                    path.AddLineTo(GetPointOnCircle(pathRadius, a));
                }

                path.CloseFigure();

                var ellipseRect = new RectD(center - new SizeD(pathRadius, pathRadius), new SizeD(pathRadius * 2, pathRadius * 2));
                dc.FillEllipse(Brushes.Tomato, ellipseRect);
                dc.DrawPath(Pens.Black, path);
                dc.DrawEllipse(outerCirclePen, ellipseRect);
            }

            DrawFrame();
            DrawFigure();

            var drawTexts = true;

            if (drawTexts)
            {
                dc.DrawTextWithAngle(
                    "Vertical Text",
                    innerFrame.Location.OffsetBy(280, 50),
                    Control.DefaultFont.Scaled(1.3).AsBold,
                    Color.Red,
                    Color.Empty,
                    270);

                dc.DrawTextWithAngle(
                    "Text with angle = 25",
                    innerFrame.Location.OffsetBy(180, 250),
                    Control.DefaultFont.Scaled(1.3),
                    Color.DarkKhaki,
                    Color.Gray100,
                    25);

                dc.DrawText(
                    "AlterNET UI",
                    Control.DefaultFont.Scaled(1.5),
                    Brushes.Blue,
                    innerFrame.Location.OffsetBy(150, 10));

                dc.DrawText("This is sample text",
                    innerFrame.InflatedBy(-150, -150).Location,
                    Control.DefaultFont,
                    Color.DarkOliveGreen,
                    Color.LightGoldenrodYellow);

                dc.DrawLabel(
                    "This is sample label",
                    Control.DefaultFont,
                    Color.LightSkyBlue,
                    Color.Olive,
                    KnownColorSvgImages.ImgInformation.AsImage(64),
                    innerFrame.InflatedBy(-250, -250));
            }

            dc.DrawImage(Resources.LogoImage, innerFrame.InflatedBy(-10, -10).TopLeft);
        }

        private static void DrawSubTransformedPart(Graphics dc)
        {
            dc.PushTransform(TransformMatrix.CreateTranslation(210, 50));
            dc.PushTransform(TransformMatrix.CreateScale(2, 2));
            dc.PushTransform(TransformMatrix.CreateRotation(10));

            var r = new RectD(10, 10, 100, 200);
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