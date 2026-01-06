using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a horizontal and vertical drop-down alignment.
    /// </summary>
    public readonly struct HVDropDownAlignment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HVDropDownAlignment"/> struct
        /// with default horizontal and vertical alignments.
        /// </summary>
        public HVDropDownAlignment()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HVDropDownAlignment"/> struct
        /// with the specified horizontal and vertical alignments.
        /// </summary>
        /// <param name="horizontal">The horizontal drop-down alignment.</param>
        /// <param name="vertical">The vertical drop-down alignment.</param>
        public HVDropDownAlignment(DropDownAlignment horizontal, DropDownAlignment vertical)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HVDropDownAlignment"/> struct
        /// with the specified position.
        /// </summary>
        /// <param name="point">The position of the dropdown as a <see cref="PointD"/>.</param>
        public HVDropDownAlignment(PointD point)
        {
            Horizontal = DropDownAlignment.Position;
            Vertical = DropDownAlignment.Position;
            Position = point;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HVDropDownAlignment"/> struct
        /// with the specified horizontal and vertical position.
        /// </summary>
        /// <param name="x">The horizontal position of the dropdown.</param>
        /// <param name="y">The vertical position of the dropdown.</param>
        public HVDropDownAlignment(Coord x, Coord y)
        {
            Horizontal = DropDownAlignment.Position;
            Vertical = DropDownAlignment.Position;
            Position = (x, y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HVDropDownAlignment"/> class with
        /// the specified horizontal and vertical alignment, and position.
        /// </summary>
        /// <param name="horizontal">The horizontal alignment of the dropdown.</param>
        /// <param name="vertical">The vertical alignment of the dropdown.</param>
        /// <param name="point">The position of the dropdown as a <see cref="PointD"/>.</param>
        public HVDropDownAlignment(
            DropDownAlignment horizontal,
            DropDownAlignment vertical,
            PointD point)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            Position = point;
        }

        /// <summary>
        /// Gets a predefined alignment configuration where
        /// both horizontal and vertical alignments are set to <see cref="DropDownAlignment.BeforeEnd"/>.
        /// </summary>
        public static HVDropDownAlignment BeforeEnd => new (DropDownAlignment.BeforeEnd, DropDownAlignment.BeforeEnd);

        /// <summary>
        /// Gets a predefined alignment configuration where
        /// position is aligned to the top-right corner.
        /// </summary>
        public static HVDropDownAlignment TopRight => new (DropDownAlignment.BeforeEnd, DropDownAlignment.AfterStart);

        /// <summary>
        /// Gets a value that aligns the dropdown to the bottom-right corner relative to its anchor element.
        /// </summary>
        public static HVDropDownAlignment BottomRight => new(DropDownAlignment.BeforeEnd, DropDownAlignment.BeforeEnd);

        /// <summary>
        /// Gets the default drop-down alignment.
        /// </summary>
        public static HVDropDownAlignment Default => new(
            DropDownAlignment.AfterStart,
            DropDownAlignment.AfterEnd);

        /// <summary>
        /// Gets a predefined alignment configuration where
        /// both horizontal and vertical alignments are centered.
        /// </summary>
        public static HVDropDownAlignment Center => new(
            DropDownAlignment.Center,
            DropDownAlignment.Center);

        /// <summary>
        /// Gets a value indicating whether the horizontal alignment
        /// is set to <see cref="DropDownAlignment.Position"/>.
        /// </summary>
        public bool IsHorzPositionUsed => Horizontal == DropDownAlignment.Position;

        /// <summary>
        /// Gets a value indicating whether the vertical alignment
        /// is set to <see cref="DropDownAlignment.Position"/>.
        /// </summary>
        public bool IsVertPositionUsed => Vertical == DropDownAlignment.Position;

        /// <summary>
        /// Gets a value indicating whether either the horizontal or vertical
        /// alignment is set to <see cref="DropDownAlignment.Position"/>.
        /// </summary>
        public bool IsPositionUsed => IsHorzPositionUsed || IsVertPositionUsed;

        /// <summary>
        /// Gets a value indicating whether the vertical position is negative.
        /// Returns <c>true</c> if <see cref="IsVertPositionUsed"/> is <c>true</c> and
        /// <see cref="Position"/> has a negative Y coordinate.
        /// </summary>
        public bool IsVertPositionNegative => IsVertPositionUsed && Position.Y < 0;

        /// <summary>
        /// Gets a value indicating whether the horizontal position is negative.
        /// Returns <c>true</c> if <see cref="IsHorzPositionUsed"/> is <c>true</c>
        /// and <see cref="Position"/> has a negative X coordinate.
        /// </summary>
        public bool IsHorzPositionNegative => IsHorzPositionUsed && Position.X < 0;

        /// <summary>
        /// Gets a value indicating whether either the horizontal or vertical position is negative.
        /// Returns <c>true</c> if <see cref="IsPositionUsed"/> is <c>true</c> and <see cref="Position"/>
        /// has a negative X or Y coordinate.
        /// </summary>
        public bool IsPositionNegative => IsPositionUsed && (Position.X < 0 || Position.Y < 0);

        /// <summary>
        /// Gets the position of the object in a two-dimensional space.
        /// It is used when Horizontal or Vertical alignment is set to <see cref="DropDownAlignment.Position"/>.
        /// </summary>
        public PointD Position { get; }

        /// <summary>
        /// Gets the horizontal drop-down alignment.
        /// </summary>
        public DropDownAlignment Horizontal { get; } = DropDownAlignment.AfterStart;

        /// <summary>
        /// Gets the vertical drop-down alignment.
        /// </summary>
        public DropDownAlignment Vertical { get; } = DropDownAlignment.AfterEnd;

        /// <summary>
        /// Defines an implicit conversion from <see cref="PointD"/> to <see cref="HVDropDownAlignment"/>.
        /// </summary>
        /// <param name="point">The point to convert.</param>
        public static implicit operator HVDropDownAlignment(PointD point)
        {
            return new HVDropDownAlignment(point);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{Horizontal}, {Vertical}";
        }
    }
}
