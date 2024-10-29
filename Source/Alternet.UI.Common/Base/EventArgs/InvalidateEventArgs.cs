using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the 'Invalidated' event.
    /// </summary>
    public class InvalidateEventArgs : BaseEventArgs
    {
        private readonly RectD invalidRect;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidateEventArgs" /> class.
        /// </summary>
        /// <param name="invalidRect">The <see cref="RectD" /> that contains
        /// the invalidated window area.
        /// </param>
        public InvalidateEventArgs(RectD invalidRect)
        {
            this.invalidRect = invalidRect;
        }

        /// <summary>
        /// Gets the <see cref="RectD" /> that contains the invalidated window area.
        /// </summary>
        /// <returns>The invalidated window area.</returns>
        public RectD InvalidRect => invalidRect;
    }
}
