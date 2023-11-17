using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class PanelDeveloperTools : PanelAuiManager
    {
        private readonly LogListBox mainLogListBox = new()
        {
            HasBorder = false,
        };

        public PanelDeveloperTools()
            : base()
        {
            DefaultRightPaneBestSize = new(150, 200);
            DefaultRightPaneMinSize = new(150, 200);

            mainLogListBox.Parent = CenterNotebook;
            mainLogListBox.ContextMenu.Required();
            mainLogListBox.MenuItemShowDevTools?.SetEnabled(false);
            mainLogListBox.BindApplicationLog();
            var mainLogPage = CenterNotebook.AddPage(
                mainLogListBox,
                CommonStrings.Default.NotebookTabTitleOutput);
            ActionsControl.Required();
            PropGrid.Required();
            RightNotebook.ChangeSelection(0);
            InitActions();
            DebugUtils.HookExceptionEvents();
        }

        public static void LogOSInformation()
        {
            var os = Environment.OSVersion;
            Application.Log("Current OS Information:\n");
            Application.Log($"Platform: {os.Platform:G}");
            Application.Log($"Version String: {os.VersionString}");
            Application.Log($"Major version: {os.Version.Major}");
            Application.Log($"Minor version: {os.Version.Minor}");
            Application.Log($"Service Pack: '{os.ServicePack}'");
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
        }
    }
}
