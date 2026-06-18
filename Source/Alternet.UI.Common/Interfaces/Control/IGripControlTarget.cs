using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a contract for the control that can be resized by grips.
    /// </summary>
    public interface IGripControlTarget : IDisposableObject
    {
        /// <summary>
        /// Gets or sets the bounds of the element.
        /// </summary>
        RectD Bounds { get; set; }

        /// <summary>
        /// Gets the maximum size the element can be resized to.
        /// </summary>
        SizeD MaximumSize { get; }

        /// <summary>
        /// Gets the minimum size the element can be resized to.
        /// </summary>
        SizeD MinimumSize { get; }
    }
}
