using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a contract for handling scroll-related actions in a control,
    /// including horizontal and vertical
    /// scrolling by characters, lines, pages, or specific positions.
    /// </summary>
    /// <remarks>This interface provides methods to programmatically control scrolling
    /// behavior in a user interface component.
    /// It supports fine-grained scrolling actions,
    /// such as scrolling by individual characters or lines,
    /// as well as larger movements, such as scrolling by pages or jumping to specific positions.
    /// Implementations of this interface are expected to handle these actions
    /// appropriately based on the control's content and layout.</remarks>
    public interface IScrollEventRouter
    {
        /// <summary>
        /// Calculates and retrieves information about the horizontal and vertical scroll bars.
        /// </summary>
        /// <param name="horzScrollbar">When this method returns,
        /// contains information about the horizontal scroll bar.</param>
        /// <param name="vertScrollbar">When this method returns,
        /// contains information about the vertical scroll bar.</param>
        void CalcScrollBarInfo(out ScrollBarInfo horzScrollbar, out ScrollBarInfo vertScrollbar);

        /// <summary>
        /// Scrolls the control horizontally by one char to the left.
        /// </summary>
        void DoActionScrollCharLeft();

        /// <summary>
        /// Scrolls the control horizontally by one char to the right.
        /// </summary>
        void DoActionScrollCharRight();

        /// <summary>
        /// Scrolls the control horizontally to the first char.
        /// </summary>
        void DoActionScrollToFirstChar();

        /// <summary>
        /// Scrolls the control horizontally by one page to the left.
        /// </summary>
        void DoActionScrollPageLeft();

        /// <summary>
        /// Scrolls the control horizontally by one page to the right.
        /// </summary>
        void DoActionScrollPageRight();

        /// <summary>
        /// Scrolls the control up by one page.
        /// </summary>
        void DoActionScrollPageUp();

        /// <summary>
        /// Scrolls the control down by one page.
        /// </summary>
        void DoActionScrollPageDown();

        /// <summary>
        /// Scrolls the control up by one line.
        /// </summary>
        void DoActionScrollLineUp();

        /// <summary>
        /// Scrolls the control down by one line.
        /// </summary>
        void DoActionScrollLineDown();

        /// <summary>
        /// Scrolls to the first line in the control.
        /// </summary>
        void DoActionScrollToFirstLine();

        /// <summary>
        /// Scrolls to the last line in the control.
        /// </summary>
        void DoActionScrollToLastLine();

        /// <summary>
        /// Sets vertical scroll offset.
        /// </summary>
        /// <param name="value">Value of the vertical scroll offset.</param>
        void DoActionScrollToVertPos(int value);

        /// <summary>
        /// Sets horizontal scroll offset.
        /// </summary>
        /// <param name="value">Value of the horizontal scroll offset.</param>
        void DoActionScrollToHorzPos(int value);
    }
}
