using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Image = Alternet.UI.Image;

namespace ExplorerUISample
{
    internal class MainWindow : Window
    {
        public MainWindow()
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ExplorerUISample.MainWindow.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            var listView = (ListView)FindControl("listView");
            var date = DateTime.Now.ToShortDateString();
            listView.Items.Add(new ListViewItem(new[] {"July Report 1", "1K", date}, 0));
            listView.Items.Add(new ListViewItem(new[] {"06.21 M&A Meeting Memo", "1.5K", date}, 2));
            listView.Items.Add(new ListViewItem(new[] {"RTC Chart - Mary", "12M", date}, 3));
            listView.Items.Add(new ListViewItem(new[] {"3rd quarter results - Mary", "1M", date}, 3));

            const int FolderImageIndex = 1;
            var treeView = (TreeView)FindControl("treeView");
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

            var imageList = LoadImageList();
            treeView.ImageList = imageList;
            listView.SmallImageList = imageList;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
                new ProgressWindow().Show();
        }

        private static ImageList LoadImageList()
        {
            var smallImageList = new ImageList();

            var assembly = Assembly.GetExecutingAssembly();
            var allResourceNames = assembly.GetManifestResourceNames();
            var allImageResourceNames = allResourceNames.Where(x => x.StartsWith("ExplorerUISample.Resources.")).ToArray();
            Image LoadImage(string name) => new Image(assembly.GetManifestResourceStream(name) ?? throw new Exception());

            foreach (var name in allImageResourceNames)
                smallImageList.Images.Add(LoadImage(name));

            return smallImageList;
        }
    }
}