using System;
using System.IO;
using Alternet.Drawing;
using Alternet.UI;

namespace AuiManagerSample
{
    public partial class MainWindow : Window
    {
        private const string ResPrefix = 
            "embres:AuiManagerSample.Resources.Icons.Small.";
        private const string ResPrefixCalendar = $"{ResPrefix}Calendar16.png";
        private const string ResPrefixPhoto = $"{ResPrefix}Photo16.png";
        private const string ResPrefixPencil = $"{ResPrefix}Pencil16.png";
        private const string ResPrefixGraph = $"{ResPrefix}LineGraph16.png";

        private readonly ImageSet ImageCalendar = ImageSet.FromUrl(ResPrefixCalendar);
        private readonly ImageSet ImagePhoto = ImageSet.FromUrl(ResPrefixPhoto);
        private readonly ImageSet ImagePencil = ImageSet.FromUrl(ResPrefixPencil);
        private readonly ImageSet ImageGraph = ImageSet.FromUrl(ResPrefixGraph);
        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly ListBox listBox3;
        private readonly AuiToolbar toolbar4 = new();

        int calendarToolId;
        int photoToolId;
        int pencilToolId;
        int graphToolId;

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
            listBox.Add(paneName);
            if (parent == null)
                parent = panel;
            parent.Children.Add(listBox);
            listBox.SetBounds(0, 0, 200, 100, BoundsSpecified.Size);
            return listBox;
        }

        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:AuiManagerSample.Sample.ico");

            InitializeComponent();

            Children.Add(panel);

            manager.SetManagedWindow(panel);

            var pane1 = manager.CreatePaneInfo();
            pane1.Name("pane1").Caption("Pane 1").Left()
                .TopDockable(false).BottomDockable(false);
            var listBox1 = CreateListBox("Pane 1");
            listBox1.Add("TopDockable(false)");
            listBox1.Add("BottomDockable(false)");
            manager.AddPane(listBox1, pane1);

            var pane2 = manager.CreatePaneInfo();
            pane2.Name("pane2").Caption("Pane 2").Right()
                .TopDockable(false).BottomDockable(false);
            var listBox2 = CreateListBox("Pane 2");
            listBox2.Add("TopDockable(false)");
            listBox2.Add("BottomDockable(false)");
            manager.AddPane(listBox2, pane2);
           
            var pane3 = manager.CreatePaneInfo();
            pane3.Name("pane3").Caption("Pane 3").Bottom()
                .LeftDockable(false).RightDockable(false);
            listBox3 = CreateListBox("Pane 3");
            listBox3.Add("LeftDockable(false)");
            listBox3.Add("RightDockable(false)");
            manager.AddPane(listBox3, pane3);
            
            // Toolbar pane
            var pane4 = manager.CreatePaneInfo();
            pane4.Name("pane4").Caption("Pane 4").Top().ToolbarPane();

            calendarToolId = toolbar4.AddTool(
                "Calendar", 
                ImageCalendar, 
                "Calendar Hint");

            toolbar4.AddSeparator();

            pencilToolId = toolbar4.AddTool(
                "Pencil",
                ImagePencil,
                "Pencil Hint");

            toolbar4.AddLabel("Text1");

            var control4 = new ComboBox
            {
                IsEditable = false
            };
            control4.Add("Item 1");
            control4.Add("Item 2");
            control4.Add("Item 3");
            toolbar4.Children.Add(control4);
            toolbar4.AddControl(control4);

            photoToolId = toolbar4.AddTool(
                "Photo",
                ImagePhoto,
                "Photo Hint");

            var textBox4 = new TextBox
            {
                Text = "value"
            };
            toolbar4.Children.Add(textBox4);
            toolbar4.AddControl(textBox4);

            toolbar4.AddStretchSpacer();

            graphToolId = toolbar4.AddTool(
                "Graph",
                ImageGraph,
                "Graph Hint");
            toolbar4.SetToolDropDown(graphToolId, true);

            toolbar4.Realize();

            toolbar4.ToolDropDown += ToolButton_Click;

            AuiToolbarItemKind kind = toolbar4.GetToolKind(pencilToolId);

            panel.Children.Add(toolbar4);
            manager.AddPane(toolbar4, pane4);

            // Notenook pane
            var pane5 = manager.CreatePaneInfo();
            pane5.Name("pane5").CenterPane().PaneBorder(false);
            var notebook5 = new AuiNotebook();
            var listBox5 = CreateListBox("ListBox 5");
            var listBox6 = CreateListBox("ListBox 6");

            notebook5.AddPage(listBox5, "ListBox 5");
            notebook5.AddPage(listBox6, "ListBox 6");

            panel.Children.Add(notebook5);
            manager.AddPane(notebook5, pane5);


            manager.Update();
        }

        private void Log(string s)
        {
            listBox3.Add(s);
            listBox3.SelectLastItem();
        }

        private void ToolButton_Click(object? sender, EventArgs e)
        {
            var id = toolbar4.EventToolId;

            if(id == calendarToolId)
            {
                Log("Calendar clicked");
                return;
            }
            
            if (id == photoToolId) 
            {
                Log("Photo clicked");
                return;
            }
            
            if (id == pencilToolId) 
            {
                Log("Pencil clicked");
                return;
            }

            if (id == graphToolId) 
            {
                Log("Graph clicked");
                return;
            }
        }
    }
}