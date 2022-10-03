using Alternet.UI;

namespace test_netcoreapp6_0
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            helloLabel.Text += " 123";
        }
    }
}