using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Panel with integrated <see cref="AuiManager"/> which allows to implement
    /// advanced docking and floating toolbars and panes.
    /// </summary>
    /// <remarks>
    /// <see cref="PanelAuiManager"/> has different built-in panes and toolbars which are created
    /// by demand. If you need basic panel with <see cref="AuiManager"/>,
    /// use <see cref="PanelAuiManagerBase"/>.
    /// </remarks>
    public class PanelAuiManager : PanelAuiManagerBase
    {
        private AuiNotebook? leftNotebook;
        private AuiNotebook? rightNotebook;
        private AuiNotebook? bottomNotebook;
        private IAuiPaneInfo? leftPane;
        private IAuiPaneInfo? rightPane;
        private IAuiPaneInfo? centerPane;
        private IAuiPaneInfo? bottomPane;
        private PropertyGrid? propertyGrid;
        private PropertyGrid? eventGrid;
        private TreeView? logControl;
        private TreeView? leftTreeView;
        private ContextMenu? logContextMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelAuiManager"/> class.
        /// </summary>
        public PanelAuiManager()
        {
        }

        /// <summary>
        /// Gets control on the bottom pane which can be used for logging.
        /// </summary>
        public TreeView LogControl
        {
            get
            {
                if (logControl == null)
                {
                    logControl = new()
                    {
                        Parent = this,
                        HasBorder = false,
                        FullRowSelect = true,
                    };
                    BottomNotebook.AddPage(
                        logControl,
                        CommonStrings.Default.NotebookTabTitleOutput);
                }

                return logControl;
            }
        }

        /// <summary>
        /// Gets <see cref="TreeView"/> control on the left pane.
        /// </summary>
        public TreeView LeftTreeView
        {
            get
            {
                if (leftTreeView == null)
                {
                    leftTreeView = new()
                    {
                        Parent = this,
                        HasBorder = false,
                        FullRowSelect = true,
                    };
                    LeftNotebook.AddPage(
                        leftTreeView,
                        CommonStrings.Default.NotebookTabTitleActivity);
                }

                return leftTreeView;
            }
        }

        /// <summary>
        /// Gets context menu for the <see cref="LogControl"/>.
        /// </summary>
        public ContextMenu LogContextMenu
        {
            get
            {
                if (logContextMenu == null)
                {
                    logContextMenu = new();
                    InitLogContextMenu();
                    LogControl.MouseRightButtonUp += LogControl_MouseRightButtonUp;
                }

                return logContextMenu;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> which can be used to show properties.
        /// </summary>
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
                        CommonStrings.Default.NotebookTabTitleProperties);
                }

                return propertyGrid;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> which can be used to show events.
        /// </summary>
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
                        CommonStrings.Default.NotebookTabTitleEvents,
                        false);
                }

                return eventGrid;
            }
        }

        /// <summary>
        /// Gets <see cref="AuiNotebook"/> control which is located on the left pane.
        /// </summary>
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

        /// <summary>
        /// Gets <see cref="AuiNotebook"/> control which is located on the right pane.
        /// </summary>
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

        /// <summary>
        /// Gets <see cref="AuiNotebook"/> control which is located on the bottom pane.
        /// </summary>
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

        /// <summary>
        /// Gets the left pane.
        /// </summary>
        public IAuiPaneInfo LeftPane
        {
            get
            {
                if (leftPane is null)
                {
                    leftPane = Manager.CreatePaneInfo();
                    leftPane.Name(nameof(leftPane)).Left()
                        .PaneBorder(false).CloseButton(false)
                        .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                        .CaptionVisible(false).BestSize(200, 200).MinSize(150, 200);
                }

                return leftPane;
            }
        }

        /// <summary>
        /// Gets the right pane.
        /// </summary>
        public IAuiPaneInfo RightPane
        {
            get
            {
                if (rightPane is null)
                {
                    rightPane = Manager.CreatePaneInfo();
                    rightPane.Name(nameof(rightPane)).Right().PaneBorder(false)
                        .CloseButton(false)
                        .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                        .BestSize(350, 200).MinSize(350, 200).CaptionVisible(false);
                }

                return rightPane;
            }
        }

        /// <summary>
        /// Gets the center pane.
        /// </summary>
        public IAuiPaneInfo CenterPane
        {
            get
            {
                if (centerPane is null)
                {
                    centerPane = Manager.CreatePaneInfo();
                    centerPane.Name(nameof(centerPane)).CenterPane().PaneBorder(false);
                }

                return centerPane;
            }
        }

        /// <summary>
        /// Gets the bottom pane.
        /// </summary>
        public IAuiPaneInfo BottomPane
        {
            get
            {
                if (bottomPane is null)
                {
                    bottomPane = Manager.CreatePaneInfo();
                    bottomPane.Name(nameof(bottomPane)).Bottom()
                        .PaneBorder(false).CloseButton(false).CaptionVisible(false)
                        .LeftDockable(false).RightDockable(false).Movable(false)
                        .BestSize(200, 150).MinSize(200, 150).Floatable(false);
                }

                return bottomPane;
            }
        }

        /// <summary>
        /// Same as <see cref="Application.Log"/> but
        /// uses <see cref="LogControl"/> for the logging.
        /// </summary>
        /// <param name="message">Message text.</param>
        public virtual void Log(string message)
        {
            LogControl.Add(ConstructLogMessage(message));
            LogControl.FocusAndSelectItem(LogControl.LastRootItem);
        }

        /// <summary>
        /// Writes some debug related welcome messages to the log if called in the debug environment.
        /// </summary>
        [Conditional("DEBUG")]
        public virtual void WriteWelcomeLogMessages()
        {
            Application.DebugLog($"Net Version = {Environment.Version}");
            if (Application.LogFileIsEnabled)
                Application.DebugLog($"Log File = {Application.LogFilePath}");
        }

        /// <summary>
        /// Same as <see cref="Application.LogReplace(string, string)"/> but
        /// uses <see cref="LogControl"/> for the logging.
        /// </summary>
        /// <param name="message">Message text.</param>
        /// <param name="prefix">Message text prefix.</param>
        /// <remarks>
        /// If last logged message
        /// contains <paramref name="prefix"/>, last log item is replaced with
        /// <paramref name="message"/> instead of adding new log item.
        /// </remarks>
        public virtual void LogReplace(string message, string? prefix)
        {
            var lastItem = LogControl.LastRootItem;
            if (lastItem is null)
            {
                Log(message);
                return;
            }

            var s = lastItem.Text;
            var b = s?.StartsWith(prefix ?? string.Empty) ?? false;

            if (b)
                lastItem.Text = ConstructLogMessage(message);
            else
                Log(message);
        }

        /// <summary>
        /// Binds <see cref="LogControl"/> to show messages which are logged with
        /// <see cref="Application.Log"/>.
        /// </summary>
        public virtual void BindApplicationLogMessage()
        {
            Application.Current.LogMessage += Application_LogMessage;
        }

        internal virtual void Application_LogMessage(object? sender, LogMessageEventArgs e)
        {
#if DEBUG
            if (e.ReplaceLastMessage)
                LogReplace(e.Message, e.MessagePrefix);
            else
                Log(e.Message);
#endif
        }

        /// <summary>
        /// Adds additional information to the log messages which are logged to
        /// <see cref="LogControl"/>.
        /// </summary>
        /// <param name="msg">Log message.</param>
        /// <remarks>
        /// By default adds unique integer identifier to the end of the <paramref name="msg"/>.
        /// </remarks>
        protected virtual string ConstructLogMessage(string msg)
        {
            return $"{msg} ({LogUtils.GenNewId()})";
        }

        private void LogControl_MouseRightButtonUp(object? sender, MouseButtonEventArgs e)
        {
            LogControl.ShowPopupMenu(LogContextMenu);
        }

        private void InitLogContextMenu()
        {
            LogContextMenu.Add(new("Clear", () => { LogControl.RemoveAll(); }));

#if DEBUG
            /*LogContextMenu.Add(
             new("C++ Throw", () => { WebBrowser.DoCommandGlobal("CppThrow"); }));*/
#endif
        }
    }
}
