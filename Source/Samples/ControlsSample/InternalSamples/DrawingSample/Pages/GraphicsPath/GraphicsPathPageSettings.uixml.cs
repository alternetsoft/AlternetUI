using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    internal partial class GraphicsPathPageSettings : Control
    {
        private GraphicsPathPage? page;

        public GraphicsPathPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(GraphicsPathPage page)
        {
            this.page = page;

            foreach (var value in Enum.GetValues(typeof(RandomArt.PathSegmentType)))
                pathSegmentTypeComboBox.Items.Add(value!);

            foreach (var value in Enum.GetValues(typeof(FillMode)))
                pathFillModeComboBox.Items.Add(value!);

            pathFillModeComboBox.SelectedItem = page.PathFillMode;
            pathSegmentTypeComboBox.SelectedItem = page.PathSegmentType;
        }

        private void PathSegmentTypeComboBox_Changed(object? sender, System.EventArgs e)
        {
            if (page is not null && pathSegmentTypeComboBox.SelectedItem is not null)
                page.PathSegmentType = (RandomArt.PathSegmentType)pathSegmentTypeComboBox.SelectedItem;
        }

        private void PathFillModeComboBox_Changed(object? sender, System.EventArgs e)
        {
            if (page is not null && pathFillModeComboBox.SelectedItem is not null)
                page.PathFillMode = (FillMode)pathFillModeComboBox.SelectedItem;
        }

        private void ClearButton_Click(object? sender, System.EventArgs e)
        {
            page?.Clear();
        }

        private void CloseLastFigureButton_Click(object? sender, System.EventArgs e)
        {
            page?.CloseLastFigure();
        }
    }
}