using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="IScrollBarHandler"/> provider.
    /// </summary>
    public class PlessScrollBarHandler : PlessControlHandler, IScrollBarHandler
    {
        /// <inheritdoc/>
        public Action? Scroll { get; set; }

        /// <inheritdoc/>
        public int ThumbPosition { get; set; }

        /// <inheritdoc/>
        public int Range { get; set; }

        /// <inheritdoc/>
        public int ThumbSize { get; set; }

        /// <inheritdoc/>
        public int PageSize { get; set; }

        /// <inheritdoc/>
        public bool IsVertical { get; set; }

        /// <inheritdoc/>
        public ScrollEventType EventTypeID { get; set; }

        /// <inheritdoc/>
        public int EventOldPos { get; set; }

        /// <inheritdoc/>
        public int EventNewPos { get; set; }

        /// <inheritdoc/>
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
