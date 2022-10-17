using Alternet.UI;

namespace DrawingSample
{
    internal partial class GraphicsPathPageSettings : Control
    {
        public GraphicsPathPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(GraphicsPathPage page)
        {
            DataContext = page;
        }
    }
}