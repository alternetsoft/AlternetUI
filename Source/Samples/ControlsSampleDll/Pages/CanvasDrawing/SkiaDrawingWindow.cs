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
            LeftPanelWidth = 300,
            RightPanelWidth = 300,
            TopVisible = false,
            BottomPanelHeight = 300,
        };

        private readonly ActionsListBox actionsListBox = new()
        {
            HasBorder = false,
        };

        private readonly FontListBox fontListBox = new()
        {
            HasBorder = false,
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

        private readonly LogListBox logListBox = new()
        {
            HasBorder = false,
            BoundToApplicationLog = true,
        };

        private int counter;

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

            logListBox.Parent = mainPanel.BottomPanel;

            rightPanel.Parent = mainPanel.RightPanel;

            fontListBox.SelectionChanged += FontListBox_SelectionChanged;

            rightPanel.Add(GenericStrings.TabTitleActions, actionsListBox);
            rightPanel.Add(GenericStrings.TabTitleFonts, fontListBox);
            rightPanel.Add(GenericStrings.TabTitleProperties, propGrid);

            pictureBox.Parent = mainPanel.FillPanel;
            propGrid.SetProps(prm, true);

            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propGrid.Features = PropertyGridFeature.QuestionCharInNullable;

            Title = "SkiaSharp Drawing Demo";

            control.Parent = mainPanel.LeftPanel;

            Size = (900, 700);
            IsMaximized = true;

            actionsListBox.AddAction("GenericImage -> SKBitmap", GenericToSkia);
            actionsListBox.AddAction("Paint on SKCanvas", PaintOnCanvas);
            actionsListBox.AddAction("Draw text on SKSurface (alpha Bitmap)", DrawTextOnSkiaA);
            actionsListBox.AddAction("Draw text on SKSurface (opaque Bitmap)", DrawTextOnSkia);
            
            actionsListBox.AddAction(
                "Lock SKSurface (alpha GenericImage)",
                LockSurfaceOnGenericImageA);
            actionsListBox.AddAction(
                "Lock SKSurface (opaque GenericImage)",
                LockSurfaceOnGenericImage);

            propGrid.SuggestedInitDefaults();

            RefreshPreviewControl();

            propGrid.FitColumns();
        }

        private void RefreshPreviewControl()
        {
            DrawTextOnSkiaA();
        }

        private void FontListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var s = fontListBox.SelectedItem?.ToString() ?? AbstractControl.DefaultFont.Name;

            SkiaSampleControl.SampleFont = SkiaSampleControl.SampleFont.WithName(s);
            control.Font = SkiaSampleControl.SampleFont;
            RefreshPreviewControl();
        }

        private void GenericToSkia()
        {
            // Creates generic image from the specified url
            GenericImage image = new(backgroundUrl1);

            // Converts created generic image to SKBitmap
            var bitmap = (SKBitmap)image;

            // Converts SKBitmap to Image and assigns it to PictureBox control
            pictureBox.Image = (Image)bitmap;
        }

        private void PaintOnCanvas()
        {
            RectD rectDip = ClientRectangle;

            var canvas = SkiaUtils.CreateBitmapCanvas(
                rectDip.Size.Ceiling(),
                control.ScaleFactor,
                true);
            canvas.UseUnscaledDrawImage = true;
            canvas.Canvas.Clear(Color.White);

            try
            {
                GraphicsFactory.MeasureCanvasOverride = canvas;
                PaintEventArgs e = new(canvas, rectDip, rectDip);
                control.RaisePaint(e);
                pictureBox.Image = (Image)canvas.Bitmap!;
            }
            finally
            {
                GraphicsFactory.MeasureCanvasOverride = null;
            }
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

            SKPoint[] bezierPointsSK =
            {
                 start, control1, control2, end1,
                 control3, control4, end2
            };

            dc.DrawBeziers(blackPen, bezierPoints);

            dc.DrawPoints(SKPointMode.Points, bezierPointsSK, Color.Red.AsPen);
        }

        private Bitmap? bitmap;

        private void LockSurfaceOnGenericImageA() => LockSurfaceOnGenericImage(true);

        private void LockSurfaceOnGenericImage() => LockSurfaceOnGenericImage(false);

        private void LockSurfaceOnGenericImage(bool hasAlpha)
        {
            var width = 700;
            var height = 500;

            var image = GenericImage.Create(
                PixelFromDip(width),
                PixelFromDip(height),
                Color.Aquamarine);

            image.HasAlpha = hasAlpha;

            using (var canvasLock = image.LockSurface())
            {
                var canvas = canvasLock.Canvas;
                canvas.Scale((float)ScaleFactor);

                canvas.Clear(prm.BackColor);

                var font = SkiaSampleControl.SampleFont;

                canvas.DrawText((counter++).ToString(), (600, 0), font, Color.Black, Color.LightGreen);

                canvas.DrawRect(SKRect.Create(width, height), Color.Red.AsPen);

                DrawBeziersPoint(canvas);
                canvas.Flush();
            }

            pictureBox.Image = (Image)image;
        }

        private void DrawTextOnSkia() => DrawTextOnSkia(false);

        private void DrawTextOnSkiaA() => DrawTextOnSkia(true);

        private void DrawTextOnSkia(bool hasAlpha)
        {
            var width = 700;
            var height = 500;

            bitmap ??= new Bitmap(PixelFromDip(width), PixelFromDip(height));
            bitmap.HasAlpha = hasAlpha;
            bitmap.SetDPI(GetDPI());

            using (var canvasLock = bitmap.LockSurface())
            {
                var canvas = canvasLock.Canvas;
                canvas.Scale((float)ScaleFactor);

                canvas.Clear(prm.BackColor);
                canvas.DrawRect(SKRect.Create(width, height), Color.Red.AsPen);

                PointD pt = new(10, 10);
                PointD pt2 = new(10, 150);

                var font = SkiaSampleControl.SampleFont;

                canvas.DrawText((counter++).ToString(), (600,0), font, Color.Black, Color.LightGreen);

                canvas.DrawText(SkiaSampleControl.S1, pt, font, Color.Black, Color.LightGreen);

                canvas.DrawText(SkiaSampleControl.S2, pt2, font, Color.Red, Color.LightGreen);

                canvas.DrawPoint(pt, Color.Red);
                canvas.DrawPoint(pt2, Color.Red);

                DrawBeziersPoint(canvas);

                canvas.Flush();
            }

            pictureBox.Image = bitmap;
        }

        private class DrawTextParams
        {
            private readonly SkiaDrawingWindow owner;

            private Color backColor = Color.LightGoldenrodYellow;

            public DrawTextParams(SkiaDrawingWindow owner)
            {
                this.owner = owner;
            }

            public Color BackColor
            {
                get => backColor;

                set
                {
                    backColor = value;
                    owner.RefreshPreviewControl();
                } 
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