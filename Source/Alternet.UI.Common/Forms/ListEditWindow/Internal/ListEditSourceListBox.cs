using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class ListEditSourceListBox : ListEditSource
    {
        private IEnumerable? rootItems;

        public override IEnumerable RootItems
        {
            get
            {
                if (rootItems is not null)
                    return rootItems;

                if (Instance is VirtualListBox listBox)
                    rootItems = listBox.Items;
                else
                if (Instance is VirtualListBox comboBox)
                    rootItems = comboBox.Items;
                else
                if(Instance is not null)
                    rootItems = PropInfo?.GetValue(Instance) as IEnumerable;

                rootItems ??= new List<object>();

                return rootItems;
            }
        }

        public override object? CreateNewItem() => new ListControlItem();

        public override object CloneItem(object item)
        {
            if (item is ListControlItem listItem)
                return listItem.Clone();
            else
            {
                var result = new ListControlItem();
                result.Value = item;
                return result;
            }
        }

        public override object? GetProperties(object item)
        {
            return item;
        }

        public override void ApplyData(IEnumerableTree tree)
        {
            if (Instance is null)
                return;

            IList? items = null;

            if (RootItems is IListControlItems<ListControlItem> list1)
                items = list1.AsList;
            else
            if (RootItems is IListControlItems<object> list2)
                items = list2.AsList;
            else
                items = RootItems as IList;

            if (items is null)
                return;

            var containers = EnumerableUtils.GetItems<ListControlItem>(tree);

            if (Instance is Control control)
                control.DoInsideUpdate(() => { SetItems(items); });
            else
                SetItems(items);

            void SetItems(IList list)
            {
                list.Clear();
                foreach (var item in containers)
                    list.Add(item);
            }
        }

        private class ValueContainer<T>
        {
            private T value;

            public ValueContainer(T value)
            {
                this.value = value;
            }

            public T Value { get => value; set => this.value = value; }

            /// <inheritdoc/>
            public override string ToString()
            {
                var s = Value?.ToString()!;
                if (string.IsNullOrWhiteSpace(s))
                    return CommonStrings.Default.ListEditDefaultItemTitle;
                else
                    return s;
            }
        }
    }
}
