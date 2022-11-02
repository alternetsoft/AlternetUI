using Alternet.UI;

namespace DrawingSample
{
    partial class ImagesPageSettings : Control
    {
        public ImagesPageSettings()
        {
            InitializeComponent();
        }

        public void Initialize(ImagesPage page)
        {
            DataContext = page;
        }
    }
}