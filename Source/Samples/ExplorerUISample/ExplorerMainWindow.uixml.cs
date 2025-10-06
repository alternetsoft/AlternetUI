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
        private readonly StdTreeView treeView = new()
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

            var folderImage = FileListBox.GetDefaultFolderImage();

            var imageSize = SvgUtils.GetSvgSize(ScaleFactor);

            TreeViewItem CreateFolderItem(string name)
            {
                TreeViewItem result = new(name);
                result.SvgImage = folderImage;
                result.SvgImageSize = imageSize;
                return result;
            }

            var firstItem = CreateFolderItem("MaryM");
            firstItem.Add(CreateFolderItem("Docs"));
            firstItem.Add(CreateFolderItem("New Reports"));
            firstItem.Add(CreateFolderItem("Misc"));

            var meetings = CreateFolderItem("Meetings");
            meetings.Add(CreateFolderItem("May"));
            meetings.Add(CreateFolderItem("June"));
            meetings.Add(CreateFolderItem("July"));

            firstItem.Add(meetings);
            treeView.Add(firstItem);
            treeView.ExpandAll();            

            var imageList = LoadImageList();

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

                Label progressLabel = new("Processing files...");
                progressLabel.Margin = 10;
                progressLabel.Parent = progressWindow;

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