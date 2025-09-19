using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a unique identifier for a graphics configuration,
    /// defined by a graphics backend type and a scale factor.
    /// </summary>
    /// <remarks>This structure is immutable and provides value-based equality semantics.
    /// It is primarily used
    /// to uniquely identify a specific graphics configuration in rendering systems.</remarks>
    public readonly struct GraphicsConfigIdentity : IEquatable<GraphicsConfigIdentity>
    {
        private readonly int hashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsConfigIdentity"/> struct.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> instance to use.</param>
        public GraphicsConfigIdentity(Graphics graphics)
            : this(graphics.BackendType, graphics.ScaleFactor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsConfigIdentity"/> class
        /// with the specified backend type and scale factor.
        /// </summary>
        /// <param name="backendType">The <see cref="GraphicsBackendType"/> to use.</param>
        /// <param name="scaleFactor">The <see cref="Coord"/> representing the scale factor.</param>
        public GraphicsConfigIdentity(GraphicsBackendType backendType, Coord scaleFactor)
        {
            BackendType = backendType;
            ScaleFactor = scaleFactor;
            hashCode = (backendType, scaleFactor).GetHashCode();
        }

        /// <summary>
        /// Gets the graphics backend type used for rendering.
        /// </summary>
        public GraphicsBackendType BackendType { get; }

        /// <summary>
        /// Gets the scale factor used for rendering.
        /// </summary>
        public Coord ScaleFactor { get; }

        /// <summary>
        /// Determines whether two <see cref="GraphicsConfigIdentity"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="GraphicsConfigIdentity"/> instance to compare.</param>
        /// <param name="right">The second <see cref="GraphicsConfigIdentity"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the specified <see cref="GraphicsConfigIdentity"/>
        /// instances are equal; otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator ==(GraphicsConfigIdentity left, GraphicsConfigIdentity right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two <see cref="GraphicsConfigIdentity"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="GraphicsConfigIdentity"/> instance to compare.</param>
        /// <param name="right">The second <see cref="GraphicsConfigIdentity"/> instance to compare.</param>
        /// <returns><see langword="true"/> if the two instances are not equal;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(GraphicsConfigIdentity left, GraphicsConfigIdentity right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is GraphicsConfigIdentity other && Equals(other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GraphicsConfigIdentity"/>
        /// is equal to the current instance.
        /// </summary>
        /// <param name="other">The <see cref="GraphicsConfigIdentity"/> to compare
        /// with the current instance.</param>
        /// <returns><see langword="true"/> if the specified <see cref="GraphicsConfigIdentity"/>
        /// is equal to the current
        /// instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(GraphicsConfigIdentity other)
        {
            return BackendType == other.BackendType && ScaleFactor == other.ScaleFactor;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}
