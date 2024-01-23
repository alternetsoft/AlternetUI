using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples.ListView
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            listView.Columns.Add(new("Name"));
            listView.Columns.Add(new("Info"));

            var imageLists = ResourceLoader.LoadImageLists();
            listView.SmallImageList = imageLists.Small;
            listView.LargeImageList = imageLists.Large;

            void AddItems(int count)
            {
                int start = listView.Items.Count + 1;

                listView.BeginUpdate();
                try
                {
                    for (int i = start; i < start + count; i++)
                    {
                        var columns = new[] { "Item " + i, "Some Info " + i };
                        var item = new ListViewItem(columns, i % 4);
                        listView.Items.Add(item);
                    }
                }
                finally
                {
                    listView.EndUpdate();
                }
            }

            AddItems(4);
            listView.SelectedIndex = 1;
        }

        public void ListViewExample1()
        {
            #region ListViewCSharpCreation
            var ListView = new Alternet.UI.ListView();
            ListView.Items.Add(new Alternet.UI.ListViewItem { Text = "Item1"});
            ListView.Items.Add(new Alternet.UI.ListViewItem { Text = "Item2" });
            ListView.Items.Add(new Alternet.UI.ListViewItem { Text = "Item2" });
            ListView.SelectedIndex = 1;
            #endregion
        }

        #region ListViewEventHandler
        private void ListView_SelectionChanged(object? sender, EventArgs e)
        {
            Application.Log($"ListView: SelectionChanged. SelectedIndex: {listView.SelectedIndex}");
        }
        #endregion    
    }
}