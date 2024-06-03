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
    internal class DeveloperToolsPanel : Control
    {
        private static DeveloperToolsWindow? devToolsWindow;

        private readonly ActionsListBox actionsListBox = new()
        {
            HasBorder = false,
        };

        private readonly LogListBox logListBox = new()
        {
            HasBorder = false,
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
        };

        private readonly SideBarPanel centerNotebook = new()
        {
        };

        private readonly SideBarPanel rightNotebook = new()
        {
        };

        private ListBox? typesListBox;
        private ListBox? controlsListBox;
        private bool insideSetProps;

        public DeveloperToolsPanel()
        {
            centerNotebook.Parent = panel.FillPanel;
            rightNotebook.Parent = panel.RightPanel;
            panel.Parent = this;

            centerNotebook.Add("Output", logListBox);
            logListBox.ContextMenu.Required();
            logListBox.MenuItemShowDevTools?.SetEnabled(false);
            logListBox.BindApplicationLog();

            rightNotebook.Add("Actions", actionsListBox);

            propGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            propGrid.Features = PropertyGridFeature.QuestionCharInNullable;
            propGrid.ProcessException += PropertyGrid_ProcessException;
            propGrid.CreateStyleEx = PropertyGridCreateStyleEx.AlwaysAllowFocus;
            rightNotebook.Add("Properties", propGrid);

            InitActions();

            TypesListBox.SelectionChanged += TypesListBox_SelectionChanged;
        }

        public SideBarPanel CenterNotebook => centerNotebook;

        public SideBarPanel RightNotebook => rightNotebook;

        public PropertyGrid PropGrid => propGrid;

        internal object? LastFocusedControl { get; set; }

        [Browsable(false)]
        internal ListBox TypesListBox
        {
            get
            {
                if (typesListBox == null)
                {
                    typesListBox = new ListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };

                    IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));

                    void AddControl(Type type)
                    {
                        typesListBox.Add(type.Name, type);
                    }

                    AddControl(typeof(Control));
                    AddControl(typeof(FrameworkElement));
                    AddControl(typeof(UIElement));

                    foreach (var type in result)
                    {
                        if (type.Assembly != typeof(Control).Assembly)
                            continue;
                        if (!AssemblyUtils.HasOwnEvents(type))
                            continue;
                        AddControl(type);
                    }

                    centerNotebook.Add("Types", typesListBox);
                }

                return typesListBox;
            }
        }

        [Browsable(false)]
        internal ListBox ControlsListBox
        {
            get
            {
                if (controlsListBox == null)
                {
                    controlsListBox = new ListBox()
                    {
                        Parent = this,
                        HasBorder = false,
                    };
                    centerNotebook.Add("Controls", controlsListBox);
                }

                return controlsListBox;
            }
        }

        /// <summary>
        /// Shows developer tools window.
        /// </summary>
        public static void ShowDeveloperTools()
        {
            if (devToolsWindow is null)
            {
                devToolsWindow = new();
                devToolsWindow.Closing += DevToolsWindow_Closing;
                devToolsWindow.Disposed += DevToolsWindow_Disposed;
            }

            devToolsWindow.Show();

            static void DevToolsWindow_Closing(object? sender, WindowClosingEventArgs e)
            {
            }

            static void DevToolsWindow_Disposed(object? sender, EventArgs e)
            {
                devToolsWindow = null;
            }
        }

        /// <summary>
        /// Outputs all <see cref="Control"/> descendants to the debug console.
        /// </summary>
        public static void ControlsToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                AssemblyUtils.GetTypeDescendants(typeof(Control)),
                (t) => Debug.WriteLine(t.Name));
        }

        /*/// <summary>
        /// Outputs all <see cref="Native.NativeObject"/> descendants to the debug console.
        /// </summary>
        public static void NativeObjectToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                AssemblyUtils.GetTypeDescendants(typeof(Native.NativeObject), true, false),
                (t) => Debug.WriteLine(t.Name));
        }*/

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
                    var isBinded = LogUtils.IsEventLogged(type, item);
                    var prop = eventGrid.CreateBoolItem(item.Name, null, isBinded);
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

        private static void Event_PropertyChanged(object? sender, EventArgs e)
        {
            if (sender is not IPropertyGridItem item)
                return;
            var type = item.FlagsAndAttributes.Attr.GetAttribute<Type?>("InstanceType");
            var eventInfo = item.FlagsAndAttributes.Attr.GetAttribute<EventInfo?>("EventInfo");
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

        private void ControlsActionMainForm()
        {
            rightNotebook.SelectedControl = propGrid;
            PropGridSetProps(App.FirstWindow());
        }

        private void ControlsActionFocusedControl()
        {
            PropGridSetProps(LastFocusedControl);
        }

        private void TypesListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var item = TypesListBox.SelectedItem as ListControlItem;
            var type = item?.Value as Type;
            UpdateEventsPropertyGrid(propGrid, type);
        }

        private void AddLogAction(string title, Action action)
        {
            actionsListBox.AddAction(title, Fn);

            void Fn()
            {
                App.DoInsideBusyCursor(() =>
                {
                    App.LogBeginUpdate();
                    try
                    {
                        action();
                    }
                    finally
                    {
                        App.LogEndUpdate();
                    }
                });
            }
        }

        private void InitActions()
        {
            LogUtils.EnumLogActions(AddLogAction);

            AddAction("Show Props FirstWindow", ControlsActionMainForm);
            AddAction("Show Props FocusedControl", ControlsActionFocusedControl);

            AddAction("Show Second MainForm", () =>
            {
                var type = App.FirstWindow()?.GetType();
                var instance = Activator.CreateInstance(type ?? typeof(Window)) as Window;
                instance?.Show();
            });

            AddAction("Exception: Throw C++", () =>
            {
                App.Current.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                App.Current.SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode.CatchException);
                WebBrowser.DoCommandGlobal("CppThrow");
            });

            AddAction("Exception: Throw C#", () =>
            {
                App.Current.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                App.Current.SetUnhandledExceptionModeIfDebugger(UnhandledExceptionMode.CatchException);
                throw new FileNotFoundException("Test message", "MyFileName.dat");
            });

            AddAction("Exception: Show ThreadExceptionWindow", () =>
            {
                try
                {
                    throw new ApplicationException("This is exception message");
                }
                catch (Exception e)
                {
                    App.ShowExceptionWindow(e, "This is an additional info", true);
                }
            });

            AddAction("Exception: HookExceptionEvents()", DebugUtils.HookExceptionEvents);

            AddLogAction("Log control info", () => { LogUtils.LogControlInfo(this); });

            AddLogAction("Log NativeControlPainter metrics", () =>
            {
                ControlPainter.LogPartSize(this);
            });
        }
    }
}