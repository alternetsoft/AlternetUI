using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ListEditSourceStatusBar : ListEditSource
    {
        public override IEnumerable? RootItems => (Instance as StatusBar)?.Panels;

        public override object? CreateNewItem() => new StatusBarPanel();

        public override object CloneItem(object item) => ((StatusBarPanel)item).Clone();

        public override void ApplyData(IEnumerableTree tree)
        {
            if (Instance is not StatusBar statusBar)
                return;

            List<StatusBarPanel> items = [];
            items.AddRange(EnumerableUtils.GetItems<StatusBarPanel>(tree));
            statusBar.BeginUpdate();
            try
            {
                statusBar.Panels.SetCount(items.Count, () => new StatusBarPanel());
                for (int i = 0; i < items.Count; i++)
                    statusBar.Panels[i].Assign(items[i]);
            }
            finally
            {
                statusBar.EndUpdate();
            }
        }
    }
}
