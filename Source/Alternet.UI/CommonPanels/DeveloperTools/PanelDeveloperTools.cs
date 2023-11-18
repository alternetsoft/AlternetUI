using System;
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
            TypesListBox.SelectionChanged += ControlsListBox_SelectionChanged;
            CenterNotebook.ChangeSelection(0);
            PropGrid.SuggestedInitDefaults();
        }

        internal IAuiNotebookPage? MainLogPage => mainLogPage;

        internal IAuiNotebookPage? TypesPage => typesPage;

        /// <summary>
        /// Gets control on the bottom pane which can be used for logging.
        /// </summary>
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

                    foreach (var type in result)
                    {
                        if (type.Assembly != typeof(Control).Assembly)
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
            AddAction("Log display info", Display.Log);
            AddAction("HookExceptionEvents()", DebugUtils.HookExceptionEvents);
            AddAction("Log useful defines", LogUsefulDefines);
            AddAction("Log OS information", LogOSInformation);
            AddAction("C++ Throw", () => { WebBrowser.DoCommandGlobal("CppThrow"); });
        }
    }
}