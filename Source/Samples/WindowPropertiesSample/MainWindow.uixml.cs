using Alternet.UI;
using System;

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
            testWindow.Activated += TestWindow_Activated;
            testWindow.Deactivated += TestWindow_Deactivated;
            UpdateControls();
        }

        void UpdateActiveWindowInfoLabel()
        {
            var title = ActiveWindow?.Title ?? "N/A";
            activeWindowTitleLabel.Text = "Active window title: " + title;

            if (testWindow != null)
                isWindowActiveLabel.Text = "Test window active: " + (testWindow.IsActive ? "Yes" : "No");
            else
                isWindowActiveLabel.Text = string.Empty;
        }

        private void TestWindow_Deactivated(object? sender, System.EventArgs e)
        {
            LogEvent("Deactivated");
            UpdateActiveWindowInfoLabel();
        }

        private void TestWindow_Activated(object? sender, System.EventArgs e)
        {
            LogEvent("Activated");
            UpdateActiveWindowInfoLabel();
        }

        private int lastEventNumber = 1;

        void LogEvent(string message)
        {
            if (IsDisposed)
                return;

            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }

        private void UpdateControls()
        {
            var haveTestWindow = testWindow != null;
            
            createAndShowWindowButton.Enabled = !haveTestWindow;
            activateButton.Enabled = haveTestWindow;

            UpdateActiveWindowInfoLabel();
        }

        private void TestWindow_Closed(object? sender, WindowClosedEventArgs e)
        {
            if (testWindow == null)
                throw new InvalidOperationException();

            testWindow.Activated -= TestWindow_Activated;
            testWindow.Deactivated -= TestWindow_Deactivated;
            testWindow.Closed -= TestWindow_Closed;

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

        private void ActivateButton_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Activate();
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            UpdateActiveWindowInfoLabel();
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            UpdateActiveWindowInfoLabel();
        }
    }
}