using System;
using Alternet.Drawing;
using Alternet.UI;

namespace WindowPropertiesSample
{
    public partial class MainWindow : Window
    {
        private readonly CardPanelHeader panelHeader;
        /*private readonly PopupPropertyGrid popupSetBounds;*/
        /*private readonly PopupPropertyGrid popupWindowProps;*/
        private readonly SetBoundsProperties setBoundsProperties;
        private TestWindow? testWindow;

        public MainWindow()
        {
            panelHeader = new();
            /*popupSetBounds = new();*/
            setBoundsProperties = new(this);
            Icon = new("embres:WindowPropertiesSample.Sample.ico");

            InitializeComponent();

            stateComboBox.AddEnumValues<WindowState>();
            startLocationComboBox.AddEnumValues(WindowStartLocation.Default);
            sizeToContentModeComboBox.AddEnumValues(WindowSizeToContentMode.WidthAndHeight);
            UpdateControls();

            panelHeader.Add("Actions", actionsPanel);
            panelHeader.Add("Settings", settingsPanel);
            panelHeader.Add("Bounds", boundsPanel);
            pageControl.Children.Prepend(panelHeader);
            panelHeader.SelectFirstTab();

            eventsListBox.BindApplicationLog();
            eventsListBox.ContextMenu.Required();

            /*popupSetBounds.IsTransient = false;
            popupSetBounds.AfterHide += PopupSetBounds_AfterHide;
            popupSetBounds.MainControl.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue;
            popupSetBounds.HideOnEnter = false;
            popupSetBounds.HideOnDeactivate = false;
            popupSetBounds.HideOnClick = false;
            popupSetBounds.HideOnDoubleClick = false;
            popupSetBounds.MainControl.SuggestedInitDefaults();

            popupWindowProps = PopupPropertyGrid.CreatePropertiesPopup();*/
        }

        private void PopupSetBounds_AfterHide(object? sender, EventArgs e)
        {
            var rect = (setBoundsProperties.X, setBoundsProperties.Y, setBoundsProperties.Width, setBoundsProperties.Height);
            testWindow?.SetBounds(rect, setBoundsProperties.Flags);
        }

        private void PropertiesButton_Click(object? sender, EventArgs e)
        {
            if (testWindow is null)
                return;
            /*popupWindowProps.MainControl.SetProps(testWindow, true);
            popupWindowProps.ShowPopup(propertiesButton);*/
        }

        private void SetBoundsExButton_Click(object? sender, EventArgs e)
        {
            if (testWindow is null)
                return;
            /*setBoundsProperties.X = testWindow.Location.X;
            setBoundsProperties.Y = testWindow.Location.Y;
            setBoundsProperties.Width = testWindow.Size.Width;
            setBoundsProperties.Height = testWindow.Size.Height;
            popupSetBounds.MainControl.SuggestedSize = (400, 400);
            popupSetBounds.SetSizeToContent();
            popupSetBounds.MainControl.SetProps(setBoundsProperties);
            var flagsItem = popupSetBounds.MainControl.GetProperty("Flags");
            popupSetBounds.MainControl.Expand(flagsItem);
            popupSetBounds.ShowPopup(setBoundsExButton);*/
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

            Application.Log("ModalResult: " + testWindow.ModalResult);
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
            testWindow.Location = new PointD();

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
            var s = "LocationChanged. Bounds: ";
            Application.LogReplace(s + testWindow?.Bounds.ToString(), s);
        }

        private void TestWindow_SizeChanged(object? sender, EventArgs e)
        {
            var s = "SizeChanged. Bounds: ";
            Application.LogReplace(s + testWindow?.Bounds.ToString(), s);
        }

        private void TestWindow_StateChanged(object? sender, EventArgs e)
        {
            Application.Log("StateChanged");
            UpdateWindowState();
        }

        private void UpdateWindowState()
        {
            stateComboBox.SelectedItem = testWindow?.State;
        }

        private void UpdateActiveWindowInfoLabel()
        {
            var title = ActiveWindow?.Title ?? "N/A";
            activeWindowTitleLabel.Text = title;

            if (testWindow != null)
                isWindowActiveLabel.Text = "Test window active: " + (testWindow.IsActive ? "Yes" : "No");
            else
                isWindowActiveLabel.Text = string.Empty;
        }

        private void TestWindow_Deactivated(object? sender, EventArgs e)
        {
            Application.Log("Deactivated");
            UpdateActiveWindowInfoLabel();
        }

        private void TestWindow_Activated(object? sender, EventArgs e)
        {
            Application.Log("Activated");
            UpdateActiveWindowInfoLabel();
        }

        private void UpdateControls()
        {
            var haveTestWindow = testWindow != null;

            Group(
                createAndShowWindowButton,
                createAndShowModalWindowButton,
                startLocationComboBox).Enabled(!haveTestWindow);

            Group(
                hideWindowCheckBox,
                activateButton,
                addOwnedWindow,
                stateComboBox,
                sizeToContentModeComboBox,
                setSizeToContentButton,
                setSizeButton,
                setClientSizeButton,
                setBoundsButton,
                increaseLocationButton,
                setMinMaxSizeButton,
                setIcon1Button,
                setIcon2Button,
                clearIconButton,
                setBoundsExButton,
                propertiesButton).Enabled(haveTestWindow);

            propertiesButton.Enabled = false;
            setBoundsExButton.Enabled = false;

            if (!haveTestWindow)
                currentBoundsLabel.Text = string.Empty;

            UpdateActiveWindowInfoLabel();
        }

        private void TestWindow_Closed(object? sender, WindowClosedEventArgs e)
        {
            Application.Log("Closed");

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
            Application.Log("Closing");
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
            testWindow?.Activate();
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

            var ownedWindow = new OwnedWindow
            {
                Owner = testWindow
            };

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

        private void ClearIconButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Icon = null;
        }

        private void SetSizeButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Size = new SizeD(300, 300);
        }

        private void SetClientSizeButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.ClientSize = new SizeD(300, 300);
        }

        private void IncreaseLocationButton_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Location += new SizeD(10, 10);
        }

        private void SetBoundsButton_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Bounds = new RectD(0, 0, 400, 400);
        }

        private void SetMinMaxSizeButton_Click(object sender, System.EventArgs e)
        {
            if (testWindow != null)
            {
                testWindow.MinimumSize = new SizeD(100, 100);
                testWindow.MaximumSize = new SizeD(300, 300);
            }
        }

        private void HideWindowCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (testWindow != null)
                testWindow.Visible = !hideWindowCheckBox.IsChecked;
        }

        private void SetSizeToContentButton_Click(object sender, System.EventArgs e)
        {
            testWindow?.SetSizeToContent(
                (WindowSizeToContentMode)sizeToContentModeComboBox.SelectedItem!);
        }

        public class SetBoundsProperties : BaseChildObject<MainWindow>
        {
            public SetBoundsProperties(MainWindow owner)
                : base(owner)
            {
            }

            public double X
            {
                get; set;
            }

            public double Y
            {
                get; set;
            }

            public double Width
            {
                get; set;
            }

            public double Height
            {
                get; set;
            }

            public SetBoundsFlags Flags
            {
                get; set;
            }
        }
    }
}