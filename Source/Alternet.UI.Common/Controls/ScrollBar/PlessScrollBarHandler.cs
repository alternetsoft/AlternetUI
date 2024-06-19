using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class PlessScrollBarHandler : PlessControlHandler, IScrollBarHandler
    {
        public Action? Scroll { get; set; }

        public int ThumbPosition { get; set; }

        public int Range { get; set; }

        public int ThumbSize { get; set; }

        public int PageSize { get; set; }

        public bool IsVertical { get; set; }

        public ScrollEventType EventTypeID { get; set; }

        public int EventOldPos { get; set; }

        public int EventNewPos { get; set; }

        public void SetScrollbar(
            int position,
            int thumbSize,
            int range,
            int pageSize,
            bool refresh = true)
        {
            ThumbPosition = position;
            Range = range;
            ThumbSize = thumbSize;
            PageSize = pageSize;
        }
    }
}
