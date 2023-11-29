using Alternet.Drawing;
using Alternet.UI;
using System.Collections.Generic;

namespace PaintSample
{
    public partial class ColorSelector : VerticalStackPanel, ISelectedColors
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

        private List<ColorSwatch> swatches = new();
        private SelectedColorDisplay selectedColorDisplay = new ();

        public ColorSelector()
        {
            InitializeComponent();

            selectedColorDisplay.Margin = new Thickness(0,0,5,0);
            selectedColorDisplay.VerticalAlignment = VerticalAlignment.Center;

            container.Children.Add(selectedColorDisplay);

            CreateSwatches();

            LogListBox listBox = new();
            listBox.HasBorder = false;
            listBox.BindApplicationLog();
            listBox.SuggestedSize = (200, 150);
            listBox.Parent = bottomPanel;

            if (selectedColorDisplay != null)
                selectedColorDisplay.SelectedColor = Color.Blue;
        }

        Color ISelectedColors.Stroke => selectedColorDisplay.SelectedColor;

        private void CreateSwatches()
        {
            if (container == null)
                return;

            var size = Alternet.UI.Toolbar.GetDefaultImageSize(this) * 2;

            foreach (var color in SwatchColors)
            {
                var swatch = new ColorSwatch(color)
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = (0, 0, 5, 0),
                    SuggestedSize = size,
                };

                swatch.MouseLeftButtonDown += Swatch_MouseLeftButtonDown;

                swatches.Add(swatch);
                container.Children.Add(swatch);
            }
        }

        private void Swatch_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedColorDisplay.SelectedColor = ((ColorSwatch)sender!).SwatchColor;
        }
    }
}