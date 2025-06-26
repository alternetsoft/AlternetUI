using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    internal partial class GraphicsPathPageSettings : Panel
    {
        private GraphicsPathPage? page;

        public GraphicsPathPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(GraphicsPathPage page)
        {
            this.page = page;

            pathSegmentTypeComboBox.EnumType = typeof(RandomArt.PathSegmentType);
            pathFillModeComboBox.EnumType = typeof(FillMode);

            pathFillModeComboBox.Value = page.PathFillMode;
            pathSegmentTypeComboBox.Value = page.PathSegmentType;
        }

        private void PathSegmentTypeComboBox_Changed(object? sender, System.EventArgs e)
        {
            if (page is not null && pathSegmentTypeComboBox.Value is RandomArt.PathSegmentType pst)
                page.PathSegmentType = pst;
        }

        private void PathFillModeComboBox_Changed(object? sender, System.EventArgs e)
        {
            if (page is not null && pathFillModeComboBox.Value is FillMode f)
                page.PathFillMode = f;
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