using System;

namespace Alternet.UI
{
    internal class NativeListViewHandler : NativeControlHandler<ListView, Native.ListView>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.ListView();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();
            ApplyView();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.Columns.ItemInserted += Columns_ItemInserted;
            Control.Columns.ItemRemoved += Columns_ItemRemoved;
            Control.ViewChanged += Control_ViewChanged;
        }

        private void Control_ViewChanged(object? sender, EventArgs e)
        {
            ApplyView();
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.ViewChanged -= Control_ViewChanged;
            Control.Columns.ItemInserted -= Columns_ItemInserted;
            Control.Columns.ItemRemoved -= Columns_ItemRemoved;

            base.OnDetach();
        }

        private void ApplyView()
        {
            NativeControl.CurrentView = (Native.ListViewView)Control.View;
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            for (var itemIndex = 0; itemIndex < items.Count; itemIndex++)
            {
                var item = control.Items[itemIndex];
                InsertItem(itemIndex, item);
            }
        }

        private void InsertItem(int itemIndex, ListViewItem item)
        {
            for (var columnIndex = 0; columnIndex < item.Cells.Count; columnIndex++)
                NativeControl.InsertItemAt(itemIndex, item.Cells[columnIndex].Text, columnIndex, item.ImageIndex ?? -1);
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<ListViewItem> e)
        {
            InsertItem(e.Index, e.Item);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewItem> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }

        private void Columns_ItemInserted(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            NativeControl.InsertColumnAt(e.Item.Index ?? throw new Exception(), e.Item.Title);
        }

        private void Columns_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            NativeControl.RemoveColumnAt(e.Item.Index ?? throw new Exception());
        }
    }
}