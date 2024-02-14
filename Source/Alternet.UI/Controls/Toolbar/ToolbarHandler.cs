using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class ToolBarHandler : ControlHandler
    {
        private readonly bool mainToolbar = false;

        public ToolBarHandler(bool mainToolbar = true)
            : base()
        {
            this.mainToolbar = mainToolbar;
        }

        /// <summary>
        /// Gets a <see cref="ToolBar"/> this handler provides the implementation for.
        /// </summary>
        public new ToolBar Control => (ToolBar)base.Control;

        public new Native.Toolbar NativeControl => (Native.Toolbar)base.NativeControl!;

        public bool NoDivider
        {
            get => NativeControl.NoDivider;
            set => NativeControl.NoDivider = value;
        }

        public bool IsVertical
        {
            get => NativeControl.IsVertical;
            set => NativeControl.IsVertical = value;
        }

        public bool IsBottom
        {
            get
            {
                return NativeControl.IsBottom;
            }

            set
            {
                NativeControl.IsBottom = value;
            }
        }

        public bool IsRight
        {
            get
            {
                return NativeControl.IsRight;
            }

            set
            {
                NativeControl.IsRight = value;
            }
        }

        public bool ItemTextVisible
        {
            get => NativeControl.ItemTextVisible;
            set => NativeControl.ItemTextVisible = value;
        }

        public bool ItemImagesVisible
        {
            get => NativeControl.ItemImagesVisible;
            set => NativeControl.ItemImagesVisible = value;
        }

        public ImageToText ImageToText
        {
            get => (ImageToText)NativeControl.ImageToTextDisplayMode;
            set => NativeControl.ImageToTextDisplayMode = (Native.ToolbarItemImageToTextDisplayMode)value;
        }

        public void Realize()
        {
            NativeControl.Realize();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeToolbar(mainToolbar);
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

        private void InsertItem(ToolBarItem item, int index)
        {
            var handler = item.Handler as ToolBarItemHandler ??
                throw new InvalidOperationException();
            NativeControl.InsertItemAt(index, handler.NativeControl);
        }

        private void Items_ItemInserted(object? sender, int index, ToolBarItem item)
        {
            InsertItem(item, index);
        }

        private void Items_ItemRemoved(object? sender, int index, ToolBarItem item)
        {
            NativeControl.RemoveItemAt(index);
        }

        private class NativeToolbar : Native.Toolbar
        {
            public NativeToolbar(bool mainToolbar)
            {
                SetNativePointer(NativeApi.Toolbar_CreateEx_(mainToolbar));
            }

            public NativeToolbar(IntPtr nativePointer)
                : base(nativePointer)
            {
            }
        }
    }
}