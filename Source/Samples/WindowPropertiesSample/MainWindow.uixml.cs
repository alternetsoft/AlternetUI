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

            testWindow.AlwaysOnTop = alwaysOnTopCheckBox.IsChecked;
            testWindow.IsToolWindow = isToolWindowCheckBox.IsChecked;
            testWindow.Resizable = resizableCheckBox.IsChecked;
            testWindow.HasBorder = hasBorderCheckBox.IsChecked;
            testWindow.HasTitleBar = hasTitleBarCheckBox.IsChecked;

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

        private void AlwaysOnTopCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.AlwaysOnTop = alwaysOnTopCheckBox.IsChecked;
        }

        private void IsToolWindowCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.IsToolWindow = isToolWindowCheckBox.IsChecked;
        }

        private void ResizableCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Resizable = resizableCheckBox.IsChecked;
        }

        private void HasBorderCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.HasBorder = hasBorderCheckBox.IsChecked;
        }

        private void HasTitleBarCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.HasTitleBar = hasTitleBarCheckBox.IsChecked;
        }
    }
}