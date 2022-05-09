using Alternet.UI;

namespace WindowPropertiesSample
{
    public partial class MainWindow : Window
    {
        private TestWindow? testWindow;

        public MainWindow()
        {
            InitializeComponent();

            UpdateControls();
        }

        private void CreateAndShowWindowButton_Click(object sender, System.EventArgs e)
        {
            testWindow = new TestWindow();
            
            if (setOwnerCheckBox.IsChecked)
                testWindow.Owner = this;

            testWindow.ShowInTaskbar = showInTaskBarCheckBox.IsChecked;

            testWindow.MinimizeEnabled = minimizeEnabledCheckBox.IsChecked;
            testWindow.MaximizeEnabled = maximizeEnabledCheckBox.IsChecked;
            testWindow.CloseEnabled = closeEnabledCheckBox.IsChecked;

            testWindow.Show();
            testWindow.Closed += TestWindow_Closed;
            UpdateControls();
        }

        private void UpdateControls()
        {
            createAndShowWindowButton.Enabled = testWindow == null;
        }

        private void TestWindow_Closed(object? sender, WindowClosedEventArgs e)
        {
            testWindow = null;
            UpdateControls();
        }

        private void ShowInTaskBarCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.ShowInTaskbar = showInTaskBarCheckBox.IsChecked;
        }

        private void MinimizeEnabledCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.MinimizeEnabled = minimizeEnabledCheckBox.IsChecked;
        }

        private void MaximizeEnabledCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.MaximizeEnabled = maximizeEnabledCheckBox.IsChecked;
        }

        private void CloseEnabledCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.CloseEnabled = closeEnabledCheckBox.IsChecked;
        }
    }
}