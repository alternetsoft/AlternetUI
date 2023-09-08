using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.Base.Collections;

namespace PropertyGridSample
{
    public partial class MainWindow : Window, IComponentDesigner
    {
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly ListBox controlsListBox;
        private readonly ListBox logListBox;
        private readonly ContextMenu contextMenu2 = new();
        private readonly StackPanel controlPanel = new()
        {
            Padding = new(15, 15, 15, 15),
        };

        internal readonly PropertyGrid propertyGrid = new();

        private bool updatePropertyGrid = false;

        static MainWindow()
        {
            var localizableEnum = PropertyGrid.GetChoices<BrushType>();
            localizableEnum.SetLabelForValue<BrushType>(BrushType.LinearGradient, "Linear Gradient");
            localizableEnum.SetLabelForValue<BrushType>(BrushType.RadialGradient, "Radial Gradient");

            WebBrowser.CrtSetDbgFlag(0);
        }

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

        private static void InitIgnorePropNames(ICollection<string> items)
        {
            items.Add("Width");
            items.Add("Height");
            items.Add("Left");
            items.Add("Top");
        }

        public MainWindow()
        {
            propertyGrid.Features = PropertyGridFeature.QuestionCharInNullable;
            PropertyGridSettings.Default = new(this);
            propertyGrid.ProcessException += PropertyGrid_ProcessException;

            InitIgnorePropNames(propertyGrid.IgnorePropNames);

            propertyGrid.CreateStyleEx = PropertyGridCreateStyleEx.AlwaysAllowFocus;

            Icon = ImageSet.FromUrlOrNull("embres:PropertyGridSample.Sample.ico");
            InitLogContextMenu();

            InitializeComponent();

            panel.Layout = LayoutPanelKind.Native;
            Children.Add(panel);

            manager.SetFlags(AuiManagerOption.Default);
            manager.SetManagedWindow(panel);

            // Left Pane
            var pane1 = manager.CreatePaneInfo();
            pane1.Name("pane1").Caption("Controls").Left().PaneBorder(false).CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                .CaptionVisible(false);
            controlsListBox = CreateListBox();
            controlsListBox.SetBounds(0, 0, 150, 100, BoundsSpecified.Size);
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

            // Center pane
            var pane5 = manager.CreatePaneInfo();
            pane5.Name("pane5").CenterPane().PaneBorder(false);
            controlPanel.HorizontalAlignment = HorizontalAlignment.Center;
            controlPanel.VerticalAlignment = VerticalAlignment.Center;
            panel.Children.Add(controlPanel);
            manager.AddPane(controlPanel, pane5);

            manager.Update();

            ControlListBoxItem item = new(typeof(WelcomeControl));
            controlsListBox.Add(item);

            Type[] badTypes = new Type[] 
            {
              typeof(WebBrowser),
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
              typeof(MainMenu),
              typeof(StatusBar),
              typeof(ContextMenu),
              typeof(Popup),
              typeof(NonVisualControl),
              typeof(PropertyGrid),
              typeof(Toolbar),
              typeof(TabPage),
              typeof(TabControl),
              typeof(Window),
              typeof(ToolbarItem),
            };

            IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));
            foreach (Type type in result)
            {
                if (Array.IndexOf(badTypes, type) >= 0)
                    continue;
                if (type.Assembly != typeof(Control).Assembly)
                    continue;
                item = new(type);
                controlsListBox.Add(item);
            }

            AddDialog<ColorDialog>();
            AddDialog<OpenFileDialog>();
            AddDialog<SaveFileDialog>();
            AddDialog<SelectDirectoryDialog>();
            AddDialog<FontDialog>();
            AddContextMenu<ContextMenu>();

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

            propertyGrid.ApplyKnownColors(PropertyGridSettings.Default.ColorScheme);
            propertyGrid.CenterSplitter();
            propertyGrid.SetVerticalSpacing();

            controlsListBox.SelectedIndex = 0;

            Application.Current.Idle += ApplicationIdle;
            RunTests();            
        }

        internal void AddMainWindow()
        {
            controlsListBox.Add(new ControlListBoxItem(typeof(Window), this.ParentWindow));
        }

        private void AddDialog<T>()
            where T : CommonDialog
        {
            var dialog = (T)ControlListBoxItem.CreateInstance(typeof(T))!;
            var button = new ShowDialogButton
            {
                Dialog = dialog,
            };
            var item = new ControlListBoxItem(typeof(T), button)
            {
                PropInstance = dialog,
            };
            controlsListBox.Add(item);
        }

        private void AddContextMenu<T>()
            where T : ContextMenu
        {
            var menu = (T)ControlListBoxItem.CreateInstance(typeof(T))!;

            var button = new ShowContextMenuButton
            {
                Menu = menu,
            };
            var item = new ControlListBoxItem(typeof(T), button)
            {
                PropInstance = menu,
            };
            controlsListBox.Add(item);
        }

        private void ApplicationIdle(object? sender, EventArgs e)
        {
            if (updatePropertyGrid)
            {
                updatePropertyGrid = false;
                try
                {
                    UpdatePropertyGrid();
                }
                catch
                {
                }
            }
        }

        private void PropertyGrid_ProcessException(object? sender, PropertyGridExceptionEventArgs e)
        {
            Log("Exception: " + e.InnerException.Message);
        }

        private void InitDefaultPropertyGrid()
        {
            propertyGrid.BeginUpdate();
            try
            {
                propertyGrid.Clear();

                var prop = propertyGrid.CreatePropCategory("Properties");
                propertyGrid.Add(prop);
                propertyGrid.AddProps(WelcomeProps.Default, null, true);

                // New category
                prop = propertyGrid.CreatePropCategory("Properties 2");
                propertyGrid.Add(prop);

                // String with button
                prop = propertyGrid.CreateStringProperty("Str and button");
                propertyGrid.SetPropertyEditorByName(prop, "TextCtrlAndButton");
                propertyGrid.Add(prop);

                void AddCollectionProp(string label, string propName)
                {
                    prop = propertyGrid.CreateProperty(label, null, this, propName);
                    if (prop != null)
                    {
                        propertyGrid.SetPropertyReadOnly(prop, false, false);
                        propertyGrid.SetPropertyEditorByName(prop, "TextCtrlAndButton");
                        propertyGrid.Add(prop);
                    }
                }

                AddCollectionProp("Collection<string>", "ItemsString");
                AddCollectionProp("Collection<object>", "ItemsObject");

                prop = propertyGrid.CreateBoolProperty("Bool 2");
                propertyGrid.Add(prop);
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.UseCheckbox,
                    false);

                prop = propertyGrid.CreateBoolProperty("Bool 3");
                propertyGrid.Add(prop);
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.UseCheckbox,
                    true);

                prop = propertyGrid.CreateLongStringProperty("Long string");
                propertyGrid.Add(prop);

                // Date
                prop = propertyGrid.CreateDateProperty("Date");
                propertyGrid.Add(prop);
                // If none Date is selected (checkbox next to date editor is unchecked)
                // DateTime.MinValue is returned.
                propertyGrid.SetPropertyKnownAttribute(
                    prop,
                    PropertyGridItemAttrId.PickerStyle,
                    (int)(DatePickerStyleFlags.DropDown | DatePickerStyleFlags.ShowCentury
                    | DatePickerStyleFlags.AllowNone));

                // New category
                prop = propertyGrid.CreatePropCategory("Properties 3");
                propertyGrid.Add(prop);

                var choices1 = PropertyGrid.CreateChoicesOnce(typeof(PropertyGridCreateStyle));
                prop = propertyGrid.CreateFlagsProperty("Flags", null, choices1,
                    PropertyGrid.DefaultCreateStyle);
                propertyGrid.Add(prop);

                var choices2 = PropertyGrid.CreateChoicesOnce(typeof(HorizontalAlignment));
                prop = propertyGrid.CreateChoicesProperty("Enum", null, choices2,
                    HorizontalAlignment.Center);
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateStringProperty(
                    "Readonly",
                    null,
                    "Some Text");
                propertyGrid.SetPropertyReadOnly(prop, true, false);
                propertyGrid.Add(prop);

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
                    choices.GetValueFromLabel(Font.Default.Name));
                propertyGrid.Add(prop);

                prop = propertyGrid.CreatePropCategory("Nullable Properties");
                propertyGrid.Add(prop);
                propertyGrid.AddProps(NullableProps.Default);

                propertyGrid.SetCategoriesBackgroundColor(Color.LightGray);
            }
            finally
            {
                propertyGrid.EndUpdate();
            }
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            UpdatePropertyGrid();
        }

        internal void UpdatePropertyGrid()
        {
            void DoAction()
            {
                controlPanel.Children.Clear();
                var item = controlsListBox.SelectedItem as ControlListBoxItem;
                var type = item?.InstanceType;

                if (item?.Instance is Control control)
                {
                    control.Designer = this;
                    if (control.Parent == null)
                        controlPanel.Children.Add(control);
                }

                if (type == typeof(WelcomeControl))
                    InitDefaultPropertyGrid();
                else
                    propertyGrid.SetProps(item?.PropInstance);
            }

            controlPanel.SuspendLayout();
            try
            {
                DoAction();
            }
            finally
            {
                controlPanel.ResumeLayout();
            }
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
            if(PropertyGridSettings.Default!.LogPropertySelected)
                LogEvent("PropertySelected"); 
        }

        private void PGPropertyChanged(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyChanged)
                LogEvent("PropertyChanged");
        }

        private void PGPropertyChanging(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyChanging)
                LogEvent("PropertyChanging");
            if(propertyGrid.EventPropName == "Error if changed")
                e.Cancel = true;
        }

        private void PGPropertyHighlighted(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyHighlighted)
                LogEvent("PropertyHighlighted");
        }

        private void PGPropertyRightClick(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyRightClick)
                LogEvent("PropertyRightClick");
        }

        private void PGPropertyDoubleClick(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogPropertyDoubleClick)
                LogEvent("PropertyDoubleClick");
        }

        private void PGItemCollapsed(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogItemCollapsed)
                LogEvent("ItemCollapsed");
        }

        private void PGItemExpanded(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogItemExpanded)
                LogEvent("ItemExpanded");
        }

        private void PGLabelEditBegin(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogLabelEditBegin)
                LogEvent("LabelEditBegin");
        }

        private void PGLabelEditEnding(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogLabelEditEnding)
                LogEvent("LabelEditEnding");
        }

        private void PGColBeginDrag(object? sender, CancelEventArgs e)
        {
            if (PropertyGridSettings.Default!.LogColBeginDrag)
                LogEvent("ColBeginDrag");
        }

        private void PGColDragging(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogColDragging)
                LogEvent("ColDragging");
        }

        private void PGColEndDrag(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogColEndDrag)
                LogEvent("ColEndDrag");
        }

        private void PropertyGrid_ButtonClick(object? sender, EventArgs e)
        {
            if (PropertyGridSettings.Default!.LogButtonClick)
                LogEvent("ButtonClick");

            var prop = propertyGrid.EventProperty;
            if (prop == null)
                return;
            var propInfo = prop.PropInfo;
            var instance = prop.Instance;

            if (propInfo == null || instance == null)
                return;

            var propType = AssemblyUtils.GetRealType(propInfo.PropertyType);

            var value = propInfo.GetValue(instance);

            if (value is not ICollection)
                return;

            UIDialogCollectionEdit dialog = new();
            dialog.DataSource = value;
            dialog.Show();
        }

        void IComponentDesigner.PropertyChanged(object? sender, string? propName)
        {
            updatePropertyGrid = true;
        }

        private class ControlListBoxItem
        {
            private object? instance;
            private object? propInstance;
            private readonly Type type;

            public ControlListBoxItem(Type type, object? instance = null)
            {
                this.type = type;
                this.instance = instance;
            }

            public Type InstanceType => type;

            public object? PropInstance
            {
                get
                {
                    if (propInstance == null)
                        return Instance;
                    return propInstance;
                }

                set
                {
                    propInstance = value;
                }
            }

            public object Instance
            {
                get
                {
                    instance ??= CreateInstance(type);
                    
                    return instance!;
                }
            }

            public static object CreateInstance(Type type)
            {
                var instance = Activator.CreateInstance(type);
                if (!ObjectInitializers.Actions.TryGetValue(
                    type,
                    out Action<Object>? action))
                    return instance!;
                action(instance!);
                return instance!;
            }

            public override string ToString()
            {
                if (type.Name == nameof(WelcomeControl))
                    return "Welcome Page";
                return type.Name;
            }
        }
    }
}