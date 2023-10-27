using System;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeTabControlHandler : TabControlHandler
    {
        private bool skipChildrenInsertionCheck;
        private bool layoutFirstTime = true;

        public override TabAlignment TabAlignment
        {
            get => (TabAlignment)NativeControl.TabAlignment;
            set => NativeControl.TabAlignment = (Native.TabAlignment)value;
        }

        public new Native.TabControl NativeControl => (Native.TabControl)base.NativeControl!;

        public override Size GetPreferredSize(Size availableSize)
        {
            return NativeControl.GetTotalPreferredSizeFromPageSize(
                GetSpecifiedOrChildrenPreferredSize(availableSize));
        }

        public override void OnLayout()
        {
            LayoutSelectedPage();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.TabControl();
        }

        protected virtual void OnPageRemoved(int index, TabPage page)
        {
            page.TitleChanged -= Page_TitleChanged;
            RemovePage(index, page);
            UpdateSelectedPageFromNativeControl();
            page.Index = null;
            UpdatePageIndices(index);
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.Pages.ItemInserted += Pages_ItemInserted;
            Control.Pages.ItemRemoved += Pages_ItemRemoved;
            Control.Children.ItemInserted += Children_ItemInserted;

            // NativeControl.SelectedPageIndexChanging += NativeControl_SelectedPageIndexChanging;
            NativeControl.SelectedPageIndexChanged += NativeControl_SelectedPageIndexChanged;
            Control.SelectedPageChanged += Control_SelectedPageChanged;
        }

        /*// private void NativeControl_SelectedPageIndexChanging
        // (object? sender, Native.NativeEventArgs<Native.TabPageSelectionEventData> e)
        // {
        //    var oldValue = e.Data.oldSelectedTabPageIndex == -1 ? null :
        //    Control.Pages[e.Data.oldSelectedTabPageIndex];
        //    var newValue = e.Data.newSelectedTabPageIndex == -1 ? null :
        //    Control.Pages[e.Data.newSelectedTabPageIndex];
        //    var ea = new SelectedTabPageChangingEventArgs(oldValue, newValue);
        //    Control.RaiseSelectedPageChanging(ea);
        //    e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        // }*/

        protected virtual void OnPageInserted(int index, TabPage page)
        {
            page.Index = index;
            InsertPage(index, page);
            page.TitleChanged += Page_TitleChanged;
            UpdatePageIndices(index + 1);
            UpdateSelectedPageFromNativeControl();
        }

        protected override void OnDetach()
        {
            // NativeControl.SelectedPageIndexChanging -= NativeControl_SelectedPageIndexChanging;
            NativeControl.SelectedPageIndexChanged -= NativeControl_SelectedPageIndexChanged;
            Control.SelectedPageChanged -= Control_SelectedPageChanged;
            Control.Children.ItemInserted -= Children_ItemInserted;

            Control.Pages.ItemInserted -= Pages_ItemInserted;
            Control.Pages.ItemRemoved -= Pages_ItemRemoved;

            base.OnDetach();
        }

        private void LayoutSelectedPage()
        {
            var selectedPageIndex = NativeControl.SelectedPageIndex;
            if (selectedPageIndex >= 0)
            {
                var page = Control.Pages[selectedPageIndex];

                if(Application.IsLinuxOS && layoutFirstTime)
                    page.Handler.Bounds = ChildrenLayoutBounds;
                layoutFirstTime = false;

                page.PerformLayout();
            }
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

            var pageNativeControl = page.Handler.NativeControl
                ?? throw new InvalidOperationException();
            NativeControl.InsertPage(index, pageNativeControl, page.Title);
            PerformLayout();
        }

        private void RemovePage(int index, TabPage page)
        {
            var pageNativeControl = page.Handler.NativeControl ??
                throw new InvalidOperationException();
            NativeControl.RemovePage(index, pageNativeControl);
            Control.Children.RemoveAt(index);
        }

        private void UpdatePageIndices(int startIndex)
        {
            for (int i = startIndex; i < Control.Pages.Count; i++)
                Control.Pages[i].Index = i;
        }

        private void Page_TitleChanged(object? sender, EventArgs e)
        {
            var page = (TabPage)sender!;
            NativeControl.SetPageTitle(page.Index ?? throw new Exception(), page.Title);
        }

        private void Pages_ItemInserted(object? sender, int index, TabPage item)
        {
            OnPageInserted(index, item);
        }

        private void Children_ItemInserted(object? sender, int index, Control item)
        {
            if (!skipChildrenInsertionCheck)
            {
                throw new InvalidOperationException(
                                    "Use TabControl.Pages instead of TabControl.Children.");
            }
        }

        private void Control_SelectedPageChanged(object sender, RoutedEventArgs e)
        {
            var index = Control.SelectedPage == null
                ? -1 : Control.Pages.IndexOf(Control.SelectedPage);
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

        private void Pages_ItemRemoved(object? sender, int index, TabPage item)
        {
            OnPageRemoved(index, item);
        }
    }
}