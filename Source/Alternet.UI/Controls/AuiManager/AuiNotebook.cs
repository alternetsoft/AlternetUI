using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Notebook control, managing multiple windows with associated tabs.
    /// Used with <see cref="AuiManager"/>.
    /// </summary>
    public class AuiNotebook : Control
    {
        /// <summary>
        /// Occurs when a page is about to be closed.
        /// </summary>
        public event EventHandler? PageClose;

        /// <summary>
        /// Occurs when a page has been closed.
        /// </summary>
        public event EventHandler? PageClosed;

        /// <summary>
        /// Occurs when the page selection was changed.
        /// </summary>
        public event EventHandler? PageChanged;

        /// <summary>
        /// Occurs when the page selection is about to be changed.
        /// This event can be vetoed.
        /// </summary>
        public event EventHandler? PageChanging;

        /// <summary>
        /// Occurs when the window list button has been pressed.
        /// </summary>
        public event EventHandler? WindowListButton;

        /// <summary>
        /// Occurs when dragging is about to begin.
        /// </summary>
        public event EventHandler? BeginDrag;

        /// <summary>
        /// Occurs when dragging has ended.
        /// </summary>
        public event EventHandler? EndDrag;

        /// <summary>
        /// Occurs during a drag and drop operation.
        /// </summary>
        public event EventHandler? DragMotion;

        /// <summary>
        /// Occurs when application needs to query whether to allow a tab
        /// to be dropped. This event must be specially allowed.
        /// </summary>
        public event EventHandler? AllowTabDrop;

        /// <summary>
        /// Occurs to notify that the tab has been dragged.
        /// </summary>
        public event EventHandler? DragDone;

        /// <summary>
        /// Occurs when the middle mouse button is pressed on a tab.
        /// </summary>
        public event EventHandler? TabMiddleMouseDown;

        /// <summary>
        /// Occurs when the middle mouse button is released on a tab.
        /// </summary>
        public event EventHandler? TabMiddleMouseUp;

        /// <summary>
        /// Occurs when the right mouse button is pressed on a tab.
        /// </summary>
        public event EventHandler? TabRightMouseDown;

        /// <summary>
        /// Occurs when the right mouse button is released on a tab.
        /// </summary>
        public event EventHandler? TabRightMouseUp;

        /// <summary>
        /// Occurs when the mouse button is double clicked on the tabs
        /// background area.
        /// </summary>
        public event EventHandler? BgDclickMouse;

        public int EventSelection
        {
            get
            {
                return NativeControl.EventSelection;
            }
        }

        public int EventOldSelection
        {
            get
            {
                return NativeControl.EventOldSelection;
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.AuiNotebook;

        internal new NativeAuiNotebookHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeAuiNotebookHandler)base.Handler;
            }
        }

        internal Native.AuiNotebook NativeControl => Handler.NativeControl;

        /// <summary>
        /// Ensures that all tabs have the same height, even if some of them
        /// don't have bitmaps.
        /// </summary>
        /// <param name="width">Bitmap width for tabs without images.</param>
        /// <param name="height">Bitmap height for tabs without images.</param>
        /// <remarks>
        /// Passing (-1,-1) as size undoes the effect of a previous call to this
        /// function and instructs the control to use dynamic tab height.
        /// </remarks>
        public void SetUniformBitmapSize(int width, int height)
        {
            NativeControl.SetUniformBitmapSize(width, height);
        }

        /// <summary>
        /// Sets the tab height.
        /// </summary>
        /// <param name="height">New tab height value.</param>
        /// <remarks>
        /// By default, the tab control height is calculated by measuring the
        /// text height and bitmap sizes on the tab captions. Calling this
        /// method will override that calculation and set the tab control
        /// to the specified height parameter. A call to this method will
        /// override any call to <see cref="SetUniformBitmapSize"/>.
        /// </remarks>
        /// <remarks>
        /// Specifying -1 as the height will return the control to its
        /// default auto-sizing behaviour.
        /// </remarks>
        public void SetTabCtrlHeight(int height)
        {
            NativeControl.SetTabCtrlHeight(height);
        }

        /// <summary>
        /// Adds a new page.
        /// </summary>
        /// <param name="page">Controls for the new page.</param>
        /// <param name="caption">Text for the new page.</param>
        /// <param name="select">Specifies whether the page should be selected.</param>
        /// <param name="bitmap">image for the new page. Optional.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// If the <paramref name="select"/> parameter is true, calling
        /// this will generate a page change event.
        /// </remarks>
        /// <remarks>
        /// <see cref="InsertPage"/> is similar to <see cref="AddPage"/>, but allows
        /// the ability to specify the insert location.
        /// </remarks>
        public bool AddPage(
            Control page,
            string caption,
            bool select = true,
            ImageSet? bitmap = null)
        {
            return NativeControl.AddPage(
                page.WxWidget,
                caption,
                select,
                bitmap?.NativeImageSet);
        }

        /// <summary>
        /// Inserts a new page at the specified position.
        /// </summary>
        /// <param name="pageIdx">Position for the new page.</param>
        /// <param name="page">Controls for the new page.</param>
        /// <param name="caption">Text for the new page.</param>
        /// <param name="select">Specifies whether the page should be selected.</param>
        /// <param name="bitmap">image for the new page. Optional.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// If the <paramref name="select"/> parameter is true, calling
        /// this will generate a page change event.
        /// </remarks>
        /// <remarks>
        /// <see cref="InsertPage"/> is similar to <see cref="AddPage"/>, but allows
        /// the ability to specify the insert location.
        /// </remarks>
        public bool InsertPage(
            ulong pageIdx,
            Control page,
            string caption,
            bool select = true,
            ImageSet? bitmap = null)
        {
            return NativeControl.InsertPage(
                pageIdx,
                page.WxWidget,
                caption,
                select,
                bitmap?.NativeImageSet);
        }

        /// <summary>
        /// Deletes a page at the given index.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <returns><c>true</c> if operation was successfull,
        /// <c>false</c> otherwise.</returns>
        /// <remarks>
        /// Calling this method will generate a page change event.
        /// </remarks>
        public bool DeletePage(ulong page)
        {
            return NativeControl.DeletePage(page);
        }

        /// <summary>
        /// Removes a page, without deleting the attached control.
        /// </summary>
        /// <param name="page">Page index</param>
        /// <returns><c>true</c> if operation was successfull,
        /// <c>false</c> otherwise.</returns>
        public bool RemovePage(ulong page)
        {
            return NativeControl.RemovePage(page);
        }

        /// <summary>
        /// Returns the number of pages in the notebook.
        /// </summary>
        public ulong GetPageCount()
        {
            return NativeControl.GetPageCount();
        }

        /// <summary>
        /// Returns the page index for the specified control.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <remarks>
        /// If the window is not found in the notebook, -1 is returned.
        /// </remarks>
        public int FindPage(Control page)
        {
            return NativeControl.FindPage(page.WxWidget);
        }

        /// <summary>
        /// Sets the tab label for the page.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="text">New tab label.</param>
        /// <returns></returns>
        public bool SetPageText(ulong page, string text)
        {
            return NativeControl.SetPageText(page, text);
        }

        /// <summary>
        /// Returns the tab label for the page.
        /// </summary>
        /// <param name="pageIdx">Page index.</param>
        public string GetPageText(ulong pageIdx)
        {
            return NativeControl.GetPageText(pageIdx);
        }

        /// <summary>
        /// Sets the tooltip displayed when hovering over the tab label of the page.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="text">New tooltip value.</param>
        /// <returns><c>true</c> if tooltip was updated, <c>false</c> if it failed,
        /// e.g. because the page index is invalid.</returns>
        public bool SetPageToolTip(ulong page, string text)
        {
            return NativeControl.SetPageToolTip(page, text);
        }

        /// <summary>
        /// Returns the tooltip for the tab label of the page.
        /// </summary>
        /// <param name="pageIdx">Page index.</param>
        public string GetPageToolTip(ulong pageIdx)
        {
            return NativeControl.GetPageToolTip(pageIdx);
        }

        /// <summary>
        /// Sets the bitmap for the page.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="bitmap">Bitmap for the page.</param>
        /// <remarks>
        /// To remove a bitmap from the tab caption, pass <c>null</c>.
        /// </remarks>
        public bool SetPageBitmap(ulong page, ImageSet? bitmap)
        {
            return NativeControl.SetPageBitmap(page, bitmap?.NativeImageSet);
        }

        /// <summary>
        /// Sets the page selection.
        /// </summary>
        /// <param name="newPage">New selected page index.</param>
        /// <remarks>
        /// Calling this method will generate a page change event.
        /// </remarks>
        public long SetSelection(ulong newPage)
        {
            return NativeControl.SetSelection(newPage);
        }

        /// <summary>
        /// Returns the currently selected page index.
        /// </summary>
        /// <returns></returns>
        public long GetSelection()
        {
            return NativeControl.GetSelection();
        }

        /// <summary>
        /// Performs a split operation programmatically.
        /// </summary>
        /// <param name="page">Indicates the page that will be split off.
        /// This page will also become the active page after the split.</param>
        /// <param name="direction">Specifies where the pane should go, it should
        /// be one of the following: Top, Bottom, Left, or Right.</param>
        /// <exception cref="ArgumentException"><paramref name="direction"/>
        /// is incorrect.</exception>
        public void Split(ulong page, GenericDirection direction)
        {
            if (direction == GenericDirection.Top || direction == GenericDirection.Bottom
                || direction == GenericDirection.Right ||
                direction == GenericDirection.Left)
                NativeControl.Split(page, (int)direction);
            else
                throw new ArgumentException(nameof(direction));
        }

        /// <summary>
        /// Returns the height of the tab control.
        /// </summary>
        public int GetTabCtrlHeight()
        {
            return NativeControl.GetTabCtrlHeight();
        }

        /// <summary>
        /// Gets the height of the notebook for a given page height.
        /// </summary>
        /// <param name="pageHeight">Page height.</param>
        /// <returns>The desired height of the notebook for the given
        /// page height.</returns>
        /// <remarks>
        /// Use this to fit the notebook to a given page size.
        /// </remarks>
        public int GetHeightForPageHeight(int pageHeight)
        {
            return NativeControl.GetHeightForPageHeight(pageHeight);
        }

        /// <summary>
        /// Shows the window menu for the active tab control associated with
        /// this notebook, and returns true if a selection was made.
        /// </summary>
        /// <returns></returns>
        public bool ShowWindowMenu()
        {
            return NativeControl.ShowWindowMenu();
        }

        /// <summary>
        /// Deletes all pages.
        /// </summary>
        /// <returns><c>true</c> if operation was performed successfully.</returns>
        public bool Clear()
        {
            return NativeControl.DeleteAllPages();
        }

        /// <summary>
        /// Changes the selection for the given page, returning the previous selection.
        /// </summary>
        /// <param name="newPage">New selected page index.</param>
        /// <remarks>
        /// This function behaves as <see cref="SetSelection"/> but does not
        /// generate the page changing events.
        /// </remarks>
        public long ChangeSelection(ulong newPage)
        {
            return NativeControl.ChangeSelection(newPage);
        }

        /// <summary>
        /// Selects next or previous page.
        /// </summary>
        /// <param name="forward">If <c>true</c> selects next page, if
        /// <c>false</c> selects previous page.</param>
        public void AdvanceSelection(bool forward = true)
        {
            NativeControl.AdvanceSelection(forward);
        }

        /// <summary>
        /// Sets the font for measuring tab labels.
        /// </summary>
        /// <param name="font">New font value.</param>
        public void SetMeasuringFont(Font? font)
        {
            if (font == null)
                font = Font.Default;
            NativeControl.SetMeasuringFont(font.NativeFont);
        }

        /// <summary>
        /// Sets the font for drawing unselected tab labels.
        /// </summary>
        /// <param name="font">New font value.</param>
        public void SetNormalFont(Font? font)
        {
            if (font == null)
                font = Font.Default;
            NativeControl.SetNormalFont(font.NativeFont);
        }

        /// <summary>
        /// Sets the font for drawing selected tab labels.
        /// </summary>
        /// <param name="font">New font value.</param>
        public void SetSelectedFont(Font? font)
        {
            if (font == null)
                font = Font.Default;
            NativeControl.SetSelectedFont(font.NativeFont);
        }

        /// <summary>
        /// Returns the page specified by the given index.
        /// </summary>
        /// <param name="pageIdx">Page index.</param>
        internal IntPtr GetPage(ulong pageIdx)
        {
            return NativeControl.GetPage(pageIdx);
        }

        /// <summary>
        /// Sets the art provider to be used by the notebook.
        /// </summary>
        /// <param name="art">New art provider.</param>
        internal void SetArtProvider(IntPtr art)
        {
            NativeControl.SetArtProvider(art);
        }

        internal void RaisePageClose(EventArgs e)
        {
            OnPageClose(e);
            PageClose?.Invoke(this, e);
        }

        internal void RaisePageClosed(EventArgs e)
        {
            OnPageClosed(e);
            PageClosed?.Invoke(this, e);
        }

        internal void RaisePageChanged(EventArgs e)
        {
            OnPageChanged(e);
            PageChanged?.Invoke(this, e);
        }

        internal void RaisePageChanging(EventArgs e)
        {
            OnPageChanging(e);
            PageChanging?.Invoke(this, e);
        }

        internal void RaiseWindowListButton(EventArgs e)
        {
            OnWindowListButton(e);
            WindowListButton?.Invoke(this, e);
        }

        internal void RaiseBeginDrag(EventArgs e)
        {
            OnBeginDrag(e);
            BeginDrag?.Invoke(this, e);
        }

        internal void RaiseEndDrag(EventArgs e)
        {
            OnEndDrag(e);
            EndDrag?.Invoke(this, e);
        }

        internal void RaiseDragMotion(EventArgs e)
        {
            OnDragMotion(e);
            DragMotion?.Invoke(this, e);
        }

        internal void RaiseAllowTabDrop(EventArgs e)
        {
            OnAllowTabDrop(e);
            AllowTabDrop?.Invoke(this, e);
        }

        internal void RaiseDragDone(EventArgs e)
        {
            OnDragDone(e);
            DragDone?.Invoke(this, e);
        }

        internal void RaiseTabMiddleMouseDown(EventArgs e)
        {
            OnTabMiddleMouseDown(e);
            TabMiddleMouseDown?.Invoke(this, e);
        }

        internal void RaiseTabMiddleMouseUp(EventArgs e)
        {
            OnTabMiddleMouseUp(e);
            TabMiddleMouseUp?.Invoke(this, e);
        }

        internal void RaiseTabRightMouseDown(EventArgs e)
        {
            OnTabRightMouseDown(e);
            TabRightMouseDown?.Invoke(this, e);
        }

        internal void RaiseTabRightMouseUp(EventArgs e)
        {
            OnTabRightMouseUp(e);
            TabRightMouseUp?.Invoke(this, e);
        }

        internal void RaiseBgDclickMouse(EventArgs e)
        {
            OnBgDclickMouse(e);
            BgDclickMouse?.Invoke(this, e);
        }

        /// <summary>
        /// Returns the associated art provider.
        /// </summary>
        internal IntPtr GetArtProvider()
        {
            return NativeControl.GetArtProvider();
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateAuiNotebookHandler(this);
        }

        /// <summary>
        /// Called when a page is about to be closed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPageClose(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when a page has been closed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPageClosed(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the page selection was changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPageChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the page selection is about to be changed.
        /// This event can be vetoed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPageChanging(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the window list button has been pressed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnWindowListButton(EventArgs e)
        {
        }

        /// <summary>
        /// Called when dragging is about to begin.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnBeginDrag(EventArgs e)
        {
        }

        /// <summary>
        /// Called when dragging has ended.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnEndDrag(EventArgs e)
        {
        }

        /// <summary>
        /// Called during a drag and drop operation.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnDragMotion(EventArgs e)
        {
        }

        /// <summary>
        /// Occurs when application needs to query whether to allow a tab
        /// to be dropped. This event must be specially allowed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnAllowTabDrop(EventArgs e)
        {
        }

        /// <summary>
        /// Called to notify that the tab has been dragged.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnDragDone(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the middle mouse button is pressed on a tab.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnTabMiddleMouseDown(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the middle mouse button is released on a tab.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnTabMiddleMouseUp(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the right mouse button is pressed on a tab.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnTabRightMouseDown(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the right mouse button is released on a tab.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnTabRightMouseUp(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the mouse button is double clicked on the tabs
        /// background area.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnBgDclickMouse(EventArgs e)
        {
        }
    }
}