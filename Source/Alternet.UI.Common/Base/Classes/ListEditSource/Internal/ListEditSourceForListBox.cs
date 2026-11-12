using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a list edit source for a list box control.
    /// </summary>
    public class ListEditSourceForListBox : ListEditSource
    {
        private IEnumerable? rootItems;

        /// <inheritdoc/>
        public override IEnumerable RootItems
        {
            get
            {
                if (rootItems is not null)
                    return rootItems;

                if (Instance is VirtualListBox listBox)
                    rootItems = listBox.Items;
                else
                if(Instance is not null)
                    rootItems = PropInfo?.GetValue(Instance) as IEnumerable;

                rootItems ??= new List<object>();

                return rootItems;
            }
        }

        /// <inheritdoc/>
        public override object? CreateNewItem() => new ListControlItem();

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override object? GetProperties(object item)
        {
            return item;
        }

        /// <inheritdoc/>
        public override void ApplyData(IEnumerableTree tree)
        {
            if (Instance is null)
                return;

            if (RootItems is not IList items)
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
