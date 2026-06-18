using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents event data for formatting a value into a display-friendly string.
    /// </summary>
    /// <typeparam name="T">Type of the value to format.</typeparam>
    public class FormatValueEventArgs<T> : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatValueEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The value to format.</param>
        public FormatValueEventArgs(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the raw value to format.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the formatted representation of the value.
        /// </summary>
        public string? FormattedValue { get; set; }
    }
}
