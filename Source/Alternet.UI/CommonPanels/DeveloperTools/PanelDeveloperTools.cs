﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class PanelDeveloperTools : PanelAuiManager
    {
        private readonly IAuiNotebookPage? mainLogPage;

        private readonly LogListBox mainLogListBox = new()
        {
            HasBorder = false,
        };

        private ListBox? typesListBox;
        private IAuiNotebookPage? typesPage;
        private ListBox? controlsListBox;
        private IAuiNotebookPage? controlsPage;
        private bool insideSetProps;

        public PanelDeveloperTools()
            : base()
        {
            DefaultRightPaneBestSize = (350, 200);
            DefaultRightPaneMinSize = (350, 200);

            CenterNotebookDefaultCreateStyle = AuiNotebookCreateStyle.Top;
            RightNotebookDefaultCreateStyle = AuiNotebookCreateStyle.Top;

            mainLogListBox.Parent = CenterNotebook;
            mainLogListBox.ContextMenu.Required();
            mainLogListBox.MenuItemShowDevTools?.SetEnabled(false);
            mainLogListBox.BindApplicationLog();
            mainLogPage = CenterNotebook.AddPage(
                mainLogListBox,
                CommonStrings.Default.NotebookTabTitleOutput);
            ActionsControl.Required();
            PropGrid.Required();

            PropGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            PropGrid.Features = PropertyGridFeature.QuestionCharInNullable;
            PropGrid.ProcessException += PropertyGrid_ProcessException;
            PropGrid.CreateStyleEx = PropertyGridCreateStyleEx.AlwaysAllowFocus;

            InitActions();
            DebugUtils.HookExceptionEvents();
            TypesListBox.SelectionChanged += TypesListBox_SelectionChanged;
        }

        internal IAuiNotebookPage? MainLogPage => mainLogPage;

        internal IAuiNotebookPage? TypesPage => typesPage;

        internal IAuiNotebookPage? ControlsPage => controlsPage;

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

                    typesPage = CenterNotebook.AddPage(
                        typesListBox,
                        "Types");
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
                    controlsPage = CenterNotebook.AddPage(controlsListBox, "Controls");
                }

                return controlsListBox;
            }
        }

        internal void PropGridSetProps(object? instance)
        {
            if (insideSetProps)
                return;
            insideSetProps = true;
            try
            {
                PropGrid.DoInsideUpdate(() =>
                {
                    PropGrid.Clear();
                    if (instance is null)
                        return;
                    PropGrid.AddConstItem("(type)", "(type)", instance.GetType().Name);
                    PropGrid.AddProps(instance, null, true);
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
            Application.Log("Current OS Information:\n");
            Application.Log($"Platform: {os.Platform:G}");
            Application.Log($"Version String: {os.VersionString}");
            Application.Log($"Major version: {os.Version.Major}");
            Application.Log($"Minor version: {os.Version.Minor}");
            Application.Log($"Service Pack: '{os.ServicePack}'");
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            RightNotebook.ChangeSelection(PropGrid);
            controlsListBox?.SelectedAction?.Invoke();
        }

        private void PropertyGrid_ProcessException(object? sender, ControlExceptionEventArgs e)
        {
            Application.LogFileIsEnabled = true;
            LogUtils.LogException(e.InnerException);
        }

        private void ControlsActionMainForm()
        {
            RightNotebook.ChangeSelection(PropGrid);
            PropGridSetProps(Application.FirstWindow());
        }

        private void ControlsActionFocusedControl()
        {
            PropGridSetProps(LastFocusedControl);
        }

        private void TypesListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var item = TypesListBox.SelectedItem as ListControlItem;
            var type = item?.Value as Type;
            EventLogManager.UpdateEventsPropertyGrid(PropGrid, type);
        }

        private void LogUsefulDefines()
        {
            var s = WebBrowser.DoCommandGlobal("GetUsefulDefines");
            var splitted = s?.Split(' ');
            LogUtils.LogAsSection(splitted);
        }

        private void InitActions()
        {
            AddAction("Log system settings", SystemSettings.Log);
            AddAction("Log font families", LogUtils.LogFontFamilies);
            AddAction("Log system fonts", SystemSettings.LogSystemFonts);
            AddAction("Log fixed width fonts", SystemSettings.LogFixedWidthFonts);
            AddAction("Log display info", Display.Log);
            AddAction("Log control info", LogControlInfo);
            AddAction("Log useful defines", LogUsefulDefines);
            AddAction("Log OS information", LogOSInformation);
            AddAction("Log System Colors", ColorUtils.LogSystemColors);
            AddAction("HookExceptionEvents()", DebugUtils.HookExceptionEvents);
            AddAction("C++ Throw", () => { WebBrowser.DoCommandGlobal("CppThrow"); });

            AddAction("Show Props FirstWindow", ControlsActionMainForm);
            AddAction("Show Props FocusedControl", ControlsActionFocusedControl);

            AddAction("Show ThreadExceptionWindow", () =>
            {
                try
                {
                    throw new ApplicationException("This is exception message");
                }
                catch (Exception e)
                {
                    ThreadExceptionWindow.Show(e, "This is an additional info", true);
                }
            });

            AddAction("Enum Embedded Resources in Alternet.UI", () =>
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
        }

        private void LogControlInfo()
        {
            Application.Log($"Toolbar images: {UI.ToolBar.GetDefaultImageSize(this)}");
            Log($"Control.DefaultFont: {Control.DefaultFont.ToInfoString()}");
            Log($"Font.Default: {Font.Default.ToInfoString()}");
            Log($"Splitter.MinSashSize: {AllPlatformDefaults.PlatformCurrent.MinSplitterSashSize}");
        }
    }
}