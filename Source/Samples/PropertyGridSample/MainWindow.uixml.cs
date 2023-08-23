using System;
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
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false);
            controlsListBox = CreateListBox();
            manager.AddPane(controlsListBox, pane1);

            // Right Pane
            var pane2 = manager.CreatePaneInfo();
            pane2.Name("pane2").Caption("Properties").Right().PaneBorder(false).CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false)
                .BestSize(400,200);
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
            foreach(Type type in result)
            {
                ControlListBoxItem item = new(type);
                controlsListBox.Add(item);
            }

            InitDefaultPropertyGrid();
        }

        private void InitDefaultPropertyGrid()
        {
            propertyGrid.Clear();

            var prop = propertyGrid.CreateBoolProperty("Bool");
            propertyGrid.Add(prop);

            prop = propertyGrid.CreateIntProperty("Int");
            propertyGrid.Add(prop);

            prop = propertyGrid.CreateFloatProperty("Float");
            propertyGrid.Add(prop);

            prop = propertyGrid.CreateUIntProperty("UInt");
            propertyGrid.Add(prop);

            prop = propertyGrid.CreateLongStringProperty("Long string");
            propertyGrid.Add(prop);

            prop = propertyGrid.CreateDateProperty("Date");
            propertyGrid.Add(prop);

            var choices1 = propertyGrid.CreateChoices(typeof(PropertyGridCreateStyle));
            prop = propertyGrid.CreateFlagsProperty("Flags", null, choices1,
                PropertyGrid.DefaultCreateStyle);
            propertyGrid.Add(prop);

            var choices2 = propertyGrid.CreateChoices(typeof(HorizontalAlignment));
            prop = propertyGrid.CreateEnumProperty("Enum", null, choices2,
                HorizontalAlignment.Center);
            propertyGrid.Add(prop);
        }

        private void ControlsListBox_SelectionChanged(object? sender, EventArgs e)
        {
            propertyGrid.Clear();
            var item = controlsListBox.SelectedItem as ControlListBoxItem;
            if (item == null)
                return;
            var control = item.ControlInstance;

            Type myType = control.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo p in props)
            {
                object? propValue = p.GetValue(control, null);
                string propName = p.Name;

                var prop = propertyGrid.CreateStringProperty(propName, null, propValue?.ToString());
                propertyGrid.Add(prop);
            }

        }

        private void Log_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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