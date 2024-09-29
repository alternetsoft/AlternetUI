using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains some of the known assembly references.
    /// </summary>
    public static class KnownAssemblies
    {
        /// <summary>
        /// Gets 'Alternet.UI.Maui' assembly if it is referenced by the application.
        /// </summary>
        public static readonly LazyStruct<Assembly?> LibraryMaui = new(() =>
        {
            return AssemblyUtils.GetAssemblyByName("Alternet.UI.Maui");
        });

        /// <summary>
        /// Gets 'Alternet.UI.Common' assembly.
        /// </summary>
        public static readonly Assembly LibraryCommon = typeof(Control).Assembly;

        /// <summary>
        /// Gets 'Alternet.UI.Interfaces' assembly.
        /// </summary>
        public static readonly Assembly LibraryInterfaces = typeof(DockStyle).Assembly;
    }
}
