using Alternet.UI;
using System;
using System.ComponentModel;
using System.Drawing;

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
            listView.Items.Add(new ListViewItem(new[] {"06.21 M&A Meeting Memo", "1.5K", date}, 1));
            listView.Items.Add(new ListViewItem(new[] {"RTC Chart - Mary", "12M", date}, 2));
            listView.Items.Add(new ListViewItem(new[] {"3rd quarter results - Mary", "1M", date}, 2));

            var treeView = (TreeView)FindControl("treeView");
            var maryM = new TreeViewItem("MaryM", 3);
            maryM.Items.Add(new TreeViewItem("Docs", 3));
            maryM.Items.Add(new TreeViewItem("New Reports", 3));
            maryM.Items.Add(new TreeViewItem("Misc", 3));
            var meetings = new TreeViewItem("Meetings", 3);
            meetings.Items.Add(new TreeViewItem("May", 3));
            meetings.Items.Add(new TreeViewItem("June", 3));
            meetings.Items.Add(new TreeViewItem("July", 3));
            maryM.Items.Add(meetings);
            treeView.Items.Add(maryM);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
                new ProgressWindow().Show();
        }
    }
}