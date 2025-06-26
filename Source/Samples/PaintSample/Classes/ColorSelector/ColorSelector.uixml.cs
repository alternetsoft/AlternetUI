using System;
using Alternet.Drawing;
using Alternet.UI;
using System.Collections.Generic;

namespace PaintSample
{
    public partial class ColorSelector : VerticalStackPanel, ISelectedColors
    {
        private static readonly Color[] SwatchColors =
        {
            Color.Black,
            Color.Blue,
            Color.Green,
            Color.Cyan,
            Color.Red,
            Color.Magenta,
            Color.Brown,
            Color.DarkGray,
            Color.LightGray,
            Color.White,
        };

        private readonly List<SpeedButton> swatches = new();
        private readonly SelectedColorDisplay selectedColorDisplay = new();

        public ColorSelector()
        {
            Padding = 5;

            InitializeComponent();

            selectedColorDisplay.Margin = new Thickness(0,0,5,0);
            selectedColorDisplay.VerticalAlignment = VerticalAlignment.Center;

            container.Children.Add(selectedColorDisplay);

            CreateSwatches();

            LogListBox listBox = new()
            {
                HasBorder = false,
                SuggestedSize = (200, 150),
                Parent = bottomPanel,
            };
            listBox.BindApplicationLog();

            if (selectedColorDisplay != null)
                selectedColorDisplay.SelectedColor = Color.Blue;
        }

        Color ISelectedColors.Stroke => selectedColorDisplay.SelectedColor;

        private void CreateSwatches()
        {
            if (container == null)
                return;

            var sizePixels = ToolBarUtils.GetDefaultImageSize(this).Width;
            var sizeDips = PixelToDip(sizePixels);

            var biggerSize = sizeDips * 2;

            foreach (var color in SwatchColors)
            {
                var swatch = new SpeedButton()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Image = (Image)color.AsImage((int)(sizePixels * 1.5)),
			SuggestedSize = biggerSize + 5,
                    Margin = (0, 5, 5, 5),
                    Padding = 2,
                };

                swatch.CustomAttr.SetAttribute("SwatchColor", color);
                swatch.Click += Swatch_Click;

                swatches.Add(swatch);
                container.Children.Add(swatch);
            }
        }

        private void Swatch_Click(object? sender, EventArgs e)
        {
            selectedColorDisplay.SelectedColor =
                (((SpeedButton)sender!).CustomAttr.GetAttribute("SwatchColor") as Color)!;
        }
    }
}