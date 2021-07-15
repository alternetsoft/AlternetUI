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

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;

            base.OnDetach();
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            for (var i = 0; i < items.Count; i++)
                NativeControl.InsertItemAt(i, control.Items[i].Text);
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<ListViewItem> e)
        {
            NativeControl.InsertItemAt(e.Index, e.Item.Text);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewItem> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }
    }
}