using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class VirtualListBox
    {
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