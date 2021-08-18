using Alternet.UI;
using System;

namespace DrawingSample
{
    internal class BrushesAndPensPageSettings : Control
    {
        private readonly BrushesAndPensPage page;
        private ComboBox brushComboBox;
        private Control hatchStylePanel;
        private ComboBox hatchStyleComboBox;
        private ComboBox dashStyleComboBox;

        public BrushesAndPensPageSettings(BrushesAndPensPage page)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("DrawingSample.Pages.BrushesAndPens.BrushesAndPensPageSettings.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);
            this.page = page;

            ((Slider)FindControl("shapeCountSlider")).ValueChanged += ShapeCountSlider_ValueChanged;
            ((Slider)FindControl("brushColorHueSlider")).ValueChanged += BrushColorHueSlider_ValueChanged;
            ((Slider)FindControl("penColorHueSlider")).ValueChanged += PenColorHueSlider_ValueChanged;
            ((Slider)FindControl("penWidthSlider")).ValueChanged += PenWidthSlider_ValueChanged;

            ((CheckBox)FindControl("rectanglesCheckBox")).CheckedChanged += (o, e) => ApplyIncludedShape(BrushesAndPensPage.Shapes.Rectangles, ((CheckBox)o!).IsChecked);

            brushComboBox = (ComboBox)FindControl("brushComboBox");
            foreach (var brushType in Enum.GetValues(typeof(BrushesAndPensPage.BrushType)))
                brushComboBox.Items.Add(brushType!);
            brushComboBox.SelectedItem = page.Brush;

            brushComboBox.SelectedItemChanged += BrushComboBox_SelectedItemChanged;

            hatchStylePanel = FindControl("hatchStylePanel");

            hatchStyleComboBox = (ComboBox)FindControl("hatchStyleComboBox");
            foreach (var style in Enum.GetValues(typeof(BrushHatchStyle)))
                hatchStyleComboBox.Items.Add(style!);
            hatchStyleComboBox.SelectedItem = page.HatchStyle;

            hatchStyleComboBox.SelectedItemChanged += HatchStyleComboBox_SelectedItemChanged;

            dashStyleComboBox = (ComboBox)FindControl("dashStyleComboBox");
            foreach (var style in Enum.GetValues(typeof(PenDashStyle)))
                dashStyleComboBox.Items.Add(style!);
            dashStyleComboBox.SelectedItem = page.PenDashStyle;

            dashStyleComboBox.SelectedItemChanged += DashStyleComboBox_SelectedItemChanged;

        }

        private void PenWidthSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.PenWidth = ((Slider)sender!).Value;
        }

        private void DashStyleComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.PenDashStyle = (PenDashStyle)dashStyleComboBox.SelectedItem!;
        }

        private void BrushComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.Brush = (BrushesAndPensPage.BrushType)brushComboBox.SelectedItem!;

            hatchStylePanel.Visible = page.Brush == BrushesAndPensPage.BrushType.Hatch;
        }

        private void HatchStyleComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            page.HatchStyle = (BrushHatchStyle)hatchStyleComboBox.SelectedItem!;
        }

        private void BrushColorHueSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.BrushColorHue = ((Slider)sender!).Value / 10.0;
        }

        private void PenColorHueSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.PenColorHue = ((Slider)sender!).Value / 10.0;
        }

        private void ShapeCountSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.ShapeCount = ((Slider)sender!).Value;
        }

        private void ApplyIncludedShape(BrushesAndPensPage.Shapes shapes, bool value)
        {
            if (value)
                page.IncludedShapes |= shapes;
            else
                page.IncludedShapes &= ~shapes;
        }
    }
}