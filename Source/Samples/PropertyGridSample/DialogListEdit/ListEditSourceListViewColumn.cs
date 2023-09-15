using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ListEditSourceListViewColumn : ListEditSourceListViewItem
    {
        public override IEnumerable? RootItems
        {
            get
            {
                return ListView?.Columns;
            }
        }

        public override object? CreateNewItem() => new ListViewColumn();

        public override void ApplyData(IEnumerableTree tree)
        {
            if (ListView == null)
                return;
            ListView listView = ListView;

            listView.DoInsideUpdate(() =>
            {
                foreach (var srcItem in tree)
                {
                    if (tree.GetData(srcItem) is not ListViewColumn data)
                        continue;

                }
            });
        }
    }
}
