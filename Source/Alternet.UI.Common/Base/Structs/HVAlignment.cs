using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains vertical alignment and horizontal alignment.
    /// </summary>
    public readonly struct HVAlignment
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
        /// Implicit operator declaration for the conversion from <see cref="HVAlignment"/> to
        /// <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HorizontalAlignment(HVAlignment value)
        {
            return value.Horizontal;
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from <see cref="HorizontalAlignment"/> to
        /// <see cref="HVAlignment"/>.
        /// </summary>
        /// <param name="horizontal">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HVAlignment(HorizontalAlignment horizontal)
        {
            return new(horizontal);
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from <see cref="GenericAlignment"/> to
        /// <see cref="HVAlignment"/>.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        public static implicit operator HVAlignment(GenericAlignment value)
        {
            var vertical = AlignUtils.GetVertical(value);
            var horizontal = AlignUtils.GetHorizontal(value);
            return new(horizontal: horizontal, vertical: vertical);
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from <see cref="VerticalAlignment"/> to
        /// <see cref="HVAlignment"/>.
        /// </summary>
        /// <param name="vertical">Value to convert.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator HVAlignment(VerticalAlignment vertical)
        {
            return new(vertical);
        }

        /// <summary>
        /// Implicit operator declaration for the conversion from tuple with two alignment values to
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
    }
}
