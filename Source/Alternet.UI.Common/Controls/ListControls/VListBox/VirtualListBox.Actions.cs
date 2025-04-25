using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class VirtualListBox
    {
        public virtual void DoActionScrollPageUp()
        {
            var idx = GetIndexOnPreviousPage(TopIndex);
            if (idx is not null)
            {
                ScrollToRow(Math.Min(idx.Value + 1, TopIndex));
            }
        }

        public virtual void DoActionScrollPageDown()
        {
            var idx = GetIndexOnNextPage(TopIndex);
            if (idx is not null)
            {
                ScrollToRow(Math.Max(idx.Value - 1, TopIndex));
            }
        }

        public virtual void DoActionScrollLineUp()
        {
            ScrollToRow(TopIndex - 1);
        }

        public virtual void DoActionScrollLineDown()
        {
            ScrollToRow(TopIndex + 1);
        }

        public virtual void DoActionScrollToFirstLine()
        {
            ScrollToRow(0);
        }

        public virtual void DoActionScrollToLastLine()
        {
            ScrollToRow(Count - 1);
        }
    }
}