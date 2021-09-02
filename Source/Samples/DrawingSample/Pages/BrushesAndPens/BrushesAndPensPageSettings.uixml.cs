using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    partial class BrushesAndPensPageSettings : Control
    {
        private readonly BrushesAndPensPage page;

        public BrushesAndPensPageSettings(BrushesAndPensPage page)
        {
            InitializeComponent();
            this.page = page;

            shapeCountSlider.Value = page.ShapeCount;
            brushColorHueSlider.Value = (int)MapRanges(page.BrushColorHue, 0, 1, brushColorHueSlider.Minimum, brushColorHueSlider.Maximum);
            penColorHueSlider.Value = (int)MapRanges(page.PenColorHue, 0, 1, brushColorHueSlider.Minimum, brushColorHueSlider.Maximum);
            penWidthSlider.Value = (int)page.PenWidth;
            rectanglesCheckBox.IsChecked = IsShapeIncluded(BrushesAndPensPage.AllShapeTypes.Rectangle);
            ellipsesCheckBox.IsChecked = IsShapeIncluded(BrushesAndPensPage.AllShapeTypes.Ellipse);

            foreach (var brushType in Enum.GetValues(typeof(BrushesAndPensPage.BrushType)))
                brushComboBox.Items.Add(brushType!);
            brushComboBox.SelectedItem = page.Brush;

            foreach (var style in Enum.GetValues(typeof(BrushHatchStyle)))
                hatchStyleComboBox.Items.Add(style!);
            hatchStyleComboBox.SelectedItem = page.HatchStyle;

            foreach (var style in Enum.GetValues(typeof(PenDashStyle)))
                dashStyleComboBox.Items.Add(style!);
            dashStyleComboBox.SelectedItem = page.PenDashStyle;
        }

        private static double MapRanges(double value, double fromLow, double fromHigh, double toLow, double toHigh) =>
                    ((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow)) + toLow;

        private void PenWidthSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.PenWidth = ((Slider)sender!).Value;
        }

        private void RectanglesCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyIncludedShape(BrushesAndPensPage.AllShapeTypes.Rectangle, ((CheckBox)sender!).IsChecked);
        }

        private void EllipsesCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            ApplyIncludedShape(BrushesAndPensPage.AllShapeTypes.Ellipse, ((CheckBox)sender!).IsChecked);
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
            var slider = (Slider)sender!;
            page.BrushColorHue = MapRanges(slider.Value, slider.Minimum, slider.Maximum, 0, 1);
        }

        private void PenColorHueSlider_ValueChanged(object? sender, EventArgs e)
        {
            var slider = (Slider)sender!;
            page.PenColorHue = MapRanges(slider.Value, slider.Minimum, slider.Maximum, 0, 1);
        }

        private void ShapeCountSlider_ValueChanged(object? sender, EventArgs e)
        {
            page.ShapeCount = ((Slider)sender!).Value;
        }

        private void ApplyIncludedShape(BrushesAndPensPage.ShapeType shape, bool value)
        {
            var shapes = page.IncludedShapes.ToList();

            if (value)
                shapes.Add(shape);
            else
                shapes.Remove(shape);

            page.IncludedShapes = shapes.ToArray();
        }

        private bool IsShapeIncluded(BrushesAndPensPage.ShapeType shape) => page.IncludedShapes.Contains(shape);
    }
}