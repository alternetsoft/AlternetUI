using Alternet.Drawing;
using Alternet.UI;

namespace DrawingSample
{
    internal partial class GraphicsPathPageSettings : Control
    {
        GraphicsPathPage page;

        public GraphicsPathPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(GraphicsPathPage page)
        {
            DataContext = page;
            this.page = page;
        }

        private void StartFigureButton_Click(object sender, System.EventArgs e)
        {
        }
    }
}