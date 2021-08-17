using Alternet.UI;
using System;

namespace DrawingSample
{
    internal class BrushesPageSettings : Control
    {
        private readonly BrushesPage page;

        public BrushesPageSettings(BrushesPage page)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("DrawingSample.Pages.Brushes.BrushesPageSettings.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);
            this.page = page;

            ((Slider)FindControl("shapeCountSlider")).ValueChanged += ShapeCountSlider_ValueChanged;
            ((Slider)FindControl("brushColorHueSlider")).ValueChanged += BrushColorHueSlider_ValueChanged;

            ((CheckBox)FindControl("rectanglesCheckBox")).CheckedChanged += (o, e) => ApplyIncludedShape(BrushesPage.Shapes.Rectangles, ((CheckBox)o!).IsChecked);

            var brushComboBox = (ComboBox)FindControl("brushComboBox");
            foreach (var brushType in Enum.GetValues(typeof(BrushesPage.BrushType)))
                brushComboBox.Items.Add(brushType!);
        }

        private void BrushColorHueSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.BrushColorHue = ((Slider)sender!).Value / 255.0;
        }

        private void ShapeCountSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.ShapeCount = ((Slider)sender!).Value;
        }

        private void ApplyIncludedShape(BrushesPage.Shapes shapes, bool value)
        {
            if (value)
                page.IncludedShapes |= shapes;
            else
                page.IncludedShapes &= ~shapes;
        }
    }
}