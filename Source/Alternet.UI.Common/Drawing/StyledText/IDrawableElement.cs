using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides access to the drawable element methods and properties.
    /// </summary>
    public interface IDrawableElement
    {
        /// <summary>
        /// Draws element.
        /// </summary>
        /// <param name="dc">Canvas where element is painted.</param>
        void Draw(Graphics dc, RectD container);

        /// <summary>
        /// Gets width and height of the element.
        /// </summary>
        /// <param name="dc">Canvas where element is painted.</param>
        /// <returns></returns>
        SizeD Measure(Graphics dc, SizeD availableSize);
    }
}
