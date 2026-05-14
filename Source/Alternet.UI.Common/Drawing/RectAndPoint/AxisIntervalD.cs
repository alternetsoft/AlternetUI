using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a structure that encapsulates a start position and a length,
    /// commonly used in layout calculations to define the starting point and the extent of a layout
    /// item along a specific axis (horizontal or vertical).
    /// </summary>
    public readonly struct AxisIntervalD : IEquatable<AxisIntervalD>
    {
        /// <summary>
        /// Represents an empty start and length, where both start and length are set to zero.
        /// </summary>
        public static readonly AxisIntervalD Empty = new(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="AxisIntervalD"/> struct.
        /// </summary>
        /// <param name="start">Start position.</param>
        /// <param name="length">Length.</param>
        public AxisIntervalD(Coord start, Coord length)
        {
            Start = start;
            Length = length;
        }

        /// <summary>
        /// Gets start position.
        /// </summary>
        public Coord Start { get; }

        /// <summary>
        /// Gets length.
        /// </summary>
        public Coord Length { get; }

        /// <summary>
        /// Gets the end position (Start + Length).
        /// </summary>
        public Coord End => Start + Length;

        /// <summary>
        /// Gets a value indicating whether this interval is empty (Length == 0).
        /// </summary>
        public bool IsEmpty => Length == 0;

        /// <summary>
        /// Returns true if <paramref name="value"/> lies within the interval using half-open semantics: [Start, End).
        /// </summary>
        public bool Contains(Coord value) => value >= Start && value < End;

        /// <summary>
        /// Returns true if <paramref name="value"/> lies within the interval using closed semantics: [Start, End].
        /// </summary>
        public bool ContainsClosed(Coord value) => value >= Start && value <= End;

        /// <summary>
        /// Determines whether two specified <see cref="AxisIntervalD"/> values are equal.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see
        /// langword="false"/>.</returns>
        public static bool operator ==(AxisIntervalD left, AxisIntervalD right) => left.Equals(right);

        /// <summary>
        /// Determines whether two specified <see cref="AxisIntervalD"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first instance to compare.</param>
        /// <param name="right">The second instance to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise,
        /// <see langword="false"/>.</returns>
        public static bool operator !=(AxisIntervalD left, AxisIntervalD right) => !left.Equals(right);

        /// <inheritdoc/>
        public override string? ToString()
        {
            return $"Start: {Start}, Length: {Length}, End: {End}";
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is AxisIntervalD other && Start == other.Start && Length == other.Length;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length);
        }

        /// <inheritdoc/>
        public bool Equals(AxisIntervalD other)
        {
            return Start == other.Start && Length == other.Length;
        }
    }
}
