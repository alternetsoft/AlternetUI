using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class SkiaDrawingWindow : Window
    {
        private const string ResPrefixBackground = "embres:ControlsSampleDll.Resources.Backgrounds.";

        internal const string backgroundUrl1 = $"{ResPrefixBackground}textured-background2.jpg";

        internal static Image background1 = new Bitmap(backgroundUrl1);

        private readonly UserControl control = new();

        private readonly Button button = new("Button 1")
        {
            Visible = true,
        };

        private readonly Button button2 = new("Button 2")
        {
            Visible = true,
        };

        public SkiaDrawingWindow()
        {
            Layout = LayoutStyle.Vertical;

            Title = "SkiaSharp drawing demo";

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
            };           

            button.ClickAction = () =>
            {
            };
        }

        private void Control_Paint(object? sender, PaintEventArgs e)
        {
            var brush = background1.AsBrush;
            e.Graphics.FillRectangle(brush, e.ClipRectangle);
        }
    }
}