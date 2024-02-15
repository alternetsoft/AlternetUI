using Alternet.UI;

namespace CustomControlInUixml
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CustomControl control = new();
            control.Margin = 10;
            control.Parent = mainPanel;
        }
    }
}