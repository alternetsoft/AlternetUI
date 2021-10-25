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
            void AddItems(Alternet.UI.TreeView treeView, int count)
            {
                int start = treeView.Items.Count + 1;

                treeView.BeginUpdate();
                try
                {
                    for (int i = start; i < start + count; i++)
                    {
                        int imageIndex = i % 4;
                        var item = new TreeViewItem("Item " + i, imageIndex);
                        for (int j = 0; j < 3; j++)
                            item.Items.Add(new TreeViewItem(item.Text + "." + j, imageIndex));
                        treeView.Items.Add(item);
                    }
                }
                finally
                {
                    treeView.EndUpdate();
                }
            }

            AddItems(treeView, 10);
        }

        public void TreeViewExample1()
        {
            #region TreeViewCSharpCreation
            void AddItems(Alternet.UI.TreeView treeView, int count)
            {
                int start = treeView.Items.Count + 1;

                treeView.BeginUpdate();
                try
                {
                    for (int i = start; i < start + count; i++)
                    {
                        int imageIndex = i % 4;
                        var item = new TreeViewItem("Item " + i, imageIndex);
                        for (int j = 0; j < 3; j++)
                            item.Items.Add(new TreeViewItem(item.Text + "." + j, imageIndex));
                        treeView.Items.Add(item);
                    }
                }
                finally
                {
                    treeView.EndUpdate();
                }
            }

            var TreeView = new Alternet.UI.TreeView();
            AddItems(TreeView, 10);
            #endregion
        }

        #region TreeViewEventHandler
        private void TreeView_SelectionChanged(object? sender, EventArgs e)
        {
            MessageBox.Show("TreeView: SelectionChanged. SelectedItem: "  + treeView.SelectedItem.Text, string.Empty);
        }

        private void TreeView_ExpandedChanged(object? sender, TreeViewItemExpandedChangedEventArgs e)
        {
            MessageBox.Show("TreeView: ExpandedChanged. Item: " + e.Item.Text + e.Item.IsExpanded.ToString(), string.Empty);
        }

        #endregion    
    }
}