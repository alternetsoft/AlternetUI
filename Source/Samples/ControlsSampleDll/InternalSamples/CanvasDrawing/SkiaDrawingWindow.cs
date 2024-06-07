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

        private readonly SplittedPanel mainPanel = new()
        {
            LeftPanelWidth = Display.Primary.BoundsDip.Width / 2,
            RightPanelWidth = 300,
            TopVisible = false,
            BottomVisible = false,
        };

        private readonly FontListBox fontListBox = new()
        {
            HasBorder = false,
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

        private readonly DrawTextParams prm;

        private readonly SideBarPanel rightPanel = new()
        {
        };

        static SkiaDrawingWindow()
        {
            PropertyGrid.RegisterCollectionEditors();
        }

        public SkiaDrawingWindow()
        {
            prm = new(this);

            Layout = LayoutStyle.Vertical;

            mainPanel.Parent = this;
            mainPanel.VerticalAlignment = VerticalAlignment.Fill;

            rightPanel.Parent = mainPanel.RightPanel;

            fontListBox.SelectionChanged += FontListBox_SelectionChanged;

            rightPanel.Add("Fonts", fontListBox);
            rightPanel.Add("Properties", propGrid);

            pictureBox.Parent = mainPanel.LeftPanel;
            propGrid.SetProps(prm, true);

            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propGrid.Features = PropertyGridFeature.QuestionCharInNullable;

            Title = "SkiaSharp drawing demo";

            control.Parent = mainPanel.FillPanel;

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

            DrawTextOnSkia2();
        }

        private void FontListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var s = fontListBox.SelectedItemAs<string?>() ?? Control.DefaultFont.Name;

            SkiaSampleControl.SampleFont = SkiaSampleControl.SampleFont.WithName(s);
            control.Font = SkiaSampleControl.SampleFont;
            DrawTextOnSkia2();
            propGrid.CenterSplitter();
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

        internal void DrawTextOnSkia()
        {
            RectD rect = (0, 0, 800, 600);

            SKBitmap bitmap = new((int)rect.Width, (int)rect.Height);

            SKCanvas canvas = new(bitmap);

            canvas.DrawRect(rect, Brushes.White);

            PointD pt = new(20, 150);
            PointD pt2 = new(300, 150);

            var font = SkiaSampleControl.SampleFont;

            canvas.DrawText("He|l lo", pt, font, Color.Black, Color.LightGreen);

            canvas.DrawText("; hello ", pt2, font, Color.Red, Color.LightGreen);

            canvas.DrawPoint(pt, Color.Red);
            canvas.DrawPoint(pt2, Color.Red);

            pictureBox.Image = (Image)bitmap;

        }

        private void DrawBeziersPoint(SKCanvas dc)
        {
            Pen blackPen = Color.Black.GetAsPen(3);

            PointD start = new(100, 100);
            PointD control1 = new(200, 10);
            PointD control2 = new(350, 50);
            PointD end1 = new(500, 100);
            PointD control3 = new(600, 150);
            PointD control4 = new(650, 250);
            PointD end2 = new(500, 300);

            PointD[] bezierPoints =
            {
                 start, control1, control2, end1,
                 control3, control4, end2
             };

            dc.DrawBeziers(blackPen, bezierPoints);
        }

        private void DrawTextOnSkia2()
        {
            var width = 700;
            var height = 500;

            var bitmap = new Bitmap(PixelFromDip(width), PixelFromDip(height));
            bitmap.HasAlpha = true;
            bitmap.SetDPI(GetDPI());

            using var canvasLock = bitmap.LockSurface();

            var canvas = canvasLock.Canvas;
            canvas.Scale((float)GetPixelScaleFactor());

            canvas.Clear(Color.Yellow);
            canvas.DrawRect(SKRect.Create(width, height), Color.Red.AsPen);

            PointD pt = new(10, 10);
            PointD pt2 = new(10, 150);

            var font = SkiaSampleControl.SampleFont;

            canvas.DrawText(SkiaSampleControl.S1, pt, font, Color.Black, Color.LightGreen);

            canvas.DrawText(SkiaSampleControl.S2, pt2, font, Color.Red, Color.LightGreen);

            canvas.DrawPoint(pt, Color.Red);
            canvas.DrawPoint(pt2, Color.Red);

            DrawBeziersPoint(canvas);

            canvas.Flush();

            pictureBox.Image = bitmap;
        }

        private class DrawTextParams
        {
            private readonly SkiaDrawingWindow owner;

            public DrawTextParams(SkiaDrawingWindow owner)
            {
                this.owner = owner;
            }

            public Font Font
            {
                get => SkiaSampleControl.SampleFont;

                set
                {
                    SkiaSampleControl.SampleFont = value;
                    owner.FontListBox_SelectionChanged(null, EventArgs.Empty);
                }
            }
        }
    }
}