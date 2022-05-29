using Alternet.Base.Collections;
using System;

namespace Alternet.UI
{
    internal class NativeMainMenuHandler : NativeControlHandler<MainMenu, Native.MainMenu>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.MainMenu();
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
            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        void InsertItem(MenuItem item, int index)
        {
            var handler = item.Handler as NativeMenuItemHandler;
            if (handler == null)
                throw new InvalidOperationException();

            var submenu = handler.EnsureNativeSubmenuCreated() ?? throw new Exception();

            NativeControl.InsertItemAt(index, submenu, item.Text);

            item.TextChanged += Item_TextChanged;
        }

        private void Item_TextChanged(object? sender, EventArgs e)
        {
            var item = (MenuItem)sender!;
            NativeControl.SetItemText(Control.Items.IndexOf(item), item.Text);
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<MenuItem> e)
        {
            InsertItem(e.Item, e.Index);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<MenuItem> e)
        {
            e.Item.TextChanged -= Item_TextChanged;
            NativeControl.RemoveItemAt(e.Index);
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;

            foreach (var item in Control.Items)
                item.TextChanged -= Item_TextChanged;
        }
    }
}