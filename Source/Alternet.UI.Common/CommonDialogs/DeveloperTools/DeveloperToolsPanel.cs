using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

using SkiaSharp;

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

        /*/// <summary>
        /// Outputs all <see cref="Native.NativeObject"/> descendants to the debug console.
        /// </summary>
        public static void NativeObjectToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                AssemblyUtils.GetTypeDescendants(typeof(Native.NativeObject), true, false),
                (t) => Debug.WriteLine(t.Name));
        }*/

        public static void UpdateEventsPropertyGrid(PropertyGrid eventGrid, Type? type)
        {
            eventGrid.DoInsideUpdate(() =>
            {
                eventGrid.Clear();
                if (type == null)
                    return;
                var events = AssemblyUtils.EnumEvents(type, true);

                foreach (var item in events)
                {
                    if (item.DeclaringType != type)
                        continue;
                    var isBinded = LogUtils.IsEventLogged(type, item);
                    var prop = eventGrid.CreateBoolItem(item.Name, null, isBinded);
                    prop.FlagsAndAttributes.Attr["InstanceType"] = type;
                    prop.FlagsAndAttributes.Attr["EventInfo"] = item;
                    prop.PropertyChanged += Event_PropertyChanged;
                    eventGrid.Add(prop);
                }

                eventGrid.FitColumns();
            });
        }

        /// <summary>
        /// Logs <see cref="FontFamily.FamiliesNames"/>.
        /// </summary>
        public static void LogFontFamilies()
        {
            List<string> skiaNames = new(SKFontManager.Default.FontFamilies);
            skiaNames.Sort();

            var s = string.Empty;
            var names = FontFamily.FamiliesNames;
            foreach (string s2 in names)
            {
                var skia = skiaNames.IndexOf(s2) >= 0 ? "(skia)" : string.Empty;

                s += $"{s2} {skia}{Environment.NewLine}";
            }

            LogUtils.LogToFile(LogUtils.SectionSeparator);
            LogUtils.LogToFile("Font Families:");
            LogUtils.LogToFile(s);
            LogUtils.LogToFile(LogUtils.SectionSeparator);

            BaseApplication.Log($"{names.Count()} FontFamilies logged to file.");
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
                    propGrid.FitColumns();
                });
            }
            finally
            {
                insideSetProps = false;
            }
        }

        private static void Event_PropertyChanged(object? sender, EventArgs e)
        {
            if (sender is not IPropertyGridItem item)
                return;
            var type = item.FlagsAndAttributes.Attr.GetAttribute<Type?>("InstanceType");
            var eventInfo = item.FlagsAndAttributes.Attr.GetAttribute<EventInfo?>("EventInfo");
            var value = item.Owner.GetPropertyValueAsBool(item);
            LogUtils.SetEventLogged(type, eventInfo, value);
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
            UpdateEventsPropertyGrid(propGrid, type);
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
                BaseApplication.DoInsideBusyCursor(() =>
                {
                    BaseApplication.LogBeginUpdate();
                    try
                    {
                        action();
                    }
                    finally
                    {
                        BaseApplication.LogEndUpdate();
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

                BaseApplication.Log("Embedded Resource Names added to log file");

                var items = ResourceLoader.GetAssets(new Uri(s), null);
                LogUtils.LogToFile(LogUtils.SectionSeparator);
                foreach (var item in items)
                {
                    LogUtils.LogToFile(item);
                }

                LogUtils.LogToFile(LogUtils.SectionSeparator);
            });

            AddLogAction("Log Embedded Resources", () =>
            {
                LogUtils.LogResourceNames();
                BaseApplication.Log("Resource Names added to log file");
            });

            AddAction("Show Second MainForm", () =>
            {
                var type = BaseApplication.FirstWindow()?.GetType();
                var instance = Activator.CreateInstance(type ?? typeof(Window)) as Window;
                instance?.Show();
            });

            AddAction("Log test error and warning items", () =>
            {
                BaseApplication.Log("Sample error", LogItemKind.Error);
                BaseApplication.Log("Sample warning", LogItemKind.Warning);
                BaseApplication.Log("Sample info", LogItemKind.Information);
            });

            AddLogAction("Log NativeControlPainter metrics", () =>
            {
                ControlPainter.LogPartSize(this);
            });

            AddAction("Exception: Throw C++", () =>
            {
                BaseApplication.Current.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                BaseApplication.Current.SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode.CatchException);
                WebBrowser.DoCommandGlobal("CppThrow");
            });

            AddAction("Exception: Throw C#", () =>
            {
                BaseApplication.Current.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                BaseApplication.Current.SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode.CatchException);
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

            AddAction("Log SKFontManager", LogSkiaFontManager);
            AddAction("Log SKFont", LogSkiaFont);
        }

        public void LogSkiaFont()
        {
            BaseApplication.LogNameValue(
                "SKTypeface.Default.FamilyName",
                SKTypeface.Default.FamilyName);
        }

        public void LogSkiaFontManager()
        {
            List<string> wxNames = new(FontFamily.FamiliesNames);
            wxNames.Sort();

            var s = string.Empty;
            var count = SKFontManager.Default.FontFamilyCount;
            for (int i = 0; i < count; i++)
            {
                var s2 = SKFontManager.Default.GetFamilyName(i);

                var wx = wxNames.IndexOf(s2) >= 0 ? "(wx)" : string.Empty;

                s += $"{s2} {wx}{Environment.NewLine}";

                var styles = SKFontManager.Default.GetFontStyles(i);

                for (int k = 0; k < styles.Count; k++)
                {
                    var style = styles[k];
                    var fontWeight = Font.GetWeightClosestToNumericValue(style.Weight);
                    var fontWidth = Font.GetSkiaWidthClosestToNumericValue(style.Width).ToString()
                        ?? style.Width.ToString();
                    var fontSlant = style.Slant;

                    s += $"{StringUtils.FourSpaces}{fontSlant}, {fontWeight}, {fontWidth}{Environment.NewLine}";
                }
            }

            LogUtils.LogToFile(LogUtils.SectionSeparator);
            LogUtils.LogToFile("Skia Font Families:");
            LogUtils.LogToFile(s);
            LogUtils.LogToFile(LogUtils.SectionSeparator);

            BaseApplication.Log($"{count} Skia font families logged to file.");
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