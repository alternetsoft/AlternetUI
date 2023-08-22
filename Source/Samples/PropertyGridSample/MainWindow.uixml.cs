using System;
using System.ComponentModel;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace PropertyGridSample
{
    public partial class MainWindow : Window
    {
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly ListBox controlsListBox;
        private readonly ListBox propertiesListBox;
        private readonly ListBox logListBox;
        private readonly ContextMenu contextMenu2 = new();

        static MainWindow()
        {
            WebBrowser.CrtSetDbgFlag(0);
        }

        private ListBox CreateListBox(string paneName, Control? parent = null)
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

            manager.SetFlags(AuiManagerOption.Default | AuiManagerOption.AllowActivePane);
            manager.SetManagedWindow(panel);

            // Left Pane
            var pane1 = manager.CreatePaneInfo();
            pane1.Name("pane1").Caption("Controls").Left().PaneBorder(false).CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false);
            controlsListBox = CreateListBox("Pane 1");
            manager.AddPane(controlsListBox, pane1);

            // Right Pane
            var pane2 = manager.CreatePaneInfo();
            pane2.Name("pane2").Caption("Properties").Right().PaneBorder(false).CloseButton(false)
                .TopDockable(false).BottomDockable(false).Movable(false).Floatable(false);
            propertiesListBox = CreateListBox("Pane 2");
            manager.AddPane(propertiesListBox, pane2);

            // Bottom Pane    
            var pane3 = manager.CreatePaneInfo();
            pane3.Name("pane3").Caption("Output").Bottom().PaneBorder(false).CloseButton(false)
                .LeftDockable(false).RightDockable(false).Movable(false).Floatable(false);
            logListBox = CreateListBox("Pane 3");
            manager.AddPane(logListBox, pane3);

            // Notenook pane
            var pane5 = manager.CreatePaneInfo();
            pane5.Name("pane5").CenterPane().PaneBorder(false);
            var controlPanel = new Panel();

            panel.Children.Add(controlPanel);
            manager.AddPane(controlPanel, pane5);

            manager.Update();

            logListBox.MouseRightButtonUp += Log_MouseRightButtonUp;
        }

        private void Log_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            contextMenu2.Show(logListBox, e.GetPosition(logListBox));
        }

        private void Log(string s)
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
    }
}