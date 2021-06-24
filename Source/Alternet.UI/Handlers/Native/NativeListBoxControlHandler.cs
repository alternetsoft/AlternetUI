using System;
using System.Drawing;

namespace Alternet.UI
{
    internal class NativeListBoxHandler : NativeControlHandler<ListBox, Native.ListBox>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.ListBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            for (var i = 0; i < items.Count; i++)
                NativeControl.InsertItem(i, GetItemText(control.Items[i]));
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;

            base.OnDetach();
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.InsertItem(e.Index, GetItemText(e.Item));
        }

        private static string GetItemText(object value)
        {
            return value.ToString(); // todo
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }
    }
}