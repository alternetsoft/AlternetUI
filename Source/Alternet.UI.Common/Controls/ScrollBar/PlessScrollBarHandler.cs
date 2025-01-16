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
        public int ThumbPosition { get; set; }

        /// <inheritdoc/>
        public int Range { get; set; }

        /// <inheritdoc/>
        public int PageSize { get; set; }

        /// <inheritdoc/>
        public bool IsVertical { get; set; }

        /// <inheritdoc/>
        public void SetScrollbar(
            int? position,
            int? range,
            int? pageSize,
            bool refresh = true)
        {
            if(position is not null)
                ThumbPosition = position.Value;
            if(range is not null)
                Range = range.Value;
            if(pageSize is not null)
                PageSize = pageSize.Value;
        }
    }
}
