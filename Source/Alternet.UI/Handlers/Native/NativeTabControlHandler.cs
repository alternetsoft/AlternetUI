using System;
using Alternet.Drawing;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeTabControlHandler : TabControlHandler
    {
        internal override Native.Control CreateNativeControl()
        {
            return new Native.TabControl();
        }

        public new Native.TabControl NativeControl => (Native.TabControl)base.NativeControl!;

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.Pages.ItemInserted += Pages_ItemInserted;
            Control.Pages.ItemRemoved += Pages_ItemRemoved;
            Control.Children.ItemInserted += Children_ItemInserted;

            NativeControl.SelectedPageIndexChanged += NativeControl_SelectedPageIndexChanged;
            //NativeControl.SelectedPageIndexChanging += NativeControl_SelectedPageIndexChanging;
            Control.SelectedPageChanged += Control_SelectedPageChanged;
        }

        //private void NativeControl_SelectedPageIndexChanging(object? sender, Native.NativeEventArgs<Native.TabPageSelectionEventData> e)
        //{
        //    var oldValue = e.Data.oldSelectedTabPageIndex == -1 ? null : Control.Pages[e.Data.oldSelectedTabPageIndex];
        //    var newValue = e.Data.newSelectedTabPageIndex == -1 ? null : Control.Pages[e.Data.newSelectedTabPageIndex];
            
        //    var ea = new SelectedTabPageChangingEventArgs(oldValue, newValue);
        //    Control.RaiseSelectedPageChanging(ea);
        //    e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        //}

        private void Children_ItemInserted(object? sender, CollectionChangeEventArgs<Control> e)
        {
            if (!skipChildrenInsertionCheck)
                throw new InvalidOperationException(
                    "Do not modify TabControl.Children collection directly. Use TabControl.Pages to add pages to the tab control instead.");
        }

        private void Control_SelectedPageChanged(object sender, RoutedEventArgs e)
        {
            var index = Control.SelectedPage == null ? -1 : Control.Pages.IndexOf(Control.SelectedPage);
            NativeControl.SelectedPageIndex = index;
        }

        private void NativeControl_SelectedPageIndexChanged(object? sender, EventArgs e)
        {
            UpdateSelectedPageFromNativeControl();
            LayoutSelectedPage();
        }

        private void UpdateSelectedPageFromNativeControl()
        {
            var selectedPageIndex = NativeControl.SelectedPageIndex;
            Control.SelectedPage = selectedPageIndex == -1 ? null : Control.Pages[selectedPageIndex];
        }

        protected override void OnDetach()
        {
            NativeControl.SelectedPageIndexChanged -= NativeControl_SelectedPageIndexChanged;
            //NativeControl.SelectedPageIndexChanging -= NativeControl_SelectedPageIndexChanging;
            Control.SelectedPageChanged -= Control_SelectedPageChanged;
            Control.Children.ItemInserted -= Children_ItemInserted;

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

        bool skipChildrenInsertionCheck;

        public override TabAlignment TabAlignment
        {
            get => (TabAlignment)NativeControl.TabAlignment;
            set => NativeControl.TabAlignment = (Native.TabAlignment)value;
        }

        private void InsertPage(int index, TabPage page)
        {
            skipChildrenInsertionCheck = true;
            try
            {
                Control.Children.Insert(index, page);
            }
            finally
            {
                skipChildrenInsertionCheck = false;
            }

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

        void UpdatePageIndices(int startIndex)
        {
            for (int i = startIndex; i < Control.Pages.Count; i++)
                Control.Pages[i].Index = i;
        }

        protected virtual void OnPageInserted(int index, TabPage page)
        {
            page.Index = index;
            InsertPage(index, page);
            page.TitleChanged += Page_TitleChanged;
            UpdatePageIndices(index + 1);
            UpdateSelectedPageFromNativeControl();
        }

        private void Page_TitleChanged(object? sender, EventArgs e)
        {
            var page = (TabPage)sender!;
            NativeControl.SetPageTitle(page.Index ?? throw new Exception(), page.Title);
        }

        protected virtual void OnPageRemoved(int index, TabPage page)
        {
            page.TitleChanged -= Page_TitleChanged;
            RemovePage(index, page);
            UpdateSelectedPageFromNativeControl();
            page.Index = null;
            UpdatePageIndices(index);
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