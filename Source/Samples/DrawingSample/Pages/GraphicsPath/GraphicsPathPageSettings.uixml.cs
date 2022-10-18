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
            DataContext = page;
            this.page = page;

            foreach (var value in Enum.GetValues(typeof(RandomArt.PathSegmentType)))
                pathSegmentTypeComboBox.Items.Add(value!);

            foreach (var value in Enum.GetValues(typeof(FillMode)))
                pathFillModeComboBox.Items.Add(value!);
        }

        private void ClearButton_Click(object sender, System.EventArgs e)
        {
            page?.Clear();
        }

        private void CloseLastFigureButton_Click(object sender, System.EventArgs e)
        {
            page?.CloseLastFigure();
        }
    }
}