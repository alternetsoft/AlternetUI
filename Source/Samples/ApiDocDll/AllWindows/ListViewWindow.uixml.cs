using Alternet.UI;
using System;

namespace ApiDoc
{
    public partial class ListViewWindow : Window
    {
        public ListViewWindow()
        {
            InitializeComponent();

            var columnName = new ListViewColumn("Name")
            {
            };

            var columnInfo = new ListViewColumn("Info")
            {
            };

            listView.Columns.Add(columnName);
            listView.Columns.Add(columnInfo);

            var imageLists = SampleResourceLoader.LoadImageLists();
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

            columnName.Width = 30;
            columnName.WidthMode = ListViewColumnWidthMode.FixedInPercent;
            columnInfo.Width = 65;
            columnInfo.WidthMode = ListViewColumnWidthMode.FixedInPercent;
        }

        public static void ListViewExample1()
        {
            #region ListViewCSharpCreation
            var ListView = new ListView();
            ListView.Items.Add(new ListViewItem { Text = "Item1"});
            ListView.Items.Add(new ListViewItem { Text = "Item2" });
            ListView.Items.Add(new ListViewItem { Text = "Item2" });
            ListView.SelectedIndex = 1;
            #endregion
        }

        #region ListViewEventHandler
        private void ListView_SelectionChanged(object? sender, EventArgs e)
        {
            App.Log($"ListView: SelectionChanged. SelectedIndex: {listView.SelectedIndex}");
        }
        #endregion    
    }
}