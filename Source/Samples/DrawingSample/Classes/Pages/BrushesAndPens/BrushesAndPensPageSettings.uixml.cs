using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    partial class BrushesAndPensPageSettings : Panel
    {
        private readonly Label dashStyleLabel = new("Dash Style:");
        private readonly Label lineCapLabel = new("Line Cap:");
        private readonly Label lineJoinLabel = new("Line Join:");
        private readonly EnumPicker dashStyleComboBox = new();
        private readonly EnumPicker lineCapComboBox = new();
        private readonly EnumPicker lineJoinComboBox = new();
        private BrushesAndPensPage? page;

        public BrushesAndPensPageSettings()
        {
            DoInsideLayout(() =>
            {
                InitializeComponent();

                ControlSet labels = new(dashStyleLabel, lineCapLabel, lineJoinLabel);
                ControlSet comboBoxes = new(dashStyleComboBox, lineCapComboBox, lineJoinComboBox);
                labels.Margin(new(0, 5, 10, 5)).VerticalAlignment(VerticalAlignment.Center);
                comboBoxes.Margin(new(0, 5, 0, 5));
                var gridControls = ControlSet.GridFromColumns(labels, comboBoxes);

                propGrid.Setup(gridControls);
            });
        }

        public void Initialize(BrushesAndPensPage page)
        {
            DataContext = page;
            this.page = page;

            brushComboBox.EnumType = typeof(BrushesAndPensPage.BrushType);
            hatchStyleComboBox.EnumType = typeof(BrushHatchStyle);
            dashStyleComboBox.EnumType = typeof(DashStyle);
            lineJoinComboBox.EnumType = typeof(LineJoin);
            lineCapComboBox.EnumType = typeof(LineCap);

            dashStyleComboBox.Value = page.PenDashStyle;
            dashStyleComboBox.ValueChanged += (s, e) =>
            {
                page.PenDashStyle = dashStyleComboBox.ValueAs<DashStyle>();
            };

            lineCapComboBox.Value = page.LineCap;
            lineCapComboBox.ValueChanged += (s, e) =>
            {
                page.LineCap = lineCapComboBox.ValueAs<LineCap>();
            };

            lineJoinComboBox.Value = page.LineJoin;
            lineJoinComboBox.ValueChanged += (s, e) =>
            {
                page.LineJoin = lineJoinComboBox.ValueAs<LineJoin>();
            };

            hatchStyleComboBox.Value = page.HatchStyle;
            hatchStyleComboBox.ValueChanged += (s, e) =>
            {
                page.HatchStyle = hatchStyleComboBox.ValueAs<BrushHatchStyle>();
            };

            brushComboBox.Value = page.Brush;
            brushComboBox.ValueChanged += (s, e) =>
            {
                page.Brush = brushComboBox.ValueAs<BrushesAndPensPage.BrushType>();
            };

            shapeCountSlider.Value = page.ShapeCount;
            shapeCountSlider.ValueChanged += (s, e) =>
            {
                page.ShapeCount = shapeCountSlider.Value;
            };

            brushColorHueSlider.Value = page.BrushColorHue;
            brushColorHueSlider.ValueChanged += (s, e) =>
            {
                page.BrushColorHue = brushColorHueSlider.Value;
            };

            penColorHueSlider.Value = page.PenColorHue;
            penColorHueSlider.ValueChanged += (s, e) =>
            {
                page.PenColorHue = penColorHueSlider.Value;
            };

            penWidthSlider.Value = page.PenWidth;
            penWidthSlider.ValueChanged += (s, e) =>
            {
                page.PenWidth = penWidthSlider.Value;
            };

            rectanglesIncludedCheckBox.IsChecked = page.RectanglesIncluded;
            rectanglesIncludedCheckBox.CheckedChanged += (s, e) =>
            {
                page.RectanglesIncluded = rectanglesIncludedCheckBox.IsChecked;
            };

            ellipsesIncludedCheckBox.IsChecked = page.EllipsesIncluded;
            ellipsesIncludedCheckBox.CheckedChanged += (s, e) =>
            {
                page.EllipsesIncluded = ellipsesIncludedCheckBox.IsChecked;
            };
        }

        private void BrushComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            hatchStylePanel.Visible = page!.Brush == BrushesAndPensPage.BrushType.Hatch;
        }
    }
}