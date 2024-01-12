using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    public class SplittedPanelEx : SplittedPanel
    {
        private TreeView? leftTreeView;
        private PropertyGrid? propertyGrid;
        private LogListBox? logControl;
        private ListBox? actionsControl;

        private readonly CardPanelHeader rightPanelHeader = new();

        public SplittedPanelEx()
        {
            rightPanelHeader.Margin = (0, 5, 0, 0);
            rightPanelHeader.BorderWidth = 0;
            rightPanelHeader.Parent = RightPanel;
            rightPanelHeader.UpdateCardsMode = WindowSizeToContentMode.None;
            TopVisible = false;
        }

        public override Thickness DefaultPanelSize => (200, 5, 350, 150);

        /// <summary>
        /// Gets or sets whether <see cref="LeftTreeView"/> should look like <see cref="ListBox"/>.
        /// </summary>
        /// <remarks>
        /// This property must be assigned before first use of <see cref="LeftTreeView"/>
        /// </remarks>
        public bool LeftTreeViewAsListBox { get; set; } = false;

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
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Visible = false,
                        HasBorder = false,
                        Parent = RightPanel,
                    };
                    actionsControl.MouseDoubleClick += Actions_MouseDoubleClick;
                    rightPanelHeader.Add(CommonStrings.Default.NotebookTabTitleActions, actionsControl);
                    rightPanelHeader.SelectFirstTab();
                }

                return actionsControl;
            }
        }

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
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Visible = false,
                        Parent = RightPanel,
                    };

                    rightPanelHeader.Add(CommonStrings.Default.WindowTitleProperties, propertyGrid);
                    rightPanelHeader.SelectFirstTab();
                }

                return propertyGrid;
            }
        }

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
                        Parent = LeftPanel,
                        HasBorder = false,
                        FullRowSelect = true,
                    };
                    if (LeftTreeViewAsListBox)
                        leftTreeView.MakeAsListBox();
                }

                return leftTreeView;
            }
        }

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
                        Parent = BottomPanel,
                        HasBorder = false,
                    };

                    logControl.ContextMenu.Required();
                }

                return logControl;
            }
        }

        /// <summary>
        /// Binds <see cref="LogControl"/> to show messages which are logged with
        /// <see cref="Application.Log"/>.
        /// </summary>
        public virtual void BindApplicationLog()
        {
            LogControl.BindApplicationLog();
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
            actionsControl?.Add(item);
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

        private void Actions_MouseDoubleClick(object? sender, MouseEventArgs e)
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

        protected override Control CreateRightPanel() => new VerticalStackPanel();
    }
}
