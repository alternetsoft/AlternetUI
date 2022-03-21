using System;
using Alternet.Drawing;
using Alternet.Base.Collections;

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

            NativeControl.SelectedPageIndexChanged += NativeControl_SelectedPageIndexChanged;
            Control.SelectedPageChanged += Control_SelectedPageChanged;
        }

        private void Control_SelectedPageChanged(object sender, RoutedEventArgs e)
        {
            var index = Control.SelectedPage == null ? -1 : Control.Pages.IndexOf(Control.SelectedPage);
            NativeControl.SelectedPageIndex = index;
        }

        private void NativeControl_SelectedPageIndexChanged(object? sender, EventArgs e)
        {
            var selectedPageIndex = NativeControl.SelectedPageIndex;
            Control.SelectedPage = selectedPageIndex == -1 ? null : Control.Pages[selectedPageIndex];
            
            LayoutSelectedPage();
        }

        protected override void OnDetach()
        {
            NativeControl.SelectedPageIndexChanged -= NativeControl_SelectedPageIndexChanged;
            Control.SelectedPageChanged -= Control_SelectedPageChanged;

            Control.Pages.ItemInserted -= Pages_ItemInserted;
            Control.Pages.ItemRemoved -= Pages_ItemRemoved;

            base.OnDetach();
        }

        public override void OnLayout()
        {
            LayoutSelectedPage();
        }

        private void LayoutSelectedPage()
        {
            var selectedPageIndex = NativeControl.SelectedPageIndex;
            if (selectedPageIndex >= 0)
            {
                var page = Control.Pages[selectedPageIndex];

#if NETCOREAPP
           if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                page.Handler.Bounds = ChildrenLayoutBounds;
#endif
                page.PerformLayout();
            }
        }

        private void InsertPage(int index, TabPage page)
        {
            Control.Children.Insert(index, page);
            var pageNativeControl = page.Handler.NativeControl;
            if (pageNativeControl == null)
                throw new InvalidOperationException();

            NativeControl.InsertPage(index, pageNativeControl, page.Title);
            PerformLayout();
        }

        private void RemovePage(int index, TabPage page)
        {
            var pageNativeControl = page.Handler.NativeControl;
            if (pageNativeControl == null)
                throw new InvalidOperationException();

            NativeControl.RemovePage(index, pageNativeControl);
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

        public override Size GetPreferredSize(Size availableSize)
        {
            return NativeControl.GetTotalPreferredSizeFromPageSize(GetChildrenMaxPreferredSize(availableSize));
        }
    }
}