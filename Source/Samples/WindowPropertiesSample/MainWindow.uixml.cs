using Alternet.UI;
using System;

namespace WindowPropertiesSample
{
    public partial class MainWindow : Window
    {
        private TestWindow? testWindow;

        private int lastEventNumber = 1;

        public MainWindow()
        {
            InitializeComponent();

            foreach (var brushType in Enum.GetValues(typeof(WindowState)))
                stateComboBox.Items.Add(brushType!);

            UpdateControls();
        }

        private void CreateAndShowWindowButton_Click(object sender, System.EventArgs e)
        {
            CreateWindowAndSetProperties();

            if (testWindow == null)
                throw new InvalidOperationException();

            testWindow.Show();

            UpdateWindowState();
            UpdateControls();
        }

        private void CreateAndShowModalWindowButton_Click(object sender, System.EventArgs e)
        {
            CreateWindowAndSetProperties();

            if (testWindow == null)
                throw new InvalidOperationException();

            testWindow.ShowModal();

            MessageBox.Show("ModalResult: " + testWindow.ModalResult);
            testWindow.Dispose();
            OnWindowClosed();
        }

        private void CreateWindowAndSetProperties()
        {
            testWindow = new TestWindow();

            if (setOwnerCheckBox.IsChecked)
                testWindow.Owner = this;

            testWindow.BeginInit();

            testWindow.ShowInTaskbar = showInTaskBarCheckBox.IsChecked;

            testWindow.MinimizeEnabled = minimizeEnabledCheckBox.IsChecked;
            testWindow.MaximizeEnabled = maximizeEnabledCheckBox.IsChecked;
            testWindow.CloseEnabled = closeEnabledCheckBox.IsChecked;

            testWindow.AlwaysOnTop = alwaysOnTopCheckBox.IsChecked;
            testWindow.IsToolWindow = isToolWindowCheckBox.IsChecked;
            testWindow.Resizable = resizableCheckBox.IsChecked;
            testWindow.HasBorder = hasBorderCheckBox.IsChecked;
            testWindow.HasTitleBar = hasTitleBarCheckBox.IsChecked;

            testWindow.EndInit();

            testWindow.Closed += TestWindow_Closed;
            testWindow.Closing += TestWindow_Closing;
            testWindow.Activated += TestWindow_Activated;
            testWindow.Deactivated += TestWindow_Deactivated;
            testWindow.StateChanged += TestWindow_StateChanged;
        }

        private void TestWindow_StateChanged(object? sender, EventArgs e)
        {
            LogEvent("StateChanged");
            UpdateWindowState();
        }

        private void UpdateWindowState()
        {
            stateComboBox.SelectedItem = (testWindow ?? throw new Exception()).State;
        }

        private void UpdateActiveWindowInfoLabel()
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

        private void LogEvent(string message)
        {
            if (eventsListBox.IsDisposed)
                return;

            eventsListBox.Items.Add($"{lastEventNumber++}. {message}");
            eventsListBox.SelectedIndex = eventsListBox.Items.Count - 1;
        }

        private void UpdateControls()
        {
            var haveTestWindow = testWindow != null;

            createAndShowWindowButton.Enabled = !haveTestWindow;
            activateButton.Enabled = haveTestWindow;
            addOwnedWindow.Enabled = haveTestWindow;
            stateComboBox.Enabled = haveTestWindow;

            setIcon1Button.Enabled = haveTestWindow;
            setIcon2Button.Enabled = haveTestWindow;

            UpdateActiveWindowInfoLabel();
        }

        private void TestWindow_Closed(object? sender, WindowClosedEventArgs e)
        {
            LogEvent("Closed");

            if (testWindow == null)
                throw new InvalidOperationException();
            if (testWindow.Modal)
                return;

            OnWindowClosed();
        }

        private void OnWindowClosed()
        {
            if (testWindow == null)
                throw new InvalidOperationException();

            testWindow.Activated -= TestWindow_Activated;
            testWindow.Deactivated -= TestWindow_Deactivated;
            testWindow.Closed -= TestWindow_Closed;
            testWindow.Closing -= TestWindow_Closing;
            testWindow.StateChanged -= TestWindow_StateChanged;

            testWindow = null;
            UpdateControls();
        }

        private void TestWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            LogEvent("Closing");
            e.Cancel = cancelClosingCheckBox.IsChecked;
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

        private void AddOwnedWindow_Click(object sender, System.EventArgs e)
        {
            if (testWindow == null)
                return;

            var ownedWindow = new OwnedWindow();
            ownedWindow.Owner = testWindow;

            ownedWindow.SetLabel("Owned Window #" + testWindow.OwnedWindows.Length);
            ownedWindow.Show();
        }

        private void StateComboBox_SelectedItemChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.State = (WindowState)(stateComboBox.SelectedItem ?? throw new Exception());
        }

        private void SetIcon1Button_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Icon = Icons.Icon1;
        }

        private void SetIcon2Button_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Icon = Icons.Icon2;
        }
    }
}