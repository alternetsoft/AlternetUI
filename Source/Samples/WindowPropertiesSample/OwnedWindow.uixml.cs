using Alternet.Drawing;
using Alternet.UI;

namespace WindowPropertiesSample
{
    public partial class OwnedWindow : Window
    {
        public OwnedWindow()
        {
            InitializeComponent();
        }

        public void SetLabel(string text)
        {
            label.Text = text;
        }
    }
}
