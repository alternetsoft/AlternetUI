using Alternet.UI;
using System;

namespace DrawingSample
{
    internal class BrushesPageSettings : Control
    {
        private readonly BrushesPage page;
        private ComboBox brushComboBox;
        private ComboBox hatchStyleComboBox;

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

            brushComboBox = (ComboBox)FindControl("brushComboBox");
            foreach (var brushType in Enum.GetValues(typeof(BrushesPage.BrushType)))
                brushComboBox.Items.Add(brushType!);
            brushComboBox.SelectedItem = page.Brush;

            brushComboBox.SelectedItemChanged += BrushComboBox_SelectedItemChanged;

            hatchStyleComboBox = (ComboBox)FindControl("hatchStyleComboBox");
            foreach (var style in Enum.GetValues(typeof(BrushHatchStyle)))
                hatchStyleComboBox.Items.Add(style!);
            hatchStyleComboBox.SelectedItem = page.HatchStyle;

            hatchStyleComboBox.SelectedItemChanged += HatchStyleComboBox_SelectedItemChanged;
        }

        private void BrushComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.Brush = (BrushesPage.BrushType)brushComboBox.SelectedItem!;
        }

        private void HatchStyleComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.HatchStyle = (BrushHatchStyle)hatchStyleComboBox.SelectedItem!;
        }

        private void BrushColorHueSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.BrushColorHue = ((Slider)sender!).Value / 10.0;
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