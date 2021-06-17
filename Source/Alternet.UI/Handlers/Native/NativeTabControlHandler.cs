using System;
using System.Drawing;

namespace Alternet.UI
{
    internal class NativeTabControlHandler : NativeControlHandler<TabControl, Native.TabControl>
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.TabControl();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.Pages.ItemInserted += Pages_ItemInserted;
            Control.Pages.ItemRemoved += Pages_ItemRemoved;
        }

        protected override void OnDetach()
        {
            Control.Pages.ItemInserted -= Pages_ItemInserted;
            Control.Pages.ItemRemoved -= Pages_ItemRemoved;

            base.OnDetach();
        }

        private void InsertPage(int index, TabPage page)
        {
            Control.Children.Insert(index, page);
            var pageNativeControl = page.Handler.NativeControl;
            if (pageNativeControl == null)
                throw new InvalidOperationException();

            NativeControl.InsertPage(index, pageNativeControl, page.Title);
        }

        private void RemovePage(int index, TabPage page)
        {
            NativeControl.RemovePage(index);
            Control.Children.RemoveAt(index);
        }

        protected virtual void OnPageInserted(int index, TabPage page)
        {
            InsertPage(index, page);
        }

        protected virtual void OnPageRemoved(int index, TabPage page)
        {
            RemovePage(index, page);
        }

        private void Pages_ItemInserted(object? sender, CollectionChangeEventArgs<TabPage> e)
        {
            OnPageInserted(e.Index, e.Item);
        }

        private void Pages_ItemRemoved(object? sender, CollectionChangeEventArgs<TabPage> e)
        {
            OnPageRemoved(e.Index, e.Item);
        }
    }
}