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
        public override void DoActionScrollCharLeft()
        {
            IncHorizontalOffsetChars(-1);
        }

        /// <summary>
        /// Scrolls the control horizontally by one char to the right.
        /// </summary>
        public override void DoActionScrollCharRight()
        {
            IncHorizontalOffsetChars(1);
        }

        /// <summary>
        /// Scrolls the control horizontally to the first char.
        /// </summary>
        public override void DoActionScrollToFirstChar()
        {
            SetHorizontalOffset(0);
        }

        /// <summary>
        /// Scrolls the control horizontally by one page to the left.
        /// </summary>
        public override void DoActionScrollPageLeft()
        {
            IncHorizontalOffsetChars(-DefaultHorizontalScrollBarLargeIncrement);
        }

        /// <summary>
        /// Scrolls the control horizontally by one page to the right.
        /// </summary>
        public override void DoActionScrollPageRight()
        {
            IncHorizontalOffsetChars(DefaultHorizontalScrollBarLargeIncrement);
        }

        /// <summary>
        /// Scrolls the control up by one page.
        /// </summary>
        public override void DoActionScrollPageUp()
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
        public override void DoActionScrollPageDown()
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
        public override void DoActionScrollLineUp()
        {
            ScrollToRow(TopIndex - 1);
        }

        /// <summary>
        /// Scrolls the control down by one line.
        /// </summary>
        public override void DoActionScrollLineDown()
        {
            ScrollToRow(TopIndex + 1);
        }

        /// <summary>
        /// Scrolls to the first line in the control.
        /// </summary>
        public override void DoActionScrollToFirstLine()
        {
            ScrollToRow(0);
        }

        /// <summary>
        /// Scrolls to the last line in the control.
        /// </summary>
        public override void DoActionScrollToLastLine()
        {
            ScrollToRow(Count - 1);
        }
    }
}