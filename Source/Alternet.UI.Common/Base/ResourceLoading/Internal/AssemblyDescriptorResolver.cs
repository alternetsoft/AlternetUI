using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Alternet.UI
{
    internal class AssemblyDescriptorResolver : IAssemblyDescriptorResolver
    {
        private readonly Dictionary<string, IAssemblyDescriptor> assemblyNameCache = new();

        public IAssemblyDescriptor? GetAssemblyFromUrl(Uri url)
        {
            var resName = url.AbsolutePath;

            var assembly = AssemblyUtils.FindAssemblyForResource(resName);

            if (assembly is null)
                return null;

            return GetOrLoadAssemblyDescriptor(assembly.GetName().Name!, assembly);
        }

        public IAssemblyDescriptor GetOrLoadAssemblyDescriptor(string name, Assembly assembly)
        {
            if (!assemblyNameCache.TryGetValue(name, out var result))
            {
                result = LoadAssemblyDescriptor(name, assembly);
            }

            return result;
        }

        public IAssemblyDescriptor LoadAssemblyDescriptor(string name, Assembly assembly)
        {
            var result = new AssemblyDescriptor(assembly);
            assemblyNameCache[name] = result;
            return result;
        }

        public IAssemblyDescriptor GetAssembly(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (!assemblyNameCache.TryGetValue(name, out var rv))
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                var match = loadedAssemblies.FirstOrDefault(a => a.GetName().Name == name);

                if (match != null)
                {
                    rv = LoadAssemblyDescriptor(name, match);
                }
                else
                {
                    if (!RuntimeFeature.IsDynamicCodeSupported)
                    {
                        throw new InvalidOperationException(
                            $"Assembly {name} needs to be referenced and explicitly loaded before loading resources");
                    }

                    name = Uri.UnescapeDataString(name);
                    rv = LoadAssemblyDescriptor(name, Assembly.Load(name));
                }
            }

            return rv;
        }
    }

#pragma warning disable
    internal interface IAssemblyDescriptorResolver
    {
        IAssemblyDescriptor GetAssembly(string name);
        IAssemblyDescriptor? GetAssemblyFromUrl(Uri url);
    }
#pragma warning restore
}