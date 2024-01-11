using System;
using System.ComponentModel;
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

        private static readonly Image ImageCalendar = Image.FromUrl(ResPrefixCalendar);
        private static readonly Image ImagePhoto = Image.FromUrl(ResPrefixPhoto);
        private static readonly Image ImagePencil = Image.FromUrl(ResPrefixPencil);
        private static readonly Image ImageGraph = Image.FromUrl(ResPrefixGraph);

        private static readonly Image ImageDisabledCalendar = ImageCalendar.ToGrayScale();
        private static readonly Image ImageDisabledPhoto = ImagePhoto.ToGrayScale();
        private static readonly Image ImageDisabledPencil = ImagePencil.ToGrayScale();
        private static readonly Image ImageDisabledGraph = ImageGraph.ToGrayScale();

        private readonly AuiManager manager = new();
        private readonly LayoutPanel panel = new();
        private readonly LogListBox logListBox;
        private readonly AuiToolbar toolbar4 = new();
        private readonly AuiNotebook notebook5;
        private readonly ListBox listBox5;
        private readonly ListBox listBox6;
        private readonly ContextMenu contextMenu = new();

        private readonly int calendarToolId;
        private readonly int photoToolId;
        private readonly int pencilToolId;
        private readonly int graphToolId;

        /*private readonly PopupListBox popupListBox = new();*/

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

        private LogListBox CreateLogListBox(Control? parent = null)
        {
            LogListBox listBox = new()
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
            Icon = new("embres:AuiManagerSample.Sample.ico");
            InitContextMenu();

            InitializeComponent();

            Children.Add(panel);

            manager.SetFlags(AuiManagerOption.Default | AuiManagerOption.AllowActivePane);
            manager.SetManagedWindow(panel);
            manager.SetDefaultSplitterSashProps();

            // Left Pane
            var pane1 = manager.CreatePaneInfo();
            pane1.Name("pane1").Caption("Pane 1").Left().PaneBorder(false)
                .TopDockable(false).BottomDockable(false);
            var listBox1 = CreateListBox();
            listBox1.Add("TopDock = false");
            listBox1.Add("BottomDock = false");
            manager.AddPane(listBox1, pane1);

            // Right Pane
            var pane2 = manager.CreatePaneInfo();
            pane2.Name("pane2").Caption("Pane 2").Right().PaneBorder(false)
                .TopDockable(false).BottomDockable(false).Image((ImageSet)ImageCalendar);
            var listBox2 = CreateListBox();
            listBox2.Add("TopDock = false");
            listBox2.Add("BottomDock = false");
            manager.AddPane(listBox2, pane2);

            // Bottom Pane    
            var pane3 = manager.CreatePaneInfo();
            pane3.Name("pane3").Caption("Pane 3").Bottom().PaneBorder(false)
                .LeftDockable(false).RightDockable(false);
            logListBox = CreateLogListBox();
            logListBox.ContextMenu.Required();
            logListBox.BindApplicationLog();
            LogUtils.DebugLogVersion();
            manager.AddPane(logListBox, pane3);

            // Toolbar pane
            var pane4 = manager.CreatePaneInfo();
            pane4.Name("pane4").Caption("Pane 4").Top().ToolbarPane().PaneBorder(false);

            calendarToolId = toolbar4.AddTool(
                "Calendar",
                (ImageSet)ImageCalendar,
                "Calendar Hint",
                null,
                (ImageSet)ImageDisabledCalendar);
            toolbar4.SetToolName(calendarToolId, "Calendar");

            int separatorToolId = toolbar4.AddSeparator();
            toolbar4.SetToolName(separatorToolId, "Separator");

            pencilToolId = toolbar4.AddTool(
                "Pencil",
                (ImageSet)ImagePencil,
                "Pencil Hint",
                null,
                (ImageSet)ImageDisabledPencil);
            toolbar4.SetToolName(pencilToolId, "Pencil");

            int labelToolId = toolbar4.AddLabel("Text1");
            toolbar4.SetToolName(labelToolId, "Text1");

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
                (ImageSet)ImagePhoto,
                "Photo Hint",
                null,
                (ImageSet)ImageDisabledPhoto);
            toolbar4.SetToolName(photoToolId, "Photo");

            var textBoxId = toolbar4.AddControl(textBox4);
            toolbar4.SetToolName(textBoxId, "TextBox");

            var heights = toolbar4.GetToolMinHeights(comboBoxId, textBoxId);
            Application.Log($"Editors MinHeights: {StringUtils.ToString<int>(heights)}");

            var minHeight = toolbar4.GetToolMaxOfMinHeights(comboBoxId, textBoxId);

            // We need to specify min width. On MacOs without this call control's width
            // will be too small. Width and height here is not DIP, it's pixel.
            // On Linux height of the ComboBox is greater than height of the TextBox.
            // On MacOs height of the TextBox greater than height of the ComboBox.
            // We need to increase min height of TextBox and ComboBox to the max of their min heights.
            toolbar4.GrowToolMinSize(comboBoxId, 100, minHeight);
            toolbar4.GrowToolMinSize(textBoxId, 100, minHeight);

            int stretchSpacerId = toolbar4.AddStretchSpacer();
            toolbar4.SetToolName(stretchSpacerId, "StretchSpacer");

            graphToolId = toolbar4.AddTool(
                "Graph",
                (ImageSet)ImageGraph,
                "Graph Hint",
                null,
                (ImageSet)ImageDisabledGraph);
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
            listBox5 = CreateListBox();
            listBox6 = CreateListBox();

            notebook5.AddPage(listBox5, "ListBox 5", false, (ImageSet)ImagePencil);
            listBox5.Add("This page can be closed");
            notebook5.AddPage(listBox6, "ListBox 6", true, (ImageSet)ImagePhoto);
            listBox6.Add("This page can not be closed");

            panel.Children.Add(notebook5);
            manager.AddPane(notebook5, pane5);

            // Other initializations.

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

            if (!Application.IsMacOS)
            {
                // Under MacOs we have exceptions when drop down menus are shown.
                toolbar4.SetToolDropDownOnEvent(photoToolId, AuiToolbarItemDropDownOnEvent.Click);
                toolbar4.SetToolDropDownOnEvent(graphToolId, AuiToolbarItemDropDownOnEvent.ClickArrow);
                toolbar4.SetToolDropDownMenu(photoToolId, contextMenu);
                toolbar4.SetToolDropDownMenu(graphToolId, contextMenu);
            }

            toolbar4.ToolCommand += Toolbar4_ToolCommand;
            toolbar4.ToolDropDown += ToolDropDown_Click;
            toolbar4.BeginDrag += Toolbar4_BeginDrag;
            toolbar4.ToolMiddleClick += Toolbar4_ToolMiddleClick;
            toolbar4.OverflowClick += Toolbar4_OverflowClick;
            toolbar4.ToolRightClick += Toolbar4_ToolRightClick;

            /*popupListBox.VisibleChanged += PopupListBox_VisibleChanged;*/

            disableImagesMenuItem.Click += DisableImagesMenuItem_Click;
        }

        private void DisableImagesMenuItem_Click(object? sender, EventArgs e)
        {
            var newEnabled = !toolbar4.GetToolEnabled(calendarToolId);
            toolbar4.EnableTool(calendarToolId, newEnabled);
            toolbar4.EnableTool(photoToolId, newEnabled);
            toolbar4.EnableTool(pencilToolId, newEnabled);
            toolbar4.EnableTool(graphToolId, newEnabled);
        }

        internal void AddDefaultItems(ListControl control)
        {
            control.Add("One");
            control.Add("Two");
            control.Add("Three");
            control.Add("Four");
            control.Add("Five");
            control.Add("Six");
            control.Add("Seven");
            control.Add("Eight");
            control.Add("Nine");
            control.Add("Ten");
        }

        private void ShowPopupListBoxButton_Click(object? sender, EventArgs e)
        {
            /*if (popupListBox.MainControl.Items.Count == 0)
            {
                popupListBox.MainControl.SuggestedSize = new(150, 300);
                AddDefaultItems(popupListBox.MainControl);
                popupListBox.MainControl.SelectFirstItem();
            }

            RectD toolRect = toolbar4.GetToolRect(toolbar4.EventToolId);
            var pos = toolbar4.ClientToScreen(toolRect.Location);
            var sz = (0, toolRect.Height);
            popupListBox.ShowPopup(pos, sz);*/
        }

        private void PopupListBox_VisibleChanged(object? sender, EventArgs e)
        {
            /*if (popupListBox.Visible)
                return;
            var resultItem = popupListBox.ResultItem ?? "<null>";
            Application.Log($"PopupResult: {popupListBox.PopupResult}, Item: {resultItem}");*/
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
            Application.Log(s);
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
            ShowPopupListBoxButton_Click(sender, e);
        }

        private void ToolDropDown_Click(object? sender, EventArgs e)
        {
            var isDropDownClicked = toolbar4.EventIsDropDownClicked;
            Log($"Toolbar: ToolDropDown {toolbar4.EventToolNameOrId}, DropDownPart = {isDropDownClicked}");
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
            LogNotebook("BgDoubleClick");
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

    }
}