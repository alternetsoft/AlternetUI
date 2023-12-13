#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{

    //https://docs.wxwidgets.org/3.2/classwx_scroll_bar.html
    public class ScrollBar : Control
    {
        // Returns true for scrollbars that have the vertical style set.
        public bool IsVertical { get; set; }

        // Process scroll to top or leftmost (minimum) position events.
        public event EventHandler? ScrollTop;

        // Process scroll to bottom or rightmost (maximum) position events.
        public event EventHandler? ScrollBottom;

        // Process scroll line up or left events.
        public event EventHandler? ScrollLineUp;

        // Process scroll line down or right events.
        public event EventHandler? ScrollLineDown;

        // Process scroll page up or left events.
        public event EventHandler? ScrollPageUp;

        // Process scroll page down or right events.
        public event EventHandler? ScrollPageDown;

        // Process scroll thumbtrack events (frequent events sent
        // as the user drags the thumbtrack).
        public event EventHandler? ScrollThumbTrack;

        // Process scroll thumb release events.
        public event EventHandler? ScrollThumbRelease;

        // Process wxEVT_SCROLL_CHANGED end of scrolling events(MSW only).
        // public event EventHandler? ScrollChanged;

        // Gets or sets the position of the scrollbar thumb.
        public int ThumbPosition { get; set; }

        // Returns the length of the scrollbar.
        public int Range { get; }

        // Returns the thumb or 'view' size.
        public int ThumbSize { get; }

        // Returns the page size of the scrollbar.
        // This is the number of scroll units that will be scrolled when the user
        // pages up or down. Often it is the same as the thumb size.
        public int PageSize { get; }

        // Sets the scrollbar properties.
        // position - The position of the scrollbar in scroll units.
        // thumbSize - The size of the thumb, or visible portion of the scrollbar, in scroll units.
        // range - The maximum position of the scrollbar.
        // pageSize - The size of the page size in scroll units. This is the number of units the scrollbar will scroll when it is paged up or down. Often it is the same as the thumb size.
        // refresh	- true to redraw the scrollbar, false otherwise.
        // Let's say you wish to display 50 lines of text, using the same font. The window is sized so that you can only see 16 lines at a time. You would use:
        // scrollbar->SetScrollbar(0, 16, 50, 15);
        // The page size is 1 less than the thumb size so that the last line of the previous page will be visible on the next page, to help orient the user. Note that with the window at this size, the thumb position can never go above 50 minus 16, or 34. You can determine how many lines are currently visible by dividing the current view size by the character height in pixels. When defining your own scrollbar behaviour, you will always need to recalculate the scrollbar settings when the window size changes. You could therefore put your scrollbar calculations and SetScrollbar() call into a function named AdjustScrollbars, which can be called initially and also from a wxSizeEvent event handler function.
        // Reimplemented from wxWindow.
        public void SetScrollbar(int position, int thumbSize, int range,
            int pageSize, bool refresh = true)
        { }
    }
}