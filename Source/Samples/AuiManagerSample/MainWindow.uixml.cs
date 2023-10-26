using System;
using System.ComponentModel;
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
        private readonly AuiNotebook notebook5;
        private readonly ListBox listBox5;
        private readonly ListBox listBox6;
        private readonly ContextMenu contextMenu = new();
        private readonly ContextMenu contextMenu2 = new();

        private readonly int calendarToolId;
        private readonly int photoToolId;
        private readonly int pencilToolId;
        private readonly int graphToolId;

        static MainWindow()
        {
            WebBrowser.CrtSetDbgFlag(0);
            AuiNotebook.DefaultCreateStyle =
                AuiNotebookCreateStyle.Top |
                AuiNotebookCreateStyle.TabMove |
                AuiNotebookCreateStyle.ScrollButtons |
                AuiNotebookCreateStyle.WindowListButton |
                AuiNotebookCreateStyle.CloseOnAllTabs;
            AuiToolbar.DefaultCreateStyle = 
                //AuiToolbarCreateStyle.PlainBackground |
                AuiToolbarCreateStyle.DefaultStyle;
        }

        private ListBox CreateListBox(string paneName, Control? parent = null)
        {
            ListBox listBox = new()
            {
                HasBorder = false
            };
            listBox.Add(paneName);
            parent ??= panel;
            parent.Children.Add(listBox);
            listBox.SetBounds(0, 0, 200, 100, BoundsSpecified.Size);
            return listBox;
        }

        public MainWindow()
        {
            Icon = ImageSet.FromUrlOrNull("embres:AuiManagerSample.Sample.ico");
            InitContextMenu();
            InitLogContextMenu();

            InitializeComponent();

            Children.Add(panel);

            manager.SetFlags(AuiManagerOption.Default | AuiManagerOption.AllowActivePane);
            manager.SetManagedWindow(panel);

            // Left Pane
            var pane1 = manager.CreatePaneInfo();
            pane1.Name("pane1").Caption("Pane 1").Left().PaneBorder(false)
                .TopDockable(false).BottomDockable(false);
            var listBox1 = CreateListBox("Pane 1");
            listBox1.Add("TopDockable(false)");
            listBox1.Add("BottomDockable(false)");
            manager.AddPane(listBox1, pane1);

            // Right Pane
            var pane2 = manager.CreatePaneInfo();
            pane2.Name("pane2").Caption("Pane 2").Right().PaneBorder(false)
                .TopDockable(false).BottomDockable(false).Image(ImageCalendar);
            var listBox2 = CreateListBox("Pane 2");
            listBox2.Add("TopDockable(false)");
            listBox2.Add("BottomDockable(false)");
            manager.AddPane(listBox2, pane2);

            // Bottom Pane    
            var pane3 = manager.CreatePaneInfo();
            pane3.Name("pane3").Caption("Pane 3").Bottom().PaneBorder(false)
                .LeftDockable(false).RightDockable(false);
            listBox3 = CreateListBox("Pane 3");
            listBox3.Add("LeftDockable(false)");
            listBox3.Add("RightDockable(false)");
            manager.AddPane(listBox3, pane3);

            // Toolbar pane
            var pane4 = manager.CreatePaneInfo();
            pane4.Name("pane4").Caption("Pane 4").Top().ToolbarPane().PaneBorder(false);

            calendarToolId = toolbar4.AddTool(
                "Calendar",
                ImageCalendar,
                "Calendar Hint");
            toolbar4.SetToolName(calendarToolId, "Calendar");

            int separatorToolId = toolbar4.AddSeparator();

            pencilToolId = toolbar4.AddTool(
                "Pencil",
                ImagePencil,
                "Pencil Hint");
            toolbar4.SetToolName(pencilToolId, "Pencil");

            int labelToolId = toolbar4.AddLabel("Text1");

            var control4 = new ComboBox
            {
                IsEditable = false,
            };
            control4.Add("Item 1");
            control4.Add("Item 2");
            control4.Add("Item 3");

            var textBox4 = new TextBox
            {
                Text = "value",
            };

            var comboBoxId = toolbar4.AddControl(control4);
            toolbar4.SetToolName(comboBoxId, "ComboBox");

            photoToolId = toolbar4.AddTool(
                "Photo",
                ImagePhoto,
                "Photo Hint");
            toolbar4.SetToolName(photoToolId, "Photo");

            var textBoxId = toolbar4.AddControl(textBox4);
            toolbar4.SetToolName(textBoxId, "TextBox");

            var minHeight1 = toolbar4.GetToolMinHeight(comboBoxId);
            var minHeight2 = toolbar4.GetToolMinHeight(textBoxId);
            var minHeight = Math.Max(minHeight1, minHeight2);

            // We need to specify min width. On MacOs without this call control's width
            // will be too small. Width and height here is not DIP, it's pixel.
            // On Linux height of the ComboBox is greater than height of the TextBox.
            // On MacOs height of the TextBox greater than height of the ComboBox.
            // We need to increase min height of TextBox and ComboBox to the max of their min heights.
            toolbar4.GrowToolMinSize(comboBoxId, 100, minHeight);
            toolbar4.GrowToolMinSize(textBoxId, 100, minHeight);

            int stretchSpacerId = toolbar4.AddStretchSpacer();

            graphToolId = toolbar4.AddTool(
                "Graph",
                ImageGraph,
                "Graph Hint");
            toolbar4.SetToolDropDown(graphToolId, true);
            toolbar4.SetToolName(graphToolId, "Graph");

            toolbar4.Realize();

            AuiToolbarItemKind kind = toolbar4.GetToolKind(pencilToolId);

            panel.Children.Add(toolbar4);

            manager.AddPane(toolbar4, pane4);

            // Center pane
            var pane5 = manager.CreatePaneInfo();
            pane5.Name("pane5").CenterPane().PaneBorder(false);
            notebook5 = new AuiNotebook();
            listBox5 = CreateListBox("ListBox 5");
            listBox6 = CreateListBox("ListBox 6");

            notebook5.AddPage(listBox5, "ListBox 5", false, ImagePencil);
            notebook5.AddPage(listBox6, "ListBox 6", true, ImagePhoto);
            listBox6.Add("This page can not be closed");

            panel.Children.Add(notebook5);
            manager.AddPane(notebook5, pane5);

            manager.ArtProvider.SetMetric(
                AuiDockArtSetting.GradientType,
                (int)AuiPaneDockArtGradients.None);

            var captionSize = manager.ArtProvider.GetMetric(AuiDockArtSetting.CaptionSize);
            captionSize += 3;
            manager.ArtProvider.SetMetric(AuiDockArtSetting.CaptionSize, captionSize);

            manager.ArtProvider.SetColor(AuiDockArtSetting.ActiveCaptionColor, Color.LightSkyBlue);
            manager.ArtProvider.SetColor(AuiDockArtSetting.ActiveCaptionTextColor, Color.Black);

            manager.Update();

            toolbar4.AddToolOnClick(calendarToolId, CalendarButton_Click);
            toolbar4.AddToolOnClick(photoToolId, PhotoButton_Click);
            toolbar4.AddToolOnClick(pencilToolId, PencilButton_Click);
            toolbar4.AddToolOnClick(graphToolId, GraphButton_Click);

            notebook5.PageClose += NotebookPageClose;
            notebook5.PageClosed += NotebookPageClosed;
            notebook5.PageChanged += NotebookPageChanged;
            notebook5.PageChanging += NotebookPageChanging;
            notebook5.PageButton += NotebookPageButton;
            notebook5.BeginDrag += NotebookBeginDrag;
            notebook5.EndDrag += NotebookEndDrag;
            notebook5.AllowTabDrop += NotebookAllowTabDrop;
            notebook5.DragDone += NotebookDragDone;
            notebook5.TabMiddleMouseDown += NotebookTabMiddleMouseDown;
            notebook5.TabMiddleMouseUp += NotebookTabMiddleMouseUp;
            notebook5.TabRightMouseDown += NotebookTabRightMouseDown;
            notebook5.TabRightMouseUp += NotebookTabRightMouseUp;
            notebook5.BgDoubleClick += NotebookBgDclickMouse;

            toolbar4.SetToolDropDownOnEvent(photoToolId, AuiToolbarItemDropDownOnEvent.Click);
            toolbar4.SetToolDropDownOnEvent(graphToolId, AuiToolbarItemDropDownOnEvent.ClickArrow);
            toolbar4.SetToolDropDownMenu(photoToolId, contextMenu);
            toolbar4.SetToolDropDownMenu(graphToolId, contextMenu);

            toolbar4.ToolCommand += Toolbar4_ToolCommand;
            toolbar4.ToolDropDown += ToolDropDown_Click;
            toolbar4.BeginDrag += Toolbar4_BeginDrag;
            toolbar4.ToolMiddleClick += Toolbar4_ToolMiddleClick;
            toolbar4.OverflowClick += Toolbar4_OverflowClick;
            toolbar4.ToolRightClick += Toolbar4_ToolRightClick;

            listBox3.MouseRightButtonUp += Log_MouseRightButtonUp;

            Log("Net Version = " + Environment.Version.ToString());
        }

        private void Log_MouseRightButtonUp(object? sender, MouseButtonEventArgs e)
        {
            contextMenu2.Show(listBox3, e.GetPosition(listBox3));
        }

        private void Toolbar4_ToolCommand(object? sender, EventArgs e)
        {
            Log($"Toolbar: ToolCommand {toolbar4.EventToolNameOrId}");
        }

        private void Toolbar4_ToolRightClick(object? sender, EventArgs e)
        {
            Log($"Toolbar: ToolRightClick {toolbar4.EventToolNameOrId}");
        }

        private void Toolbar4_OverflowClick(object? sender, EventArgs e)
        {
            Log($"Toolbar: OverflowClick");
        }

        private void Toolbar4_ToolMiddleClick(object? sender, EventArgs e)
        {
            Log($"Toolbar: ToolMiddleClick {toolbar4.EventToolNameOrId}");
        }

        private void Toolbar4_BeginDrag(object? sender, EventArgs e)
        {
            Log($"Toolbar: BeginDrag");
        }

        private void Log(string s)
        {
            listBox3.Add(s);
            listBox3.SelectLastItem();
        }

        private void CalendarButton_Click(object? sender, EventArgs e)
        {
            Log("Tool Calendar clicked");
        }

        private void PhotoButton_Click(object? sender, EventArgs e)
        {
            Log("Tool Photo clicked");
        }

        private void PencilButton_Click(object? sender, EventArgs e)
        {
            Log("Tool Pencil clicked");
        }

        private void ToolDropDown_Click(object? sender, EventArgs e)
        {
            Log($"Toolbar: ToolDropDown {toolbar4.EventToolNameOrId}");
        }

        private void NotebookPageClose(object? sender, CancelEventArgs e)
        {
            LogNotebook("PageClose");
        }

        private void NotebookPageClosed(object? sender, EventArgs e)
        {
            LogNotebook("PageClosed");
        }

        private void NotebookPageChanged(object? sender, EventArgs e)
        {
            LogNotebook("PageChanged");
        }

        private void NotebookPageChanging(object? sender, CancelEventArgs e)
        {
            LogNotebook("PageChanging");
        }

        private void NotebookPageButton(object? sender, CancelEventArgs e)
        {
            var selection = notebook5.EventSelection;
            LogNotebook("PageButton");
            if (selection == notebook5.FindPage(listBox6))
                e.Cancel = true;
        }

        private void NotebookBeginDrag(object? sender, EventArgs e)
        {
            LogNotebook("BeginDrag");
        }

        private void NotebookEndDrag(object? sender, EventArgs e)
        {
            LogNotebook("EndDrag");
        }

        private void NotebookAllowTabDrop(object? sender, EventArgs e)
        {
            LogNotebook("AllowTabDrop");
        }

        private void NotebookDragDone(object? sender, EventArgs e)
        {
            LogNotebook("DragDone");
        }

        private void NotebookTabMiddleMouseDown(object? sender, EventArgs e)
        {
            LogNotebook("TabMiddleMouseDown");
        }

        private void NotebookTabMiddleMouseUp(object? sender, EventArgs e)
        {
            LogNotebook("TabMiddleMouseUp");
        }

        private void NotebookTabRightMouseDown(object? sender, EventArgs e)
        {
            LogNotebook("TabRightMouseDown");
        }

        private void NotebookTabRightMouseUp(object? sender, EventArgs e)
        {
            LogNotebook($"TabRightMouseUp");
        }

        private void NotebookBgDclickMouse(object? sender, EventArgs e)
        {
            LogNotebook("BgDclickMouse");
        }

        private void LogNotebook(string s)
        {
            Log($"Notebook: {s}, Sel {notebook5.EventSelection}," +
                $" OldSel {notebook5.EventOldSelection}");
        }

        private void GraphButton_Click(object? sender, EventArgs e)
        {
            var isDropDownClicked = toolbar4.EventIsDropDownClicked;
            Log($"Graph clicked, DropDownPart = {isDropDownClicked}");
        }

        private void InitContextMenu()
        {
            MenuItem menuItem1 = new()
            {
                Text = "_Open...",
                Shortcut = "Ctrl+O",
            };
            menuItem1.Click += (sender, e) => { Log("Click drop down menu item: Open"); };

            MenuItem menuItem2 = new()
            {
                Text = "_Save...",
                Shortcut = "Ctrl+S",
            };
            menuItem2.Click += (sender, e) => { Log("Click drop down menu item: Save"); };

            contextMenu.Items.Add(menuItem1);
            contextMenu.Items.Add(menuItem2);
        }

        private void InitLogContextMenu()
        {
            MenuItem menuItem1 = new()
            {
                Text = "Clear",
            };
            menuItem1.Click += (sender, e) => { listBox3.Items.Clear(); };

            contextMenu2.Items.Add(menuItem1);
        }
    }
}