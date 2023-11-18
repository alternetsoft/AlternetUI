using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private ListBox? controlsListBox;
        private IAuiNotebookPage? controlsPage;

        public PanelDeveloperTools()
            : base()
        {
            DefaultRightPaneBestSize = new(350, 200);
            DefaultRightPaneMinSize = new(350, 200);

            mainLogListBox.Parent = CenterNotebook;
            mainLogListBox.ContextMenu.Required();
            mainLogListBox.MenuItemShowDevTools?.SetEnabled(false);
            mainLogListBox.BindApplicationLog();
            mainLogPage = CenterNotebook.AddPage(
                mainLogListBox,
                CommonStrings.Default.NotebookTabTitleOutput);
            ActionsControl.Required();
            PropGrid.Required();
            RightNotebook.ChangeSelection(0);
            InitActions();
            DebugUtils.HookExceptionEvents();
            ControlsListBox.SelectionChanged += ControlsListBox_SelectionChanged;
            CenterNotebook.ChangeSelection(0);
            PropGrid.SuggestedInitDefaults();

            Control.GetDefaults(ControlTypeId.TextBox).InitDefaults += PanelDeveloperTools_InitDefaults;
        }

        internal IAuiNotebookPage? MainLogPage => mainLogPage;

        internal IAuiNotebookPage? ControlsPage => controlsPage;

        /// <summary>
        /// Gets control on the bottom pane which can be used for logging.
        /// </summary>
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

                    IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));

                    void AddControl(Type type)
                    {
                        controlsListBox.Add(type.Name, type);
                    }

                    AddControl(typeof(Control));

                    foreach (var type in result)
                    {
                        if (type.Assembly != typeof(Control).Assembly)
                            continue;
                        AddControl(type);
                    }

                    controlsPage = CenterNotebook.AddPage(
                        controlsListBox,
                        "Controls");
                }

                return controlsListBox;
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
            var item = ControlsListBox.SelectedItem as ListControlItem;
            var type = item?.Value as Type;
            EventLogManager.UpdateEventsPropertyGrid(PropGrid, type);
        }

        private void LogUsefulDefines()
        {
            var s = WebBrowser.DoCommandGlobal("GetUsefulDefines");
            var splitted = s?.Split(' ');
            LogUtils.LogAsSection(splitted);
        }

        private void PanelDeveloperTools_InitDefaults(object? sender, EventArgs e)
        {
        }

        private void InitActions()
        {
            AddAction("Log system settings", SystemSettings.Log);
            AddAction("Log font families", LogUtils.LogFontFamilies);
            AddAction("Log system fonts", SystemSettings.LogSystemFonts);
            AddAction("Log display info", Display.Log);
            AddAction("HookExceptionEvents()", DebugUtils.HookExceptionEvents);
            AddAction("Log useful defines", LogUsefulDefines);
            AddAction("Log OS information", LogOSInformation);
            AddAction("C++ Throw", () => { WebBrowser.DoCommandGlobal("CppThrow"); });
        }
    }
}