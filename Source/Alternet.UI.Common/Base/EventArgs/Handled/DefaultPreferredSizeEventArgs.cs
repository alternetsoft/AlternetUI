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
        /// <param name="context">The <see cref="PreferredSizeContext"/> providing
        /// the available size and other layout information.</param>
        public DefaultPreferredSizeEventArgs(LayoutStyle layout, PreferredSizeContext context)
        {
            Layout = layout;
            Context = context;
            Result = SizeD.MinusOne;
        }

        /// <summary>
        /// Gets layout style to use.
        /// </summary>
        public LayoutStyle Layout { get; set; }

        /// <summary>
        /// Gets the available space that a parent element
        /// can allocate a child control.
        /// </summary>
        public SizeD AvailableSize => Context.AvailableSize;

        /// <summary>
        /// Gets the context that determines the preferred size calculation behavior.
        /// </summary>
        public PreferredSizeContext Context { get; set; }

        /// <summary>
        /// Gets or sets preferred size (a result of event execution).
        /// </summary>
        public SizeD Result { get; set; }
    }
}
