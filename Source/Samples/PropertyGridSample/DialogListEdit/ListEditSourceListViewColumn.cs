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

        public override object CloneItem(object item)
        {
            var result = ((ListViewColumn)item).Clone();
            return result;
        }

        public override void ApplyData(IEnumerableTree tree)
        {
            if (ListView == null)
                return;
            ListView listView = ListView;

            listView.DoInsideUpdate(() =>
            {
                List<ListViewColumn> columns = new();
                columns.AddRange(GetItems<ListViewColumn>(tree));
                listView.Columns.SetCount(columns.Count, () => new ListViewColumn());
                for (int i = 0; i < columns.Count; i++)
                    listView.Columns[i].Assign(columns[i]);
            });
        }
    }
}