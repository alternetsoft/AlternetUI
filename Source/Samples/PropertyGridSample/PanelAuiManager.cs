using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    public class PanelAuiManager : LayoutPanel
    {

        private readonly AuiManager manager = new();
        private AuiNotebook? leftNotebook;
        private AuiNotebook? rightNotebook;
        private AuiNotebook? bottomNotebook;
        private IAuiPaneInfo? leftPane;
        private IAuiPaneInfo? rightPane;
        private IAuiPaneInfo? centerPane;
        private IAuiPaneInfo? bottomPane;
        private PropertyGrid? propertyGrid;
        private PropertyGrid? eventGrid;
        private ListBox? logControl;
        private ContextMenu? logContextMenu;

        public AuiManager Manager => manager;

        public PanelAuiManager()
        {
            Manager.SetFlags(AuiManagerOption.Default);
            Manager.SetManagedWindow(this);
        }

        public ListBox LogControl
        {
            get
            {
                if (logControl == null)
                {
                    logControl = new()
                    {
                        HasBorder = false
                    };
                    BottomNotebook.AddPage(
                        logControl,
                        CommonStrings.Default.NotebookTabTitleOutput, true);
                }
                return logControl;
            }
        }

        public ContextMenu LogContextMenu
        {
            get
            {
                if (logContextMenu == null)
                {
                    logContextMenu = new();
                    LogControl.MouseRightButtonUp += LogControl_MouseRightButtonUp;
                }
                return logContextMenu;
            }
        }

        public PropertyGrid PropGrid
        {
            get
            {
                if (propertyGrid == null)
                {
                    propertyGrid = new()
                    {
                        HasBorder = false,
                    };
                    RightNotebook.AddPage(
                        propertyGrid,
                        CommonStrings.Default.NotebookTabTitleProperties, true);
                }
                return propertyGrid;
            }
        }
        
        public PropertyGrid EventGrid
        {
            get
            {
                if (eventGrid == null)
                {
                    eventGrid = new()
                    {
                        HasBorder = false,
                    };
                    RightNotebook.AddPage(
                        eventGrid,
                        CommonStrings.Default.NotebookTabTitleEvents, false);
                }
                return eventGrid;
            }
        }

        public AuiNotebook LeftNotebook
        {
            get
            {
                if (leftNotebook == null)
                {
                    leftNotebook = new();
                    Manager.AddPane(leftNotebook, LeftPane);
                }
                return leftNotebook;
            }
        }

        public AuiNotebook RightNotebook
        {
            get
            {
                if (rightNotebook == null)
                {
                    rightNotebook = new();
                    Manager.AddPane(rightNotebook, RightPane);
                }
                return rightNotebook;
            }
        }

        public AuiNotebook BottomNotebook
        {
            get
            {
                if (bottomNotebook == null)
                {
                    bottomNotebook = new();
                    Manager.AddPane(bottomNotebook, BottomPane);
                }
                return bottomNotebook;
            }
        }

        public IAuiPaneInfo LeftPane
        {
            get
            {
                if(leftPane is null)
                {
                    leftPane = manager.CreatePaneInfo();
                    leftPane.Name(nameof(leftPane)).Left()
                        .PaneBorder(false).CloseButton(false)
                        .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                        .CaptionVisible(false).BestSize(200, 200).MinSize(150, 200);
                }
                return leftPane;
            }
        }

        public IAuiPaneInfo RightPane
        {
            get
            {
                if (rightPane is null)
                {
                    rightPane = manager.CreatePaneInfo();
                    rightPane.Name(nameof(rightPane)).Right().PaneBorder(false)
                        .CloseButton(false)
                        .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                        .BestSize(350, 200).MinSize(350, 200).CaptionVisible(false);
                }
                return rightPane;
            }
        }

        public IAuiPaneInfo CenterPane
        {
            get
            {
                if (centerPane is null)
                {
                    centerPane = manager.CreatePaneInfo();
                    centerPane.Name(nameof(centerPane)).CenterPane().PaneBorder(false);
                }
                return centerPane;
            }
        }

        public IAuiPaneInfo BottomPane
        {
            get
            {
                if (bottomPane is null)
                {
                    bottomPane = manager.CreatePaneInfo();
                    bottomPane.Name(nameof(bottomPane)).Bottom()
                        .PaneBorder(false).CloseButton(false).CaptionVisible(false)
                        .LeftDockable(false).RightDockable(false).Movable(false)
                        .BestSize(200, 150).MinSize(200, 150).Floatable(false);
                }
                return bottomPane;
            }
        }

        public virtual void Log(string s)
        {
            LogControl.Add(ConstructMessage(s));
            LogControl.SelectLastItem();
        }

        public virtual void InitLogContextMenu()
        {
            LogContextMenu.Add(new("Clear", () => { LogControl.RemoveAll(); }));

#if DEBUG
            LogContextMenu.Add(new("C++ Throw", () => { WebBrowser.DoCommandGlobal("CppThrow"); }));
#endif
        }

        public virtual void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
#if DEBUG
            if (e.ReplaceLastMessage)
                LogSmart(e.Message, e.MessagePrefix);
            else
                Log(e.Message);
#endif
        }

        public virtual void LogSmart(string message, string? prefix)
        {
            var s = LogControl.LastItem?.ToString();
            var b = s?.StartsWith(prefix ?? string.Empty) ?? false;

            if (b)
            {
                LogControl.LastItem = ConstructMessage(message);
            }
            else
                Log(message);
        }

        public virtual void BindApplicationLogMessage()
        {
            Application.Current.LogMessage += Application_LogMessage;
        }

        protected virtual string ConstructMessage(string s)
        {
            return $"{s} ({LogUtils.GenNewId()})";
        }

        private void LogControl_MouseRightButtonUp(object? sender, MouseButtonEventArgs e)
        {
            LogControl.ShowPopupMenu(LogContextMenu);
        }
    }
}
