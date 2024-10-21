using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI.Extensions
{
    internal static partial class ExtensionsInternal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string SafeToString(this object? obj)
        {
            return obj?.ToString() ?? string.Empty;
        }
    }
}
