using Alternet.UI;
using System;
using System.ComponentModel;
using Alternet.Drawing;
using System.Linq;
using System.Reflection;

namespace ExplorerUISample
{
    partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var date = System.DateTime.Now.ToShortDateString();
            listView.Items.Add(new ListViewItem(new[] { "July Report 1", "1K", date }, 0));
            listView.Items.Add(new ListViewItem(new[] { "06.21 M&A Meeting Memo", "1.5K", date }, 2));
            listView.Items.Add(new ListViewItem(new[] { "RTC Chart - Mary", "12M", date }, 3));
            listView.Items.Add(new ListViewItem(new[] { "3rd quarter results - Mary", "1M", date }, 3));

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
            Closed+=MainWindow_Closed;
        }

        private void MainWindow_Closed(object? sender, WindowClosedEventArgs e)
        {
            Application.Current.Exit();
        }
        
        private void ShowProgress_Click(object sender, EventArgs e)
        {
            var progressWindow = new ProgressWindow();
            progressWindow.Show();
        }

        private static ImageList LoadImageList()
        {
            var smallImageList = new ImageList();

            var assembly = Assembly.GetExecutingAssembly();
            var allResourceNames = assembly.GetManifestResourceNames();
            var allImageResourceNames = allResourceNames.Where(x => x.StartsWith("ExplorerUISample.Resources.")).ToArray();
            Image LoadImage(string name) => new Bitmap(assembly.GetManifestResourceStream(name) ?? throw new Exception());

            foreach (var name in allImageResourceNames)
                smallImageList.Images.Add(LoadImage(name));

            return smallImageList;
        }
    }
}