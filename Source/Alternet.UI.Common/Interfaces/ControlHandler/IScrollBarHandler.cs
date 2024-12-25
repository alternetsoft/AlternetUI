using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to methods and properties of the native scrollbar control.
    /// </summary>
    public interface IScrollBarHandler
    {
        /// <summary>
        /// Gets or sets action called when scrolling is done.
        /// </summary>
        Action? Scroll { get; set; }

        /// <summary>
        /// Gets or sets thumb position.
        /// </summary>
        int ThumbPosition { get; set; }

        /// <summary>
        /// Gets or sets scrolling range.
        /// </summary>
        int Range { get; }

        /// <summary>
        /// Gets or sets page size.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets or sets whether scrollbar is vertical.
        /// </summary>
        bool IsVertical { get; set; }

        /// <summary>
        /// Gets type of the scroll event.
        /// </summary>
        ScrollEventType EventTypeID { get; }

        /// <summary>
        /// Gets old position of the scroll thumb passed to the scroll event.
        /// </summary>
        int EventOldPos { get; }

        /// <summary>
        /// Gets new position of the scroll thumb passed to the scroll event.
        /// </summary>
        int EventNewPos { get; }

        /// <inheritdoc cref="ScrollBar.SetScrollbar(int?, int?, int?, bool)"/>
        void SetScrollbar(
            int? position,
            int? range,
            int? pageSize,
            bool refresh = true);
    }
}
