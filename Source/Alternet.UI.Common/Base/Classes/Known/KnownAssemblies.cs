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
        public static readonly Assembly LibraryCommon = typeof(AbstractControl).Assembly;

        /// <summary>
        /// Gets 'Alternet.UI.Interfaces' assembly.
        /// </summary>
        public static readonly Assembly LibraryInterfaces = typeof(DockStyle).Assembly;

        private static List<Assembly>? allAlternet;

        /// <summary>
        /// Gets list of assemblies which includes <see cref="LibraryInterfaces"/>,
        /// <see cref="LibraryCommon"/> and all loaded assemblies which
        /// use <see cref="LibraryCommon"/>.
        /// </summary>
        public static List<Assembly> AllAlternet
        {
            get
            {
                if(allAlternet is null)
                {
                    allAlternet = new();
                    allAlternet.Add(LibraryCommon);
                    allAlternet.Add(LibraryInterfaces);

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var assembly in assemblies)
                    {
                        if(AssemblyUtils.IsAssemblyReferencedFrom(assembly, allAlternet))
                            allAlternet.Add(assembly);
                    }
                }

                return allAlternet;
            }
        }
    }
}
