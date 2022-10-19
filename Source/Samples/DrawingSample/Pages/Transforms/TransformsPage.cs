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
            dc.DrawRectangle(Pens.Red, new Rect(10, 10, 100, 200));

            dc.PushTransform(TransformMatrix.CreateTranslation(50, 50));
            dc.PushTransform(TransformMatrix.CreateScale(2, 2));
            dc.PushTransform(TransformMatrix.CreateRotation(10));
            var r = new Rect(10, 10, 100, 200);
            dc.DrawRectangle(Pens.Blue, r);
            dc.DrawText(
                "This rectangle is drawn with additional transforms pushed on stack.",
                Control.DefaultFont,
                Brushes.Blue,
                r,
                new TextFormat { Wrapping = TextWrapping.Word });
            dc.Pop();
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