using Alternet.Drawing;
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
            Icon = ImageSet.FromUrlOrNull(
                "embres:WindowPropertiesSample.Sample.ico");

            InitializeComponent();

            stateComboBox.AddEnumValues(typeof(WindowState));
            startLocationComboBox.AddEnumValues(typeof(WindowStartLocation),
                WindowStartLocation.Default);
            sizeToContentModeComboBox.AddEnumValues(typeof(WindowSizeToContentMode),
                WindowSizeToContentMode.WidthAndHeight);
            UpdateControls();
        }

        private void CreateAndShowWindowButton_Click(object sender, EventArgs e)
        {
            CreateWindowAndSetProperties();

            if (testWindow == null)
                throw new InvalidOperationException();

            testWindow.Show();

            UpdateWindowState();
            UpdateControls();
        }

        private void CreateAndShowModalWindowButton_Click(object sender, EventArgs e)
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

            testWindow.StartLocation = (WindowStartLocation)startLocationComboBox.SelectedItem!;
            testWindow.Location = new Point();

            testWindow.EndInit();

            testWindow.Closed += TestWindow_Closed;
            testWindow.Closing += TestWindow_Closing;
            testWindow.Activated += TestWindow_Activated;
            testWindow.Deactivated += TestWindow_Deactivated;
            testWindow.StateChanged += TestWindow_StateChanged;
            testWindow.SizeChanged += TestWindow_SizeChanged;
            testWindow.LocationChanged += TestWindow_LocationChanged;
        }

        private void TestWindow_LocationChanged(object? sender, EventArgs e)
        {
            LogEvent("LocationChanged. Bounds: "+testWindow?.Bounds.ToString());
        }

        private void TestWindow_SizeChanged(object? sender, EventArgs e)
        {
            LogEvent("SizeChanged. Bounds: "+testWindow?.Bounds.ToString());
        }

        private void TestWindow_StateChanged(object? sender, EventArgs e)
        {
            LogEvent("StateChanged"); 
            UpdateWindowState();
        }

        private void UpdateWindowState()
        {
            stateComboBox.SelectedItem = testWindow?.State;
        }

        private void UpdateActiveWindowInfoLabel()
        {
            var title = ActiveWindow?.Title ?? "N/A";
            activeWindowTitleLabel.Text =  title;

            if (testWindow != null)
                isWindowActiveLabel.Text = "Test window active: " + (testWindow.IsActive ? "Yes" : "No");
            else
                isWindowActiveLabel.Text = string.Empty;
        }

        private void TestWindow_Deactivated(object? sender, EventArgs e)
        {
            LogEvent("Deactivated");
            UpdateActiveWindowInfoLabel();
        }

        private void TestWindow_Activated(object? sender, EventArgs e)
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

            hideWindowCheckBox.Enabled = haveTestWindow;
            createAndShowWindowButton.Enabled = !haveTestWindow;
            createAndShowModalWindowButton.Enabled = !haveTestWindow;
            startLocationComboBox.Enabled = !haveTestWindow;

            activateButton.Enabled = haveTestWindow;
            addOwnedWindow.Enabled = haveTestWindow;
            stateComboBox.Enabled = haveTestWindow;

            sizeToContentModeComboBox.Enabled = haveTestWindow;
            setSizeToContentButton.Enabled = haveTestWindow;
            setSizeButton.Enabled = haveTestWindow;
            setClientSizeButton.Enabled = haveTestWindow;
            setBoundsButton.Enabled = haveTestWindow;
            increaseLocationButton.Enabled = haveTestWindow;
            setMinMaxSizeButton.Enabled = haveTestWindow;
            if (!haveTestWindow)
                currentBoundsLabel.Text = string.Empty;

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
            testWindow.SizeChanged -= TestWindow_SizeChanged;
            testWindow.LocationChanged -= TestWindow_LocationChanged;

            testWindow = null;

            hideWindowCheckBox.IsChecked = false;

            UpdateControls();
        }

        private void TestWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            LogEvent("Closing");
            e.Cancel = cancelClosingCheckBox.IsChecked;
        }

        private void ShowInTaskBarCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.ShowInTaskbar = showInTaskBarCheckBox.IsChecked;
        }

        private void MinimizeEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.MinimizeEnabled = minimizeEnabledCheckBox.IsChecked;
        }

        private void MaximizeEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.MaximizeEnabled = maximizeEnabledCheckBox.IsChecked;
        }

        private void CloseEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.CloseEnabled = closeEnabledCheckBox.IsChecked;
        }

        private void AlwaysOnTopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.AlwaysOnTop = alwaysOnTopCheckBox.IsChecked;
        }

        private void IsToolWindowCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.IsToolWindow = isToolWindowCheckBox.IsChecked;
        }

        private void ResizableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Resizable = resizableCheckBox.IsChecked;
        }

        private void HasBorderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.HasBorder = hasBorderCheckBox.IsChecked;
        }

        private void HasTitleBarCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.HasTitleBar = hasTitleBarCheckBox.IsChecked;
        }

        private void ActivateButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Activate();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            UpdateActiveWindowInfoLabel();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            UpdateActiveWindowInfoLabel();
        }

        private void AddOwnedWindow_Click(object sender, EventArgs e)
        {
            if (testWindow == null)
                return;

            var ownedWindow = new OwnedWindow();
            ownedWindow.Owner = testWindow;

            ownedWindow.SetLabel("Owned Window #" + testWindow.OwnedWindows.Length);
            ownedWindow.Show();
        }

        private void StateComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.State = (WindowState)(stateComboBox.SelectedItem ?? throw new Exception());
        }

        private void SetIcon1Button_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Icon = Icons.Icon1;
        }

        private void SetIcon2Button_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Icon = Icons.Icon2;
        }

        private void SetSizeButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Size = new Size(300, 300);
        }

        private void SetClientSizeButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.ClientSize = new Size(300, 300);
        }

        private void IncreaseLocationButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Location += new Size(10, 10);
        }

        private void SetBoundsButton_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Bounds = new Rect(0, 0, 400, 400);
        }

        private void SetMinMaxSizeButton_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
            {
                testWindow.MinimumSize = new Size(100, 100);
                testWindow.MaximumSize = new Size(300, 300);
            }
        }

        private void HideWindowCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Visible = !hideWindowCheckBox.IsChecked;
        }

        private void SetSizeToContentButton_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.SetSizeToContent((WindowSizeToContentMode)sizeToContentModeComboBox.SelectedItem!);
        }
    }
}