using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a two-value class that has different values for light and dark themes for <see cref="Pen"/> type.
    /// </summary>
    public class ThemedPen : ThemedDrawingObject<Pen>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedPen"/> class with the same light and dark values.
        /// </summary>
        /// <param name="value">The value to use for both the light and dark theme variants.</param>
        public ThemedPen(Pen value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedPen"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        public ThemedPen(Pen light, Pen dark)
            : base(light, dark)
        {
        }
    }
}
