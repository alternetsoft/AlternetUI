using System;
using Alternet.Drawing;
using Alternet.UI;

namespace WindowPropertiesSample
{
    public partial class MainWindow : Window
    {
        private readonly CardPanelHeader panelHeader;
        private readonly SetBoundsProperties setBoundsProperties;

        private Window? testWindow;

        public MainWindow()
        {
            panelHeader = new();
            setBoundsProperties = new(this);
            Icon = new("embres:WindowPropertiesSample.Sample.ico");

            InitializeComponent();

            stateComboBox.AddEnumValues<WindowState>();
            startLocationComboBox.AddEnumValues(WindowStartLocation.Default);
            startLocationComboBox.Add("250, 250, 450, 450");
            startLocationComboBox.Add("50,50,500,500");
            sizeToContentModeComboBox.AddEnumValues(WindowSizeToContentMode.WidthAndHeight);
            UpdateControls();

            panelHeader.Add("Actions", actionsPanel);
            panelHeader.Add("Settings", settingsPanel);
            panelHeader.Add("Bounds", boundsPanel);
            pageControl.Children.Prepend(panelHeader);
            panelHeader.SelectFirstTab();

            eventsListBox.BindApplicationLog();
            eventsListBox.ContextMenu.Required();
        }

        private void Page1Button_Click(object? sender, EventArgs e)
        {
            panelHeader.SelectedTabIndex = 0;
        }

        private void Page2Button_Click(object? sender, EventArgs e)
        {
            panelHeader.SelectedTabIndex = 1;
        }

        private void Page3Button_Click(object? sender, EventArgs e)
        {
            panelHeader.SelectedTabIndex = 2;
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
            WindowPropertyGrid.ShowDefault(null, testWindow);
        }

        private void CreateAndShowWindowButton_Click(object sender, EventArgs e)
        {
            CreateWindowAndSetProperties(typeof(Window));

            if (testWindow == null)
                throw new InvalidOperationException();

            UpdateWindowState();
            UpdateControls();

            testWindow.Show();
        }

        private void CreateAndShowMiniFrameButton_Click(object sender, EventArgs e)
        {
            CreateWindowAndSetProperties(typeof(MiniFrameWindow));

            if (testWindow == null)
                throw new InvalidOperationException();

            UpdateWindowState();
            UpdateControls();

            testWindow.Show();
        }

        private void CreateAndShowModalWindowButton_Click(object sender, EventArgs e)
        {
            CreateWindowAndSetProperties(typeof(DialogWindow));

            if (testWindow is not DialogWindow dialogWindow)
                throw new InvalidOperationException();

            dialogWindow.ShowModal();

            Application.Log("ModalResult: " + dialogWindow.ModalResult);
            dialogWindow.Dispose();
            OnWindowClosed();
        }

        private void CreateWindowAndSetProperties(Type type)
        {
            WindowStartLocation? sLocation = null;
            var startLocationItem = startLocationComboBox.SelectedItem;

            if (startLocationItem is WindowStartLocation)
            {
                sLocation = (WindowStartLocation)startLocationComboBox.SelectedItem!;
            }

            if (startLocationItem is string s)
            {
                var parsed = RectD.Parse(s);
                Window.DefaultBounds = parsed;
                sLocation = WindowStartLocation.Manual;
            }

            testWindow = (Window)Activator.CreateInstance(type)!;

            if (setOwnerCheckBox.IsChecked)
                testWindow.Owner = this;

            testWindow.BeginInit();

            testWindow.Title = "Test Window";

            testWindow.ShowInTaskbar = showInTaskBarCheckBox.IsChecked;

            testWindow.MinimizeEnabled = minimizeEnabledCheckBox.IsChecked;
            testWindow.MaximizeEnabled = maximizeEnabledCheckBox.IsChecked;
            testWindow.CloseEnabled = closeEnabledCheckBox.IsChecked;

            testWindow.AlwaysOnTop = alwaysOnTopCheckBox.IsChecked;
            testWindow.IsToolWindow = isToolWindowCheckBox.IsChecked;
            testWindow.Resizable = resizableCheckBox.IsChecked;
            testWindow.HasBorder = hasBorderCheckBox.IsChecked;
            testWindow.HasTitleBar = hasTitleBarCheckBox.IsChecked;

            if (sLocation is not null)
            {
                testWindow.StartLocation = sLocation.Value;
            }

            testWindow.EndInit();

            testWindow.Closed += TestWindow_Closed;
            testWindow.Closing += TestWindow_Closing;
            testWindow.Activated += TestWindow_Activated;
            testWindow.Deactivated += TestWindow_Deactivated;
            testWindow.StateChanged += TestWindow_StateChanged;
            testWindow.SizeChanged += TestWindow_SizeChanged;
            testWindow.LocationChanged += TestWindow_LocationChanged;

            VerticalStackPanel panel = new()
            {
                Padding = 10,
                Parent = testWindow,
                AllowStretch = true,
            };

            PanelOkCancelButtons buttons = new()
            {
                Parent = panel,
                UseModalResult = true,
            };

            buttons.OkButton.Click += OkButton_Click;
            buttons.CancelButton.Click += CancelButton_Click;

            ListBox listBox = new()
            {
                Parent = panel,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            Application.Log("OK Clicked");
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            Application.Log("Cancel Clicked");
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
            Application.AddIdleTask(() =>
            {
                var title = ActiveWindow?.Title ?? "N/A";
                activeWindowTitleLabel.Text = title;

                if (testWindow != null)
                    isWindowActiveLabel.Text = "Test window active: " + (testWindow.IsActive ? "Yes" : "No");
                else
                    isWindowActiveLabel.Text = string.Empty;
            });
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
            Application.AddIdleTask(Fn);

            void Fn(object? data)
            {
                var haveTestWindow = testWindow != null;

                if(eventsListBox.CanFocus)
                    eventsListBox.SetFocus();

                Group(
                    createAndShowWindowButton,
                    createAndShowMiniFrameButton,
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
                    propertiesButton).Enabled(haveTestWindow);

                if (!haveTestWindow)
                    currentBoundsLabel.Text = string.Empty;

                UpdateActiveWindowInfoLabel();
            }
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