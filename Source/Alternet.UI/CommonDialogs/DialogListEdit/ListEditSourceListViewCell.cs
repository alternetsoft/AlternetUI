using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ListEditSourceListViewCell : ListEditSource
    {
        public override bool AllowAdd => false;

        public override bool AllowDelete => false;

        public ListViewItem? ListViewItem => Instance as ListViewItem;

        public override ImageList? ImageList => ListViewItem?.ListView?.SmallImageList;

        public override IEnumerable? RootItems => ListViewItem?.Cells;

        public override object? CreateNewItem() => new ListViewItemCell();

        public override object CloneItem(object item) => ((ListViewItemCell)item).Clone();

        public override int? GetItemImageIndex(object item) => (item as ListViewItemCell)?.ImageIndex;

        public override void ApplyData(IEnumerableTree tree)
        {
            if (ListViewItem == null)
                return;
            ListViewItem listViewItem = ListViewItem;

            List<ListViewItemCell> cells = [];
            cells.AddRange(EnumerableUtils.GetItems<ListViewItemCell>(tree));
            listViewItem.Cells.SetCount(cells.Count, () => new ListViewItemCell());
            for (int i = 0; i < cells.Count; i++)
                listViewItem.Cells[i].Assign(cells[i]);
        }
    }
}
