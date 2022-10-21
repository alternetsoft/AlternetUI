using Alternet.Drawing;
using Alternet.UI;
using System;
using System.Linq;

namespace DrawingSample
{
    partial class BrushesAndPensPageSettings : Control
    {
        private BrushesAndPensPage? page;

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

            foreach (var style in Enum.GetValues(typeof(BrushHatchStyle)))
                hatchStyleComboBox.Items.Add(style!);

            foreach (var style in Enum.GetValues(typeof(PenDashStyle)))
                dashStyleComboBox.Items.Add(style!);

            foreach (var value in Enum.GetValues(typeof(LineJoin)))
                lineJoinComboBox.Items.Add(value!);

            foreach (var value in Enum.GetValues(typeof(LineCap)))
                lineCapComboBox.Items.Add(value!);
        }

        private void BrushComboBox_SelectedItemChanged(object? sender, EventArgs e)
        {
            hatchStylePanel.Visible = page!.Brush == BrushesAndPensPage.BrushType.Hatch;
        }
    }
}