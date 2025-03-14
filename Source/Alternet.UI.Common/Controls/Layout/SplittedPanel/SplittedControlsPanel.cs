﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Descendant of <see cref="SplittedPanel"/> control with additional features.
    /// </summary>
    public class SplittedControlsPanel : SplittedPanel
    {
        private TreeView? leftTreeView;
        private VirtualListBox? leftListBox;
        private PropertyGrid? propertyGrid;
        private LogListBox? logControl;
        private VirtualListBox? actionsControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedControlsPanel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SplittedControlsPanel(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedControlsPanel"/> class.
        /// </summary>
        public SplittedControlsPanel()
        {
            TopVisible = false;
        }

        /// <summary>
        /// Occurs after an action is double clicked in the <see cref="ActionsControl"/>.
        /// </summary>
        public event EventHandler? AfterDoubleClickAction;

        /// <inheritdoc/>
        public override Thickness DefaultPanelSize => (200, 5, 350, 150);

        /// <summary>
        /// Gets or sets whether <see cref="LeftTreeView"/> should look like <see cref="ListBox"/>.
        /// </summary>
        /// <remarks>
        /// This property must be assigned before first use of <see cref="LeftTreeView"/>
        /// </remarks>
        public virtual bool LeftTreeViewAsListBox { get; set; } = false;

        /// <summary>
        /// Gets the control with actions list.
        /// </summary>
        [Browsable(false)]
        public VirtualListBox ActionsControl
        {
            get
            {
                if (actionsControl == null)
                {
                    actionsControl = new()
                    {
                        VerticalAlignment = UI.VerticalAlignment.Fill,
                        Visible = false,
                        HasBorder = false,
                    };
                    RightPanel.Add(CommonStrings.Default.NotebookTabTitleActions, actionsControl);
                    RightPanel.SelectFirstTab();
                }

                return actionsControl;
            }
        }

        /// <inheritdoc cref="SplittedPanel.RightPanel"/>
        [Browsable(false)]
        public new SideBarPanel RightPanel => (SideBarPanel)base.RightPanel;

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
                        VerticalAlignment = UI.VerticalAlignment.Fill,
                        Visible = false,
                    };

                    RightPanel.Add(
                        CommonStrings.Default.WindowTitleProperties,
                        propertyGrid);
                    RightPanel.SelectFirstTab();
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
                        HorizontalAlignment = UI.HorizontalAlignment.Fill,
                        VerticalAlignment = UI.VerticalAlignment.Fill,
                    };
                    if (LeftTreeViewAsListBox)
                        leftTreeView.MakeAsListBox();
                }

                return leftTreeView;
            }
        }

        /// <summary>
        /// Gets <see cref="TreeView"/> control on the left pane.
        /// </summary>
        [Browsable(false)]
        public VirtualListBox LeftListBox
        {
            get
            {
                leftListBox ??= new()
                {
                    Parent = LeftPanel,
                    HasBorder = false,
                    HorizontalAlignment = UI.HorizontalAlignment.Fill,
                    VerticalAlignment = UI.VerticalAlignment.Fill,
                };

                return leftListBox;
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
        /// <see cref="App.Log"/>.
        /// </summary>
        [Browsable(false)]
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
        public virtual ListControlItem AddAction(string title, Action? action)
        {
            ListControlItem item = new(title)
            {
                DoubleClickAction = () =>
                {
                    RunWhenIdle(() =>
                    {
                        if (DisposingOrDisposed)
                            return;
                        logControl?.Log("Do action: " + title);
                        action?.Invoke();
                        AfterDoubleClickAction?.Invoke(this, EventArgs.Empty);
                    });
                },
            };

            ActionsControl.Required();
            actionsControl?.Add(item);
            return item;
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
            var orderedActions = actions.OrderBy(item => item.Title);
            foreach (var action in orderedActions)
            {
                AddAction(action.Title, action.Action);
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

        /// <inheritdoc/>
        protected override AbstractControl CreateRightPanel()
        {
            return new SideBarPanel();
        }
    }
}
