using Alternet.UI;

namespace TestDotNetNew
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
	    mainLabel.Text = WebBrowser.DoCommandGlobal("UIVersion");		
        }
    }
}



