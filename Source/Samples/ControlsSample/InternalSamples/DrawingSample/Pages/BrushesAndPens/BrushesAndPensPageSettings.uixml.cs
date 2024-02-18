using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    partial class BrushesAndPensPageSettings : Control
    {
        private readonly Label dashStyleLabel = new("Dash Style:");
        private readonly Label lineCapLabel = new("Line Cap:");
        private readonly Label lineJoinLabel = new("Line Join:");
        private readonly ComboBox dashStyleComboBox = new();
        private readonly ComboBox lineCapComboBox = new();
        private readonly ComboBox lineJoinComboBox = new();
        private BrushesAndPensPage? page;

        public BrushesAndPensPageSettings()
        {
            DoInsideLayout(() =>
            {
                InitializeComponent();

                ControlSet labels = new(dashStyleLabel, lineCapLabel, lineJoinLabel);
                ControlSet comboBoxes = new(dashStyleComboBox, lineCapComboBox, lineJoinComboBox);
                labels.Margin(new(0, 5, 10, 5)).VerticalAlignment(VerticalAlignment.Center);
                comboBoxes.Margin(new(0, 5, 0, 5)).IsEditable(false);
                var gridControls = ControlSet.GridFromColumns(labels, comboBoxes);

                LayoutFactory.SetupGrid(propGrid, gridControls);
            });
        }

        public void Initialize(BrushesAndPensPage page)
        {
            DataContext = page;
            this.page = page;
            brushComboBox.AddEnumValues<BrushesAndPensPage.BrushType>();
            hatchStyleComboBox.AddEnumValues<BrushHatchStyle>();
            dashStyleComboBox.AddEnumValues<DashStyle>();
            lineJoinComboBox.AddEnumValues<LineJoin>();
            lineCapComboBox.AddEnumValues<LineCap>();

            dashStyleComboBox.BindSelectedItem(nameof(BrushesAndPensPage.PenDashStyle));
            lineCapComboBox.BindSelectedItem(nameof(BrushesAndPensPage.LineCap));
            lineJoinComboBox.BindSelectedItem(nameof(BrushesAndPensPage.LineJoin));
        }

        private void BrushComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            hatchStylePanel.Visible = page!.Brush == BrushesAndPensPage.BrushType.Hatch;
        }
    }
}