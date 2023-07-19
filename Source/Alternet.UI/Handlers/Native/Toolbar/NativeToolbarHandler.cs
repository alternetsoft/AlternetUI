using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeToolbarHandler : ToolbarHandler
    {
        bool mainToolbar = false;

        public NativeToolbarHandler(bool mainToolbar = true)
            : base()
        {
            this.mainToolbar = mainToolbar;
        }

        public new Native.Toolbar NativeControl => (Native.Toolbar)base.NativeControl!;

        public override bool NoDivider
        {
            get => NativeControl.NoDivider;
            set => NativeControl.NoDivider = value;
        }

        public override bool IsVertical
        {
            get => NativeControl.IsVertical;
            set => NativeControl.IsVertical = value;
        }

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

        public override void Realize()
        {
            NativeControl.Realize();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new FloatingToolbar(mainToolbar);
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

        private class FloatingToolbar : Native.Toolbar
        {
            public FloatingToolbar(bool mainToolbar)
            {
                SetNativePointer(NativeApi.Toolbar_CreateEx_(mainToolbar));
            }

            public FloatingToolbar(IntPtr nativePointer)
                : base(nativePointer)
            {
            }
        }
    }
}