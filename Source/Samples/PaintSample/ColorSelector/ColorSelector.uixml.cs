using Alternet.Drawing;
using Alternet.UI;
using System.Collections.Generic;

namespace PaintSample
{
    public partial class ColorSelector : Control, ISelectedColors
    {
        private static readonly Color[] SwatchColors = new[]
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

        private List<ColorSwatch> swatches = new List<ColorSwatch>();
        private SelectedColorDisplay selectedColorDisplay = new ();

        public ColorSelector()
        {
            InitializeComponent();

            selectedColorDisplay.Margin = new Thickness(0,0,5,0);
            selectedColorDisplay.VerticalAlignment = VerticalAlignment.Center;

            container.Children.Add(selectedColorDisplay);

            CreateSwatches();
            
            if (selectedColorDisplay != null)
                selectedColorDisplay.SelectedColor = Color.Blue;
        }

        Color ISelectedColors.Stroke => selectedColorDisplay.SelectedColor;

        private void CreateSwatches()
        {
            if (container == null)
                return;

            foreach (var color in SwatchColors)
            {
                var swatch = new ColorSwatch(color)
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 5, 0),
                    ToolTip = color.Name
                };

                swatch.Click += Swatch_Click;

                swatches.Add(swatch);
                container.Children.Add(swatch);
            }
        }

        private void Swatch_Click(object? sender, System.EventArgs e)
        {
            selectedColorDisplay.SelectedColor = ((ColorSwatch)sender!).SwatchColor;
        }
    }
}