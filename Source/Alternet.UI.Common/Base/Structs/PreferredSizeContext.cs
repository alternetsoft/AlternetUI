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
        /// Represents a preferred size context initialized to half of the maximum value
        /// for the integer-based size.
        /// </summary>
        /// <remarks>This static readonly field provides a predefined
        /// <see cref="PreferredSizeContext"/>
        /// instance  with a size value set to half of the maximum allowable integer size.
        /// It can be used as a
        /// convenient default or reference value in scenarios where
        /// such a size context is appropriate.</remarks>
        public static readonly PreferredSizeContext HalfOfMaxValueI = new(SizeD.HalfOfMaxValueI);

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
