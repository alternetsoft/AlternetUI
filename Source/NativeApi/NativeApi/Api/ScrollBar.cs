#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{

    /*
    A ScrollBar is a control that represents a horizontal or vertical scrollbar.

    It is distinct from the two scrollbars that some windows provide automatically,
    but the two types of scrollbar share the way events are received.

    Remarks
    A scrollbar has the following main attributes: range, thumb size,
    page size, and position. The range is the total number of units
    associated with the view represented by the scrollbar. For a table
    with 15 columns, the range would be 15. The thumb size is the number of units
    that are currently visible. For the table example, the window might be sized
    so that only 5 columns are currently visible, in which case the application
    would set the thumb size to 5. When the thumb size becomes the same as or
    greater than the range, the scrollbar will be automatically hidden on
    most platforms. The page size is the number of units that the scrollbar
    should scroll by, when 'paging' through the data. This value is normally the
    same as the thumb size length, because it is natural to assume that the visible
    window size defines a page. The scrollbar position is the current thumb position.
    Most applications will find it convenient to provide a function called
    AdjustScrollbars() which can be called initially, from an OnSize event
    handler, and whenever the application data changes in size. It will adjust
    the view, object and page size according to the size of the window and the size of the data. 

     */
    //https://docs.wxwidgets.org/3.2/classwx_scroll_bar.html
    public class ScrollBar : Control
    {
        public bool IsVertical { get; set; }


        /*
        EVT_SCROLL(func):
        Process all scroll events.
        
        EVT_SCROLL_TOP(func):
        Process wxEVT_SCROLL_TOP scroll to top or leftmost (minimum) position events.
        
        EVT_SCROLL_BOTTOM(func):
        Process wxEVT_SCROLL_BOTTOM scroll to bottom or rightmost (maximum) position events.
        
        EVT_SCROLL_LINEUP(func):
        Process wxEVT_SCROLL_LINEUP line up or left events.
        
        EVT_SCROLL_LINEDOWN(func):
        Process wxEVT_SCROLL_LINEDOWN line down or right events.
        
        EVT_SCROLL_PAGEUP(func):
        Process wxEVT_SCROLL_PAGEUP page up or left events.
        
        EVT_SCROLL_PAGEDOWN(func):
        Process wxEVT_SCROLL_PAGEDOWN page down or right events.
        
        EVT_SCROLL_THUMBTRACK(func):
        Process wxEVT_SCROLL_THUMBTRACK thumbtrack events (frequent events sent
        as the user drags the thumbtrack).
        
        EVT_SCROLL_THUMBRELEASE(func):
        Process wxEVT_SCROLL_THUMBRELEASE thumb release events.
        
        EVT_SCROLL_CHANGED(func):
        Process wxEVT_SCROLL_CHANGED end of scrolling events (MSW only).
        
        EVT_COMMAND_SCROLL(id, func):
        Process all scroll events.
        
        EVT_COMMAND_SCROLL_TOP(id, func):
        Process wxEVT_SCROLL_TOP scroll to top or leftmost (minimum) position events.
        
        EVT_COMMAND_SCROLL_BOTTOM(id, func):
        Process wxEVT_SCROLL_BOTTOM scroll to bottom or rightmost (maximum) position events.
        
        EVT_COMMAND_SCROLL_LINEUP(id, func):
        Process wxEVT_SCROLL_LINEUP line up or left events.
        
        EVT_COMMAND_SCROLL_LINEDOWN(id, func):
        Process wxEVT_SCROLL_LINEDOWN line down or right events.
        
        EVT_COMMAND_SCROLL_PAGEUP(id, func):
        Process wxEVT_SCROLL_PAGEUP page up or left events.
        
        EVT_COMMAND_SCROLL_PAGEDOWN(id, func):
        Process wxEVT_SCROLL_PAGEDOWN page down or right events.
        
        EVT_COMMAND_SCROLL_THUMBTRACK(id, func):
        Process wxEVT_SCROLL_THUMBTRACK thumbtrack events (frequent events sent as
        the user drags the thumbtrack).
        
        EVT_COMMAND_SCROLL_THUMBRELEASE(func):
        Process wxEVT_SCROLL_THUMBRELEASE thumb release events.
        
        EVT_COMMAND_SCROLL_CHANGED(func):
        Process wxEVT_SCROLL_CHANGED end of scrolling events (MSW only). 

         */

        /*
        The difference between EVT_SCROLL_THUMBRELEASE and EVT_SCROLL_CHANGED
        The EVT_SCROLL_THUMBRELEASE event is only emitted when actually dragging
        the thumb using the mouse and releasing it (This EVT_SCROLL_THUMBRELEASE
        event is also followed by an EVT_SCROLL_CHANGED event).

        The EVT_SCROLL_CHANGED event also occurs when using the keyboard to change
        the thumb position, and when clicking next to the thumb (In all these
        cases the EVT_SCROLL_THUMBRELEASE event does not happen).

        In short, the EVT_SCROLL_CHANGED event is triggered when scrolling/moving
        has finished independently of the way it had started. Please see the
        Widgets Sample ("Slider" page) to see the difference between
        EVT_SCROLL_THUMBRELEASE and EVT_SCROLL_CHANGED in action. 
         */

        /*
        wxScrollBar() [1/2]
        wxScrollBar::wxScrollBar	(		)	
        Default constructor.

        wxScrollBar() [2/2]
        wxScrollBar::wxScrollBar	(	wxWindow * 	parent,
        wxWindowID 	id,
        const wxPoint & 	pos = wxDefaultPosition,
        const wxSize & 	size = wxDefaultSize,
        long 	style = wxSB_HORIZONTAL,
        const wxValidator & 	validator = wxDefaultValidator,
        const wxString & 	name = wxScrollBarNameStr 
        )		
        Constructor, creating and showing a scrollbar.

        Parameters
        parent	Parent window. Must be non-NULL.
        id	Window identifier. The value wxID_ANY indicates a default value.
        pos	Window position. If wxDefaultPosition is specified then a default position is chosen.
        size	Window size. If wxDefaultSize is specified then a default size is chosen.
        style	Window style. See wxScrollBar.
        validator	Window validator.
        name	Window name.
        See also
        Create(), wxValidator
        ~wxScrollBar()
        virtual wxScrollBar::~wxScrollBar	(		)	
        virtual
        Destructor, destroying the scrollbar.

        Member Function Documentation
        Create()
        bool wxScrollBar::Create	(	wxWindow * 	parent,
        wxWindowID 	id,
        const wxPoint & 	pos = wxDefaultPosition,
        const wxSize & 	size = wxDefaultSize,
        long 	style = wxSB_HORIZONTAL,
        const wxValidator & 	validator = wxDefaultValidator,
        const wxString & 	name = wxScrollBarNameStr 
        )		
        Scrollbar creation function called by the scrollbar constructor.

        See wxScrollBar() for details.

        GetPageSize()
        virtual int wxScrollBar::GetPageSize	(		)	const
        virtual
        Returns the page size of the scrollbar.

        This is the number of scroll units that will be scrolled when the user pages up or down. Often it is the same as the thumb size.

        See also
        SetScrollbar()
        GetRange()
        virtual int wxScrollBar::GetRange	(		)	const
        virtual
        Returns the length of the scrollbar.

        See also
        SetScrollbar()
        GetThumbPosition()
        virtual int wxScrollBar::GetThumbPosition	(		)	const
        virtual
        Returns the current position of the scrollbar thumb.

        See also
        SetThumbPosition()
        GetThumbSize()
        virtual int wxScrollBar::GetThumbSize	(		)	const
        virtual
        Returns the thumb or 'view' size.

        See also
        SetScrollbar()
        IsVertical()
        bool wxScrollBar::IsVertical	(		)	const
        Returns true for scrollbars that have the vertical style set.

        SetScrollbar()
        virtual void wxScrollBar::SetScrollbar	(	int 	position,
        int 	thumbSize,
        int 	range,
        int 	pageSize,
        bool 	refresh = true 
        )		
        virtual
        Sets the scrollbar properties.

        Parameters
        position	The position of the scrollbar in scroll units.
        thumbSize	The size of the thumb, or visible portion of the scrollbar, in scroll units.
        range	The maximum position of the scrollbar.
        pageSize	The size of the page size in scroll units. This is the number of units the scrollbar will scroll when it is paged up or down. Often it is the same as the thumb size.
        refresh	true to redraw the scrollbar, false otherwise.
        Remarks
        Let's say you wish to display 50 lines of text, using the same font. The window is sized so that you can only see 16 lines at a time. You would use:
        scrollbar->SetScrollbar(0, 16, 50, 15);
        The page size is 1 less than the thumb size so that the last line of the previous page will be visible on the next page, to help orient the user. Note that with the window at this size, the thumb position can never go above 50 minus 16, or 34. You can determine how many lines are currently visible by dividing the current view size by the character height in pixels. When defining your own scrollbar behaviour, you will always need to recalculate the scrollbar settings when the window size changes. You could therefore put your scrollbar calculations and SetScrollbar() call into a function named AdjustScrollbars, which can be called initially and also from a wxSizeEvent event handler function.
        Reimplemented from wxWindow.

        SetThumbPosition()
        virtual void wxScrollBar::SetThumbPosition	(	int 	viewStart	)	
        virtual
        Sets the position of the scrollbar.

        Parameters
        viewStart	The position of the scrollbar thumb.
        See also
        GetThumbPosition() 

         */
    }
}
