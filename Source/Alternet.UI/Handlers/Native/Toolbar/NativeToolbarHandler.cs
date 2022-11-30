using Alternet.Base.Collections;
using System;

namespace Alternet.UI
{
    internal class NativeToolbarHandler : ToolbarHandler
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.Toolbar();
        }

        public new Native.Toolbar NativeControl => (Native.Toolbar)base.NativeControl!;

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        private void ApplyItems()
        {
            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        void InsertItem(ToolbarItem item, int index)
        {
            var handler = item.Handler as NativeToolbarItemHandler;
            if (handler == null)
                throw new InvalidOperationException();

            NativeControl.InsertItemAt(index, handler.NativeControl);
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<ToolbarItem> e)
        {
            InsertItem(e.Item, e.Index);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<ToolbarItem> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
        }
    }
}