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
        private ListBox listBox3;
        private AuiToolbar toolbar4 = new();

        int calendarToolId;
        int photoToolId;
        int pencilToolId;
        int graphToolId;

        static MainWindow()
        {
            WebBrowser.CrtSetDbgFlag(0);
        }

        private ListBox CreateListBox(string paneName)
        {
            ListBox listBox = new()
            {
                HasBorder = false
            };
            listBox.Add(paneName);
            panel.Children.Add(listBox);
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

            var pane4 = manager.CreatePaneInfo();
            pane4.Name("pane4").Caption("Pane 4").Top().ToolbarPane();

            calendarToolId = toolbar4.AddTool(
                "Calendar", 
                ImageCalendar, 
                "Calendar Hint");

            toolbar4.SetToolSticky(calendarToolId, false);

            photoToolId = toolbar4.AddTool(
                "Photo",
                ImagePhoto,
                "Photo Hint");

            toolbar4.AddSeparator();

            pencilToolId = toolbar4.AddTool(
                "Pencil",
                ImagePencil,
                "Pencil Hint");

            graphToolId = toolbar4.AddTool(
                "Graph",
                ImageGraph,
                "Graph Hint");
            toolbar4.SetToolDropDown(graphToolId, true);


            toolbar4.AddStretchSpacer();
            toolbar4.AddLabel("Text1");

            toolbar4.Realize();

            toolbar4.ToolDropDown += ToolButton_Click;

            AuiToolbarItemKind kind = toolbar4.GetToolKind(pencilToolId);

        //toolbar4.SetBounds(0, 0, 200, 30, BoundsSpecified.Size);
        panel.Children.Add(toolbar4);
            manager.AddPane(toolbar4, pane4);

            var pane5 = manager.CreatePaneInfo();
            pane5.Name("pane5").Caption("Pane 5").CenterPane();
            var notebook5 = new AuiNotebook();
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

/*
    m_mgr.AddPane(wnd10, wxAuiPaneInfo().
                  Name("test10").Caption("Text Pane with Hide Prompt").
                  Bottom().Layer(1).Position(1).
                  Icon(wxArtProvider::GetBitmapBundle(wxART_WARNING,
                                                      wxART_OTHER,
                                                      wxSize(iconSize, iconSize))));

    m_mgr.AddPane(CreateSizeReportCtrl(), wxAuiPaneInfo().
                  Name("test11").Caption("Fixed Pane").
                  Bottom().Layer(1).Position(2).Fixed());


    m_mgr.AddPane(new SettingsPanel(this,this), wxAuiPaneInfo().
                  Name("settings").Caption("Dock Manager Settings").
                  Dockable(false).Float().Hide());

    // create some center panes

    m_mgr.AddPane(CreateGrid(), wxAuiPaneInfo().Name("grid_content").
                  CenterPane().Hide());

    m_mgr.AddPane(CreateTreeCtrl(), wxAuiPaneInfo().Name("tree_content").
                  CenterPane().Hide());

    m_mgr.AddPane(CreateSizeReportCtrl(), wxAuiPaneInfo().Name("sizereport_content").
                  CenterPane().Hide());

    m_mgr.AddPane(CreateTextCtrl(), wxAuiPaneInfo().Name("text_content").
                  CenterPane().Hide());

    m_mgr.AddPane(CreateHTMLCtrl(), wxAuiPaneInfo().Name("html_content").
                  CenterPane().Hide());

    m_mgr.AddPane(CreateNotebook(), wxAuiPaneInfo().Name("notebook_content").
                  CenterPane().PaneBorder(false));

    // add the toolbars to the manager
    m_mgr.AddPane(tb1, wxAuiPaneInfo().
                  Name("tb1").Caption("Big Toolbar").
                  ToolbarPane().Top());

    m_mgr.AddPane(tb2, wxAuiPaneInfo().
                  Name("tb2").Caption("Toolbar 2 (Horizontal)").
                  ToolbarPane().Top().Row(1));

    m_mgr.AddPane(tb3, wxAuiPaneInfo().
                  Name("tb3").Caption("Toolbar 3").
                  ToolbarPane().Top().Row(1).Position(1));

    m_mgr.AddPane(tb4, wxAuiPaneInfo().
                  Name("tb4").Caption("Sample Bookmark Toolbar").
                  ToolbarPane().Top().Row(2));

    m_mgr.AddPane(tb5, wxAuiPaneInfo().
                  Name("tb5").Caption("Sample Vertical Toolbar").
                  ToolbarPane().Left().
                  GripperTop());

    m_mgr.AddPane(new wxButton(this, wxID_ANY, _("Test Button")),
                  wxAuiPaneInfo().Name("tb6").
                  ToolbarPane().Top().Row(2).Position(1).
                  LeftDockable(false).RightDockable(false));
 
 
 */