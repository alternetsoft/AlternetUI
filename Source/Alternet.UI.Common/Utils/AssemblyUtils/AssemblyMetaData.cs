using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods to load assembly metadata.
    /// </summary>
    public static class AssemblyMetaData
    {
        /// <summary>
        /// A dictionary to store metadata load contexts.
        /// </summary>
        public static readonly ConcurrentDictionary<string, MetadataLoadContext> Contexts = new ();

        /// <summary>
        /// A thread-safe dictionary that stores loaded assemblies, keyed by their names.
        /// </summary>
        /// <remarks>This dictionary is used to manage and retrieve assemblies that have
        /// been loaded into
        /// the application. It ensures thread-safe access and modification, making
        /// it suitable for concurrent
        /// operations.</remarks>
        public static readonly ConcurrentDictionary<string, Assembly> LoadedAssemblies = new ();

        private static readonly object LoadAssemblyMetadataLocker = new object();

        /// <summary>
        /// Gets or sets a value indicating whether loaded assemblies should be cached.
        /// If set to <c>true</c> (default value), loaded assemblies will be stored in a cache to
        /// improve performance on subsequent loads.
        /// </summary>
        public static bool CacheLoadedAssemblies { get; set; } = true;

        /// <summary>
        /// Clears the cache of metadata load contexts.
        /// </summary>
        public static void ResetMetadataLoadContextsCache()
        {
            Contexts.Clear();
        }

        /// <summary>
        /// Clears the cache of loaded assemblies.
        /// </summary>
        public static void ResetLoadedAssembliesCache()
        {
            LoadedAssemblies.Clear();
        }

        /// <summary>
        /// Loads an assembly metadata from a specific path on the disk.
        /// </summary>
        /// <param name="pathToDll">The path to the assembly.</param>
        /// <param name="context">The optional context to load the assembly from.</param>
        /// <returns>The loaded assembly.</returns>
        public static Assembly? LoadAssemblyMetadata(
            string pathToDll,
            MetadataLoadContext? context = null)
        {
            Assembly? assembly = null;

            if (CacheLoadedAssemblies)
            {
                if (LoadedAssemblies.TryGetValue(pathToDll, out assembly))
                {
                    return assembly;
                }
            }

            var folder = Path.GetDirectoryName(pathToDll);

            if (string.IsNullOrEmpty(folder) || !File.Exists(pathToDll))
            {
                try
                {
                    assembly = Assembly.Load(pathToDll);
                }
                catch
                {
                }
            }
            else
            {
                if (context == null)
                {
                    lock (LoadAssemblyMetadataLocker)
                    {
                        if (!Contexts.TryGetValue(folder, out context))
                        {
                            context = CreateContext(folder);
                            Contexts[folder] = context;
                        }
                    }
                }

                assembly = context.LoadFromAssemblyPath(pathToDll);
            }

            if (assembly is not null && CacheLoadedAssemblies)
            {
                LoadedAssemblies.TryAdd(pathToDll, assembly);
            }

            return assembly;
        }

        /// <summary>
        /// Creates a new <see cref="MetadataLoadContext"/> for the specified folder.
        /// </summary>
        /// <param name="folder">The folder to create the context for.</param>
        /// <returns>A new <see cref="MetadataLoadContext"/>.</returns>
        private static MetadataLoadContext CreateContext(string folder)
        {
            HashSet<string> folders = new ();
            var location = typeof(object).Assembly.Location;
            if (!string.IsNullOrEmpty(location))
            {
                var dirName = Path.GetDirectoryName(location);
                if (!string.IsNullOrEmpty(dirName))
                    folders.Add(dirName);
            }

            location = RuntimeEnvironment.GetRuntimeDirectory();
            if (!string.IsNullOrEmpty(location))
            {
                var dirName = Path.GetDirectoryName(location);
                if (!string.IsNullOrEmpty(dirName))
                    folders.Add(dirName);
            }

            folders.Add(folder);

            List<string> assemblies = new ();
            foreach (var path in folders)
            {
                foreach (var file in Directory.GetFiles(path, "*.dll"))
                    assemblies.Add(file);
            }

            var resolver = new PathAssemblyResolver(assemblies);
            return new MetadataLoadContext(resolver);
        }
    }
}
