using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains refrences to some of the known types.
    /// </summary>
    public static class KnownTypes
    {
        /// <summary>
        /// Gets 'Alternet.UI.MauiUtils' type.
        /// </summary>
        public static LazyStruct<Type?> MauiUtils = new(() =>
        {
            var result = KnownAssemblies.LibraryMaui.Value?.GetType("Alternet.UI.MauiUtils");
            return result;
        });
    }
}
