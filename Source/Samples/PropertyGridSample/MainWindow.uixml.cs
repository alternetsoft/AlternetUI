using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Alternet.Drawing;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class MainWindow : Window
    {
        private const string ResPrefix =
            "embres:PropertyGridSample.Resources.";
        private const string ResPrefixImage = $"{ResPrefix}logo-128x128.png";

        private static readonly Image DefaultImage = Image.FromUrl(ResPrefixImage);

        private static readonly Dictionary<Type, Action<Control>> Initializers = new();
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly ListBox controlsListBox;
        private readonly PropertyGrid propertyGrid = new();
        private readonly ListBox logListBox;
        private readonly ContextMenu contextMenu2 = new();
        private readonly StackPanel controlPanel = new();

        static MainWindow()
        {
            WebBrowser.CrtSetDbgFlag(0);

            Initializers.Add(typeof(Label), (c) => { (c as Label)!.Text = "Label"; });

            Initializers.Add(typeof(Border), (c) => { c.Height = 150; });

            Initializers.Add(typeof(Button), (c) => 
            { 
                (c as Button)!.Text = "Button"; 
            });

            Initializers.Add(typeof(CheckBox), (c) =>
            {
                (c as CheckBox)!.Text = "CheckBox";
            });

            Initializers.Add(typeof(RadioButton), (c) =>
            {
                (c as RadioButton)!.Text = "RadioButton";
            });

            Initializers.Add(typeof(LinkLabel), (c) =>
            {
                LinkLabel linkLabel = (c as LinkLabel)!;
                linkLabel.Text = "LinkLabel";
                linkLabel.Url = "https://www.google.com/";
            });

            Initializers.Add(typeof(GroupBox), (c) =>
            {
                GroupBox control = (c as GroupBox)!;
                control.Title = "GroupBox";
                control.Height = 150;
            });

            Initializers.Add(typeof(PictureBox), (c) =>
            {
                PictureBox control = (c as PictureBox)!;
                control.Image = DefaultImage;
            });
        }

        public Decimal DecimalValue { get; set; }
        public Font FontValue { get; set; } = Font.Default;
        public Brush BrushValue { get; set; } = Brush.Default;
        public Pen PenValue { get; set; } = Pen.Default;

        private ListBox CreateListBox(Control? parent = null)
        {
            ListBox listBox = new()
            {
                HasBorder = false
            };
            parent ??= panel;
            parent.Children.Add(listBox);
            listBox.SetBounds(0, 0, 200, 100, BoundsSpecified.Size);
            return listBox;
        }

        public MainWindow()
        {
            propertyGrid.CreateStyleEx = PropertyGridCreateStyleEx.AlwaysAllowFocus;

            Icon = ImageSet.FromUrlOrNull("embres:PropertyGridSample.Sample.ico");
            InitLogContextMenu();

            InitializeComponent();

            Children.Add(panel);

            manager.SetFlags(AuiManagerOption.Default);
            manager.SetManagedWindow(panel);

            // Left Pane
            var pane1 = manager.CreatePaneInfo();
            pane1.Name("pane1").Caption("Controls").Left().PaneBorder(false).CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                .CaptionVisible(false);
            controlsListBox = CreateListBox();
            manager.AddPane(controlsListBox, pane1);

            // Right Pane
            var pane2 = manager.CreatePaneInfo();
            pane2.Name("pane2").Caption("Properties").Right().PaneBorder(false)
                .CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                .BestSize(350, 200).CaptionVisible(false).MinSize(350, 200);
            propertyGrid.HasBorder = false;
            panel.Children.Add(propertyGrid);
            manager.AddPane(propertyGrid, pane2);

            // Bottom Pane    
            var pane3 = manager.CreatePaneInfo();
            pane3.Name("pane3").Caption("Output").Bottom().PaneBorder(false).CloseButton(false)
                .LeftDockable(false).RightDockable(false).Movable(false).Floatable(false);
            logListBox = CreateListBox();
            manager.AddPane(logListBox, pane3);

            // Notenook pane
            var pane5 = manager.CreatePaneInfo();
            pane5.Name("pane5").CenterPane().PaneBorder(false);

            controlPanel.HorizontalAlignment = HorizontalAlignment.Center;
            controlPanel.VerticalAlignment = VerticalAlignment.Center;

            panel.Children.Add(controlPanel);
            manager.AddPane(controlPanel, pane5);

            manager.Update();

            ControlListBoxItem item;

            item = new(typeof(SampleProps));
            controlsListBox.Add(item);

            Type[] badTypes = new Type[] 
            {
              typeof(WebBrowser),
              typeof(Panel),
              typeof(Control),
              typeof(AuiNotebook),
              typeof(AuiToolbar),
              typeof(SplitterPanel),
              typeof(Grid),
              typeof(StackPanel),
              typeof(HorizontalStackPanel),
              typeof(VerticalStackPanel),
              typeof(ScrollViewer),
              typeof(StatusBarPanel),
              typeof(LayoutPanel),
              typeof(MenuItem),
              typeof(UserPaintControl),
              typeof(StatusBar),
              typeof(Toolbar),
              typeof(TabPage),
              typeof(TabControl),
              typeof(Window),
              typeof(ToolbarItem),
              typeof(SampleProps),
            };

            IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));
            foreach (Type type in result)
            {
                if (Array.IndexOf(badTypes, type) >= 0)
                    continue;
                item = new(type);
                controlsListBox.Add(item);
            }

            InitDefaultPropertyGrid();

            logListBox.MouseRightButtonUp += Log_MouseRightButtonUp;
            controlsListBox.SelectionChanged += ControlsListBox_SelectionChanged;

            propertyGrid.PropertySelected += PGPropertySelected;
            propertyGrid.PropertyChanged += PGPropertyChanged;
            propertyGrid.PropertyChanging += PGPropertyChanging;
            propertyGrid.PropertyHighlighted += PGPropertyHighlighted;
            propertyGrid.PropertyRightClick += PGPropertyRightClick;
            propertyGrid.PropertyDoubleClick += PGPropertyDoubleClick;
            propertyGrid.ItemCollapsed += PGItemCollapsed;
            propertyGrid.ItemExpanded += PGItemExpanded;
            propertyGrid.LabelEditBegin += PGLabelEditBegin;
            propertyGrid.LabelEditEnding += PGLabelEditEnding;
            propertyGrid.ColBeginDrag += PGColBeginDrag;
            propertyGrid.ColDragging += PGColDragging;
            propertyGrid.ColEndDrag += PGColEndDrag;
            propertyGrid.ButtonClick += PropertyGrid_ButtonClick;

            propertyGrid.AddActionTrigger(
            PropertyGridKeyboardAction.ActionNextProperty,
            Key.DownArrow,
            ModifierKeys.Control);

            propertyGrid.ApplyColors(PropertyGridColors.ColorSchemeWhite);
            propertyGrid.CenterSplitter();
        }

        private void InitDefaultPropertyGrid()
        {
            propertyGrid.BeginUpdate();
            try
            {
                propertyGrid.Clear();

                var prop = propertyGrid.CreatePropCategory("Category 1");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateBoolProperty("Bool");
                propertyGrid.Add(prop);
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.UseCheckbox,
                    false);

                prop = propertyGrid.CreateBoolProperty("Bool 2");
                propertyGrid.Add(prop);
                propertyGrid.SetPropertyKnownAttribute(prop, PropertyGridItemAttrId.UseCheckbox, true);

                prop = propertyGrid.CreateLongProperty("Int");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateDoubleProperty("Float");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateULongProperty("UInt");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateStringProperty("String");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateLongStringProperty("Long string");
                propertyGrid.Add(prop);

                // Date
                prop = propertyGrid.CreateDateProperty("Date");
                propertyGrid.Add(prop);
                // If none Date is selected (if checkbox next to date editor is unchecked)
                // DateTime.MinValue is returned.
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.PickerStyle,
                    (int)(DatePickerStyleFlags.DropDown | DatePickerStyleFlags.ShowCentury
                    | DatePickerStyleFlags.AllowNone));

                // Category 2
                prop = propertyGrid.CreatePropCategory("Category 2");
                propertyGrid.Add(prop);

                var choices1 = PropertyGrid.CreateChoicesOnce(typeof(PropertyGridCreateStyle));
                prop = propertyGrid.CreateFlagsProperty("Flags", null, choices1,
                    PropertyGrid.DefaultCreateStyle);
                propertyGrid.Add(prop);

                var choices2 = PropertyGrid.CreateChoicesOnce(typeof(HorizontalAlignment));
                prop = propertyGrid.CreateChoicesProperty("Enum", null, choices2,
                    HorizontalAlignment.Center);
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateColorProperty("Color", null, Color.Red);
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateColorProperty("Color with alpha", null, Color.Yellow);
                propertyGrid.SetPropertyKnownAttribute(prop, PropertyGridItemAttrId.HasAlpha, true);
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateStringProperty(
                    "Readonly prop",
                    null,
                    "Some Text");
                propertyGrid.SetPropertyReadOnly(prop, true, false);
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateProperty(new Border(), "BorderColor");
                propertyGrid.Add(prop!);

                prop = propertyGrid.CreateProperty(this, "DecimalValue");
                propertyGrid.Add(prop!);

                prop = propertyGrid.CreateProperty(this, "FontValue");
                propertyGrid.Add(prop!);

                prop = propertyGrid.CreateProperty(this, "BrushValue");
                propertyGrid.Add(prop!);

                prop = propertyGrid.CreateProperty(this, "PenValue");
                propertyGrid.Add(prop!);

                prop = propertyGrid.CreateStringProperty(
                                    "Error if changed",
                                    null,
                                    "Some Text");
                propertyGrid.Add(prop);

                // Filename
                prop = propertyGrid.CreateFilenameProperty(
                    "Filename",
                    null,
                    PathUtils.GetAppFolder());
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.Wildcard,
                    "Text Files (*.txt)|*.txt");
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.DialogTitle,
                    "Custom File Dialog Title");
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.ShowFullPath,
                    false);
                propertyGrid.Add(prop);

                // Dir
                prop = propertyGrid.CreateDirProperty("Dir", null, PathUtils.GetAppFolder());
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.DialogTitle,
                    "This is a custom dir dialog title");
                propertyGrid.Add(prop);

                // Image filename
                prop = propertyGrid.CreateImageFilenameProperty(
                    "Image Filename",
                    null,
                    PathUtils.GetAppFolder());
                propertyGrid.Add(prop);

                // System color
                prop = propertyGrid.CreateSystemColorProperty(
                    "System color",
                    null,
                    Color.FromKnownColor(KnownColor.Window));
                propertyGrid.Add(prop);

                // Color with ComboBox
                prop = propertyGrid.CreateColorProperty(
                    "Color with ComboBox",
                    null,
                    Color.Black);
                propertyGrid.SetPropertyEditorByName(prop, "ComboBox");
                propertyGrid.Add(prop);

                // Password
                prop = propertyGrid.CreateStringProperty("Password");
                propertyGrid.Add(prop);
                // Password attribute must be set after adding property to PropertyGrid
                propertyGrid.SetPropertyKnownAttribute(prop, PropertyGridItemAttrId.Password, true);

                // String with button
                prop = propertyGrid.CreateStringProperty("Str and button");
                propertyGrid.SetPropertyEditorByName(prop, "TextCtrlAndButton");
                propertyGrid.Add(prop);

                // Editable enum. Can have values which are not in choices.
                var choices = PropertyGrid.CreateChoices();
                choices.Add("Item 1");
                choices.Add("Item 2");
                choices.Add("Item 3");
                prop = propertyGrid.CreateEditEnumProperty("Editable enum", null, choices, "Item 2");
                propertyGrid.Add(prop);

                //Font Name
                choices = PropertyGridAdapterFont.FontNameChoices;
                prop = propertyGrid.CreateChoicesProperty(
                    "Font name", 
                    null, 
                    choices, 
                    choices.GetValue(Font.Default.Name));
                propertyGrid.Add(prop);
            }
            finally
            {
                propertyGrid.EndUpdate();
            }
        }

        internal void TestLong()
        {
            IPropertyGridVariant variant = PropertyGrid.CreateVariant();

            long minLong = long.MinValue;
            long maxLong = long.MaxValue;
            ulong minULong = ulong.MinValue;
            ulong maxULong = ulong.MaxValue;

            variant.AsLong = minLong;
            long minLong2 = variant.AsLong;

            variant.AsLong = maxLong;
            long maxLong2 = variant.AsLong;

            variant.AsULong = minULong;
            ulong minULong2 = variant.AsULong;

            variant.AsULong = maxULong;
            ulong maxULong2 = variant.AsULong;

            Log($"{minLong} - {minLong2}");
            Log($"{maxLong} - {maxLong2}");
            Log($"{minULong} - {minULong2}");
            Log($"{maxULong} - {maxULong2}");

            variant.AsBool = true;

            variant.AsLong = 150;

            variant.AsDateTime = DateTime.Now;

            variant.AsDouble = 18;

            variant.AsString = "hello";
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            controlPanel.Children.Clear();
            var item = controlsListBox.SelectedItem as ControlListBoxItem;
            var type = item?.ControlType;

            if(type == typeof(SampleProps))
            {
                InitDefaultPropertyGrid();
                return;
            }

            var control = item?.ControlInstance;
            if(control != null)
            {
                controlPanel.Padding = new(25, 100, 25, 100);
                if (control.Parent == null)
                    controlPanel.Children.Add(control);
            }

            propertyGrid.SetProps(control);
        }

        private void Log_MouseRightButtonUp(object? sender, MouseButtonEventArgs e)
        {
            logListBox.ShowPopupMenu(contextMenu2);
        }

        internal void Log(string s)
        {
            logListBox.Add(s);
            logListBox.SelectLastItem();
        }

        private void InitLogContextMenu()
        {
            MenuItem menuItem1 = new()
            {
                Text = "Clear",
            };
            menuItem1.Click += (sender, e) => { logListBox.Items.Clear(); };

            contextMenu2.Items.Add(menuItem1);
        }

        private void LogEvent(string name)
        {
            var propValue = propertyGrid.EventPropValue;
            propValue ??= "NULL";
            string propName = propertyGrid.EventPropName;
            string s = $"Event: {name}. PropName: <{propName}>. Value: <{propValue}>";
            if(logListBox.LastItem?.ToString() != s)
                Log(s);
        }

        private void PGPropertySelected(object? sender, EventArgs e)
        { 
            LogEvent("PropertySelected"); 
        }

        private void PGPropertyChanged(object? sender, EventArgs e)
        {
            LogEvent("PropertyChanged");

            var prop = propertyGrid.EventProperty;
            if (prop == null)
                return;
            if(prop.Instance != null && prop.PropInfo != null)
            {
                var variant = propertyGrid.EventPropValueAsVariant;
                var newValue = variant.GetCompatibleValue(prop.PropInfo);

                prop.PropInfo.SetValue(prop.Instance, newValue);
            }

            prop.RaisePropertyChanged();
        }

        private void PGPropertyChanging(object? sender, CancelEventArgs e)
        {
            LogEvent("PropertyChanging");
            if(propertyGrid.EventPropName == "Error if changed")
                e.Cancel = true;
        }

        private void PGPropertyHighlighted(object? sender, EventArgs e)
        {
            /*LogEvent("PropertyHighlighted");*/
        }

        private void PGPropertyRightClick(object? sender, EventArgs e)
        {
            LogEvent("PropertyRightClick");
        }

        private void PGPropertyDoubleClick(object? sender, EventArgs e)
        {
            LogEvent("PropertyDoubleClick");
        }

        private void PGItemCollapsed(object? sender, EventArgs e)
        {
            LogEvent("ItemCollapsed");
        }

        private void PGItemExpanded(object? sender, EventArgs e)
        {
            LogEvent("ItemExpanded");
        }

        private void PGLabelEditBegin(object? sender, CancelEventArgs e)
        {
            LogEvent("LabelEditBegin");
        }

        private void PGLabelEditEnding(object? sender, CancelEventArgs e)
        {
            LogEvent("LabelEditEnding");
        }

        private void PGColBeginDrag(object? sender, CancelEventArgs e)
        {
            LogEvent("ColBeginDrag");
        }

        private void PGColDragging(object? sender, EventArgs e)
        {
            LogEvent("ColDragging");
        }

        private void PGColEndDrag(object? sender, EventArgs e)
        {
            LogEvent("ColEndDrag");
        }

        private void PropertyGrid_ButtonClick(object? sender, EventArgs e)
        {
            LogEvent("ButtonClick");
        }

        public class SampleProps : Control
        {
        }

        private class ControlListBoxItem
        {
            private Control? instance;
            private readonly Type type;

            public ControlListBoxItem(Type type)
            {
                this.type = type;
            }

            public Type ControlType => type;

            public Control ControlInstance
            {
                get
                {
                    if(instance == null)
                    {
                        instance = (Control?)Activator.CreateInstance(type);
                        if (!Initializers.TryGetValue(
                            instance!.GetType(),
                            out Action<Control>? action))
                            return instance;
                        action(instance);
                    }
                    
                    return instance;
                }
            }

            public override string ToString()
            {
                return type.Name;
            }
        }
    }
}