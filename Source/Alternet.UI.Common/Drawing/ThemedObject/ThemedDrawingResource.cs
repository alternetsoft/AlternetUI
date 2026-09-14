using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a two-value class that has different values for light and dark themes for <see cref="DrawingResource"/> type.
    /// </summary>
    public class ThemedDrawingResource : ThemedDrawingObject<DrawingResource>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the same light and dark values.
        /// </summary>
        /// <param name="value">The value to use for both the light and dark theme variants.</param>
        public ThemedDrawingResource(DrawingResource value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        public ThemedDrawingResource(DrawingResource light, DrawingResource dark)
            : base(light, dark)
        {
        }
    }
}
