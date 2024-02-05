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

namespace PropertyGridSample
{
    public partial class MainWindow : Window
    {
        internal readonly SplittedControlsPanel panel = new();

        private readonly Control controlPanel = new()
        {
        };

        private readonly Panel parentParent = new()
        {
            Padding = (5, 15, 5, 15),
        };

        private readonly Border controlPanelBorder = new()
        {
            BorderColor = Color.Red,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Padding = 0,
        };

        private readonly ContextMenuStrip propGridContextMenu = new();
        private readonly MenuItem resetMenu;

        private bool updatePropertyGrid = false;
        private bool useIdle = false;

        static MainWindow()
        {
            InitSampleLocalization();

            AuiNotebook.DefaultCreateStyle = AuiNotebookCreateStyle.Top;

            // Registers known collection property editors.
            PropertyGrid.RegisterCollectionEditors();
        }

        private static void InitSampleLocalization()
        {
            // Sample localization of "Custom" color item (which calls color dialog)
            KnownColorStrings.Default.Custom = "Custom...";

            // Sample localization of color name
            KnownColorStrings.Default.Azure = "Azure color";

            // Sample localization of Enum property values
            var brushTypeChoices = PropertyGrid.GetChoices<BrushType>();
            brushTypeChoices.SetLabelForValue<BrushType>(BrushType.LinearGradient, "Linear Gradient");
            brushTypeChoices.SetLabelForValue<BrushType>(BrushType.RadialGradient, "Radial Gradient");

            // Sample of hiding Enum value in PropertyGrid
            var knownColorsChoices = PropertyGrid.GetChoices<PropertyGridKnownColors>();
            knownColorsChoices.RemoveValue<PropertyGridKnownColors>(PropertyGridKnownColors.Custom);
            knownColorsChoices.RemoveValue<PropertyGridKnownColors>(PropertyGridKnownColors.Black);

            // Sample localization of the property label
            var prm = PropertyGrid.GetNewItemParams(typeof(Control), "Name");
            prm.Label = "(name)";
        }

        private static void InitIgnorePropNames(ICollection<string> items)
        {
            if (Application.IsLinuxOS)
                return;
            items.Add("Width");
            items.Add("Height");
            items.Add("Left");
            items.Add("Top");
            /*items.Add("SuggestedWidth");
            items.Add("SuggestedHeight");*/
        }

        public PropertyGrid PropGrid => panel.PropGrid;

        public Border ControlPanelBorder => controlPanelBorder;

        public MainWindow()
        {
            Activated += MainWindow_Activated;
            Deactivated += MainWindow_Deactivated;
            SizeChanged += MainWindow_SizeChanged;

            resetMenu = propGridContextMenu.Add(CommonStrings.Default.ButtonReset);
            resetMenu.Click += ResetMenu_Click;

            propGridContextMenu.Opening += PropGridContextMenu_Opening;

            controlPanelBorder.Normal.Paint += BorderSettings.DrawDesignCorners;
            controlPanelBorder.Normal.DrawDefaultBorder = false;

            panel.BindApplicationLog();

            PropGrid.ApplyFlags |= PropertyGridApplyFlags.PropInfoSetValue
                | PropertyGridApplyFlags.ReloadAfterSetValue;
            PropGrid.PropertyRightClick += PropGrid_PropertyRightClick;
            PropGrid.Features = PropertyGridFeature.QuestionCharInNullable;
            PropertyGridSettings.Default = new(this);
            PropGrid.ProcessException += PropertyGrid_ProcessException;
            InitIgnorePropNames(PropGrid.IgnorePropNames);
            PropGrid.CreateStyleEx = PropertyGridCreateStyleEx.AlwaysAllowFocus;

            Icon = new("embres:PropertyGridSample.Sample.ico");

            InitializeComponent();

            Children.Add(panel);

            panel.LeftTreeView.SelectionChanged += ControlsListBox_SelectionChanged;
            panel.LogControl.Required();
            panel.PropGrid.Required();
            panel.ActionsControl.Required();

            controlPanel.Parent = controlPanelBorder;

            var parentParentColor = SystemColors.Window;

            controlPanelBorder.Parent = parentParent;

            parentParent.Parent = panel.FillPanel;

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

            panel.PropGrid.SuggestedInitDefaults();

            panel.LeftTreeView.SelectedItem = panel.LeftTreeView.FirstItem;

            Application.Current.Idle += ApplicationIdle;
            RunTests();

            ComponentDesigner.InitDefault();
            ComponentDesigner.SafeDefault.PropertyChanged += Designer_PropertyChanged;
            ComponentDesigner.SafeDefault.MouseLeftButtonDown += Designer_MouseLeftButtonDown;

            panel.FillPanel.MouseDown += ControlPanel_MouseDown;
            controlPanel.DragStart += ControlPanel_DragStart;

            panel.WriteWelcomeLogMessages();
        }

        private void PropGrid_PropertyCustomCreate(object? sender, CreatePropertyEventArgs e)
        {
            /*
            if (e.PropInfo.PropertyType != typeof(Color))
                return;
            e.PropertyItem = CreatePropertyAsColor(e.Label, e.PropName, e.Instance, e.PropInfo);
            e.Handled = true;
            */
        }

        public virtual IPropertyGridItem CreatePropertyAsColor(
                    string? label,
                    string? name,
                    object instance,
                    PropertyInfo propInfo)
        {
            string propName = propInfo.Name;
            label ??= propName;

            var color = propInfo.GetValue(instance, null) as Color;
            string strValue = PropGrid.ColorToString(color);

            var prm = PropertyGrid.GetNewItemParams(instance.GetType(), propInfo);
            prm.EditKindString = PropertyGridEditKindString.Ellipsis;
            prm.ButtonClick += Prm_ButtonClick;
            prm = prm.Constructed;

            var prop = PropGrid.CreateStringItemWithKind(label, propName, strValue, prm);

            PropGrid.OnPropertyCreated(prop, instance, propInfo, prm);
            return prop;

            void Prm_ButtonClick(object? sender, EventArgs e)
            {
                Application.AddIdleTask(Fn);

                void Fn()
                {
                    if (sender is not IPropertyGridItem item)
                        return;
                    var value = PropGrid.GetPropertyValueAsString(item);
                    Color? color = null;

                    PropGrid.AvoidException(() =>
                    {
                        color = Color.Parse(value);
                    });

                    ColorDialog.Default.Color = color ?? Color.Black;
                    if (ColorDialog.Default.ShowModal() != ModalResult.Accepted)
                        return;
                    var newValue = PropGrid.ColorToString(ColorDialog.Default.Color);
                    if (newValue == value)
                        return;
                    PropGrid.SetPropertyValueAsStr(item, newValue);
                }
            }
        }

        private void MainWindow_SizeChanged(object? sender, EventArgs e)
        {
            Application.LogIf($"Window SizeChanged {Bounds}", false);
        }

        private void MainWindow_Deactivated(object? sender, EventArgs e)
        {
            Application.LogIf("Window Deactivated", false);
        }

        private void MainWindow_Activated(object? sender, EventArgs e)
        {
            Application.LogIf("Window Activated", false);
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
            Application.Log($"Reset: {selectedProp?.DefaultName}");
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

            resetMenu.Enabled = PropGrid.CanResetProp(selectedProp);
        }

        private static void Designer_MouseLeftButtonDown(object? sender, MouseEventArgs e)
        {
            /*if(sender is Control control)
            {
                var name = control.Name ?? control.GetType().Name;
                Application.LogNameValue("MouseLeftButtonDown", name);
            }*/

            /*
            if (sender is not GroupBox groupBox)
                return;
            Application.LogNameValue("groupBox.GetTopBorderForSizer", groupBox.GetTopBorderForSizer());
            Application.LogNameValue("groupBox.GetOtherBorderForSizer", groupBox.GetOtherBorderForSizer());
            Application.LogNameValue("groupBox.IntrinsicPreferredSizePadding", groupBox.IntrinsicPreferredSizePadding);
            Application.LogNameValue("groupBox.Padding", groupBox.Padding);
            Application.LogNameValue("groupBox.IntrinsicLayoutPadding", groupBox.IntrinsicLayoutPadding);
            */
        }

        internal bool LogSize { get; set; } = false;

        private void LeftTreeView_SizeChanged(object? sender, EventArgs e)
        {
            if(LogSize)
                Application.Log("LeftTreeView_SizeChanged");
        }

        private void CenterNotebook_LayoutUpdated(object? sender, EventArgs e)
        {
            if (LogSize)
                Application.Log("CenterNotebook_LayoutUpdated");
        }

        private void CenterNotebook_SizeChanged(object? sender, EventArgs e)
        {
            if (LogSize)
                Application.Log("CenterNotebook_SizeChanged");
        }

        private void Designer_PropertyChanged(object? sender, ObjectPropertyChangedEventArgs e)
        {
            var item = panel.LeftTreeView.SelectedItem as ControlListBoxItem;
            var type = item?.InstanceType;
            if (type == typeof(WelcomePage))
                return;
            if(item?.Instance == e.Instance || e.Instance is null)
                updatePropertyGrid = true;
        }

        private void PropertyGrid_ProcessException(object? sender, ControlExceptionEventArgs e)
        {
            Application.LogFileIsEnabled = true;
            LogUtils.LogException(e.InnerException);
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            updatePropertyGrid = true;
        }

        internal void UpdatePropertyGrid(object? instance = null)
        {
            if(instance != null)
            {
                if (PropGrid.FirstItemInstance == instance)
                    return;
                PropGrid.SetProps(instance, true);
                return;
            }

            void DoAction()
            {
                controlPanel.GetVisibleChildOrNull()?.Hide();
                if (panel.LeftTreeView.SelectedItem is not ControlListBoxItem item)
                {
                    PropGrid.Clear();
                    return;
                }

                var type = item.InstanceType;

                if (item.Instance is Control control)
                {
                    parentParent.DoInsideLayout(() =>
                    {
                        controlPanelBorder.HasBorder = PropertyGridSettings.Default!.DesignCorners
                        && item.HasTicks
                        && !control.FlagsAndAttributes.HasFlag("NoDesignBorder");

                        if (control.Name == null)
                        {
                            var s = control.GetType().ToString();
                            var splitted = s.Split('.');
                            control.Name = splitted[splitted.Length - 1] + LogUtils.GenNewId().ToString();
                        }

                        if (control.Parent == null)
                        {
                            control.VerticalAlignment = VerticalAlignment.Top;
                            control.Parent = controlPanel;
                        }

                        control.Visible = true;
                    });
                }
                else
                {
                    controlPanelBorder.HasBorder = false;
                }

                if (type == typeof(WelcomePage))
                {
                    if (useIdle)
                        Application.AddIdleTask(InitDefaultPropertyGrid);
                    else
                        InitDefaultPropertyGrid();
                    useIdle = true;
                    SetBackground(SystemColors.Window);
                    panel.RemoveActions();
                }
                else
                {
                    Application.AddIdleTask(() => {
                        PropGrid.SetProps(item.PropInstance, true);
                        PropGrid.Refresh();
                    });
                    
                    SetBackground(SystemColors.Control);
                    panel.RemoveActions();
                    panel.AddActions(type);
                }
            }

            DoAction();

            void SetBackground(Color color)
            {
                if (PropertyGridSettings.Default!.DemoBackgroundIsWhite)
                    color = Color.White;

                parentParent.BackgroundColor = color;
                controlPanelBorder.BackgroundColor = color;
                controlPanel.BackgroundColor = color;
            }
        }

        public class SettingsControl : Control
        {

        }
    }
}