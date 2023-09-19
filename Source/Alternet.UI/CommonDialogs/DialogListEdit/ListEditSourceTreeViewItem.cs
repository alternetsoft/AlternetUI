using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ListEditSourceTreeViewItem : ListEditSource
    {
        public override bool AllowSubItems => true;

        public TreeView? TreeView => Instance as TreeView;

        public override IEnumerable? RootItems
        {
            get
            {
                return TreeView?.Items;
            }
        }

        public override object CloneItem(object item)
        {
            var result = ((TreeViewItem)item).Clone(false);
            return result;
        }

        public override IEnumerable? GetChildren(object item)
        {
            if (item is not TreeViewItem tvItem || !tvItem.HasItems)
                return Array.Empty<object>();
            return tvItem.Items;
        }

        public override ImageList? ImageList => TreeView?.ImageList;

        public override int? GetItemImageIndex(object item) => (item as TreeViewItem)?.ImageIndex;

        public override object? CreateNewItem() => new TreeViewItem();

        public override void ApplyData(IEnumerableTree tree)
        {
            if (TreeView == null)
                return;
            TreeView treeView = TreeView;

            ForEachItem(
                tree, 
                (item) =>
                {
                    var data = tree.GetData(item);
                    if (data is TreeViewItem treeItem)
                    {
                        treeItem.Items.Clear();
                        var children = GetChildren<TreeViewItem>(tree, item);
                        treeItem.Items.AddRange(children);
                    }
                });

            treeView.DoInsideUpdate(() =>
            {
                treeView.RemoveAll();
                treeView.Items.AddRange(GetItems<TreeViewItem>(tree));
            });
        }
    }
}