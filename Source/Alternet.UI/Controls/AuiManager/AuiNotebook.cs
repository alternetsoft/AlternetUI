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
    /// </summary>
    public class AuiNotebook : Control
    {
        public event EventHandler? PageClose;

        public event EventHandler? PageClosed;

        public event EventHandler? PageChanged;

        public event EventHandler? PageChanging;

        public event EventHandler? WindowListButton;

        public event EventHandler? BeginDrag;

        public event EventHandler? EndDrag;

        public event EventHandler? DragMotion;

        public event EventHandler? AllowTabDrop;

        public event EventHandler? DragDone;

        public event EventHandler? TabMiddleMouseDown;

        public event EventHandler? TabMiddleMouseUp;

        public event EventHandler? TabRightMouseDown;

        public event EventHandler? TabRightMouseUp;

        public event EventHandler? BgDclickMouse;

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

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateAuiNotebookHandler(this);
        }

        /*
Sets the art provider to be used by the notebook.         
         */
        internal void SetArtProvider(IntPtr art)
        {
            NativeControl.SetArtProvider(art);
        }

        /*
Returns the associated art provider.         
         */
        internal IntPtr GetArtProvider()
        {
            return NativeControl.GetArtProvider();
        }

        /*
Ensure that all tabs have the same height, even if some of them don't have bitmaps.
Passing wxDefaultSize as size undoes the effect of a previous call to this function and instructs the control to use dynamic tab height.         
         */
        public void SetUniformBitmapSize(int width, int height)
        {
            NativeControl.SetUniformBitmapSize(width, height);
        }

        /*
Sets the tab height.

By default, the tab control height is calculated by measuring the text height and bitmap sizes on the tab captions. Calling this method will override that calculation and set the tab control to the specified height parameter. A call to this method will override any call to SetUniformBitmapSize().

Specifying -1 as the height will return the control to its default auto-sizing behaviour.
         
         */
        public void SetTabCtrlHeight(int height)
        {
            NativeControl.SetTabCtrlHeight(height);
        }

        /*
Adds a page.

If the select parameter is true, calling this will generate a page change event.
Adds a new page.

The page must have the book control itself as the parent and must not have been added to this control previously.

The call to this function may generate the page changing events.

Parameters
page	Specifies the new page.
text	Specifies the text for the new page.
select	Specifies whether the page should be selected.
imageId	Specifies the optional image index for the new page.
Returns
true if successful, false otherwise.
Remarks
Do not delete the page, it will be deleted by the book control.
         
         */
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

        /*
Inserts a new page at the specified position.

Parameters
index	Specifies the position for the new page.
page	Specifies the new page.
text	Specifies the text for the new page.
select	Specifies whether the page should be selected.
imageId	Specifies the optional image index for the new page.
Returns
true if successful, false otherwise.
Remarks
Do not delete the page, it will be deleted by the book control.
InsertPage() is similar to AddPage, but allows the ability to specify the insert location.
If the select parameter is true, calling this will generate a page change event.
        
         */
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

        /*
Deletes a page at the given index.
Calling this method will generate a page change event.
         
         */
        public bool DeletePage(ulong page)
        {
            return NativeControl.DeletePage(page);
        }

        /*
Removes a page, without deleting the window pointer.         
         */
        public bool RemovePage(ulong page)
        {
            return NativeControl.RemovePage(page);
        }

        /*
Returns the number of pages in the notebook.         
         */
        public ulong GetPageCount()
        {
            return NativeControl.GetPageCount();
        }

        /*
Returns the page specified by the given index.         
         */
        internal IntPtr GetPage(ulong pageIdx)
        {
            return NativeControl.GetPage(pageIdx);
        }

        /*
Returns the page index for the specified window.
If the window is not found in the notebook, wxNOT_FOUND is returned.
         */
        public int FindPage(Control page)
        {
            return NativeControl.FindPage(page.WxWidget);
        }

        /*
Sets the tab label for the page.         
         */
        public bool SetPageText(ulong page, string text)
        {
            return NativeControl.SetPageText(page, text);
        }

        /*
Returns the tab label for the page.         
         */
        public string GetPageText(ulong pageIdx)
        {
            return NativeControl.GetPageText(pageIdx);
        }

        /*
Sets the tooltip displayed when hovering over the tab label of the page.

true if tooltip was updated, false if it failed, e.g. because the page index is invalid.
         
         */
        public bool SetPageToolTip(ulong page, string text)
        {
            return NativeControl.SetPageToolTip(page, text);
        }

        /*
Returns the tooltip for the tab label of the page.
        
         */
        public string GetPageToolTip(ulong pageIdx)
        {
            return NativeControl.GetPageToolTip(pageIdx);
        }

        /*
Sets the bitmap for the page.
To remove a bitmap from the tab caption, pass an empty wxBitmapBundle.
         */
        public bool SetPageBitmap(ulong page, ImageSet? bitmap)
        {
            return NativeControl.SetPageBitmap(page, bitmap?.NativeImageSet);
        }

        /*
Sets the page selection.
Calling this method will generate a page change event.
         
         */
        public long SetSelection(ulong newPage)
        {
            return NativeControl.SetSelection(newPage);
        }

        /*
Returns the currently selected page.
        
         */
        public long GetSelection()
        {
            return NativeControl.GetSelection();
        }

        /*
Split performs a split operation programmatically.
The argument page indicates the page that will be split off. This page will also become the active page after the split.
The direction argument specifies where the pane should go, it should be one of the following: wxTOP, wxBOTTOM, wxLEFT, or wxRIGHT.         
         */
        public void Split(ulong page, GenericDirection direction)
        {
            if (direction == GenericDirection.Top || direction == GenericDirection.Bottom
                || direction == GenericDirection.Right ||
                direction == GenericDirection.Left)
                NativeControl.Split(page, (int)direction);
            else
                throw new ArgumentException(nameof(direction));
        }

        // Gets the tab control height
        /*
Returns the height of the tab control.         
         */
        public int GetTabCtrlHeight()
        {
            return NativeControl.GetTabCtrlHeight();
        }

        // Gets the height of the notebook for a given page height
        /*
Returns the desired height of the notebook for the given page height.
Use this to fit the notebook to a given page size.
         
         */
        public int GetHeightForPageHeight(int pageHeight)
        {
            return NativeControl.GetHeightForPageHeight(pageHeight);
        }

        /*
Shows the window menu for the active tab control associated with this notebook, and returns true if a selection was made.
         
         */
        public bool ShowWindowMenu()
        {
            return NativeControl.ShowWindowMenu();
        }

        /*
Deletes all pages.         
         */
        public bool DeleteAllPages()
        {
            return NativeControl.DeleteAllPages();
        }

        /*
         Changes the selection for the given page, returning the previous selection.

This function behaves as SetSelection() but does not generate the page changing events.
 
         */
        public long ChangeSelection(ulong newPage)
        {
            return NativeControl.ChangeSelection(newPage);
        }

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
    }
}