using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains vertical alignment and horizontal alignment.
    /// </summary>
    public readonly struct HVAlignment : IEquatable<HVAlignment>
    {
        /// <summary>
        /// Gets <see cref="HVAlignment"/> with centered horizontal and vertical alignments.
        /// </summary>
        public static readonly HVAlignment Center
            = new(HorizontalAlignment.Center, VerticalAlignment.Center);

        /// <summary>
        /// Gets <see cref="HVAlignment"/> with alignment set to top-left.
        /// </summary>
        public static readonly HVAlignment TopLeft
            = new(HorizontalAlignment.Left, VerticalAlignment.Top);

        /// <summary>
        /// Gets <see cref="HVAlignment"/> with alignment set to bottom-left.
        /// </summary>
        public static readonly HVAlignment BottomLeft
            = new(HorizontalAlignment.Left, VerticalAlignment.Bottom);

        /// <summary>
        /// Gets <see cref="HVAlignment"/> with alignment set to bottom-right.
        /// </summary>
        public static readonly HVAlignment BottomRight
            = new(HorizontalAlignment.Right, VerticalAlignment.Bottom);

        /// <summary>
        /// Gets <see cref="HVAlignment"/> with alignment set to top-right.
        /// </summary>
        public static readonly HVAlignment TopRight
            = new(HorizontalAlignment.Right, VerticalAlignment.Top);

        /// <summary>
        /// Gets <see cref="HVAlignment"/> with alignment set to center-left.
        /// </summary>
        public static readonly HVAlignment CenterLeft
            = new(HorizontalAlignment.Left, VerticalAlignment.Center);

        /// <summary>
        /// Gets horizontal alignment.
        /// </summary>
        public readonly HorizontalAlignment Horizontal;

        /// <summary>
        /// Gets vertical alignment.
        /// </summary>
        public readonly VerticalAlignment Vertical;

        /// <summary>
        /// Initializes a new instance of the <see cref="HVAlignment"/> struct.
        /// </summary>
        public HVAlignment()
        {
            Horizontal = HorizontalAlignment.Stretch;
            Vertical = VerticalAlignment.Stretch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HVAlignment"/> struct.
        /// </summary>
        /// <param name="vertical">Vertical alignment.</param>
        /// <param name="horizontal">Horizontal alignment.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HVAlignment(HorizontalAlignment horizontal, VerticalAlignment vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HVAlignment"/> struct.
        /// </summary>
        /// <param name="horizontal">Horizontal alignment.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HVAlignment(HorizontalAlignment horizontal)
        {
            Horizontal = horizontal;
            Vertical = VerticalAlignment.Stretch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HVAlignment"/> struct.
        /// </summary>
        /// <param name="vertical">Vertical alignment.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public HVAlignment(VerticalAlignment vertical)
        {
            Horizontal = HorizontalAlignment.Stretch;
            Vertical = vertical;
        }

        /// <summary>
        /// Gets whether vertical alignment is <see cref="VerticalAlignment.Stretch"/>
        /// or <see cref="VerticalAlignment.Fill"/>.
        /// </summary>
        [Browsable(false)]
        public bool IsVerticalStretchOrFill => Vertical == VerticalAlignment.Stretch
                || Vertical == VerticalAlignment.Fill;

        /// <summary>
        /// Gets whether horizontal alignment is <see cref="HorizontalAlignment.Stretch"/>
        /// or <see cref="HorizontalAlignment.Fill"/>.
        /// </summary>
        [Browsable(false)]
        public bool IsHorizontalStretchOrFill => Horizontal == HorizontalAlignment.Stretch
                || Horizontal == HorizontalAlignment.Fill;

        /// <summary>
        /// Gets whether <see cref="IsHorizontalStretchOrFill"/>
        /// or <see cref="IsVerticalStretchOrFill"/> returns True.
        /// </summary>
        [Browsable(false)]
        public bool IsStretchOrFill => IsHorizontalStretchOrFill || IsVerticalStretchOrFill;

        /// <summary>
        /// Implicit operator declaration for the conversion from <see cref="HVAlignment"/> to
        /// <see cref="VerticalAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator VerticalAlignment(HVAlignment value)
        {
            return value.Vertical;
        }

        /// <summary>
        /// Implicit operator declaration for the conversion
        /// from <see cref="HVAlignment"/> to
        /// <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HorizontalAlignment(HVAlignment value)
        {
            return value.Horizontal;
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from
        /// <see cref="HorizontalAlignment"/> to
        /// <see cref="HVAlignment"/>.
        /// </summary>
        /// <param name="horizontal">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HVAlignment(HorizontalAlignment horizontal)
        {
            return new(horizontal);
        }

        /// <summary>
        /// Implicit operator declaration for the conversion
        /// from <see cref="VerticalAlignment"/> to
        /// <see cref="HVAlignment"/>.
        /// </summary>
        /// <param name="vertical">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HVAlignment(VerticalAlignment vertical)
        {
            return new(vertical);
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from
        /// tuple with two alignment values to
        /// <see cref="HVAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HVAlignment(
            (HorizontalAlignment Horizontal, VerticalAlignment Vertical) value)
        {
            return new(value.Horizontal, value.Vertical);
        }

        /// <summary>
        /// Tests whether two specified <see cref="HVAlignment"/>
        /// structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="HVAlignment"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="HVAlignment"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="HVAlignment"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(HVAlignment left, HVAlignment right)
        {
            return left.Horizontal == right.Horizontal
                && left.Vertical == right.Vertical;
        }

        /// <summary>
        /// Tests whether two specified <see cref="HVAlignment"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="HVAlignment"/> that is to the left
        /// of the inequality operator.</param>
        /// <param name="right">The <see cref="HVAlignment"/> that is to the right
        /// of the inequality operator.</param>
        /// <returns><c>true</c> if the two <see cref="HVAlignment"/> structures
        /// are different; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(HVAlignment left, HVAlignment right) => !(left == right);

        /// <summary>
        /// Returns next alignment or <see cref="CoordAlignment.Near"/> if
        /// <paramref name="alignment"/> is maximal.
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static CoordAlignment NextValue(CoordAlignment alignment)
        {
            if (EnumUtils.IsMaxValueUseLast(alignment))
                return CoordAlignment.Near;
            return alignment + 1;
        }

        /// <summary>
        /// Gets vertical alignment or Null if <paramref name="direction"/> parameter
        /// doesn't contain <see cref="GenericOrientation.Vertical"/>.
        /// </summary>
        /// <param name="direction">Direction filter.</param>
        /// <returns></returns>
        public VerticalAlignment? VerticalOrNull(GenericOrientation direction)
        {
            if (direction.HasFlag(GenericOrientation.Vertical))
                return Vertical;
            return null;
        }

        /// <summary>
        /// Gets horizontal alignment or Null if <paramref name="direction"/> parameter
        /// doesn't contain <see cref="GenericOrientation.Horizontal"/>.
        /// </summary>
        /// <param name="direction">Direction filter.</param>
        /// <returns></returns>
        public HorizontalAlignment? HorizontalOrNull(GenericOrientation direction)
        {
            if (direction.HasFlag(GenericOrientation.Horizontal))
                return Horizontal;
            return null;
        }

        /// <summary>
        /// Returns this object with changed vertical alignment.
        /// </summary>
        /// <param name="value">New value for the alignment.</param>
        /// <returns></returns>
        public HVAlignment WithVertical(VerticalAlignment value)
        {
            return new(Horizontal, value);
        }

        /// <summary>
        /// Returns this object with changed vertical alignment.
        /// </summary>
        /// <param name="value">New value for the alignment.</param>
        /// <returns></returns>
        public HVAlignment WithVertical(CoordAlignment value)
        {
            return new(Horizontal, (VerticalAlignment)value);
        }

        /// <summary>
        /// Returns this object with changed horizontal alignment.
        /// </summary>
        /// <param name="value">New value for the alignment.</param>
        /// <returns></returns>
        public HVAlignment WithHorizontal(CoordAlignment value)
        {
            return new((HorizontalAlignment)value, Vertical);
        }

        /// <summary>
        /// Returns this object with changed horizontal alignment.
        /// </summary>
        /// <param name="value">New value for the alignment.</param>
        /// <returns></returns>
        public HVAlignment WithHorizontal(HorizontalAlignment value)
        {
            return new(value, Vertical);
        }

        /// <summary>
        /// Returns this object if <see cref="IsStretchOrFill"/> is False; otherwise
        /// returns <paramref name="overrideValue"/> (or <see cref="HVAlignment.Center"/>
        /// if it is not specified or Null).
        /// </summary>
        /// <returns></returns>
        public HVAlignment WithoutStretchOrFill(HVAlignment? overrideValue = null)
        {
            if (IsStretchOrFill)
                return overrideValue ?? HVAlignment.Center;
            return this;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified object is equal to the
        /// current object; otherwise, <c>false</c>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj) =>
            obj is HVAlignment other && Equals(other);

        /// <summary>
        /// Indicates whether the current object is equal to another object of
        /// the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to other;
        /// otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(HVAlignment other)
        {
            return this == other;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return (Horizontal, Vertical).GetHashCode();
        }

        /// <summary>
        /// Creates a human-readable string that represents this
        /// <see cref='HVAlignment'/>.
        /// </summary>
        public override readonly string ToString()
        {
            string[] names = { nameof(Horizontal), nameof(Vertical) };
            object[] values = { Horizontal, Vertical };

            return StringUtils.ToStringWithOrWithoutNames(names, values);
        }

        /// <summary>
        /// Gets next alignment value by incrementing alignments.
        /// </summary>
        /// <returns></returns>
        public HVAlignment NextValue()
        {
            if (EnumUtils.IsMaxValueUseLast(Horizontal))
            {
                return new(
                    HorizontalAlignment.Left,
                    (VerticalAlignment)NextValue((CoordAlignment)Vertical));
            }

            return new(
                (HorizontalAlignment)NextValue((CoordAlignment)Horizontal),
                Vertical);
        }

        /// <summary>
        /// Gets next alignment value by incrementing alignments. 'Stretch' or 'Fill'
        /// alignments are not returned.
        /// </summary>
        /// <returns></returns>
        public HVAlignment NextValueNoStretchOrFill()
        {
            HVAlignment result = this;

            while (true)
            {
                result = result.NextValue();

                if (result == this || !result.IsStretchOrFill)
                    return result;
            }
        }
    }
}
