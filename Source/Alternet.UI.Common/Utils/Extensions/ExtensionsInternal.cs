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
    }
}
