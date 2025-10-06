using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;
using Alternet.UI.Localization;
using Alternet.UI.Extensions;

namespace PropertyGridSample
{
    public partial class MainWindow : Window
    {
        public static bool DoSampleLocalization = true;

        internal readonly SplittedControlsPanel panel = new();

        private readonly ContextMenuStrip propGridContextMenu = new();
        private MenuItem? resetMenu;

        private bool useIdle = false;
        private bool showDesignCorners;

        static MainWindow()
        {
            // Registers known collection property editors.
            PropertyGrid.RegisterCollectionEditors();
        }

        private static void InitSampleLocalization()
        {
            if (!DoSampleLocalization)
                return;
            DoSampleLocalization = false;

            // Sample localization of "Custom" color item (which calls color dialog)
            KnownColorStrings.Default.Custom = "Custom...";

            // Sample localization of color name
            KnownColorStrings.Default.Azure = "Azure color";

            // Sample localization of Enum property values
            var brushTypeChoices = PropertyGrid.GetChoices<BrushType>();

            brushTypeChoices.SetLabelForValue<BrushType>(
                BrushType.LinearGradient,
                "Linear Gradient");

            brushTypeChoices.SetLabelForValue<BrushType>(
                BrushType.RadialGradient,
                "Radial Gradient");

            // Sample of hiding Enum value in PropertyGrid
            var knownColorsChoices = PropertyGrid.GetChoices<PropertyGridKnownColors>();
            knownColorsChoices.RemoveValue<PropertyGridKnownColors>(PropertyGridKnownColors.Custom);
            knownColorsChoices.RemoveValue<PropertyGridKnownColors>(PropertyGridKnownColors.Black);

            // Sample localization of the property label
            var prm = PropertyGrid.GetNewItemParams(typeof(AbstractControl), "Name");
            if (prm is not null)
                prm.Label = "(name)";
        }

        private static void InitIgnorePropNames(ICollection<string> items)
        {
            items.Add("Width");
            items.Add("Height");
            items.Add("Left");
            items.Add("Top");
        }

        private bool ShowDesignCorners
        {
            get => showDesignCorners;
            set
            {
                showDesignCorners = value;
            }
        }

        public PropertyGrid PropGrid => panel.PropGrid;

        public AbstractControl ControlParent => panel.FillPanel;

        public MainWindow()
        {
            InitSampleLocalization();

            DoInsideLayout(Fn);
            panel.PropGrid.SuggestedInitDefaults();

            void Fn()
            {
                PropertyGridSettings.Default = new(this);
                SetBackground(SystemColors.Window);

                Title = "Alternet UI PropertyGrid Sample";
                Size = (900, 700);
                StartLocation = WindowStartLocation.CenterScreen;
                Padding = 5;
                Activated += MainWindow_Activated;
                Deactivated += MainWindow_Deactivated;
                SizeChanged += MainWindow_SizeChanged;

                resetMenu = propGridContextMenu.Add(CommonStrings.Default.ButtonReset);
                resetMenu.Click += ResetMenu_Click;

                propGridContextMenu.Opening += PropGridContextMenu_Opening;

                panel.BindApplicationLog();

                PropGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                    | PropertyGridApplyFlags.ReloadAllAfterSetValue;
                PropGrid.PropertyRightClick += PropGrid_PropertyRightClick;
                PropGrid.Features = PropertyGridFeature.QuestionCharInNullable;
                PropGrid.ProcessException += PropertyGrid_ProcessException;
                InitIgnorePropNames(PropGrid.IgnorePropNames);

                Icon = App.DefaultIcon;

                Children.Add(panel);

                ToolBox.SelectionChanged += ControlsListBox_SelectionChanged;
                panel.LogControl.Required();
                panel.PropGrid.Required();
                panel.ActionsControl.Required();

                panel.FillPanel.MinChildMargin = 10;
                panel.FillPanel.Paint += (s, e) =>
                {
                    if (!ShowDesignCorners)
                        return;
                    var rect = panel.FillPanel.FirstVisibleChild?.Bounds;
                    if (rect is not null)
                    {
                        var inflated = rect.Value.Inflated();
                        BorderSettings.DrawDesignCorners(
                            e.Graphics,
                            inflated,
                            BorderSettings.DebugBorder);
                    }
                };

                InitToolBox();

                PropGrid.PropertySelected += PGPropertySelected;
                PropGrid.PropertyChanged += PGPropertyChanged;
                PropGrid.PropertyChanging += PGPropertyChanging;
                PropGrid.PropertyHighlighted += PGPropertyHighlighted;
                PropGrid.PropertyRightClick += PGPropertyRightClick;
                PropGrid.PropertyDoubleClick += PGPropertyDoubleClick;
                PropGrid.ItemCollapsed += PGItemCollapsed;
                PropGrid.ItemExpanded += PGItemExpanded;
                PropGrid.LabelEditBegin += PGLabelEditBegin;
                PropGrid.LabelEditEnding += PGLabelEditEnding;
                PropGrid.ColBeginDrag += PGColBeginDrag;
                PropGrid.ColDragging += PGColDragging;
                PropGrid.ColEndDrag += PGColEndDrag;
                PropGrid.ButtonClick += PropertyGrid_ButtonClick;
                PropGrid.PropertyCustomCreate += PropGrid_PropertyCustomCreate;

                // Ctrl+Down moves to next property in PropertyGrid
                PropGrid.AddActionTrigger(
                    PropertyGridKeyboardAction.ActionNextProperty,
                    Key.DownArrow,
                    Alternet.UI.ModifierKeys.Control);

                RunTests();

                ComponentDesigner.InitDefault();
                ComponentDesigner.SafeDefault.ObjectPropertyChanged += Designer_PropertyChanged;
                ComponentDesigner.SafeDefault.MouseLeftButtonDown += Designer_MouseLeftButtonDown;

                panel.FillPanel.MouseDown += ControlPanel_MouseDown;
                panel.FillPanel.DragStart += ControlPanel_DragStart;

                Invoke(UpdatePropertyGrid);

                ControlParent.HasBorder = true;

                toolBoxFilterEdit.VerticalAlignment = VerticalAlignment.Top;
                toolBoxFilterEdit.MarginBottom = 2;
                panel.LeftPanel.Children.Prepend(toolBoxFilterEdit);

                toolBoxFilterEdit.InitFilterEdit();
                panel.LeftPanel.Layout = LayoutStyle.Vertical;
            }

            panel.AfterDoubleClickAction += (s, e) =>
                    {
                        PropGrid.ReloadPropertyValues();
                    };

            App.AddIdleTask(() =>
            {
                ToolBox.SelectFirstItemAndScroll();
            });

            toolBoxFilterEdit.DelayedTextChanged += (s, e) =>
            {
                if (DisposingOrDisposed)
                    return;
                var filter = toolBoxFilterEdit.TextBox.Text;

                if(string.IsNullOrEmpty(filter))
                {
                    ToolBox.RootItem.DoInsideUpdate(() =>
                    {
                        ToolBox.RootItem.CollapseAll();
                        ToolBox.ApplyVisibility(true);
                    });
                    return;
                }

                bool Fn(TreeViewItem item)
                {
                    var result = item.Parent == item.Root || ToolBox.ItemContainsText(item, filter);
                    return result;
                }

                ToolBox.RootItem.DoInsideUpdate(() =>
                {
                    ToolBox.RootItem.ExpandAll();
                    ToolBox.ApplyVisibilityFilter(Fn);

                    ToolBox.ApplyVisibilityFilter((item) =>
                    {
                        if(!item.IsVisible)
                            return false;
                        return !item.HasItems || item.HasVisibleItems;
                    });
                });
            };
        }


        private readonly TextBoxAndButton toolBoxFilterEdit = new()
        {
        };

        public StdTreeView ToolBox => panel.LeftListBox;

        private void PropGrid_PropertyCustomCreate(object? sender, CreatePropertyEventArgs e)
        {
        }

        private void MainWindow_SizeChanged(object? sender, EventArgs e)
        {
            App.LogIf($"Window SizeChanged {Bounds}", false);
        }

        private void MainWindow_Deactivated(object? sender, EventArgs e)
        {
            App.LogIf("Window Deactivated", false);
        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            App.LogIf("Window Activated", false);
        }

        private void PropGrid_PropertyRightClick(object? sender, EventArgs e)
        {
            var selectedProp = PropGrid.GetSelection();
            if (selectedProp == null)
                return;
            PropGrid.ShowPopupMenu(propGridContextMenu);
        }

        private void ResetMenu_Click(object? sender, EventArgs e)
        {
            var selectedProp = PropGrid.GetSelection();
            App.Log($"Reset: {selectedProp?.DefaultName}");
            PropGrid.ResetProp(selectedProp);
        }

        private void PropGridContextMenu_Opening(object? sender, CancelEventArgs e)
        {
            var mousePos = Mouse.GetPosition(PropGrid);
            var column = PropGrid.GetHitTestColumn(mousePos);
            if (column != 0)
            {
                e.Cancel = true;
                return;
            }

            var selectedProp = PropGrid.GetSelection();

            resetMenu?.SetEnabled(PropGrid.CanResetProp(selectedProp));
        }

        private static void Designer_MouseLeftButtonDown(object? sender, MouseEventArgs e)
        {
        }

        internal bool LogSize { get; set; } = false;

        private void Designer_PropertyChanged(object? sender, ObjectPropertyChangedEventArgs e)
        {
            Post(() =>
            {
                if (DisposingOrDisposed)
                    return;
                var item = ToolBox.SelectedItem as ControlListBoxItem;
                var type = item?.InstanceType;
                if (type == typeof(WelcomePage))
                    return;
                if (item?.PropInstance == e.Instance || e.Instance is null)
                    Invoke(UpdatePropertyGrid);
                Post(ControlParent.Refresh);
            });
        }

        private void PropertyGrid_ProcessException(object? sender, ThrowExceptionEventArgs e)
        {
            App.LogFileIsEnabled = true;
            LogUtils.LogException(e.InnerException);
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            Invoke(UpdatePropertyGrid);
        }

        internal void AfterSetProps()
        {

        }

        internal void UpdatePropertyGrid()
        {
            UpdatePropertyGrid(null);
        }

        internal void UpdatePropertyGrid(object? instance)
        {
            if (instance != null)
            {
                if (PropGrid.FirstItemInstance == instance)
                    return;
                PropGrid.SetProps(instance, true);
                AfterSetProps();
                return;
            }

            void DoAction()
            {
                ControlParent.GetVisibleChildOrNull()?.Hide();
                if (ToolBox.SelectedItem is not ControlListBoxItem item)
                {
                    PropGrid.Clear();
                    return;
                }

                var type = item.InstanceType;

                if (item.Instance is AbstractControl control)
                {
                    var hasBorder = PropertyGridSettings.Default!.DesignCorners
                    && item.HasTicks
                    && !control.FlagsAndAttributes.HasFlag("NoDesignBorder");

                    ControlParent.DoInsideLayout(() =>
                    {

                        if (control.Name == null)
                        {
                            var s = control.GetType().ToString();
                            var splittedText = s.Split('.');
                            control.Name
                            = splittedText[splittedText.Length - 1] + LogUtils.GenNewId().ToString();
                        }

                        if (control.Parent == null)
                        {
                            control.VerticalAlignment = VerticalAlignment.Top;
                            control.Parent = ControlParent;
                        }

                        control.Visible = true;
                        control.Refresh();
                    });

                    ShowDesignCorners = hasBorder;
                }
                else
                {
                    ShowDesignCorners = false;
                }

                ControlParent.Refresh();

                if (type == typeof(WelcomePage))
                {
                    if (useIdle)
                        App.AddIdleTask(InitDefaultPropertyGrid);
                    else
                        InitDefaultPropertyGrid();
                    useIdle = true;
                    SetBackground(SystemColors.Window);
                    panel.RemoveActions();
                }
                else
                {
                    App.AddIdleTask(() =>
                    {
                        PropGrid.SetProps(item.PropInstance, true);
                        PropGrid.Refresh();
                        AfterSetProps();
                    });

                    SetBackground(null);

                    App.AddIdleTask(() =>
                    {
                        panel.SetMethodsAndActions(type, GetSelectedControl<Control>());
                    });
                }

                App.AddIdleTask(() =>
                {
                    ControlParent.Refresh();
                });
            }

            DoAction();

        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            UpdatePropertyGrid();
        }

        protected override void DisposeManaged()
        {
            ComponentDesigner.SafeDefault.ObjectPropertyChanged -= Designer_PropertyChanged;
            ComponentDesigner.SafeDefault.MouseLeftButtonDown -= Designer_MouseLeftButtonDown;

            base.DisposeManaged();
        }

        private void SetBackground(Color? color)
        {
        }

        public class SettingsControl : HiddenBorder
        {
        }
    }
}