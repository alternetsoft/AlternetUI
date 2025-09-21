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
        private const string ResPrefixBackground = "embres:ControlsSampleDll.Resources.Backgrounds.";

        internal const string backgroundUrl1 = $"{ResPrefixBackground}textured-background2.jpg";

        private const string ResPrefix = "embres:ControlsSampleDll.Resources.NinePatchImages.";

        internal const string imageUrl1 = $"{ResPrefix}NinePatch1.png";
        internal const string imageUrl2 = $"{ResPrefix}NinePatch2.png";

        internal static Image image1 = new Bitmap(imageUrl1);
        internal static Image image2 = new Bitmap(imageUrl2);

        internal static Image background1 = new Bitmap(backgroundUrl1);

        private readonly UserControl control = new();
        private readonly PopupPictureBox picturePopup = new();

        private readonly Button button = new("Draw on screen")
        {
            Visible = true,
        };

        private readonly Button button2 = new("Screen image")
        {
            Visible = true,
        };

        public NinePatchDrawingWindow()
        {
            Layout = LayoutStyle.Vertical;

            Title = "Graphics.DrawSlicedImage demo";

            control.VerticalAlignment = VerticalAlignment.Fill;
            control.Paint += Control_Paint;
            control.Parent = this;

            Size = (600, 600);

            var buttonPanel = AddHorizontalStackPanel();

            button.Margin = 10;
            button.VerticalAlignment = VerticalAlignment.Bottom;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Parent = buttonPanel;

            button2.Margin = 10;
            button2.VerticalAlignment = VerticalAlignment.Bottom;
            button2.HorizontalAlignment = HorizontalAlignment.Left;
            button2.Parent = buttonPanel;

            button2.ClickAction = () =>
            {
                App.Log("Some Linux versions don't support getting screen image.");
                var bitmap = Image.FromScreen();
                if (bitmap is not null)
                {
                    bitmap?.Rescale((800, 600));
                    picturePopup.MainControl.Image = bitmap;
                    picturePopup.ShowPopup(button2);
                }
                else
                    App.LogError("Screen picture is null");
            };           

            button.ClickAction = () =>
            {
                App.Log("Some Linux versions don't support drawing on screen.");

                for(int i = 0; i < Display.Count; i++)
                    DrawOnDisplay(i);

                void DrawOnDisplay(int index)
                {
                    var rect = new Display(index).Bounds;

                    var dc = Graphics.FromScreen();

                    dc.FillRectangle(Color.White.AsBrush, (rect.Location, (500, 400)));

                    dc.DrawRotatedText(
                        $"Display {index}",
                        rect.Location + (50, 250),
                        (Font ?? Control.DefaultFont).Scaled(2.7),
                        Color.Red,
                        Color.Empty,
                        40);
                }
            };
        }

        private void Control_Paint(object? sender, PaintEventArgs e)
        {
            // All rectangles in this method are not in dips but in pixels.
            // Graphics methods with 'I' suffix assume pixels.

            var brush = background1.AsBrush;
            e.Graphics.FillRectangle(brush, e.ClipRectangle);

            NinePatchImagePaintParams args = new(image1);
            args.SourceRect = (24, 24, 16, 16);
            args.PatchRect = RectI.Inflate(args.SourceRect, -2, -2);
            args.DestRect = (70, 70, 64, 64);
            e.Graphics.DrawImageSliced(args);

            NinePatchImagePaintParams args2 = new(image2);
            args2.SourceRect = image2.Bounds;
            args2.PatchRect = RectI.Inflate(args2.SourceRect, -10, -10);
            args2.DestRect = (170, 170, 250, 160);
            e.Graphics.DrawImageSliced(args2);

            e.Graphics.DrawRotatedText(
                "Hello",
                (190, 250),
                (Font ?? Control.DefaultFont).Scaled(2.7),
                Color.Red,
                Color.Empty,
                40, GraphicsUnit.Pixel);

            e.Graphics.Blit(
                (450, 200),
                args2.DestRect.Size,
                e.Graphics,
                args2.DestRect.Location,
                RasterOperationMode.Copy, false, null, GraphicsUnit.Pixel);

            e.Graphics.StretchBlit(
                (450, 400),
                args2.DestRect.Size * 2,
                e.Graphics,
                args2.DestRect.Location,
                args2.DestRect.Size,
                RasterOperationMode.Copy, false, null, GraphicsUnit.Pixel);
        }
    }
}