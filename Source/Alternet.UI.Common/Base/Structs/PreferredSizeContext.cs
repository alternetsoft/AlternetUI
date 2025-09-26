using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a context for determining the preferred size of a UI element.
    /// </summary>
    public readonly struct PreferredSizeContext
    {
        /// <summary>
        /// Represents a context with a preferred size of positive infinity.
        /// </summary>
        /// <remarks>
        /// This static field provides a predefined context where
        /// the preferred size is set to <see cref="SizeD.PositiveInfinity"/>.
        /// </remarks>
        public static readonly PreferredSizeContext PositiveInfinity = new(SizeD.PositiveInfinity);

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferredSizeContext"/> struct
        /// with the specified available size.
        /// </summary>
        /// <param name="availableSize">The available size for the UI element.</param>
        public PreferredSizeContext(SizeD availableSize)
        {
            AvailableSize = availableSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferredSizeContext"/> struct
        /// with the specified available width and height.
        /// </summary>
        /// <param name="width">The available width for the UI element.</param>
        /// <param name="height">The available height for the UI element.</param>
        public PreferredSizeContext(Coord width, Coord height)
            : this(new SizeD(width, height))
        {
        }

        /// <summary>
        /// Gets a <see cref="PreferredSizeContext"/> instance representing the maximum coordinate value
        /// which can be used for the layout purposes.
        /// </summary>
        public static PreferredSizeContext MaxCoord => new(SizeD.MaxCoord);

        /// <summary>
        /// Gets the available size for the UI element in this context.
        /// </summary>
        public SizeD AvailableSize { get; }

        /// <summary>
        /// Implicitly converts a <see cref="SizeD"/> to a <see cref="PreferredSizeContext"/>.
        /// </summary>
        /// <param name="size">The <see cref="SizeD"/> to convert.</param>
        public static implicit operator PreferredSizeContext(SizeD size)
        {
            return new PreferredSizeContext(size);
        }
    }
}
