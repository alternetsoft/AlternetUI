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

        public override IEnumerable? RootItems => ListView?.Items;

        public override object? CreateNewItem() => new ListViewItem();

        public override ImageList? ImageList => ListView?.SmallImageList;

        public override int? GetItemImageIndex(object item) => (item as ListViewItem)?.ImageIndex;

        public override object CloneItem(object item)
        {
            var result = ((ListViewItem)item).Clone();
            return result;
        }

        public override void ApplyData(IEnumerableTree tree)
        {
            if (ListView == null)
                return;
            ListView listView = ListView;

            listView.DoInsideUpdate(() =>
            {
                listView.RemoveAll();
                listView.Items.AddRange(GetItems<ListViewItem>(tree));
            });
        }
    }
}