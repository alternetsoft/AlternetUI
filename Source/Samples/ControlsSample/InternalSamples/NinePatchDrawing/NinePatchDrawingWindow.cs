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

        private void DrawSlicedImage(NinePatchImagePaintArgs e)
        {
            e.Graphics.DrawImageI(image1, e.DestRect, e.PatchRect);
        }

        private void Control_Paint(object? sender, PaintEventArgs e)
        {
            var brush = background1.AsBrush;
            e.Graphics.FillRectangle(brush, e.ClipRectangle);

            NinePatchImagePaintArgs args = new(e, image2);
            args.SourceRect = (24, 24, 16, 16);
            args.PatchRect = RectI.Inflate(args.SourceRect, -2, -2);
            args.DestRect = (50, 50, 200, 200);
            DrawSlicedImage(args);
        }

        public class NinePatchImagePaintArgs : PaintEventArgs
        {
            public NinePatchImagePaintArgs(PaintEventArgs e, Image image)
                : this(e.Graphics, e.ClipRectangle, image)
            {
            }

            public NinePatchImagePaintArgs(Graphics canvas, RectD rect, Image image)
                : base(canvas, rect)
            {
                Image = image;
            }

            public Image Image { get; set; }

            public RectI SourceRect { get; set; }

            public RectI DestRect { get; set; }

            public RectI PatchRect { get; set; }

            public bool TileHorz { get; set; }

            public bool TileVert { get; set; }
        }
    }
}