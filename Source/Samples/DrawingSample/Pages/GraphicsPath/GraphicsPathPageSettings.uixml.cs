using Alternet.Drawing;
using Alternet.UI;
using System;

namespace DrawingSample
{
    internal partial class GraphicsPathPageSettings : Control
    {
        GraphicsPathPage? page;

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
        }
    }
}