using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a two-value class that has different values for light and dark themes for <see cref="Brush"/> type.
    /// </summary>
    public class ThemedBrush : ThemedDrawingObject<Brush>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedBrush"/> class with the same light and dark values.
        /// </summary>
        /// <param name="value">The value to use for both the light and dark theme variants.</param>
        public ThemedBrush(Brush value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedBrush"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        public ThemedBrush(Brush light, Brush dark)
            : base(light, dark)
        {
        }
    }
}
