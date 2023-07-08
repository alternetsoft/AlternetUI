using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeToolbarHandler : ToolbarHandler
    {
        public new Native.Toolbar NativeControl => (Native.Toolbar)base.NativeControl!;

        public override bool ItemTextVisible
        {
            get => NativeControl.ItemTextVisible;
            set => NativeControl.ItemTextVisible = value;
        }

        public override bool ItemImagesVisible
        {
            get => NativeControl.ItemImagesVisible;
            set => NativeControl.ItemImagesVisible = value;
        }

        public override ToolbarItemImageToTextDisplayMode ImageToTextDisplayMode
        {
            get => (ToolbarItemImageToTextDisplayMode)NativeControl.ImageToTextDisplayMode;
            set => NativeControl.ImageToTextDisplayMode = (Native.ToolbarItemImageToTextDisplayMode)value;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.Toolbar();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
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

        private void InsertItem(ToolbarItem item, int index)
        {
            var handler = item.Handler as NativeToolbarItemHandler ??
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
    }
}