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
        public ListControl? ListControl => Instance as ListControl;

        public override IEnumerable? RootItems => ListControl?.Items;

        public override object? CreateNewItem() =>
            new ValueContainer<string>(CommonStrings.Default.ListEditDefaultItemTitle);

        public override object CloneItem(object item)
        {
            var s = item.ToString() ?? string.Empty;
            var container = new ValueContainer<string>(s);
            return container;
        }

        public override object? GetProperties(object item)
        {
            return item;
        }

        public override void ApplyData(IEnumerableTree tree)
        {
            if (ListControl == null)
                return;
            ListControl control = ListControl;

            control.DoInsideUpdate(() =>
            {
                control.RemoveAll();
                var containers = EnumerableUtils.GetItems<ValueContainer<string>>(tree);
                foreach(var item in containers)
                    control.Items.Add(item.Value);
            });
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
