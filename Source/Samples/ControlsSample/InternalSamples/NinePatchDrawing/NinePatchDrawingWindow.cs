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
        private const string ResPrefix = "embres:ControlsSample.Resources.NinePatchImages.";

        internal const string imageUrl1 = $"{ResPrefix}NinePatch1.png";
        internal const string imageUrl2 = $"{ResPrefix}NinePatch2.png";

        internal static Image image1 = new Bitmap(imageUrl1);
        internal static Image image2 = new Bitmap(imageUrl2);

        private readonly UserControl control = new();

        public NinePatchDrawingWindow()
        {
            Title = "Graphics.DrawSlicedImage demo";

            control.Paint += Control_Paint;
            control.Parent = this;

        }

        private void DrawSlicedImage(NinePatchImagePaintArgs e)
        {
            e.Graphics.DrawImageUnscaled(image1, (10, 10));
        }

        private void Control_Paint(object? sender, PaintEventArgs e)
        {
            NinePatchImagePaintArgs args = new(e, image1);
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
        }
    }
}
