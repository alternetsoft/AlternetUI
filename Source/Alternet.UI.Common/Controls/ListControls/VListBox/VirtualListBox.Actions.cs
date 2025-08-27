using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class VirtualListBox
    {
        /// <summary>
        /// Scrolls the control horizontally by one char to the left.
        /// </summary>
        public virtual void DoActionScrollCharLeft()
        {
            IncHorizontalOffsetChars(-1);
        }

        /// <summary>
        /// Scrolls the control horizontally by one char to the right.
        /// </summary>
        public virtual void DoActionScrollCharRight()
        {
            IncHorizontalOffsetChars(1);
        }

        /// <summary>
        /// Scrolls the control horizontally to the first char.
        /// </summary>
        public virtual void DoActionScrollToFirstChar()
        {
            DoActionScrollToHorzPos(0);
        }

        /// <summary>
        /// Scrolls the control horizontally by one page to the left.
        /// </summary>
        public virtual void DoActionScrollPageLeft()
        {
            IncHorizontalOffsetChars(-DefaultHorizontalScrollBarLargeIncrement);
        }

        /// <summary>
        /// Scrolls the control horizontally by one page to the right.
        /// </summary>
        public virtual void DoActionScrollPageRight()
        {
            IncHorizontalOffsetChars(DefaultHorizontalScrollBarLargeIncrement);
        }

        /// <summary>
        /// Scrolls the control up by one page.
        /// </summary>
        public virtual void DoActionScrollPageUp()
        {
            var idx = GetIndexOnPreviousPage(TopIndex);
            if (idx is not null)
            {
                ScrollToRow(Math.Min(idx.Value + 1, TopIndex));
            }
        }

        /// <summary>
        /// Scrolls the control down by one page.
        /// </summary>
        public virtual void DoActionScrollPageDown()
        {
            var idx = GetIndexOnNextPage(TopIndex);
            if (idx is not null)
            {
                ScrollToRow(Math.Max(idx.Value - 1, TopIndex));
            }
        }

        /// <summary>
        /// Scrolls the control up by one line.
        /// </summary>
        public virtual void DoActionScrollLineUp()
        {
            ScrollToRow(TopIndex - 1);
        }

        /// <summary>
        /// Scrolls the control down by one line.
        /// </summary>
        public virtual void DoActionScrollLineDown()
        {
            ScrollToRow(TopIndex + 1);
        }

        /// <summary>
        /// Scrolls to the first line in the control.
        /// </summary>
        public virtual void DoActionScrollToFirstLine()
        {
            ScrollToRow(0);
        }

        /// <summary>
        /// Scrolls to the last line in the control.
        /// </summary>
        public virtual void DoActionScrollToLastLine()
        {
            ScrollToRow(Count - 1);
        }
    }
}