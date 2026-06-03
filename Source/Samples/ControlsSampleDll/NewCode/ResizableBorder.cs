using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a resizable border control.
    /// </summary>
    public partial class ResizableBorder : Border
    {
        /// <summary>
        /// Gets or sets the thickness of the resizable border.
        /// </summary>
        public static Thickness DefaultPadding = 3f;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableBorder"/> class.
        /// </summary>
        public ResizableBorder()
        {
            Padding = DefaultPadding;
        }
    }
}
