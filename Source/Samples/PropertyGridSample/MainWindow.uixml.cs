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
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly ListBox controlsListBox;
        private readonly PropertyGrid propertyGrid = new();
        private readonly ListBox logListBox;
        private readonly ContextMenu contextMenu2 = new();

        static MainWindow()
        {
            WebBrowser.CrtSetDbgFlag(0);
        }

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
                .BestSize(400, 200).CaptionVisible(false);
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
            var controlPanel = new Panel();

            panel.Children.Add(controlPanel);
            manager.AddPane(controlPanel, pane5);

            manager.Update();

            logListBox.MouseRightButtonUp += Log_MouseRightButtonUp;
            controlsListBox.SelectionChanged += ControlsListBox_SelectionChanged;

            IEnumerable<Type> result = AssemblyUtils.GetTypeDescendants(typeof(Control));
            foreach (Type type in result)
            {
                if (type == typeof(WebBrowser) || type == typeof(Window))
                    continue;
                ControlListBoxItem item = new(type);
                controlsListBox.Add(item);
            }

            InitDefaultPropertyGrid();

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

                prop = propertyGrid.CreateIntProperty("Int");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateFloatProperty("Float");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateUIntProperty("UInt");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateStringProperty("String");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateLongStringProperty("Long string");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateDateProperty("Date");
                propertyGrid.Add(prop);

                prop = propertyGrid.CreatePropCategory("Category 2");
                propertyGrid.Add(prop);

                var choices1 = propertyGrid.CreateChoicesOnce(typeof(PropertyGridCreateStyle));
                prop = propertyGrid.CreateFlagsProperty("Flags", null, choices1,
                    PropertyGrid.DefaultCreateStyle);
                propertyGrid.Add(prop);

                var choices2 = propertyGrid.CreateChoicesOnce(typeof(HorizontalAlignment));
                prop = propertyGrid.CreateEnumProperty("Enum", null, choices2,
                    HorizontalAlignment.Center);
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateColorProperty("Color", null, Color.Red);
                propertyGrid.Add(prop);

                prop = propertyGrid.CreateStringProperty(
                    "Readonly prop",
                    null,
                    "Some Text");
                propertyGrid.SetPropertyReadOnly(prop, true, false);
                propertyGrid.Add(prop);

                prop = CreateProperty(new Border(), "BorderColor");
                propertyGrid.Add(prop!);


                prop = propertyGrid.CreateStringProperty(
                                    "Error if changed",
                                    null,
                                    "Some Text");
                propertyGrid.Add(prop);

            }
            finally
            {
                propertyGrid.EndUpdate();
            }
        }

        public IPropertyGridItem CreateFontProperty(
            string label,
            string? name = null,
            Font? value = null)
        {
            return null!;
        }

        public IPropertyGridItem CreateBrushProperty(
            string label,
            string? name = null,
            Brush? value = null)
        {
            return null!;
        }

        public IPropertyGridItem CreatePenProperty(
            string label,
            string? name = null,
            Pen? value = null)
        {
            return null!;
        }

        public IPropertyGridItem? CreateProperty(object instance, string name)
        {
            if (instance == null)
                return null;
            var type = instance.GetType();
            var propInfo = type.GetProperty(name);
            if (propInfo == null)
                return null;
            return CreateProperty(instance, propInfo);
        }

        public IPropertyGridItem? CreateProperty(object instance, PropertyInfo p)
        {
            if (!p.CanRead)
                return null;
            ParameterInfo[] paramInfo = p.GetIndexParameters();
            if (paramInfo.Length > 0)
                return null;

            string propName = p.Name;
            var propType = p.PropertyType;

            // Move to AssemblyUtils.GetBrowsable(PropertyInfo p)
            var browsable = p.GetCustomAttribute(
                typeof(BrowsableAttribute)) as BrowsableAttribute;
            if (browsable is not null)
            {
                if (!browsable.Browsable)
                    return null;
            }
            //

            object? propValue = p.GetValue(instance, null);
            TypeCode typeCode = Type.GetTypeCode(propType);

            IPropertyGridItem? prop = null;

            if (propValue is null)     // remove it
                return null;

            if (propType.IsEnum)
            {
                var flagsAttr = propType.GetCustomAttribute(typeof(FlagsAttribute));
                var choices = propertyGrid.CreateChoicesOnce(propType);
                if (flagsAttr == null)
                {
                    prop = propertyGrid.CreateEnumProperty(
                        propName,
                        null,
                        choices,
                        propValue!);
                }
                else
                {
                    prop = propertyGrid.CreateFlagsProperty(
                        propName,
                        null,
                        choices,
                        propValue!);
                }
            }
            else
                switch (typeCode)
                {
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                        return null;
                    case TypeCode.Object:
                        prop = propertyGrid.CreateStringProperty(
                            propName,
                            null,
                            propValue?.ToString());
                        break;
                    case TypeCode.Boolean:
                        prop = propertyGrid.CreateBoolProperty(
                            propName,
                            null,
                            (bool)propValue!);
                        break;
                    case TypeCode.SByte:
                        prop = propertyGrid.CreateIntProperty(
                            propName,
                            null,
                            (sbyte)propValue!);
                        break;
                    case TypeCode.Int16:
                        prop = propertyGrid.CreateIntProperty(
                            propName,
                            null,
                            (Int16)propValue!);
                        break;
                    case TypeCode.Int32:
                        prop = propertyGrid.CreateIntProperty(
                            propName,
                            null,
                            (int)propValue!);
                        break;
                    case TypeCode.Int64:
                        prop = propertyGrid.CreateIntProperty(
                            propName,
                            null,
                            (long)propValue!);
                        break;
                    case TypeCode.Byte:
                        prop = propertyGrid.CreateUIntProperty(
                            propName,
                            null,
                            (byte)propValue!);
                        break;
                    case TypeCode.UInt32:
                        prop = propertyGrid.CreateUIntProperty(
                            propName,
                            null,
                            (uint)propValue!);
                        break;
                    case TypeCode.UInt16:
                        prop = propertyGrid.CreateUIntProperty(
                            propName,
                            null,
                            (UInt16)propValue!);
                        break;
                    case TypeCode.UInt64:
                        prop = propertyGrid.CreateUIntProperty(
                            propName,
                            null,
                            (ulong)propValue!);
                        break;

                    case TypeCode.Single:
                        prop = propertyGrid.CreateFloatProperty(
                            propName,
                            null,
                            (Single)propValue!);
                        break;
                    case TypeCode.Double:
                        prop = propertyGrid.CreateFloatProperty(
                            propName,
                            null,
                            (double)propValue!);
                        break;
                    case TypeCode.Decimal:
                        decimal asDecimal = (decimal)propValue;
                        try
                        {
                            prop = propertyGrid.CreateUIntProperty(
                                propName,
                                null,
                                (ulong)asDecimal);
                            break;
                        }
                        catch
                        {
                            return null;
                        }
                    case TypeCode.DateTime:
                        prop = propertyGrid.CreateDateProperty(
                            propName,
                            null,
                            (DateTime)propValue);
                        break;
                    case TypeCode.Char:
                    case TypeCode.String:
                        prop = propertyGrid.CreateStringProperty(
                            propName,
                            null,
                            propValue?.ToString());
                        break;
                }

            if (!p.CanWrite)
                propertyGrid.SetPropertyReadOnly(prop!, true);
            return prop;
        }

        public void AddProps(object instance)
        {
            Type myType = instance.GetType();
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;

            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties(bindingFlags));

            SortedList<string, PropertyInfo> addedNames = new();

            foreach (PropertyInfo p in props)
            {
                if (addedNames.ContainsKey(p.Name))
                    continue;
                IPropertyGridItem? prop = CreateProperty(instance, p);
                if (prop == null)
                    continue;
                propertyGrid.Add(prop!);
                addedNames.Add(p.Name, p);
            }
        }

        public void SetProps(object? instance)
        {
            propertyGrid.BeginUpdate();
            try
            {
                propertyGrid.Clear();
                if (instance == null)
                    return;
                AddProps(instance);
            }
            finally
            {
                propertyGrid.EndUpdate();
            }
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            var item = controlsListBox.SelectedItem as ControlListBoxItem;
            var control = item?.ControlInstance;
            SetProps(control);
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
            string s = $"Event: {name}. PropName: <{propertyGrid.EventPropName}>";
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
        }

        private void PGPropertyChanging(object? sender, CancelEventArgs e)
        {
            LogEvent("PropertyChanging");
            if(propertyGrid.EventPropName == "Error if changed")
                e.Cancel = true;
        }

        private void PGPropertyHighlighted(object? sender, EventArgs e)
        {
            LogEvent("PropertyHighlighted");
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
                    instance ??= (Control)Activator.CreateInstance(type)!;
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