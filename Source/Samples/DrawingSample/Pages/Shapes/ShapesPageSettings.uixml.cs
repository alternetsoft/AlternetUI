using Alternet.UI;

namespace DrawingSample
{
    partial class ShapesPageSettings : Control
    {
        public ShapesPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ShapesPage page)
        {
            DataContext = page;
        }
    }
}