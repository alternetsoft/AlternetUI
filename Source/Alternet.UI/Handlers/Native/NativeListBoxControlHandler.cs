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
            ApplySelectionMode();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.SelectionModeChanged += Control_SelectionModeChanged;
        }

        private void Control_SelectionModeChanged(object? sender, EventArgs e)
        {
            ApplySelectionMode();
        }

        private void ApplySelectionMode()
        {
            NativeControl.SelectionMode = (Native.ListBoxSelectionMode)Control.SelectionMode;
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            for (var i = 0; i < items.Count; i++)
                NativeControl.InsertItem(i, Control.GetItemText(control.Items[i]));
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;

            base.OnDetach();
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.InsertItem(e.Index, Control.GetItemText(e.Item));
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<object> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }
    }
}