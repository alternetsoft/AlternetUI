using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Notebook control, managing multiple windows with associated tabs.
    /// Used with <see cref="AuiManager"/>.
    /// </summary>
    [ControlCategory("Containers")]
    internal partial class AuiNotebook : Control
    {
        private readonly Collection<IAuiNotebookPage> pages = new();

        /// <summary>
        /// Occurs when a page is about to be closed.
        /// </summary>
        public event CancelEventHandler? PageClose;

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
        public event CancelEventHandler? PageChanging;

        /// <summary>
        /// Occurs when the page button has been pressed.
        /// </summary>
        public event CancelEventHandler? PageButton;

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
        public event EventHandler? BgDoubleClick;

        /// <summary>
        /// Defines default visual style for the newly created
        /// <see cref="AuiNotebook"/> controls.
        /// </summary>
        public static AuiNotebookCreateStyle DefaultCreateStyle { get; set; }
            = AuiNotebookCreateStyle.DefaultStyle;

        /// <summary>
        /// Defines visual style and behavior of the <see cref="AuiNotebook"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public AuiNotebookCreateStyle CreateStyle
        {
            get
            {
                return (AuiNotebookCreateStyle)Handler.CreateStyle;
            }

            set
            {
                Handler.CreateStyle = (int)value;
            }
        }

        /// <summary>
        /// Gets the collection of tab pages in this control.
        /// </summary>
        /// <value>A <see cref="ICollection{IAuiNotebookPage}"/> that contains
        /// the <see cref="IAuiNotebookPage"/>
        /// objects in this <see cref="AuiNotebook"/>.</value>
        /// <remarks>The order of tab pages in this collection reflects the order the tabs appear
        /// in the control.</remarks>
        [Browsable(false)]
        public IReadOnlyCollection<IAuiNotebookPage> Pages
        {
            get
            {
                return pages;
            }
        }

        /// <summary>
        /// Gets index of the currently selected page passed in the event handlers.
        /// </summary>
        /// <remarks>
        /// You can use it in the event handlers.
        /// </remarks>
        [Browsable(false)]
        public int EventSelection
        {
            get
            {
                return NativeControl.EventSelection;
            }
        }

        /// <summary>
        /// Gets index of the previously selected page passed in some event handlers.
        /// </summary>
        /// <remarks>
        /// You can use it in the event handlers.
        /// </remarks>
        [Browsable(false)]
        public int EventOldSelection
        {
            get
            {
                return NativeControl.EventOldSelection;
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Control> AllChildrenInLayout
            => Array.Empty<Control>();

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.AuiNotebook;

        internal new AuiNotebookHandler Handler
        {
            get
            {
                CheckDisposed();
                return (AuiNotebookHandler)base.Handler;
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
        /// <returns><see cref="IAuiNotebookPage"/> if successful, <c>null</c> otherwise.</returns>
        /// <remarks>
        /// If the <paramref name="select"/> parameter is true, calling
        /// this will generate a page change event.
        /// </remarks>
        /// <remarks>
        /// <see cref="InsertPage"/> is similar to <see cref="AddPage"/>, but allows
        /// the ability to specify the insert location.
        /// </remarks>
        public IAuiNotebookPage? AddPage(
            Control page,
            string caption,
            bool select = true,
            ImageSet? bitmap = null)
        {
            var ok = NativeControl.AddPage(
                WxPlatform.WxWidget(page),
                caption,
                select,
                (UI.Native.ImageSet?)bitmap?.Handler);

            if (ok)
            {
                IAuiNotebookPage result = new AuiNotebookPage(this)
                {
                    Index = (int)GetPageCount() - 1,
                };
                pages.Add(result);
                return result;
            }
            else
                return null;
        }

        /// <summary>
        /// Inserts a new page at the specified position.
        /// </summary>
        /// <param name="pageIdx">Position for the new page.</param>
        /// <param name="page">Controls for the new page.</param>
        /// <param name="caption">Text for the new page.</param>
        /// <param name="select">Specifies whether the page should be selected.</param>
        /// <param name="bitmap">image for the new page. Optional.</param>
        /// <returns><see cref="IAuiNotebookPage"/> if successful, <c>null</c> otherwise.</returns>
        /// <remarks>
        /// If the <paramref name="select"/> parameter is true, calling
        /// this will generate a page change event.
        /// </remarks>
        /// <remarks>
        /// <see cref="InsertPage"/> is similar to <see cref="AddPage"/>, but allows
        /// the ability to specify the insert location.
        /// </remarks>
        public IAuiNotebookPage? InsertPage(
            int pageIdx,
            Control page,
            string caption,
            bool select = true,
            ImageSet? bitmap = null)
        {
            var ok = NativeControl.InsertPage(
                (ulong)pageIdx,
                WxPlatform.WxWidget(page),
                caption,
                select,
                (UI.Native.ImageSet?)bitmap?.Handler);
            if (ok)
            {
                IAuiNotebookPage result = new AuiNotebookPage(this)
                {
                    Index = pageIdx,
                };
                pages.Insert(pageIdx, result);
                for(int i = pageIdx + 1; i < pages.Count; i++)
                {
                    (pages[i] as AuiNotebookPage)!.Index = pages[i].Index + 1;
                }

                return result;
            }
            else
                return null;
        }

        /// <summary>
        /// Removes a page, without deleting the attached control.
        /// </summary>
        /// <param name="page">Page index</param>
        /// <returns><c>true</c> if operation was successfull,
        /// <c>false</c> otherwise.</returns>
        public bool RemovePage(int page)
        {
            if (page < 0 || page >= pages.Count)
                return false;
            var ok = NativeControl.RemovePage((ulong)page);
            if (ok)
            {
                pages.RemoveAt(page);
                for (int i = page; i < pages.Count; i++)
                {
                    (pages[i] as AuiNotebookPage)!.Index = pages[i].Index - 1;
                }
            }

            return ok;
        }

        /// <summary>
        /// Returns the number of pages in the notebook.
        /// </summary>
        public int GetPageCount()
        {
            return (int)NativeControl.GetPageCount();
        }

        /// <summary>
        /// Returns the page index for the specified control.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <remarks>
        /// If the window is not found in the notebook, <c>null</c> is returned.
        /// </remarks>
        public int? FindPage(Control? page)
        {
            if (page is null)
                return null;
            var result = NativeControl.FindPage(WxPlatform.WxWidget(page));
            if (result < 0)
                return null;
            return result;
        }

        /// <summary>
        /// Sets the tab label for the page.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="text">New tab label.</param>
        /// <returns></returns>
        public bool SetPageText(int page, string text)
        {
            return NativeControl.SetPageText((ulong)page, text);
        }

        /// <summary>
        /// Returns the tab label for the page.
        /// </summary>
        /// <param name="pageIdx">Page index.</param>
        public string GetPageText(int pageIdx)
        {
            return NativeControl.GetPageText((ulong)pageIdx);
        }

        /// <summary>
        /// Sets the tooltip displayed when hovering over the tab label of the page.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="text">New tooltip value.</param>
        /// <returns><c>true</c> if tooltip was updated, <c>false</c> if it failed,
        /// e.g. because the page index is invalid.</returns>
        public bool SetPageToolTip(int page, string text)
        {
            return NativeControl.SetPageToolTip((ulong)page, text);
        }

        /// <summary>
        /// Returns the tooltip for the tab label of the page.
        /// </summary>
        /// <param name="pageIdx">Page index.</param>
        public string GetPageToolTip(int pageIdx)
        {
            return NativeControl.GetPageToolTip((ulong)pageIdx);
        }

        /// <summary>
        /// Sets the bitmap for the page.
        /// </summary>
        /// <param name="page">Page index.</param>
        /// <param name="imageSet">Bitmap for the page.</param>
        /// <remarks>
        /// To remove a bitmap from the tab caption, pass <c>null</c>.
        /// </remarks>
        public bool SetPageBitmap(int page, ImageSet? imageSet)
        {
            return NativeControl.SetPageBitmap((ulong)page, (UI.Native.ImageSet?)imageSet?.Handler);
        }

        /// <summary>
        /// Sets the page selection.
        /// </summary>
        /// <param name="newPage">New selected page index.</param>
        /// <remarks>
        /// Calling this method will generate a page change event.
        /// </remarks>
        public int SetSelection(int newPage)
        {
            return (int)NativeControl.SetSelection((ulong)newPage);
        }

        /// <summary>
        /// Returns the currently selected page index.
        /// </summary>
        /// <returns></returns>
        public int? GetSelection()
        {
            if (GetPageCount() == 0)
                return null;
            return (int)NativeControl.GetSelection();
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
        public void Split(int page, GenericDirection direction)
        {
            if (direction == GenericDirection.Top || direction == GenericDirection.Bottom
                || direction == GenericDirection.Right ||
                direction == GenericDirection.Left)
                NativeControl.Split((ulong)page, (int)direction);
            else
                throw new ArgumentException(null, nameof(direction));
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
        public int? ChangeSelection(int? newPage)
        {
            if (newPage is null || GetPageCount() == 0)
                return null;
            return (int)NativeControl.ChangeSelection((ulong)newPage.Value);
        }

        /// <summary>
        /// Changes selected page to the page which contains <paramref name="control"/>.
        /// </summary>
        /// <param name="control">Control to search for.</param>
        /// <returns></returns>
        public int? ChangeSelection(Control? control)
        {
            return ChangeSelection(FindPage(control));
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
        public void SetMeasuringFont(Font? font = null)
        {
            if (font == null)
                font = Font.Default;
            NativeControl.SetMeasuringFont((UI.Native.Font)font.Handler);
        }

        /// <inheritdoc/>
        public override void OnLayout()
        {
            if (!HasChildren)
                return;
            var children = Children;

            for (int i = children.Count - 1; i >= 0; i--)
            {
                Control child = children[i];
                if (!child.Visible)
                    continue;

                child.OnLayout();
            }
        }

        /// <summary>
        /// Sets the font for drawing unselected tab labels.
        /// </summary>
        /// <param name="font">New font value.</param>
        public void SetNormalFont(Font? font = null)
        {
            if (font == null)
                font = Font.Default;
            NativeControl.SetNormalFont((UI.Native.Font)font.Handler);
        }

        /// <summary>
        /// Sets the font for drawing selected tab labels.
        /// </summary>
        /// <param name="font">New font value.</param>
        public void SetSelectedFont(Font? font = null)
        {
            if (font == null)
                font = Font.Default;
            NativeControl.SetSelectedFont((UI.Native.Font)font.Handler);
        }

        /// <summary>
        /// Returns the page specified by the given index.
        /// </summary>
        /// <param name="pageIdx">Page index.</param>
        internal IntPtr GetPage(int pageIdx)
        {
            return NativeControl.GetPage((ulong)pageIdx);
        }

        /// <summary>
        /// Sets the art provider to be used by the notebook.
        /// </summary>
        /// <param name="art">New art provider.</param>
        internal void SetArtProvider(IntPtr art)
        {
            NativeControl.SetArtProvider(art);
        }

        internal void RaisePageClose(CancelEventArgs e)
        {
            OnPageClose(e);
            if (e.Cancel)
                return;
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

        internal void RaisePageChanging(CancelEventArgs e)
        {
            OnPageChanging(e);
            if (e.Cancel)
                return;
            PageChanging?.Invoke(this, e);
        }

        internal void RaisePageButton(CancelEventArgs e)
        {
            OnPageButton(e);
            if (e.Cancel)
                return;
            PageButton?.Invoke(this, e);
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

        internal void RaiseBgDoubleClick(EventArgs e)
        {
            OnBgDoubleClick(e);
            BgDoubleClick?.Invoke(this, e);
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
        internal bool DeletePage(int page)
        {
            throw new NotImplementedException();
            /*return NativeControl.DeletePage((ulong)page);*/
        }

        /// <summary>
        /// Returns the associated art provider.
        /// </summary>
        internal IntPtr GetArtProvider()
        {
            return NativeControl.GetArtProvider();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return new AuiNotebookHandler();
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
        protected virtual void OnPageButton(EventArgs e)
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
        protected virtual void OnBgDoubleClick(EventArgs e)
        {
        }
    }
}