#pragma warning disable
#nullable disable
#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;

using XamlX.TypeSystem;

namespace XamlX.Transform
{
#if !XAMLX_INTERNAL
    public
#endif
    static class NamespaceInfoHelper
    {
        public class NamespaceResolveResult
        {
            private string clrNamespace;

            public string ClrNamespace
            {
                get => clrNamespace;
                
                set
                {
                    clrNamespace = value;
                }
            }

            public IXamlAssembly Assembly { get; set; }

            public string AssemblyName { get; set; }
        }

        public static List<NamespaceResolveResult> TryResolve(
            TransformerConfiguration config,
            string xmlns)
        {
            if (config.XmlnsMappings.Namespaces.TryGetValue(xmlns, out var lst))
            {
                return lst.Select(p => new NamespaceResolveResult
                {
                    ClrNamespace = p.ns,
                    Assembly = p.asm,
                }).ToList();
            }

            var prefixStringComparison = StringComparison.Ordinal;

            const string clrNamespace = "clr-namespace:";
            const string assemblyNamePrefix = ";assembly=";
            if (xmlns.StartsWith(clrNamespace, prefixStringComparison))
            {
                var ns = xmlns.Substring(clrNamespace.Length);

                var indexOfAssemblyPrefix = ns.IndexOf(assemblyNamePrefix, prefixStringComparison);
                string asm = config.DefaultAssembly?.Name;
                if (indexOfAssemblyPrefix != -1)
                {
                    asm = ns.Substring(indexOfAssemblyPrefix + assemblyNamePrefix.Length).Trim();
                    ns = ns.Substring(0, indexOfAssemblyPrefix);
                }

                return new List<NamespaceResolveResult>
                {
                    new NamespaceResolveResult
                    {
                        ClrNamespace = ns,
                        AssemblyName = asm,
                    },
                };
            }

            const string usingPrefix = "using:";
            if (xmlns.StartsWith(usingPrefix, prefixStringComparison))
            {
                var ns = xmlns.Substring(usingPrefix.Length);
                return new List<NamespaceResolveResult>
                {
                    new NamespaceResolveResult
                    {
                        ClrNamespace = ns,
                    },
                };
            }

            return null;
        }
    }
}
