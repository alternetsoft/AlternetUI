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

        public override IEnumerable? GetChildren(object item)
        {
            if (item is not TreeViewItem tvItem || !tvItem.HasItems)
                return Array.Empty<object>();
            return tvItem.Items;
        }

        public override ImageList? ImageList => TreeView?.ImageList;

        public override int? GetItemImageIndex(object item) => (item as TreeViewItem)?.ImageIndex;
    }
}
