using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// A <see cref="ScrollBar"/> is a control that represents a horizontal or vertical scrollbar.
    /// </summary>
    /// <remarks>
    /// A scrollbar has the following main attributes: range, thumb size,
    /// page size, and position.The range is the total number of units
    /// associated with the view represented by the scrollbar.For a table
    /// with 15 columns, the range would be 15. The thumb size is the number of units
    /// that are currently visible.For the table example, the window might be sized
    /// so that only 5 columns are currently visible, in which case the application
    /// would set the thumb size to 5. When the thumb size becomes the same as or
    /// greater than the range, the scrollbar will be automatically hidden on
    /// most platforms.The page size is the number of units that the scrollbar
    /// should scroll by, when 'paging' through the data.This value is normally the
    /// same as the thumb size length, because it is natural to assume that the visible
    /// window size defines a page.The scrollbar position is the current thumb position.
    /// Most applications will find it convenient to provide a function called
    /// AdjustScrollbars() which can be called initially, from an OnSize event
    /// handler, and whenever the application data changes in size.It will adjust
    /// the view, object and page size according to the size of the window and the size of the data.
    /// </remarks>
    public class ScrollBar : Control
    {
        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new ScrollBarHandler();
        }

        internal class ScrollBarHandler : NativeControlHandler<ScrollBar, Native.ScrollBar>
        {
            internal override Native.Control CreateNativeControl()
            {
                var result = new Native.ScrollBar();
                return result;
            }
        }
    }
}
