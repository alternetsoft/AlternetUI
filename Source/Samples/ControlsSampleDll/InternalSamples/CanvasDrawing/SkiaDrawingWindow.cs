using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;
using Alternet.UI.Extensions;
using Alternet.Drawing;

using SkiaSharp;

namespace ControlsSample
{
    internal class SkiaDrawingWindow : Window
    {
        private const string ResPrefixBackground = "embres:ControlsSampleDll.Resources.Backgrounds.";

        internal const string backgroundUrl1 = $"{ResPrefixBackground}textured-background2.jpg";

        internal static Image background1 = new Bitmap(backgroundUrl1);

        private readonly SkiaSampleControl control = new();

        private readonly SplittedPanel panel = new()
        {
            LeftPanelWidth = Display.Primary.BoundsDip.Width / 2,
            RightPanelWidth = 250,
            TopVisible = false,
            BottomVisible = false,
        };

        private readonly Button button = new("Paint on SKCanvas")
        {
            Visible = true,
        };

        private readonly Button button2 = new("GenericImage to SKBitmap")
        {
            Visible = true,
        };

        private readonly PropertyGrid propGrid = new()
        {
            HasBorder = false,
        };

        private readonly PictureBox pictureBox = new()
        {
            ImageStretch = false,
        };

        private readonly DrawTextParams prm = new();

        static SkiaDrawingWindow()
        {
            PropertyGrid.RegisterCollectionEditors();
        }

        public SkiaDrawingWindow()
        {
            Layout = LayoutStyle.Vertical;

            panel.Parent = this;
            panel.VerticalAlignment = VerticalAlignment.Fill;

            propGrid.Parent = panel.RightPanel;
            pictureBox.Parent = panel.LeftPanel;
            propGrid.SetProps(prm, true);

            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propGrid.Features = PropertyGridFeature.QuestionCharInNullable;

            Title = "SkiaSharp drawing demo";

            control.Parent = panel.FillPanel;

            Size = (900, 700);
            IsMaximized = true;

            var buttonPanel = AddHorizontalStackPanel();

            button.Margin = 10;
            button.VerticalAlignment = VerticalAlignment.Bottom;
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.Parent = buttonPanel;

            button2.Margin = 10;
            button2.VerticalAlignment = VerticalAlignment.Bottom;
            button2.HorizontalAlignment = HorizontalAlignment.Left;
            button2.Parent = buttonPanel;

            button2.ClickAction = GenericToSkia;

            button.ClickAction = PaintOnCanvas;

            propGrid.SuggestedInitDefaults();

            DrawTextOnSkia();
        }

        private void GenericToSkia()
        {
            GenericImage image = new(backgroundUrl1);
            var bitmap = (SKBitmap)image;
            pictureBox.Image = (Image)bitmap;
        }

        private void PaintOnCanvas()
        {
            RectD rect = (0, 0, control.Width, control.Height);

            SKBitmap bitmap = new((int)rect.Width, (int)rect.Height);

            SKCanvas canvas = new(bitmap);

            canvas.DrawRect(rect, Brushes.White);

            SkiaGraphics graphics = new(canvas);

            PaintEventArgs e = new(graphics, rect);

            control.RaisePaint(e);

            pictureBox.Image = (Image)bitmap;
        }

        private void DrawTextOnSkia()
        {
            RectD rect = (0, 0, 500, 500);

            SKBitmap bitmap = new((int)rect.Width, (int)rect.Height);

            SKCanvas canvas = new(bitmap);

            canvas.DrawRect(rect, Brushes.White);

            SkiaGraphics graphics = new(canvas);

            PointD pt = new(100, 150);
            PointD pt2 = new(300, 150);

            var font = SkiaSampleControl.SampleFont;

            canvas.DrawText("He|l lo", pt, font, Color.Black, Color.LightGreen);

            canvas.DrawText("; hello ", pt2, font, Color.Black, Color.LightGreen);

            canvas.DrawPoint(pt, Color.Red);
            canvas.DrawPoint(pt2, Color.Red);

            pictureBox.Image = (Image)bitmap;

        }

        private class DrawTextParams
        {

        }
   }
}