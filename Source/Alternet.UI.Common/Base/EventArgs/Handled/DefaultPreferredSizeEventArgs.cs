using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="StaticControlEvents.RequestPreferredSize"/> event.
    /// </summary>
    public class DefaultPreferredSizeEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPreferredSizeEventArgs"/> class.
        /// </summary>
        /// <param name="layout">Layout style to use.</param>
        /// <param name="availableSize">Available size for the layout.</param>
        public DefaultPreferredSizeEventArgs(LayoutStyle layout, SizeD availableSize)
        {
            Layout = layout;
            AvailableSize = availableSize;
            Result = SizeD.MinusOne;
        }

        /// <summary>
        /// Gets layout style to use.
        /// </summary>
        public LayoutStyle Layout { get; }

        /// <summary>
        /// Gets the available space that a parent element
        /// can allocate a child control.
        /// </summary>
        public SizeD AvailableSize { get; }

        /// <summary>
        /// Gets or sets preferred size (a result of event execution).
        /// </summary>
        public SizeD Result { get; set; }
    }
}
