using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the object types handling.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        /// Gets whether the specified object is <see cref="string"/>.
        /// </summary>
        /// <param name="s">Object to test.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsString(object? s)
        {
            return s?.GetType() == typeof(string);
        }
    }
}
