using Alternet.UI;
using System;

namespace ControlsSample
{
    internal class ListViewPage : Control
    {
        private ListView listView;
        private CheckBox allowMultipleSelectionCheckBox;
        private readonly IPageSite site;

        public ListViewPage(IPageSite site)
        {
            var xamlStream = typeof(MainWindow).Assembly.GetManifestResourceStream("ControlsSample.ListViewPage.xaml");
            if (xamlStream == null)
                throw new InvalidOperationException();
            new XamlLoader().LoadExisting(xamlStream, this);

            listView = (ListView)FindControl("listView");
            //listView.SelectionChanged += ListView_SelectionChanged;

            listView.Items.Add(new ListViewItem("One"));
            listView.Items.Add(new ListViewItem("Two"));
            listView.Items.Add(new ListViewItem("Three"));

            ((Button)FindControl("addItemButton")).Click += AddItemButton_Click;
            ((Button)FindControl("removeItemButton")).Click += RemoveItemButton_Click;
            ((Button)FindControl("addManyItemsButton")).Click += AddManyItemsButton_Click;

            allowMultipleSelectionCheckBox = (CheckBox)FindControl("allowMultipleSelectionCheckBox");
            allowMultipleSelectionCheckBox.CheckedChanged += AllowMultipleSelectionCheckBox_CheckedChanged;
            this.site = site;
        }

        private void AddManyItemsButton_Click(object? sender, EventArgs e)
        {
            int start = listView.Items.Count + 1;

            listView.BeginUpdate();
            try
            {
                for (int i = start; i < start + 5000; i++)
                    listView.Items.Add(new ListViewItem("Item " + i));
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        private void ListView_SelectionChanged(object? sender, EventArgs e)
        {
            //string selectedIndicesString = listView.SelectedIndices.Count > 100 ? "too many indices to display" : string.Join(",", listView.SelectedIndices);
            //site.LogEvent($"ListView: SelectionChanged. SelectedIndices: ({selectedIndicesString})");
        }

        private void AllowMultipleSelectionCheckBox_CheckedChanged(object? sender, EventArgs e)
        {
            //listView.SelectionMode = allowMultipleSelectionCheckBox.IsChecked ? ListViewSelectionMode.Multiple : ListViewSelectionMode.Single;
        }

        private void RemoveItemButton_Click(object? sender, EventArgs e)
        {
            if (listView.Items.Count > 0)
                listView.Items.RemoveAt(listView.Items.Count - 1);
        }

        private void AddItemButton_Click(object? sender, EventArgs e)
        {
            listView.Items.Add(new ListViewItem("Item " + (listView.Items.Count + 1)));
        }
    }
}