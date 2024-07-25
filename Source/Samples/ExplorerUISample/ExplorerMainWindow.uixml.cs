using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;
using System.Linq;
using System.Reflection;

namespace ExplorerUISample
{
    partial class ExplorerMainWindow : Window
    {
        private readonly SplittedPanel mainPanel = new()
        {
            RightVisible = false,
            TopVisible = false,
            BottomVisible = false,
        };
        private readonly TreeView treeView = new()
        {
            HasBorder = false,
        };
        private readonly ListView listView = new()
        {
            View = ListViewView.Details,
            HasBorder = false,
        };

        public ExplorerMainWindow()
        {
            Icon = App.DefaultIcon;

            InitializeComponent();

            mainPanel.Parent = this;
            mainPanel.LeftPanel.Width = 200;
            mainPanel.BottomPanel.Height = 100;
            treeView.Parent = mainPanel.LeftPanel;
            listView.Parent = mainPanel.FillPanel;
            listView.Columns.Add(new ListViewColumn("Name"));
            listView.Columns.Add(new ListViewColumn("Size"));
            listView.Columns.Add(new ListViewColumn("Modified"));

            var date = System.DateTime.Now.ToShortDateString();
            listView.Items.Add(new ListViewItem(
                new[] { "July Report 1", "1K", date }, 0));
            listView.Items.Add(new ListViewItem(new[] { 
                "06.21 M&A Meeting Memo", "1.5K", date }, 2));
            listView.Items.Add(new ListViewItem(new[] { 
                "RTC Chart - Mary", "12M", date }, 3));
            listView.Items.Add(new ListViewItem(new[] { 
                "3rd quarter results - Mary", "1M", date }, 3));

            const int FolderImageIndex = 1;
            var maryM = new TreeViewItem("MaryM", FolderImageIndex);
            maryM.Items.Add(new TreeViewItem("Docs", FolderImageIndex));
            maryM.Items.Add(new TreeViewItem("New Reports", FolderImageIndex));
            maryM.Items.Add(new TreeViewItem("Misc", FolderImageIndex));
            var meetings = new TreeViewItem("Meetings", FolderImageIndex);
            meetings.Items.Add(new TreeViewItem("May", FolderImageIndex));
            meetings.Items.Add(new TreeViewItem("June", FolderImageIndex));
            meetings.Items.Add(new TreeViewItem("July", FolderImageIndex));
            maryM.Items.Add(meetings);
            treeView.Items.Add(maryM);
            treeView.ExpandAll();            

            var imageList = LoadImageList();
            treeView.ImageList = imageList;
            listView.SmallImageList = imageList;
            listView.Columns[0].WidthMode = ListViewColumnWidthMode.AutoSize;
            listView.Columns[1].WidthMode = ListViewColumnWidthMode.AutoSize;
            listView.Columns[2].WidthMode = ListViewColumnWidthMode.AutoSize;

            listView.ContextMenuStrip = new();

            listView.ContextMenuStrip.Add("Show progress", () =>
            {
                Title = "AlterNET UI";

                var progressWindow = new Window()
                {
                    Title = "Operation in Progress",
                    Size = (500,200),
                    MinimizeEnabled = false,
                    MaximizeEnabled = false,
                    HasSystemMenu = false,
                    Layout = LayoutStyle.Vertical,
                };

                progressWindow.AddLabel("Processing files...").Margin = 10;

                var progressBar = new ProgressBar()
                {
                    Margin = (10,0,10,10),
                    Parent = progressWindow,
                    Value = 70,
                };

                progressWindow.Show();
            });
        }

        private ImageList LoadImageList()
        {
            var smallImageList = new ImageList();

            var assembly = GetType().Assembly;
            var prefix = AssemblyUtils.GetAssemblyResPrefix(assembly);
            var allResourceNames = assembly.GetManifestResourceNames();
            var allImageResourceNames =
                allResourceNames.Where(x => x.StartsWith($"{prefix}Resources.ExplorerUISample.")).ToArray();

            Image LoadImage(string name) => new Bitmap(assembly.GetManifestResourceStream(name));

            foreach (var name in allImageResourceNames)
                smallImageList.Images.Add(LoadImage(name));

            return smallImageList;
        }
    }
}