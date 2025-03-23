using Alternet.UI;

namespace TestDotNetNew
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
	    mainLabel.Text = SystemSettings.Handler.GetUIVersion() ?? "Hello from Alternet.UI";		
        }
    }
}



