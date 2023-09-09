using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeStatusBarHandler : StatusBarHandler
    {
        public new Native.StatusBar NativeControl => (Native.StatusBar)base.NativeControl!;

        public override bool SizingGripVisible
        {
            get => NativeControl.SizingGripVisible;
            set => NativeControl.SizingGripVisible = value;
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.StatusBar();
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.Panels.ItemInserted -= Items_ItemInserted;
            Control.Panels.ItemRemoved -= Items_ItemRemoved;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();

            Control.Panels.ItemInserted += Items_ItemInserted;
            Control.Panels.ItemRemoved += Items_ItemRemoved;
        }

        private void ApplyItems()
        {
            for (var i = 0; i < Control.Panels.Count; i++)
                InsertItem(Control.Panels[i], i);
        }

        private void InsertItem(StatusBarPanel item, int index)
        {
            if (item.Handler is not NativeStatusBarPanelHandler handler)
                throw new InvalidOperationException();

            NativeControl.InsertPanelAt(index, handler.NativeControl);
        }

        private void Items_ItemInserted(object? sender, int index, StatusBarPanel item)
        {
            InsertItem(item, index);
        }

        private void Items_ItemRemoved(object? sender, int index, StatusBarPanel item)
        {
            NativeControl.RemovePanelAt(index);
        }
    }
}