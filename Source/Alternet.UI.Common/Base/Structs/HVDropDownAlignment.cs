using System;
using System.Collections.Generic;
using System.Text;

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
        /// Gets the default drop-down alignment.
        /// </summary>
        public static HVDropDownAlignment Default => new(
            DropDownAlignment.AfterStart,
            DropDownAlignment.AfterEnd);

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
