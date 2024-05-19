using System;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides functionality for implementing
    /// a specific <see cref="WxTabControl"/> behavior and appearance.
    /// </summary>
    internal class WxTabControlHandler : WxControlHandler
    {
        private bool skipChildrenInsertionCheck;
        /*private bool skipLinuxFix;*/

        /// <summary>
        /// Gets a <see cref="WxTabControl"/> this handler provides the implementation for.
        /// </summary>
        public new WxTabControl Control => (WxTabControl)base.Control;

        /// <summary>
        /// Gets or sets the area of the control (for example, along the top) where the tabs are aligned.
        /// </summary>
        public TabAlignment TabAlignment
        {
            get => NativeControl.TabAlignment;
            set => NativeControl.TabAlignment = value;
        }

        internal new Native.TabControl NativeControl =>
            (Native.TabControl)base.NativeControl!;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.TabControl();
        }

        internal void LayoutSelectedPage()
        {
            var selectedPageIndex = NativeControl.SelectedPageIndex;
            if (selectedPageIndex >= 0)
            {
                var page = Control.Pages[selectedPageIndex];

                // if (Application.IsLinuxOS && selectedPageIndex == 0 && !skipLinuxFix)
                /*{
                    page.Handler.Bounds = Control.ChildrenLayoutBounds;
                }*/

                page.OnLayout();
            }
        }

        protected virtual void OnPageRemoved(int index, TabPage page)
        {
            page.TitleChanged -= Page_TitleChanged;
            RemovePage(index, page);
            TabPage.SetPageIndexInternal(page, null);
            TabPage.UpdatePageIndexes(Control.Pages, index);
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            Control.Pages.ItemInserted += Pages_ItemInserted;
            Control.Pages.ItemRemoved += Pages_ItemRemoved;
            Control.Children.ItemInserted += Children_ItemInserted;

            NativeControl.SelectedPageIndexChanged = NativeControl_SelectedPageIndexChanged;
        }

        protected virtual void OnPageInserted(int index, TabPage page)
        {
            TabPage.SetPageIndexInternal(page, index);
            InsertPage(index, page);
            page.TitleChanged += Page_TitleChanged;
            TabPage.UpdatePageIndexes(Control.Pages, index + 1);
        }

        protected override void OnDetach()
        {
            NativeControl.SelectedPageIndexChanged = null;
            Control.Children.ItemInserted -= Children_ItemInserted;

            Control.Pages.ItemInserted -= Pages_ItemInserted;
            Control.Pages.ItemRemoved -= Pages_ItemRemoved;

            base.OnDetach();
        }

        protected override void NativeControlSizeChanged()
        {
            base.NativeControlSizeChanged();
            LayoutSelectedPage();
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

            var pageNativeControl = (page.Handler as WxControlHandler)?.NativeControl
                ?? throw new InvalidOperationException();
            NativeControl.InsertPage(index, pageNativeControl, page.Title);
            Control.PerformLayout();
        }

        private void RemovePage(int index, TabPage page)
        {
            var pageNativeControl = (page.Handler as WxControlHandler)?.NativeControl ??
                throw new InvalidOperationException();
            NativeControl.RemovePage(index, pageNativeControl);
            Control.Children.RemoveAt(index);
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

        private void NativeControl_SelectedPageIndexChanged()
        {
            LayoutSelectedPage();
            Control.RaiseSelectedPageChanged(EventArgs.Empty);
        }

        private void Pages_ItemRemoved(object? sender, int index, TabPage item)
        {
            OnPageRemoved(index, item);
        }
    }
}