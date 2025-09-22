using System;
using System.Collections.Concurrent;
using System.IO;

using Alternet.Drawing;
using Alternet.UI;

namespace WindowPropertiesSample
{
    public partial class WindowPropertiesWindow : Window
    {
        internal static RectD Position1 = (250, 250, 450, 450);
        internal static RectD Position2 = (50, 50, 500, 500);

        private readonly IconSet Icon1;
        private readonly IconSet Icon2;
        private readonly SetBoundsProperties setBoundsProperties;

        private Window? testWindow;
        ConcurrentStack<PropInstanceAndValue.SavedPropertiesItem>? SavedEnabledProperties;

        public WindowPropertiesWindow()
        {
            setBoundsProperties = new(this);
            Icon = App.DefaultIcon;

            InitializeComponent();
            pageControl.TabsVisible = true;

            stateComboBox.EnumType = typeof(WindowState);
            
            startLocationComboBox.AddEnumValues(WindowStartLocation.Default);
            startLocationComboBox.Add("250, 250, 450, 450");
            startLocationComboBox.Add("50, 50, 500, 500");
            startLocationComboBox.Value = WindowStartLocation.CenterScreen;

            sizeToContentModeComboBox.AddEnumValues(WindowSizeToContentMode.WidthAndHeight);
            UpdateControls();

            actionsPanel.Margin = 10;
            settingsPanel.Margin = 10;
            boundsPanel.Margin = 10;
            pageControl.Margin = 10;

            eventsListBox.BindApplicationLog();
            eventsListBox.ContextMenu.Required();

            SystemColorsChanged += WindowPropertiesWindow_SystemColorsChanged;
            DpiChanged += WindowPropertiesWindow_DpiChanged;

            Icon1 = new(GetIconUrl("TestIcon1.ico"));
            Icon2 = new(GetIconUrl("TestIcon2.ico"));

            panelSettings.ParentBackColor = false;
            panelSettings.BackColor = SystemColors.Window;

            panelSettings.AddLinkLabel("Disable all children except this window", () =>
            {
                SavedEnabledProperties = PropInstanceAndValue.DisableAllFormsChildrenExcept(this);
            });

            panelSettings.AddLinkLabel("Restore children enabled", () =>
            {
                PropInstanceAndValue.PopPropertiesMultiple(SavedEnabledProperties);
            });
        }

        internal string GetIconUrl(string name)
        {
            var asm = GetType().Assembly;
            var result = AssemblyUtils.GetImageUrlInAssembly(asm, "Resources." + name);
            return result;
        }

        private void WindowPropertiesWindow_DpiChanged(object? sender, DpiChangedEventArgs e)
        {
            App.Log("Dpi Changed");
        }

        private void WindowPropertiesWindow_SystemColorsChanged(object? sender, EventArgs e)
        {
            App.Log("System Colors Changed");
        }

        private void Page1Button_Click(object? sender, EventArgs e)
        {
            pageControl.SelectTab(0);
        }

        private void Page2Button_Click(object? sender, EventArgs e)
        {
            pageControl.SelectTab(1);
        }

        private void Page3Button_Click(object? sender, EventArgs e)
        {
            pageControl.SelectTab(2);
        }

        private void PopupSetBounds_AfterHide(object? sender, EventArgs e)
        {
            var rect = (
                setBoundsProperties.X,
                setBoundsProperties.Y,
                setBoundsProperties.Width,
                setBoundsProperties.Height);
            if(testWindow is not null)
                testWindow.Bounds = rect;
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
            ShowTestWindow();
        }

        private void ShowTestWindow()
        {
            if (testWindow == null)
                throw new InvalidOperationException();

            UpdateWindowState();
            UpdateControls();

            testWindow.ShowAndFocus(true);
        }

        private void CreateAndShowMiniFrameButton_Click(object sender, EventArgs e)
        {
            CreateWindowAndSetProperties(typeof(MiniFrameWindow));
            ShowTestWindow();
        }

        private void CreateAndShowModalWindowButton_Click(object sender, EventArgs e)
        {
            CreateWindowAndSetProperties(typeof(DialogWindow));

            if (testWindow is not DialogWindow dialogWindow)
                throw new InvalidOperationException();

            dialogWindow.ShowDialogAsync(this, (result) =>
            {
                App.Log("Modal Result: " + (result ? "Accepted" : "Canceled"));
                OnWindowClosed();

                dialogWindow.SendDispose();
            });
        }

        private void CreateWindowAndSetProperties(Type type, Window? parent = null)
        {
            WindowStartLocation? sLocation = null;
            var startLocationItem = startLocationComboBox.Value;

            if (startLocationItem is WindowStartLocation startLocation)
            {
                sLocation = startLocation;
            }

            if (startLocationItem is string s)
            {
                var parsed = RectD.Parse(s);
                Window.DefaultBounds = parsed;
                sLocation = WindowStartLocation.Manual;
            }

            testWindow = (Window)Activator.CreateInstance(type)!;
            if (parent is not null)
                testWindow.Parent = parent;

            if (setOwnerCheckBox.IsChecked)
                testWindow.Owner = this;

            testWindow.BeginInit();

            testWindow.Title = "Test Window";

            testWindow.ShowInTaskbar = showInTaskBarCheckBox.IsChecked;

            testWindow.MinimizeEnabled = minimizeEnabledCheckBox.IsChecked;
            testWindow.MaximizeEnabled = maximizeEnabledCheckBox.IsChecked;
            testWindow.CloseEnabled = closeEnabledCheckBox.IsChecked;

            testWindow.TopMost = alwaysOnTopCheckBox.IsChecked;
            testWindow.IsToolWindow = isToolWindowCheckBox.IsChecked;
            testWindow.Resizable = resizableCheckBox.IsChecked;
            testWindow.HasBorder = hasBorderCheckBox.IsChecked;
            testWindow.HasTitleBar = hasTitleBarCheckBox.IsChecked;
            testWindow.Layout = LayoutStyle.Vertical;

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
            testWindow.DpiChanged += TestWindow_DpiChanged;

            testWindow.Disposed += (s, e) =>
            {
                App.Log("Test Window: Disposed.");
            };

            PanelOkCancelButtons buttons = new()
            {
                Parent = testWindow,
                UseModalResult = true,
            };

            buttons.OkButton.Click += OkButton_Click;
            buttons.CancelButton.Click += CancelButton_Click;

            StdListBox listBox = new()
            {
                Parent = testWindow,
                VerticalAlignment = VerticalAlignment.Fill,
            };

            listBox.Add("Hello from test window");
        }

        private void TestWindow_DpiChanged(object? sender, DpiChangedEventArgs e)
        {
            App.Log($"DpiChanged {e}");
        }

        private void OkButton_Click(object? sender, EventArgs e)
        {
            App.Log("OK Clicked");
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            App.Log("Cancel Clicked");
        }

        private void ReportBoundsChanged(string prefix)
        {
            var b = testWindow?.Bounds;
            var s = $"{prefix} Bounds: {b}";
            App.LogReplace(s, prefix);
        }

        private void TestWindow_LocationChanged(object? sender, EventArgs e)
        {
            var s = "Test Window: BoundsChanged. ";
            ReportBoundsChanged(s);
        }

        private void TestWindow_SizeChanged(object? sender, EventArgs e)
        {
            var s = "Test Window: BoundsChanged.";
            ReportBoundsChanged(s);
        }

        private void TestWindow_StateChanged(object? sender, EventArgs e)
        {
            App.Log("Test Window: StateChanged");
            UpdateWindowState();
            if(sender is Window window)
            {
                window.OwnedWindowsVisible = !window.IsMinimized;
            }
        }

        private void UpdateWindowState()
        {
            stateComboBox.Value = testWindow?.State;
        }

        private void UpdateActiveWindowInfoLabel()
        {
            App.AddIdleTask(() =>
            {
                var title = ActiveWindow?.Title ?? "N/A";
                activeWindowTitleLabel.Text = title;

                if (testWindow != null)
                    isWindowActiveLabel.Text
                    = "Test window active: " + (testWindow.IsActive ? "Yes" : "No");
                else
                    isWindowActiveLabel.Text = string.Empty;
            });
        }

        private void TestWindow_Deactivated(object? sender, EventArgs e)
        {
            App.Log("Test Window: Deactivated");
            UpdateActiveWindowInfoLabel();
        }

        private void TestWindow_Activated(object? sender, EventArgs e)
        {
            App.Log("Test Window: Activated");
            UpdateActiveWindowInfoLabel();
        }

        private void UpdateControls()
        {
            App.AddIdleTask(Fn);

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

        private void TestWindow_Closed(object? sender, EventArgs e)
        {
            App.Log("Test Window: Closed");

            if (testWindow == null)
                throw new InvalidOperationException();
            if (testWindow.Modal)
                return;

            OnWindowClosed();
        }

        private void OnWindowClosed()
        {
            if (testWindow == null)
                return;

            testWindow.Activated -= TestWindow_Activated;
            testWindow.Deactivated -= TestWindow_Deactivated;
            testWindow.Closed -= TestWindow_Closed;
            testWindow.Closing -= TestWindow_Closing;
            testWindow.StateChanged -= TestWindow_StateChanged;
            testWindow.SizeChanged -= TestWindow_SizeChanged;
            testWindow.LocationChanged -= TestWindow_LocationChanged;
            testWindow.DpiChanged -= TestWindow_DpiChanged;

            testWindow = null;

            if (DisposingOrDisposed)
                return;

            hideWindowCheckBox.IsChecked = false;

            UpdateControls();
        }

        private void TestWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            App.Log("Test Window: Closing");
            if(!cancelClosingCheckBox.DisposingOrDisposed)
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
                testWindow.TopMost = alwaysOnTopCheckBox.IsChecked;
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

            var s1 = "Owned Window #" + testWindow.OwnedWindows.Length;

            var ownedWindow = new Window
            {
                Owner = testWindow,
                MinimumSize = (300, 300),
                IsToolWindow = true,
                Title = s1,
            };

            Label label = new(s1);
            label.Margin = 10;
            label.Parent = ownedWindow;

            ownedWindow.Disposed += (s, e) =>
            {
                App.Log($"Disposed: {s1}");
            };

            ownedWindow.Closed += (s, e) =>
            {
                App.Log($"Closed: {s1}");
            };

            ownedWindow.Closing += (s, e) =>
            {
                App.Log($"Closing: {s1}");
            };

            ownedWindow.Show();
         }

        private void StateComboBox_SelectedItemChanged(object sender, EventArgs e)
        {
            if (testWindow != null && stateComboBox.Value is WindowState windowState)
                testWindow.State = windowState;
        }

        private void SetIcon1Button_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Icon = Icon1;
        }

        private void SetIcon2Button_Click(object sender, EventArgs e)
        {
            if (testWindow != null)
                testWindow.Icon = Icon2;
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
            {
                var b = !hideWindowCheckBox.IsChecked;
                testWindow.OwnedWindowsVisible = b;
                testWindow.Visible = b;
            }
        }

        private void SetSizeToContentButton_Click(object sender, System.EventArgs e)
        {
            if(sizeToContentModeComboBox.Value is WindowSizeToContentMode mode)
                testWindow?.SetSizeToContent(mode);
        }

        public class SetBoundsProperties : BaseOwnedObject<Window>
        {
            public SetBoundsProperties(Window owner)
                : base(owner)
            {
            }

            public Coord X
            {
                get; set;
            }

            public Coord Y
            {
                get; set;
            }

            public Coord Width
            {
                get; set;
            }

            public Coord Height
            {
                get; set;
            }
        }
    }
}