using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
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
        private ListBox? actionsControl;
        private AuiNotebook? leftNotebook;
        private AuiNotebook? rightNotebook;
        private AuiNotebook? bottomNotebook;
        private AuiNotebook? centerNotebook;
        private IAuiPaneInfo? leftPane;
        private IAuiPaneInfo? rightPane;
        private IAuiPaneInfo? centerPane;
        private IAuiPaneInfo? bottomPane;
        private IAuiPaneInfo? toolbarPane;
        private PropertyGrid? propertyGrid;
        private PropertyGrid? eventGrid;
        private TreeView? logControl;
        private TreeView? leftTreeView;
        private ContextMenu? logContextMenu;
        private AuiToolbar? toolbar;

        private IAuiNotebookPage? logPage;
        private IAuiNotebookPage? leftTreeViewPage;
        private IAuiNotebookPage? propGridPage;
        private IAuiNotebookPage? actionsPage;
        private IAuiNotebookPage? eventGridPage;

        private AuiToolbarCreateStyle defaultToolbarStyle;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelAuiManager"/> class.
        /// </summary>
        public PanelAuiManager()
        {
            defaultToolbarStyle = InitDefaultToolbarStyle();
        }

        /// <summary>
        /// Gets <see cref="LogControl"/> page index in the <see cref="BottomNotebook"/>.
        /// </summary>
        public IAuiNotebookPage? LogPage => logPage;

        /// <summary>
        /// Gets <see cref="LeftTreeView"/> page index in the <see cref="LeftNotebook"/>.
        /// </summary>
        public IAuiNotebookPage? LeftTreeViewPage => leftTreeViewPage;

        /// <summary>
        /// Gets <see cref="PropGrid"/> page index in the <see cref="RightNotebook"/>.
        /// </summary>
        public IAuiNotebookPage? PropGridPage => propGridPage;

        /// <summary>
        /// Gets <see cref="ActionsControl"/> page index in the <see cref="RightNotebook"/>.
        /// </summary>
        public IAuiNotebookPage? ActionsPage => actionsPage;

        /// <summary>
        /// Gets <see cref="EventGrid"/> page index in the <see cref="RightNotebook"/>.
        /// </summary>
        public IAuiNotebookPage? EventGridPage => eventGridPage;

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
                    logPage = BottomNotebook.AddPage(
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
                    leftTreeViewPage = LeftNotebook.AddPage(
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
                    LogControl.MouseRightButtonDown += LogControl_ShowMenu;
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
                    propGridPage = RightNotebook.AddPage(
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
                    eventGridPage = RightNotebook.AddPage(
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
        public AuiNotebook CenterNotebook
        {
            get
            {
                if (centerNotebook == null)
                {
                    centerNotebook = new();
                    Manager.AddPane(centerNotebook, CenterPane);
                }

                return centerNotebook;
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
        /// Gets <see cref="AuiToolbar"/> located on the top of the control.
        /// </summary>
        public AuiToolbar Toolbar
        {
            get
            {
                toolbar ??= new()
                    {
                        CreateStyle = DefaultToolbarStyle,
                        ToolBitmapSize = GetToolBitmapSize(),
                    };

                return toolbar;
            }
        }

        /// <summary>
        /// Gets or sets minimal toolbar image size.
        /// </summary>
        public int DefaultMinToolbarImageSize { get; set; } = 24;

        /// <summary>
        /// Gets or sets default toolbar style.
        /// </summary>
        public AuiToolbarCreateStyle DefaultToolbarStyle
        {
            get => defaultToolbarStyle;
            set => defaultToolbarStyle = value;
        }

        /// <summary>
        /// Gets the top pane with the <see cref="AuiToolbar"/>.
        /// </summary>
        public IAuiPaneInfo ToolbarPane
        {
            get
            {
                if(toolbarPane is null)
                {
                    toolbarPane = Manager.CreatePaneInfo();
                    toolbarPane.Name(nameof(toolbarPane)).Top().ToolbarPane().PaneBorder(false)
                        .Movable(false).Floatable(false).Resizable(false).Gripper(false)
                        .Fixed().DockFixed();
                }

                return toolbarPane;
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
                        .BestSize(DefaultRightPaneBestSize.Width, DefaultRightPaneBestSize.Height)
                        .MinSize(DefaultRightPaneMinSize.Width, DefaultRightPaneMinSize.Height)
                        .CaptionVisible(false);
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
        /// Gets the control with actions list.
        /// </summary>
        public Control ActionsControl
        {
            get
            {
                if(actionsControl == null)
                {
                    actionsControl = new()
                    {
                        HasBorder = false,
                    };
                    actionsControl.MouseDoubleClick += Actions_MouseDoubleClick;
                    actionsControl.Parent = RightNotebook;
                    actionsPage = RightNotebook.AddPage(
                        actionsControl,
                        CommonStrings.Default.NotebookTabTitleActions);
                }

                return actionsControl;
            }
        }

        /// <summary>
        /// Gets or sets default best size of the right pane.
        /// </summary>
        public virtual Int32Size DefaultRightPaneBestSize { get; set; } = new(350, 200);

        /// <summary>
        /// Gets or sets default min size of the right pane.
        /// </summary>
        public virtual Int32Size DefaultRightPaneMinSize { get; set; } = new(350, 200);

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
            LogControl.SelectAndShowItem(LogControl.LastRootItem);
            Application.DoEvents();
        }

        /// <summary>
        /// Adds an empty space to the <see cref="ActionsControl"/>.
        /// </summary>
        /// <remarks>
        /// This method allows to separate different action groups.
        /// </remarks>
        public virtual void AddActionSpacer()
        {
            ActionsControl.Required();
            (actionsControl as ListBox)?.Add(string.Empty);
        }

        /// <summary>
        /// Adds <see cref="Action"/> to the <see cref="ActionsControl"/>.
        /// </summary>
        /// <param name="title">Action title.</param>
        /// <param name="action">Action method.</param>
        public virtual void AddAction(string title, Action? action)
        {
            ListControlItem item = new(title)
            {
                Action = action,
            };

            ActionsControl.Required();
            (actionsControl as ListBox)?.Add(item);
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
            {
                lastItem.Text = ConstructLogMessage(message);
                LogControl.SelectAndShowItem(lastItem);
            }
            else
                Log(message);

            Application.DoEvents();
        }

        /// <summary>
        /// Gets toolbar button bitmap size.
        /// </summary>
        /// <returns></returns>
        public virtual Int32Size GetToolBitmapSize()
        {
            var imageSize = Int32Size.Max(
                UI.Toolbar.GetDefaultImageSize(this),
                new Int32Size(DefaultMinToolbarImageSize));
            return imageSize;
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

        private void LogControl_ShowMenu(object? sender, MouseButtonEventArgs e)
        {
            LogControl.ShowPopupMenu(LogContextMenu);
        }

        private void Actions_MouseDoubleClick(object? sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox?.SelectedItem is not ListControlItem item || item.Action == null)
                return;
            Log("Do action: " + item.Text);
            item.Action();
        }

#pragma warning disable
        private AuiToolbarCreateStyle InitDefaultToolbarStyle()
#pragma warning restore
        {
            var toolbarStyle =
                AuiToolbarCreateStyle.PlainBackground |
                AuiToolbarCreateStyle.HorzLayout |
                AuiToolbarCreateStyle.Text |
                AuiToolbarCreateStyle.NoTooltips |
                AuiToolbarCreateStyle.DefaultStyle;

            toolbarStyle &= ~AuiToolbarCreateStyle.Gripper;

            return toolbarStyle;
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
