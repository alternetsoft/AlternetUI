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
                    assemblyNameCache[name] = rv = new AssemblyDescriptor(match);
                }
                else
                {
#if NET6_0_OR_GREATER
                    if (!RuntimeFeature.IsDynamicCodeSupported)
                    {
                        throw new InvalidOperationException(
                            $"Assembly {name} needs to be referenced and explicitly loaded before loading resources");
                    }
#endif
                    name = Uri.UnescapeDataString(name);
                    assemblyNameCache[name] = rv = new AssemblyDescriptor(Assembly.Load(name));
                }
            }

            return rv;
        }
    }

#pragma warning disable
    internal interface IAssemblyDescriptorResolver
    {
        IAssemblyDescriptor GetAssembly(string name);
    }
#pragma warning restore
}