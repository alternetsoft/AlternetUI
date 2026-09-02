using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI.Extensions
{
    /// <summary>
    /// Contains extension methods for internal use.
    /// </summary>
    public static partial class ExtensionsInternal
    {
        /// <summary>
        /// Returns a string representation of the specified object, or an empty string if the object is null.
        /// </summary>
        /// <param name="obj">The object to convert to a string.</param>
        /// <returns>A string representation of the object, or an empty string if the object is null.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string SafeToString(this object? obj)
        {
            return obj?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Converts a <see cref="DateOnly"/> to a <see cref="DateTime"/> by combining it with the minimum time value (00:00:00).
        /// </summary>
        /// <param name="date">The <see cref="DateOnly"/> to convert.</param>
        /// <returns>A <see cref="DateTime"/> representing the specified <see cref="DateOnly"/>
        /// combined with the minimum time value (00:00:00).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime(this DateOnly date)
        {
            return date.ToDateTime(TimeOnly.MinValue);
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a <see cref="DateOnly"/> by extracting the date component.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
        /// <returns>A <see cref="DateOnly"/> representing the date component of the specified <see cref="DateTime"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateOnly ToDateOnly(this DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        /// <summary>
        /// Converts a <see cref="DateTime"/> to a <see cref="TimeOnly"/> by extracting the time component.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
        /// <returns>A <see cref="TimeOnly"/> representing the time component of the specified <see cref="DateTime"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TimeOnly ToTimeOnly(this DateTime dateTime)
        {
            return TimeOnly.FromDateTime(dateTime);
        }
    }
}
