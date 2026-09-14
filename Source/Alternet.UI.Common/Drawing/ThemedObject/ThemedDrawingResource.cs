using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines an interface for a themed drawing resource that can be defined by a brush, pen, or color.
    /// </summary>
    public interface IThemedDrawingResource : IThemedDrawingObject<IDrawingResource>
    {
    }

    /// <summary>
    /// Represents a two-value class that has different values for light and dark themes for <see cref="IDrawingResource"/> type.
    /// </summary>
    public class ThemedDrawingResource : ThemedDrawingObject<IDrawingResource>, IThemedDrawingResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the same light and dark values.
        /// </summary>
        /// <param name="value">The value to use for both the light and dark theme variants.</param>
        public ThemedDrawingResource(IDrawingResource value)
            : base(value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemedDrawingResource"/> class with the specified light and dark values.
        /// </summary>
        /// <param name="light">The value to use for the light theme variant.</param>
        /// <param name="dark">The value to use for the dark theme variant.</param>
        public ThemedDrawingResource(IDrawingResource light, IDrawingResource dark)
            : base(light, dark)
        {
        }
    }
}
