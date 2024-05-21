using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class DeveloperToolsPanel : Control
    {
        private static DeveloperToolsWindow? devToolsWindow;

        private readonly ActionsListBox actionsListBox = new()
        {
            HasBorder = false,
        };

        private readonly LogListBox logListBox = new()
        {
            HasBorder = false,
        };

        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            LeftVisible = false,
            BottomVisible = false,
            RightPanelWidth = 350,
        };

        private readonly PropertyGrid propGrid = new()
        {
        };

        private readonly SideBarPanel centerNotebook = new()
        {
        };

        private readonly SideBarPanel rightNotebook = new()
        {
        };

        private ListBox? typesListBox;
        private ListBox? controlsListBox;
        private bool insideSetProps;

        public DeveloperToolsPanel()
        {
            centerNotebook.Parent = panel.FillPanel;
            rightNotebook.Parent = panel.RightPanel;
            panel.Parent = this;

            centerNotebook.Add("Output", logListBox);
            logListBox.ContextMenu.Required();
            logListBox.MenuItemShowDevTools?.SetEnabled(false);
            logListBox.BindApplicationLog();

            rightNotebook.Add("Actions", actionsListBox);

            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propGrid.Features = PropertyGridFeature.QuestionCharInNullable;
            propGrid.ProcessException += PropertyGrid_ProcessException;
            propGrid.CreateStyleEx = PropertyGridCreateStyleEx.AlwaysAllowFocus;
            rightNotebook.Add("Properties", propGrid);

            InitActions();

            TypesListBox.SelectionChanged += TypesListBox_SelectionChanged;
        }

        public SideBarPanel CenterNotebook => centerNotebook;

        public SideBarPanel RightNotebook => rightNotebook;

        public PropertyGrid PropGrid => propGrid;

        internal object? LastFocusedControl { get; set; }

        [Browsable(false)]
        internal ListBox TypesListBox
        {
            get
            {
                if (typesListBox == null)
                {
                    typesListBox = new ListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };

                    IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));

                    void AddControl(Type type)
                    {
                        typesListBox.Add(type.Name, type);
                    }

                    AddControl(typeof(Control));
                    AddControl(typeof(FrameworkElement));
                    AddControl(typeof(UIElement));

                    foreach (var type in result)
                    {
                        if (type.Assembly != typeof(Control).Assembly)
                            continue;
                        if (!AssemblyUtils.HasOwnEvents(type))
                            continue;
                        AddControl(type);
                    }

                    centerNotebook.Add("Types", typesListBox);
                }

                return typesListBox;
            }
        }

        [Browsable(false)]
        internal ListBox ControlsListBox
        {
            get
            {
                if (controlsListBox == null)
                {
                    controlsListBox = new ListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };
                    centerNotebook.Add("Controls", controlsListBox);
                }

                return controlsListBox;
            }
        }

        /// <summary>
        /// Logs <see cref="SystemSettings"/>.
        /// </summary>
        public static void LogSystemSettings()
        {
            BaseApplication.LogBeginSection();
            BaseApplication.Log($"IsDark = {SystemSettings.AppearanceIsDark}");
            BaseApplication.Log($"IsUsingDarkBackground = {SystemSettings.IsUsingDarkBackground}");
            BaseApplication.Log($"AppearanceName = {SystemSettings.AppearanceName}");

            var defaultColors = Control.GetStaticDefaultFontAndColor(ControlTypeId.TextBox);
            LogUtils.LogColor("TextBox.ForegroundColor (defaults)", defaultColors.ForegroundColor);
            LogUtils.LogColor("TextBox.BackgroundColor (defaults)", defaultColors.BackgroundColor);

            BaseApplication.Log($"CPP.SizeOfLong = {WebBrowser.DoCommandGlobal("SizeOfLong")}");
            BaseApplication.Log($"CPP.IsDebug = {WebBrowser.DoCommandGlobal("IsDebug")}");

            BaseApplication.LogSeparator();

            foreach (SystemSettingsFeature item in Enum.GetValues(typeof(SystemSettingsFeature)))
            {
                BaseApplication.Log($"HasFeature({item}) = {SystemSettings.HasFeature(item)}");
            }

            BaseApplication.LogSeparator();

            foreach (SystemSettingsMetric item in Enum.GetValues(typeof(SystemSettingsMetric)))
            {
                BaseApplication.Log($"GetMetric({item}) = {SystemSettings.GetMetric(item)}");
            }

            BaseApplication.LogSeparator();

            foreach (SystemSettingsFont item in Enum.GetValues(typeof(SystemSettingsFont)))
            {
                BaseApplication.Log($"GetFont({item}) = {SystemSettings.GetFont(item)}");
            }

            BaseApplication.LogEndSection();
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            if (devToolsWindow is null)
            {
                devToolsWindow = new();
                devToolsWindow.Closing += DevToolsWindow_Closing;
                devToolsWindow.Disposed += DevToolsWindow_Disposed;
            }

            devToolsWindow.Show();

            static void DevToolsWindow_Closing(object? sender, WindowClosingEventArgs e)
            {
            }

            static void DevToolsWindow_Disposed(object? sender, EventArgs e)
            {
                devToolsWindow = null;
            }
        }

        /// <summary>
        /// Outputs all <see cref="Control"/> descendants to the debug console.
        /// </summary>
        public static void ControlsToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                AssemblyUtils.GetTypeDescendants(typeof(Control)),
                (t) => Debug.WriteLine(t.Name));
        }

        /// <summary>
        /// Outputs all <see cref="Native.NativeObject"/> descendants to the debug console.
        /// </summary>
        public static void NativeObjectToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                AssemblyUtils.GetTypeDescendants(typeof(Native.NativeObject), true, false),
                (t) => Debug.WriteLine(t.Name));
        }

        /// <summary>
        /// Logs <see cref="FontFamily.FamiliesNames"/>.
        /// </summary>
        public static void LogFontFamilies()
        {
            var s = string.Empty;
            foreach (string s2 in FontFamily.FamiliesNames)
            {
                s += s2 + Environment.NewLine;
            }

            LogUtils.LogToFile(LogUtils.SectionSeparator);
            LogUtils.LogToFile("Font Families:");
            LogUtils.LogToFile(s);
            LogUtils.LogToFile(LogUtils.SectionSeparator);

            BaseApplication.Log("FontFamilies logged to file.");
        }

        public void AddAction(string title, Action? action)
        {
            actionsListBox.AddAction(title, action);
        }

        internal void PropGridSetProps(object? instance)
        {
            if (insideSetProps)
                return;
            insideSetProps = true;
            try
            {
                propGrid.DoInsideUpdate(() =>
                {
                    propGrid.Clear();
                    if (instance is null)
                        return;
                    propGrid.AddConstItem("(type)", "(type)", instance.GetType().Name);
                    propGrid.AddProps(instance, null, true);
                });
            }
            finally
            {
                insideSetProps = false;
            }
        }

        private static void LogOSInformation()
        {
            var os = Environment.OSVersion;
            BaseApplication.Log("Current OS Information:\n");
            BaseApplication.Log($"Platform: {os.Platform:G}");
            BaseApplication.Log($"Version String: {os.VersionString}");
            BaseApplication.Log($"Major version: {os.Version.Major}");
            BaseApplication.Log($"Minor version: {os.Version.Minor}");
            BaseApplication.Log($"Service Pack: '{os.ServicePack}'");
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            rightNotebook.SelectedControl = propGrid;
            controlsListBox?.SelectedAction?.Invoke();
        }

        private void PropertyGrid_ProcessException(object? sender, ControlExceptionEventArgs e)
        {
            BaseApplication.LogFileIsEnabled = true;
            LogUtils.LogException(e.InnerException);
        }

        private void ControlsActionMainForm()
        {
            rightNotebook.SelectedControl = propGrid;
            PropGridSetProps(BaseApplication.FirstWindow());
        }

        private void ControlsActionFocusedControl()
        {
            PropGridSetProps(LastFocusedControl);
        }

        private void TypesListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var item = TypesListBox.SelectedItem as ListControlItem;
            var type = item?.Value as Type;
            EventLogManager.UpdateEventsPropertyGrid(propGrid, type);
        }

        private void LogUsefulDefines()
        {
            var s = WebBrowser.DoCommandGlobal("GetUsefulDefines");
            var splitted = s?.Split(' ');
            LogUtils.LogAsSection(splitted);
        }

        private void AddLogAction(string title, Action action)
        {
            actionsListBox.AddAction(title, Fn);

            void Fn()
            {
                Application.DoInsideBusyCursor(() =>
                {
                    Application.LogBeginUpdate();
                    try
                    {
                        action();
                    }
                    finally
                    {
                        Application.LogEndUpdate();
                    }
                });
            }
        }

        private void InitActions()
        {
            AddLogAction("Log system settings", LogSystemSettings);
            AddLogAction("Log font families", LogFontFamilies);
            AddLogAction("Log system fonts", SystemSettings.LogSystemFonts);
            AddLogAction("Log fixed width fonts", SystemSettings.LogFixedWidthFonts);
            AddLogAction("Log display info", Display.Log);
            AddLogAction("Log control info", LogControlInfo);
            AddLogAction("Log useful defines", LogUsefulDefines);
            AddLogAction("Log OS information", LogOSInformation);
            AddLogAction("Log system colors", LogUtils.LogSystemColors);

            AddAction("Show Props FirstWindow", ControlsActionMainForm);
            AddAction("Show Props FocusedControl", ControlsActionFocusedControl);

            AddLogAction("Log Embedded Resources in Alternet.UI", () =>
            {
                const string s = "embres:Alternet.UI?assembly=Alternet.UI";

                Application.Log("Embedded Resource Names added to log file");

                var items = ResourceLoader.GetAssets(new Uri(s), null);
                LogUtils.LogToFile(LogUtils.SectionSeparator);
                foreach (var item in items)
                {
                    LogUtils.LogToFile(item);
                }

                LogUtils.LogToFile(LogUtils.SectionSeparator);
            });

            AddAction("Show Second MainForm", () =>
            {
                var type = Application.FirstWindow()?.GetType();
                var instance = Activator.CreateInstance(type ?? typeof(Window)) as Window;
                instance?.Show();
            });

            AddAction("Log test error and warning items", () =>
            {
                Application.Log("Sample error", LogItemKind.Error);
                Application.Log("Sample warning", LogItemKind.Warning);
                Application.Log("Sample info", LogItemKind.Information);
            });

            AddLogAction("Log NativeControlPainter metrics", () =>
            {
                NativeControlPainter.Default.LogPartSize(this);
            });

            AddAction("Exception: Throw C++", () =>
            {
                Application.Current.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.Current.SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode.CatchException);
                WebBrowser.DoCommandGlobal("CppThrow");
            });

            AddAction("Exception: Throw C#", () =>
            {
                Application.Current.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.Current.SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode.CatchException);
                throw new FileNotFoundException("Test message", "MyFileName.dat");
            });

            AddAction("Exception: Show ThreadExceptionWindow", () =>
            {
                try
                {
                    throw new ApplicationException("This is exception message");
                }
                catch (Exception e)
                {
                    BaseApplication.ShowExceptionWindow(e, "This is an additional info", true);
                }
            });

            AddAction("Exception: HookExceptionEvents()", DebugUtils.HookExceptionEvents);
        }

        private void LogControlInfo()
        {
            BaseApplication.Log($"Toolbar images: {ToolBarUtils.GetDefaultImageSize(this)}");
            Log($"Control.DefaultFont: {Control.DefaultFont.ToInfoString()}");
            Log($"Font.Default: {Font.Default.ToInfoString()}");
            Log($"Splitter.MinSashSize: {AllPlatformDefaults.PlatformCurrent.MinSplitterSashSize}");
        }
    }
}