using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ListEditSourceForToolBar : ListEditSource
    {
        public override IEnumerable? RootItems => (Instance as ToolBar)?.Panels;

        public override object? CreateNewItem() => new BarPanel();

        public override string? GetItemTitle(object item)
        {
            return (item as BarPanel)?.Text;
        }

        public override object CloneItem(object item) => ((BarPanel)item).Clone();

        public override void ApplyData(IEnumerableTree tree)
        {
            if (Instance is not ToolBar statusBar)
                return;

            List<BarPanel> items = new();
            items.AddRange(EnumerableUtils.GetItems<BarPanel>(tree));
            statusBar.BeginUpdate();
            try
            {
                statusBar.Panels.Clear();
                for (int i = 0; i < items.Count; i++)
                {
                    var item = new BarPanel();
                    item.Assign(items[i]);
                    statusBar.Panels.Add(item);
                }
            }
            finally
            {
                statusBar.EndUpdate();
            }
        }
    }
}
