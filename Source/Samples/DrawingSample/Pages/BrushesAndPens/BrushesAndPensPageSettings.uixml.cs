using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    partial class BrushesAndPensPageSettings : Control
    {
        private BrushesAndPensPage page;

        public BrushesAndPensPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(BrushesAndPensPage page)
        {
            DataContext = page;
            this.page = page;

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
    }
}