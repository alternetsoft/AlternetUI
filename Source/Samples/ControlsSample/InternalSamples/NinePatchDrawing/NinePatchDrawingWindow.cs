using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class NinePatchDrawingWindow : Window
    {
        private const string ResPrefixBackground = "embres:ControlsSample.Resources.Backgrounds.";

        internal const string backgroundUrl1 = $"{ResPrefixBackground}textured-background2.jpg";

        private const string ResPrefix = "embres:ControlsSample.Resources.NinePatchImages.";

        internal const string imageUrl1 = $"{ResPrefix}NinePatch1.png";
        internal const string imageUrl2 = $"{ResPrefix}NinePatch2.png";

        internal static Image image1 = new Bitmap(imageUrl1);
        internal static Image image2 = new Bitmap(imageUrl2);

        internal static Image background1 = new Bitmap(backgroundUrl1);

        private readonly UserControl control = new();

        public NinePatchDrawingWindow()
        {
            Title = "Graphics.DrawSlicedImage demo";

            control.Paint += Control_Paint;
            control.Parent = this;

        }

        private void Control_Paint(object? sender, PaintEventArgs e)
        {
            var brush = background1.AsBrush;
            e.Graphics.FillRectangle(brush, e.ClipRectangle);

            DrawingUtils.NinePatchImagePaintParams args = new(image1);
            args.SourceRect = (24, 24, 16, 16);
            args.PatchRect = RectI.Inflate(args.SourceRect, -2, -2);
            args.DestRect = (70, 70, 64, 64);
            DrawingUtils.DrawSlicedImage(e.Graphics, args);

            DrawingUtils.NinePatchImagePaintParams args2 = new(image2);
            args2.SourceRect = image2.Bounds;
            args2.PatchRect = RectI.Inflate(args2.SourceRect, -10, -10);
            args2.DestRect = (170, 170, 250, 160);
            DrawingUtils.DrawSlicedImage(e.Graphics, args2);
        }
    }
}