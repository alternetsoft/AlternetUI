﻿using System;
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
        /// Represents the alignment settings for a dropdown positioned at a specific point.
        /// </summary>
        /// <param name="point">The position of the dropdown as a <see cref="PointD"/>.</param>
        public HVDropDownAlignment(PointD point)
        {
            Horizontal = DropDownAlignment.Position;
            Vertical = DropDownAlignment.Position;
            Position = point;
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
        /// Gets the default drop-down alignment.
        /// </summary>
        public static HVDropDownAlignment Default => new(
            DropDownAlignment.AfterStart,
            DropDownAlignment.AfterEnd);

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
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{Horizontal}, {Vertical}";
        }
    }
}
