using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ListEditSourceListViewItem : ListEditSource
    {
        public ListView? ListView => Instance as ListView;

        public override IEnumerable? RootItems
        {
            get
            {
                return ListView?.Items;
            }
        }

        public override object? CreateNewItem() => new ListViewItem();

        public override ImageList? ImageList => ListView?.SmallImageList;

        public override int? GetItemImageIndex(object item) => (item as ListViewItem)?.ImageIndex;
    }
}
