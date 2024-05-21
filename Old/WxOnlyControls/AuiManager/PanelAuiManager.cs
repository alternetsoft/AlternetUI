using System;
using System.ComponentModel;
using System.Diagnostics;
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
    [ControlCategory("Hidden")]
    public partial class PanelAuiManager : PanelAuiManagerBase
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
        private LogListBox? logControl;
        private TreeView? leftTreeView;
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
        public PanelAuiManager(Action<PanelAuiManager>? initAction = null)
            : this()
        {
            initAction?.Invoke(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelAuiManager"/> class.
        /// </summary>
        public PanelAuiManager()
            : base()
        {
            defaultToolbarStyle = InitDefaultToolbarStyle();
            Manager.SetDefaultSplitterSashProps();
        }

        /// <summary>
        /// Gets or sets minimal toolbar image size in pixels.
        /// </summary>
        public static int DefaultMinToolbarImageSize { get; set; } = 16;

        /// <summary>
        /// Gets control on the bottom pane which can be used for logging.
        /// </summary>
        [Browsable(false)]
        public LogListBox LogControl
        {
            get
            {
                if (logControl == null)
                {
                    logControl = new LogListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };

                    if (LogControlUseNotebook)
                    {
                        logPage = BottomNotebook.AddPage(
                            logControl,
                            CommonStrings.Default.NotebookTabTitleOutput);
                    }
                    else
                    {
                        Manager.AddPane(logControl, BottomPane);
                    }

                    logControl.ContextMenu.Required();
                }

                return logControl;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="LogControl"/> is shown inside <see cref="BottomNotebook"/>.
        /// </summary>
        public bool LogControlUseNotebook { get; set; } = true;

        /// <summary>
        /// Gets or sets whether <see cref="LeftTreeView"/> is shown inside <see cref="LeftNotebook"/>.
        /// </summary>
        public bool LeftTreeViewUseNotebook { get; set; } = true;

        /// <summary>
        /// Gets <see cref="TreeView"/> control on the left pane.
        /// </summary>
        [Browsable(false)]
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
                    if (LeftTreeViewAsListBox)
                        leftTreeView.MakeAsListBox();
                    if (LeftTreeViewUseNotebook)
                    {
                        leftTreeViewPage = LeftNotebook.AddPage(
                            leftTreeView,
                            CommonStrings.Default.NotebookTabTitleActivity);
                    }
                    else
                    {
                        Manager.AddPane(leftTreeView, LeftPane);
                    }
                }

                return leftTreeView;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="LeftTreeView"/> should look like <see cref="ListBox"/>.
        /// </summary>
        /// <remarks>
        /// This property must be assigned before first use of <see cref="LeftTreeView"/>
        /// </remarks>
        public bool LeftTreeViewAsListBox { get; set; } = false;

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> which can be used to show properties.
        /// </summary>
        [Browsable(false)]
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
                        CommonStrings.Default.WindowTitleProperties);
                }

                return propertyGrid;
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyGrid"/> which can be used to show events.
        /// </summary>
        [Browsable(false)]
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
        /// Gets the control with actions list.
        /// </summary>
        [Browsable(false)]
        public Control ActionsControl
        {
            get
            {
                if (actionsControl == null)
                {
                    actionsControl = new()
                    {
                        HasBorder = false,
                    };
                    actionsControl.Parent = RightNotebook;
                    actionsPage = RightNotebook.AddPage(
                        actionsControl,
                        CommonStrings.Default.NotebookTabTitleActions);
                }

                return actionsControl;
            }
        }

        /// <summary>
        /// Gets or sets default best size of the left pane.
        /// </summary>
        public virtual SizeD DefaultLeftPaneBestSize { get; set; } = (200, 200);

        /// <summary>
        /// Gets or sets default min size of the left pane.
        /// </summary>
        public virtual SizeD DefaultLeftPaneMinSize { get; set; } = (150, 200);

        /// <summary>
        /// Gets or sets default best size of the right pane.
        /// </summary>
        public virtual SizeD DefaultRightPaneBestSize { get; set; } = (350, 200);

        /// <summary>
        /// Gets or sets default min size of the right pane.
        /// </summary>
        public virtual SizeD DefaultRightPaneMinSize { get; set; } = (350, 200);

        /// <summary>
        /// Gets or sets default best size of the bottom pane.
        /// </summary>
        public virtual SizeD DefaultBottomPaneBestSize { get; set; } = (200, 150);

        /// <summary>
        /// Gets or sets default min size of the bottom pane.
        /// </summary>
        public virtual SizeD DefaultBottomPaneMinSize { get; set; } = (200, 150);

        /// <summary>
        /// Gets or sets default create style for the <see cref="LeftNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebookCreateStyle? LeftNotebookDefaultCreateStyle { get; set; }

        /// <summary>
        /// Gets or sets default create style for the <see cref="BottomNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebookCreateStyle? BottomNotebookDefaultCreateStyle { get; set; }

        /// <summary>
        /// Gets or sets default create style for the <see cref="CenterNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebookCreateStyle? CenterNotebookDefaultCreateStyle { get; set; }

        /// <summary>
        /// Gets or sets default create style for the <see cref="RightNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebookCreateStyle? RightNotebookDefaultCreateStyle { get; set; }

        /// <summary>
        /// Gets <see cref="AuiToolbar"/> located on the top of the control.
        /// </summary>
        [Browsable(false)]
        internal AuiToolbar Toolbar
        {
            get
            {
                toolbar ??= new()
                {
                    CreateStyle = DefaultToolbarStyle,
                };

                return toolbar;
            }
        }

        /// <summary>
        /// Gets or sets default toolbar style.
        /// </summary>
        internal AuiToolbarCreateStyle DefaultToolbarStyle
        {
            get => defaultToolbarStyle;
            set => defaultToolbarStyle = value;
        }

        /// <summary>
        /// Gets the top pane with the <see cref="AuiToolbar"/>.
        /// </summary>
        [Browsable(false)]
        internal IAuiPaneInfo ToolbarPane
        {
            get
            {
                toolbarPane ??= CreateToolbarPane();
                return toolbarPane;
            }
        }

        /// <summary>
        /// Gets the left pane.
        /// </summary>
        [Browsable(false)]
        internal IAuiPaneInfo LeftPane
        {
            get
            {
                leftPane ??= CreateLeftPane();
                return leftPane;
            }
        }

        /// <summary>
        /// Gets the right pane.
        /// </summary>
        [Browsable(false)]
        internal IAuiPaneInfo RightPane
        {
            get
            {
                rightPane ??= CreateRightPane();
                return rightPane;
            }
        }

        /// <summary>
        /// Gets the center pane.
        /// </summary>
        [Browsable(false)]
        internal IAuiPaneInfo CenterPane
        {
            get
            {
                centerPane ??= CreateCenterPane();
                return centerPane;
            }
        }

        /// <summary>
        /// Gets the bottom pane.
        /// </summary>
        [Browsable(false)]
        internal IAuiPaneInfo BottomPane
        {
            get
            {
                bottomPane ??= CreateBottomPane();
                return bottomPane;
            }
        }

        /// <summary>
        /// Gets <see cref="LogControl"/> page index in the <see cref="BottomNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal IAuiNotebookPage? LogPage => logPage;

        /// <summary>
        /// Gets <see cref="LeftTreeView"/> page index in the <see cref="LeftNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal IAuiNotebookPage? LeftTreeViewPage => leftTreeViewPage;

        /// <summary>
        /// Gets <see cref="PropGrid"/> page index in the <see cref="RightNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal IAuiNotebookPage? PropGridPage => propGridPage;

        /// <summary>
        /// Gets <see cref="ActionsControl"/> page index in the <see cref="RightNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal IAuiNotebookPage? ActionsPage => actionsPage;

        /// <summary>
        /// Gets <see cref="EventGrid"/> page index in the <see cref="RightNotebook"/>.
        /// </summary>
        [Browsable(false)]
        internal IAuiNotebookPage? EventGridPage => eventGridPage;

        /// <summary>
        /// Gets <see cref="AuiNotebook"/> control which is located on the left pane.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebook LeftNotebook
        {
            get
            {
                if (leftNotebook == null)
                {
                    leftNotebook = new();
                    if (LeftNotebookDefaultCreateStyle is not null)
                        leftNotebook.CreateStyle = LeftNotebookDefaultCreateStyle.Value;
                    Manager.AddPane(leftNotebook, LeftPane);
                }

                return leftNotebook;
            }
        }

        /// <summary>
        /// Gets <see cref="AuiNotebook"/> control which is located on the bottom pane.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebook CenterNotebook
        {
            get
            {
                if (centerNotebook == null)
                {
                    centerNotebook = new();
                    if (CenterNotebookDefaultCreateStyle is not null)
                        centerNotebook.CreateStyle = CenterNotebookDefaultCreateStyle.Value;
                    Manager.AddPane(centerNotebook, CenterPane);
                }

                return centerNotebook;
            }
        }

        /// <summary>
        /// Gets <see cref="AuiNotebook"/> control which is located on the bottom pane.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebook BottomNotebook
        {
            get
            {
                if (bottomNotebook == null)
                {
                    bottomNotebook = new();
                    if (BottomNotebookDefaultCreateStyle is not null)
                        BottomNotebook.CreateStyle = BottomNotebookDefaultCreateStyle.Value;
                    Manager.AddPane(bottomNotebook, BottomPane);
                }

                return bottomNotebook;
            }
        }

        /// <summary>
        /// Gets <see cref="AuiNotebook"/> control which is located on the right pane.
        /// </summary>
        [Browsable(false)]
        internal AuiNotebook RightNotebook
        {
            get
            {
                if (rightNotebook == null)
                {
                    rightNotebook = new();
                    if (RightNotebookDefaultCreateStyle is not null)
                        rightNotebook.CreateStyle = RightNotebookDefaultCreateStyle.Value;
                    Manager.AddPane(rightNotebook, RightPane);
                }

                return rightNotebook;
            }
        }

        /// <summary>
        /// Gets base toolbar button bitmap size in pixels for svg images.
        /// </summary>
        /// <remarks>
        /// This is used to get toolbar button bitmap size for svg images. Typically it is 16
        /// on all displays (even with different DPI),
        /// as svg images are scaled automatically by the toolbar.
        /// </remarks>
        /// <returns></returns>
        public static SizeI GetBaseToolSvgSize()
        {
            var imageSize = Math.Max(ToolBarUtils.DefaultImageSize96dpi, DefaultMinToolbarImageSize);
            return imageSize;
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
                DoubleClickAction = action,
            };

            ActionsControl.Required();
            actionsControl?.Add(item);
        }

        /// <summary>
        /// Sets tool enabled state for the toolbar.
        /// </summary>
        /// <param name="buttonId">Id of the tool.</param>
        /// <param name="value">New enabled state.</param>
        public virtual void EnableTool(int buttonId, bool value)
        {
            Toolbar.EnableTool(buttonId, value);
        }

        /// <summary>
        /// Writes some debug related welcome messages to the log if called in the debug environment.
        /// </summary>
        [Conditional("DEBUG")]
        public virtual void WriteWelcomeLogMessages()
        {
            LogUtils.DebugLogVersion();
        }

        /// <summary>
        /// Adds simple actions for <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type for which required simple actions are registered.</param>
        public virtual void AddActions(Type type)
        {
            var actions = PropertyGrid.GetSimpleActions(type);
            if (actions is null)
                return;
            foreach (var action in actions)
            {
                AddAction(action.Item1, action.Item2);
            }
        }

        /// <summary>
        /// Removes all actions from the <see cref="ActionsControl"/>
        /// </summary>
        public virtual void RemoveActions()
        {
            if (actionsControl is null)
                return;
            actionsControl.RemoveAll();
        }

        /// <summary>
        /// Gets toolbar button bitmap size in pixels.
        /// </summary>
        /// <returns></returns>
        public virtual SizeI GetToolBitmapSize()
        {
            var imageSize = SizeI.Max(
                ToolBarUtils.GetDefaultImageSize(this),
                new SizeI(DefaultMinToolbarImageSize));
            return imageSize;
        }

        /// <summary>
        /// Binds <see cref="LogControl"/> to show messages which are logged with
        /// <see cref="BaseApplication.Log"/>.
        /// </summary>
        public virtual void BindApplicationLog()
        {
            LogControl.BindApplicationLog();
        }

        /// <summary>
        /// Creates left pane and assigns its properties.
        /// </summary>
        /// <returns></returns>
        internal virtual IAuiPaneInfo CreateLeftPane()
        {
            var result = Manager.CreatePaneInfo();
            result.Name(nameof(leftPane)).Left()
                .PaneBorder(false).CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                .CaptionVisible(false)
                .BestSizeDip(DefaultLeftPaneBestSize.Width, DefaultLeftPaneBestSize.Height)
                .MinSizeDip(DefaultLeftPaneMinSize.Width, DefaultLeftPaneMinSize.Height);
            return result;
        }

        /// <summary>
        /// Creates left pane and assigns its properties.
        /// </summary>
        /// <returns></returns>
        internal virtual IAuiPaneInfo CreateToolbarPane()
        {
            var result = Manager.CreatePaneInfo();
            result.Name(nameof(toolbarPane)).Top().ToolbarPane().PaneBorder(false)
                .Movable(false).Floatable(false).Resizable(false).Gripper(false)
                .Fixed().DockFixed();
            return result;
        }

        /// <summary>
        /// Creates center pane and assigns its properties.
        /// </summary>
        /// <returns></returns>
        internal virtual IAuiPaneInfo CreateCenterPane()
        {
            var result = Manager.CreatePaneInfo();
            result.Name(nameof(centerPane)).CenterPane().PaneBorder(false);
            return result;
        }

        /// <summary>
        /// Creates bottom pane and assigns its properties.
        /// </summary>
        /// <returns></returns>
        internal virtual IAuiPaneInfo CreateBottomPane()
        {
            var result = Manager.CreatePaneInfo();
            result.Name(nameof(bottomPane)).Bottom()
                .PaneBorder(false).CloseButton(false).CaptionVisible(false)
                .LeftDockable(false).RightDockable(false).Movable(false)
                .BestSizeDip(DefaultBottomPaneBestSize.Width, DefaultBottomPaneBestSize.Height)
                .MinSizeDip(DefaultBottomPaneMinSize.Width, DefaultBottomPaneMinSize.Height)
                .Floatable(false);

            return result;
        }

        /// <summary>
        /// Creates right pane and assigns its properties.
        /// </summary>
        /// <returns></returns>
        internal virtual IAuiPaneInfo CreateRightPane()
        {
            var result = Manager.CreatePaneInfo();
            result.Name(nameof(rightPane)).Right().PaneBorder(false)
                .CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                .BestSizeDip(DefaultRightPaneBestSize.Width, DefaultRightPaneBestSize.Height)
                .MinSizeDip(DefaultRightPaneMinSize.Width, DefaultRightPaneMinSize.Height)
                .CaptionVisible(false);
            return result;
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Manager.Update();
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
    }
}
