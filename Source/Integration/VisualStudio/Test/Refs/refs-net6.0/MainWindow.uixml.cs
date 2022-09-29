using Alternet.UI;

namespace test_netcoreapp6_0
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialize()
        {
            helloLabel.Text += " 123";
        }
    }
}