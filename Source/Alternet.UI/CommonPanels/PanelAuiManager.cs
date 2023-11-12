﻿using System;
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

                    logPage = BottomNotebook.AddPage(
                        logControl,
                        CommonStrings.Default.NotebookTabTitleOutput);
                    logControl.ContextMenu.Required();
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
                    if (LeftTreeViewAsListBox)
                        leftTreeView.MakeAsListBox();
                    leftTreeViewPage = LeftNotebook.AddPage(
                        leftTreeView,
                        CommonStrings.Default.NotebookTabTitleActivity);
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
        public int DefaultMinToolbarImageSize { get; set; } = 16;

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
        public virtual void BindApplicationLog()
        {
            LogControl.BindApplicationLog();
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Manager.Update();
        }

        private void Actions_MouseDoubleClick(object? sender, MouseButtonEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox?.SelectedItem is not ListControlItem item || item.Action == null)
                return;
            logControl?.Log("Do action: " + item.Text);
            BeginInvoke(() =>
            {
                item.Action();
            });
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
