using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.UI
{
    internal class PanelDevTools : HiddenBorder
    {
        private static WindowDevTools? devToolsWindow;

        private readonly ActionsListBox actionsListBox = new()
        {
            HasBorder = false,
        };

        private readonly LogListBox logListBox = new()
        {
            HasBorder = false,
        };

        private readonly Panel logListBoxPanel = new()
        {
            Layout = LayoutStyle.Vertical,
        };

        private readonly Panel actionsListBoxPanel = new()
        {
            Layout = LayoutStyle.Vertical,
        };

        private readonly ToolBar logListBoxToolBar = new()
        {
        };

        private readonly ToolBar actionsListBoxToolBar = new()
        {
        };

        private readonly SplittedPanel panel = new()
        {
            TopVisible = false,
            LeftVisible = false,
            BottomVisible = false,
            RightPanelWidth = 350,
        };

        private readonly PropertyGrid propGrid = new()
        {
            HasBorder = false,
        };

        private readonly TabControl centerNotebook = new()
        {
            TabAlignment = TabAlignment.Bottom,
        };

        private readonly TabControl rightNotebook = new()
        {
            TabAlignment = TabAlignment.Bottom,
        };

        private VirtualListBox? typesListBox;
        private VirtualListBox? controlsListBox;
        private bool insideSetProps;

        static PanelDevTools()
        {
            LogUtils.RegisterLogAction("Show Props Main Window", ControlsActionMainForm);
            LogUtils.RegisterLogAction("Show Props Second Window", ControlsActionSecondForm);
            LogUtils.RegisterLogAction("Show Props Focused Control", ControlsActionFocusedControl);

            LogUtils.RegisterLogAction(
                "Show Props Focused Window",
                ControlsActionFocusedControlForm);
        }

        public PanelDevTools()
        {
            centerNotebook.Parent = panel.FillPanel;
            rightNotebook.Parent = panel.RightPanel;
            panel.Parent = this;

            logListBox.ContextMenu.Required();

            logListBoxToolBar.VerticalAlignment = VerticalAlignment.Top;
            logListBoxToolBar.OnlyBottomBorder();

            logListBoxToolBar.Parent = logListBoxPanel;

            logListBoxToolBar.AddText("Output");

            var btnCopy = logListBoxToolBar.AddSpeedBtn(
                    "Copy selected items",
                    KnownSvgImages.ImgCopy,
                    (_, _) => logListBox.ListBox.SelectedItemsToClipboard());
            logListBoxToolBar.SetToolAlignRight(btnCopy);

            var btnClear = logListBoxToolBar.AddSpeedBtn(
                    "Clear items",
                    KnownSvgImages.ImgTrashCan,
                    (_, _) => logListBox.Clear());
            logListBoxToolBar.SetToolAlignRight(btnClear);

            var btnLogListBoxSettings = logListBoxToolBar.AddSpeedBtn(
                UI.Localization.CommonStrings.Default.ButtonMoreActions, KnownSvgImages.ImgMoreActions);
            logListBoxToolBar.SetToolDropDownMenu(btnLogListBoxSettings, logListBox.ContextMenu);
            logListBoxToolBar.SetToolAlignRight(btnLogListBoxSettings);

            logListBox.VerticalAlignment = VerticalAlignment.Fill;
            logListBox.Parent = logListBoxPanel;

            centerNotebook.Add("Output", logListBoxPanel);
            logListBox.MenuItemShowDevTools?.SetEnabled(false);
            logListBox.BindApplicationLog();

            actionsListBoxToolBar.AddText("Actions");
            actionsListBoxToolBar.OnlyBottomBorder();
            actionsListBoxToolBar.Parent = actionsListBoxPanel;
            var btnRunAction = actionsListBoxToolBar.AddSpeedBtn(
                "Run action",
                KnownSvgImages.ImgDebugRun,
                (_, _) => actionsListBox.ListBox.RunSelectedItemDoubleClickAction());
            actionsListBoxToolBar.SetToolAlignRight(btnRunAction);

            var btnActionsListBoxSettings = actionsListBoxToolBar.AddSpeedBtn(
                UI.Localization.CommonStrings.Default.ButtonMoreActions, KnownSvgImages.ImgMoreActions);
            actionsListBoxToolBar.SetToolDropDownMenu(
                btnActionsListBoxSettings, null);
            actionsListBoxToolBar.SetToolAlignRight(btnActionsListBoxSettings);
            actionsListBoxToolBar.SetToolEnabled(btnActionsListBoxSettings, false);

            actionsListBox.VerticalAlignment = VerticalAlignment.Fill;
            actionsListBox.Parent = actionsListBoxPanel;

            rightNotebook.Add("Actions", actionsListBoxPanel);

            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propGrid.Features = PropertyGridFeature.QuestionCharInNullable;
            propGrid.ProcessException += PropertyGrid_ProcessException;
            rightNotebook.Add("Members", propGrid);

            InitActions();

            TypesListBox.SelectionChanged += TypesListBox_SelectionChanged;

            LogUtils.LogVersion();
        }

        public static PanelDevTools? DevPanel => devToolsWindow?.DevPanel;

        public TabControl CenterNotebook => centerNotebook;

        public TabControl RightNotebook => rightNotebook;

        public PropertyGrid PropGrid => propGrid;

        internal object? LastFocusedControl { get; set; }

        [Browsable(false)]
        internal VirtualListBox TypesListBox
        {
            get
            {
                if (typesListBox == null)
                {
                    typesListBox = new VirtualListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };

                    IEnumerable<Type> result
                        = AssemblyUtils.GetTypeDescendants(typeof(AbstractControl));

                    void AddControl(Type type)
                    {
                        var typeName = AssemblyUtils.GetTypeDisplayName(type);
                        if (typeName.Contains("<"))
                            return;
                        ListControlItem item = new(typeName, type);
                        typesListBox.Add(item);
                    }

                    AddControl(typeof(AbstractControl));
                    AddControl(typeof(FrameworkElement));

                    foreach (var type in result)
                    {
                        if (type.Assembly != typeof(AbstractControl).Assembly)
                            continue;
                        if (!AssemblyUtils.HasOwnEvents(type))
                            continue;
                        AddControl(type);
                    }

                    typesListBox.Items.Sort();

                    centerNotebook.Add("Types", typesListBox);
                }

                return typesListBox;
            }
        }

        [Browsable(false)]
        internal VirtualListBox ControlsListBox
        {
            get
            {
                if (controlsListBox == null)
                {
                    controlsListBox = new VirtualListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };
                    centerNotebook.Add("Controls", controlsListBox);
                }

                return controlsListBox;
            }
        }

        public static WindowDevTools GetOrCreateDeveloperTools()
        {
            if (DebugUtils.RecreateDeveloperToolsWindow && devToolsWindow is not null)
            {
                devToolsWindow.Hide();
                devToolsWindow.Dispose();
                devToolsWindow = null;
            }

            if (devToolsWindow is null)
            {
                devToolsWindow = new();
                devToolsWindow.Closing += DevToolsWindow_Closing;
                devToolsWindow.Disposed += DevToolsWindow_Disposed;
            }

            return devToolsWindow;

            static void DevToolsWindow_Closing(object? sender, WindowClosingEventArgs e)
            {
            }

            static void DevToolsWindow_Disposed(object? sender, EventArgs e)
            {
                devToolsWindow = null;
            }
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            GetOrCreateDeveloperTools().ShowAndFocus();
        }

        /// <summary>
        /// Outputs all <see cref="AbstractControl"/> descendants to the debug console.
        /// </summary>
        public static void ControlsToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                AssemblyUtils.GetTypeDescendants(typeof(AbstractControl)),
                (t) => Debug.WriteLine(t.Name));
        }

        public static void UpdateEventsPropertyGrid(PropertyGrid eventGrid, Type? type)
        {
            eventGrid.DoInsideUpdate(() =>
            {
                eventGrid.Clear();
                if (type == null)
                    return;
                var events = AssemblyUtils.EnumEvents(type, true);

                foreach (var item in events)
                {
                    if (item.DeclaringType != type)
                        continue;
                    var isBound = LogUtils.IsEventLogged(type, item);
                    var prop = eventGrid.CreateBoolItem(item.Name, null, isBound);
                    prop.FlagsAndAttributes.Attr["InstanceType"] = type;
                    prop.FlagsAndAttributes.Attr["EventInfo"] = item;
                    prop.PropertyChanged += Event_PropertyChanged;
                    eventGrid.Add(prop);
                }

                eventGrid.FitColumns();
            });
        }

        public void AddAction(string title, Action? action)
        {
            actionsListBox.AddAction(title, action);
        }

        internal void PropGridSetProps(object? instance)
        {
            if (insideSetProps)
                return;
            insideSetProps = true;
            try
            {
                propGrid.DoInsideUpdate(() =>
                {
                    propGrid.Clear();
                    if (instance is null)
                        return;
                    propGrid.AddConstItem("(type)", "(type)", instance.GetType().Name);
                    propGrid.AddProps(instance, null, true);
                    propGrid.FitColumns();
                });
            }
            finally
            {
                insideSetProps = false;
            }
        }

        private static void ControlsActionMainForm()
        {
            if (DevPanel is null)
                return;
            DevPanel.rightNotebook.SelectedControl = DevPanel.propGrid;
            DevPanel.PropGridSetProps(App.MainWindow);
        }

        private static void ControlsActionSecondForm()
        {
            if (DevPanel is null || App.Current.Windows.Count < 2)
                return;
            DevPanel.rightNotebook.SelectedControl = DevPanel.propGrid;
            DevPanel.PropGridSetProps(App.Current.Windows[1]);
        }

        private static void ControlsActionFocusedControlForm()
        {
            if (DevPanel is null)
                return;
            DevPanel.rightNotebook.SelectedControl = DevPanel.propGrid;
            DevPanel.PropGridSetProps((DevPanel.LastFocusedControl as AbstractControl)?.Root);
        }

        private static void ControlsActionFocusedControl()
        {
            if (DevPanel is null)
                return;
            DevPanel.PropGridSetProps(DevPanel.LastFocusedControl);
        }

        private static void Event_PropertyChanged(object? sender, EventArgs e)
        {
            if (sender is not IPropertyGridItem item)
                return;
            var type = item.FlagsAndAttributes.Attr.GetAttribute<Type>("InstanceType");
            var eventInfo = item.FlagsAndAttributes.Attr.GetAttribute<EventInfo>("EventInfo");
            var value = item.Owner.GetPropertyValueAsBool(item);
            LogUtils.SetEventLogged(type, eventInfo, value);
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            rightNotebook.SelectedControl = propGrid;
            controlsListBox?.SelectedAction?.Invoke();
        }

        private void PropertyGrid_ProcessException(object? sender, ThrowExceptionEventArgs e)
        {
            App.LogFileIsEnabled = true;
            LogUtils.LogException(e.InnerException);
        }

        private void TypesListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var item = TypesListBox.SelectedItem as ListControlItem;
            var type = item?.Value as Type;
            UpdateEventsPropertyGrid(propGrid, type);
        }

        private void InitActions()
        {
            LogUtils.EnumLogActions(Fn);

            void Fn(string title, Action action)
            {
                if(!title.StartsWith("Test "))
                    actionsListBox.AddBusyAction(title, action);
            }
        }
    }
}