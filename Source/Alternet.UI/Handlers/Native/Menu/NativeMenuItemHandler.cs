using System;

namespace Alternet.UI
{
    internal class NativeMenuItemHandler : NativeControlHandler<MenuItem, Native.MenuItem>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.MenuItem();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyText();
            ApplyItems();

            Control.TextChanged += Control_TextChanged;
            NativeControl.Click += NativeControl_Click;

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        private void ApplyItems()
        {
            for (var i = 0; i < Control.Items.Count; i++)
                InsertItem(Control.Items[i], i);
        }

        private void ApplyText()
        {
            NativeControl.Text = Control.Text;
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.TextChanged -= Control_TextChanged;
            NativeControl.Click -= NativeControl_Click;

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
        }

        private void Items_ItemInserted(object sender, Base.Collections.CollectionChangeEventArgs<MenuItem> e)
        {
            InsertItem(e.Item, e.Index);
        }

        private void Items_ItemRemoved(object sender, Base.Collections.CollectionChangeEventArgs<MenuItem> e)
        {
            (NativeControl.Submenu ?? throw new Exception()).RemoveItemAt(e.Index);
        }

        void InsertItem(MenuItem item, int index)
        {
            var handler = item.Handler as NativeMenuItemHandler;
            if (handler == null)
                throw new InvalidOperationException();

            handler.EnsureNativeSubmenuCreated();

            EnsureNativeSubmenuCreated().InsertItemAt(index, handler.NativeControl);
        }

        internal Native.Menu EnsureNativeSubmenuCreated()
        {
            if (NativeControl.Submenu == null)
                NativeControl.Submenu = new Native.Menu();

            return NativeControl.Submenu;
        }

        private void NativeControl_Click(object? sender, EventArgs? e)
        {
            Control.RaiseClick(e ?? throw new ArgumentNullException());
        }

        private void Control_TextChanged(object? sender, EventArgs? e)
        {
            ApplyText();
        }
    }
}