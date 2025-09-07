using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class ListEditSourceTreeViewItem : ListEditSource
    {
        public override bool AllowSubItems => true;

        public TreeView? TreeView => Instance as TreeView;

        public override IEnumerable? RootItems => TreeView?.Items;

        public override ImageList? ImageList => TreeView?.ImageList;

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

        public override int? GetItemImageIndex(object item) => (item as TreeViewItem)?.ImageIndex;

        public override object? CreateNewItem()
        {
            var result = new TreeViewItem
            {
                Text = CommonStrings.Default.ListEditDefaultItemTitle,
            };
            return result;
        }

        public override void ApplyData(IEnumerableTree tree)
        {
            if (TreeView == null)
                return;
            TreeView treeView = TreeView;

            EnumerableUtils.ForEach(
                tree,
                (item) =>
                {
                    var data = tree.GetData(item);
                    if (data is TreeViewItem treeItem)
                    {
                        treeItem.Items.Clear();
                        var children = EnumerableUtils.GetChildren<TreeViewItem>(tree, item);
                        treeItem.Items.AddRange(children);
                    }
                });

            treeView.DoInsideUpdate(() =>
            {
                treeView.RemoveAll();
                treeView.Items.AddRange(EnumerableUtils.GetItems<TreeViewItem>(tree));
            });
        }
    }
}