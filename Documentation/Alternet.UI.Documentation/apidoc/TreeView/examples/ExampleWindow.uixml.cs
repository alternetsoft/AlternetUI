using Alternet.UI;
using System;
using System.Linq;

namespace Alternet.UI.Documentation.Examples.TreeView
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var imageLists = ResourceLoader.LoadImageLists();
            treeView.ImageList = imageLists.Small;

            AddItems(treeView, 10);
            treeView.Items.First?.ExpandAll();
        }

        #region TreeViewCSharpCreation
        void AddItems(Alternet.UI.TreeView treeView, int count)
        {
            var r = new Random();

            int start = treeView.Items.Count + 1;

            treeView.BeginUpdate();
            try
            {
                for (int i = start; i < start + count; i++)
                {
                    int imageIndex = r.Next(4);
                    var item = new TreeViewItem("Item " + i)
                    {
                        ImageIndex = imageIndex,
                    };
                    for (int j = 0; j < 3; j++)
                        item.Items.Add(new TreeViewItem(item.Text + "." + j, r.Next(4)));
                    treeView.Items.Add(item);
                }
            }
            finally
            {
                treeView.EndUpdate();
            }
        }

        public Alternet.UI.TreeView CreateTreeView()
        {
            var result = new Alternet.UI.TreeView();
            AddItems(result, 10);
            return result;
        }
        #endregion

        #region TreeViewEventHandler
        private void TreeView_SelectionChanged(object? sender, EventArgs e)
        {
            Application.Log($"TreeView: SelectionChanged. SelectedItem: {treeView.SelectedItem?.Text}");
        }

        private void TreeView_ExpandedChanged(object? sender, TreeViewEventArgs e)
        {
            Application.Log($"TreeView: ExpandedChanged. Item: {e.Item.Text} {e.Item.IsExpanded}");
        }

        #endregion    
    }
}