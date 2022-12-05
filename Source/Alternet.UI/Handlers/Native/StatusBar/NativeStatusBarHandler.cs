using Alternet.Base.Collections;
using System;

namespace Alternet.UI
{
    internal class NativeStatusBarHandler : StatusBarHandler
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.StatusBar();
        }

        public new Native.StatusBar NativeControl => (Native.StatusBar)base.NativeControl!;

        public override bool SizingGripVisible { get => NativeControl.SizingGripVisible; set => NativeControl.SizingGripVisible = value; }

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

        void InsertItem(StatusBarPanel item, int index)
        {
            var handler = item.Handler as NativeStatusBarPanelHandler;
            if (handler == null)
                throw new InvalidOperationException();

            NativeControl.InsertPanelAt(index, handler.NativeControl);
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<StatusBarPanel> e)
        {
            InsertItem(e.Item, e.Index);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<StatusBarPanel> e)
        {
            NativeControl.RemovePanelAt(e.Index);
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            Control.Panels.ItemInserted -= Items_ItemInserted;
            Control.Panels.ItemRemoved -= Items_ItemRemoved;
        }
    }
}