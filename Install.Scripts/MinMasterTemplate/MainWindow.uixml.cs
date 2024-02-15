using Alternet.UI;

namespace MinMaster
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            logControl.BindApplicationLog();
            LogUtils.DebugLogVersion();
            Application.Log("Some test string");
        }
    }
}
