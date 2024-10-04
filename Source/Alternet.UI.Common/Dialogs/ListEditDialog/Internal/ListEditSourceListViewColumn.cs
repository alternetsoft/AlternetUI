﻿using System.Collections;
using System.Collections.Generic;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class ListEditSourceListViewColumn : ListEditSourceListViewItem
    {
        public override IEnumerable? RootItems => ListView?.Columns;

        public override object? CreateNewItem()
        {
            var result = new ListViewColumn
            {
                Title = CommonStrings.Default.ListEditDefaultItemTitle,
            };
            return result;
        }

        public override object CloneItem(object item) => ((ListViewColumn)item).Clone();

        public override void ApplyData(IEnumerableTree tree)
        {
            if (ListView == null)
                return;
            ListView listView = ListView;

            listView.DoInsideUpdate(() =>
            {
#pragma warning disable
                List<ListViewColumn> columns = new();
#pragma warning restore
                columns.AddRange(EnumerableUtils.GetItems<ListViewColumn>(tree));
                listView.Columns.SetCount(columns.Count, () => new ListViewColumn());
                for (int i = 0; i < columns.Count; i++)
                    listView.Columns[i].Assign(columns[i]);
            });
        }
    }
}