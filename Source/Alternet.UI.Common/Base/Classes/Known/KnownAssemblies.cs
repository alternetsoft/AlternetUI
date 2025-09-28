using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

using ShimSkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// Contains some of the known assembly references.
    /// </summary>
    public static class KnownAssemblies
    {
        /// <summary>
        /// Gets 'Alternet.UI.Maui' assembly if it is used in the application.
        /// </summary>
        public static readonly LazyStruct<Assembly?> LibraryMaui = new(() =>
        {
            return AssemblyUtils.GetAssemblyByName("Alternet.UI.Maui");
        });

        /// <summary>
        /// Gets 'Alternet.UI' assembly if it is used in the application.
        /// </summary>
        public static readonly LazyStruct<Assembly?> LibraryAlternetUI = new(() =>
        {
            return AssemblyUtils.GetAssemblyByName("Alternet.UI");
        });

        /// <summary>
        /// Gets 'Microsoft.Maui' assembly if it is used in the application.
        /// </summary>
        public static readonly LazyStruct<Assembly?> LibraryMicrosoftMaui = new(() =>
        {
            return AssemblyUtils.GetAssemblyByName("Microsoft.Maui");
        });

        /// <summary>
        /// Gets or loads 'Microsoft.Maui.Essentials' assembly.
        /// </summary>
        public static readonly LazyStruct<Assembly?> LibraryMicrosoftMauiEssentials = new(() =>
        {
            return AssemblyUtils.GetOrLoadAssemblyByName("Microsoft.Maui.Essentials");
        });

        /// <summary>
        /// Gets or loads 'System.Runtime.InteropServices' assembly.
        /// </summary>
        public static readonly LazyStruct<Assembly?> LibraryInteropServices = new(() =>
        {
            return AssemblyUtils.GetOrLoadAssemblyByName("System.Runtime.InteropServices");
        });

        /// <summary>
        /// Gets 'Alternet.UI.Common' assembly.
        /// </summary>
        public static readonly Assembly LibraryCommon = typeof(AbstractControl).Assembly;

        private static bool preloadReferencedCalled;
        private static List<Assembly>? allAlternet;

        /// <summary>
        /// Gets list of assemblies which includes <see cref="LibraryCommon"/> and
        /// all loaded assemblies which use <see cref="LibraryCommon"/>.
        /// </summary>
        public static List<Assembly> AllLoadedAlternet
        {
            get
            {
                if(allAlternet is null)
                {
                    allAlternet = new();
                    allAlternet.Add(LibraryCommon);

                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var assembly in assemblies)
                    {
                        if(AssemblyUtils.IsAssemblyReferencedFrom(assembly, allAlternet))
                            allAlternet.Add(assembly);
                    }
                }

                return allAlternet;
            }

            set
            {
                allAlternet = value;
            }
        }

        /// <summary>
        /// Gets list of assemblies which includes <see cref="LibraryCommon"/>
        /// and all loaded assemblies which
        /// use <see cref="LibraryCommon"/>.
        /// </summary>
        public static IEnumerable<AssemblyName> AllReferenced
        {
            get
            {
                Assembly executingAssembly = Assembly.GetEntryAssembly();
                AssemblyName[] referencedAssemblies = executingAssembly.GetReferencedAssemblies();
                return referencedAssemblies;
            }
        }

        /// <summary>
        /// Preloads all referenced assemblies in the current application domain.
        /// This method queues the loading of each referenced assembly to the thread pool.
        /// </summary>
        public static void PreloadReferenced()
        {
            try
            {
                if (preloadReferencedCalled)
                    return;
                preloadReferencedCalled = true;

                var allReferenced = Alternet.UI.KnownAssemblies.AllReferenced;

                foreach (var asm in allReferenced)
                {
                    ThreadPool.QueueUserWorkItem((object? state) =>
                    {
                        var asmLoaded = Assembly.Load(asm);
                    });
                }
            }
            catch
            {
            }
        }
    }
}
