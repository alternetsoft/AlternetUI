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
    /// <summary>
    /// Descendant of <see cref="SplittedPanel"/> control with additional features.
    /// </summary>
    public partial class SplittedControlsPanel : SplittedPanel
    {
        private StdTreeView? leftListBox;
        private PropertyGrid? propertyGrid;
        private LogListBox? logControl;
        private StdTreeView? actionsControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SplittedControlsPanel"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SplittedControlsPanel(AbstractControl parent)
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
        /// Gets the control with actions list.
        /// </summary>
        [Browsable(false)]
        public StdTreeView ActionsControl
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
        /// Gets <see cref="StdTreeView"/> control on the left pane.
        /// </summary>
        [Browsable(false)]
        public StdTreeView LeftTreeView
        {
            get
            {
                return LeftListBox;
            }
        }

        /// <summary>
        /// Gets the control on the left pane.
        /// </summary>
        [Browsable(false)]
        public StdTreeView LeftListBox
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
            ActionsControl.Add(string.Empty);
        }

        /// <summary>
        /// Create <see cref="Action"/> which can be added to the <see cref="ActionsControl"/>.
        /// </summary>
        /// <param name="title">Action title.</param>
        /// <param name="action">Action method.</param>
        public virtual TreeViewItem CreateAction(string title, Action? action)
        {
            TreeViewItem item = new(title)
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

            return item;
        }

        /// <summary>
        /// Adds <see cref="Action"/> to the <see cref="ActionsControl"/>.
        /// </summary>
        /// <param name="title">Action title.</param>
        /// <param name="action">Action method.</param>
        public virtual TreeViewItem AddAction(string title, Action? action)
        {
            var item = CreateAction(title, action);
            ActionsControl.Add(item);
            return item;
        }

        /// <summary>
        /// Sets methods and actions for the specified <paramref name="type"/>
        /// and <paramref name="instance"/>.
        /// </summary>
        /// <param name="type">The type for which methods and actions are added.</param>
        /// <param name="instance">The instance of the specified type which is
        /// used when invoking methods.</param>
        /// <param name="addMethods">Whether to add methods of the specified type as actions.</param>
        public virtual void SetMethodsAndActions(
            Type type,
            object? instance,
            bool addMethods = true)
        {
            BeginUpdateActions();
            try
            {
                RemoveActions();
                AddActions(type);
                if (addMethods)
                {
                    ActionsControl.AddSeparator();
                    AddMethodsAsActions(type, instance);
                }
            }
            finally
            {
                EndUpdateActions();
            }
        }

        /// <summary>
        /// Adds methods of the <paramref name="type"/> as actions to the
        /// <see cref="ActionsControl"/>.
        /// </summary>
        /// <param name="type">The type for which methods are added as actions.</param>
        /// <param name="instance">The instance of the specified type which is
        /// used when invoking methods.</param>
        public virtual void AddMethodsAsActions(Type type, object? instance)
        {
            var methods = AssemblyUtils.EnumMethods(type);

            var actionsToSort = new List<TreeViewItem>();

            foreach (var method in methods)
            {
                if (method.IsSpecialName)
                    continue;
                if (method.IsGenericMethod)
                    continue;
                var retParam = method.ReturnParameter;
                var resultIsVoid = retParam.ParameterType == typeof(void);

                var methodParameters = method.GetParameters();
                if (methodParameters.Length > 0)
                    continue;
                var browsable = AssemblyUtils.GetBrowsable(method);
                if (!browsable)
                    continue;
                var methodName = $"{method.Name}()";
                var methodNameForDisplay = $"{method.DeclaringType?.Name}.<b>{methodName}</b>";

                if (resultIsVoid)
                {
                    methodNameForDisplay = $"{methodNameForDisplay} : void";
                }
                else
                {
                    var retParamDisplayName =
                    AssemblyUtils.GetTypeDisplayName(retParam.ParameterType);

                    retParamDisplayName = StringUtils.RemovePrefix(
                        retParamDisplayName,
                        "Alternet.UI.",
                        StringComparison.Ordinal);

                    retParamDisplayName = StringUtils.RemovePrefix(
                        retParamDisplayName,
                        "Alternet.Drawing.",
                        StringComparison.Ordinal);

                    retParamDisplayName = StringUtils.RemovePrefix(
                        retParamDisplayName,
                        "Alternet.",
                        StringComparison.Ordinal);

                    methodNameForDisplay
                    = $"{methodNameForDisplay} : {retParamDisplayName}";
                }

                var item = CreateAction(methodName, () =>
                {
                    var selectedControl = instance;
                    if (selectedControl is null)
                        return;
                    AssemblyUtils.InvokeMethodAndLogResult(selectedControl, method);
                });

                actionsToSort.Add(item);

                item.DisplayText = methodNameForDisplay;
                item.TextHasBold = true;
            }

            var orderedActions = actionsToSort.OrderBy(item => item.DisplayText);
            foreach (var action in orderedActions)
            {
                ActionsControl.Add(action);
            }
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
        /// Prepares the associated actions control for a batch of updates,
        /// minimizing redraws and improving
        /// performance.
        /// </summary>
        /// <remarks>This method should be called before performing multiple
        /// updates to the actions
        /// control to avoid unnecessary rendering and improve efficiency. Ensure that
        /// <see cref="EndUpdateActions"/>
        /// is called after the updates are completed to resume normal rendering.</remarks>
        public virtual void BeginUpdateActions()
        {
            if (actionsControl is null)
                return;
            actionsControl.BeginUpdate();
        }

        /// <summary>
        /// Ends the update process for the associated actions control, applying
        /// any pending changes.
        /// </summary>
        /// <remarks>This method should be called after making multiple updates
        /// to the actions control to
        /// finalize the changes and refresh its state. If the actions control
        /// is null, the method performs no
        /// operation.</remarks>
        public virtual void EndUpdateActions()
        {
            if (actionsControl is null)
                return;
            actionsControl.EndUpdate();
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
