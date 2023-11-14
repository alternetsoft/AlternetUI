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
            ActionsControl.Required();
            var mainLogPage = CenterNotebook.AddPage(
                mainLogListBox,
                CommonStrings.Default.NotebookTabTitleOutput);
            InitActions();
            DebugUtils.HookExceptionEvents();
        }

        private void InitActions()
        {
            AddAction("Log system settings", SystemSettings.Log);
            AddAction("Log font families", LogUtils.LogFontFamilies);
            AddAction("Log system fonts", SystemSettings.LogSystemFonts);
            AddAction("Log display info", Display.Log);
        }
    }
}
