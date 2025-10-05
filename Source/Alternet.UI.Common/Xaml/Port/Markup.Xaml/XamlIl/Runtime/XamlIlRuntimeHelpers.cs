#pragma warning disable
#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Alternet.UI.Data;
using Alternet.UI.Controls;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global

namespace Alternet.UI.Port
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to
    /// be used directly from your code.
    /// </summary>
    public static class XamlIlRuntimeHelpers
    {
        /*/// <summary>
        /// This item supports the framework infrastructure and is not intended
        /// to be used directly from your code.
        /// </summary>
        public static Func<IServiceProvider, object> DeferredTransformationFactoryV1(
            Func<IServiceProvider, object> builder,
            IServiceProvider provider)
        {
            return DeferredTransformationFactoryV2<Alternet.UI.Control>(builder, provider);
        }*/

        /*/// <summary>
        /// This item supports the framework infrastructure and is not intended
        /// to be used directly from your code.
        /// </summary>
        public static Func<IServiceProvider, object> DeferredTransformationFactoryV2<T>(
            Func<IServiceProvider, object> builder,
            IServiceProvider provider)
        {
            var resourceNodes = provider.GetService<IUixmlPortXamlIlParentStackProvider>().Parents
                .OfType<IResourceNode>().ToList();
            var rootObject = provider.GetService<IUixmlRootObjectProvider>().RootObject;
            var parentScope = provider.GetService<INameScope>();
            return sp =>
            {
                throw new Exception();
            };
        }*/

        class DeferredParentServiceProvider :
            IUixmlPortXamlIlParentStackProvider,
            IServiceProvider,
            IUixmlRootObjectProvider
        {
            private readonly IServiceProvider _parentProvider;
            private readonly List<IResourceNode> _parentResourceNodes;
            private readonly INameScope _nameScope;

            public DeferredParentServiceProvider(IServiceProvider parentProvider, List<IResourceNode> parentResourceNodes,
                object rootObject, INameScope nameScope)
            {
                _parentProvider = parentProvider;
                _parentResourceNodes = parentResourceNodes;
                _nameScope = nameScope;
                RootObject = rootObject;
            }

            public IEnumerable<object> Parents => GetParents();

            IEnumerable<object> GetParents()
            {
                if(_parentResourceNodes == null)
                    yield break;
                foreach (var p in _parentResourceNodes)
                    yield return p;
            }

            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(INameScope))
                    return _nameScope;
                if (serviceType == typeof(IUixmlPortXamlIlParentStackProvider))
                    return this;
                if (serviceType == typeof(IUixmlRootObjectProvider))
                    return this;
                return _parentProvider?.GetService(serviceType);
            }

            public object RootObject { get; }
            public object IntermediateRootObject => RootObject;
        }

        /// <summary>
        /// This item supports the framework infrastructure
        /// and is not intended to be used directly from your code.
        /// </summary>
        public static IServiceProvider CreateInnerServiceProviderV1(IServiceProvider compiled) 
            => new InnerServiceProvider(compiled);
       
        class InnerServiceProvider : IServiceProvider
        {
            private readonly IServiceProvider _compiledProvider;
            private XamlTypeResolver _resolver;

            public InnerServiceProvider(IServiceProvider compiledProvider)
            {
                _compiledProvider = compiledProvider;
            }
            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(IXamlTypeResolver))
                    return _resolver ?? (_resolver = new XamlTypeResolver(
                               _compiledProvider.GetService<IUixmlPortXamlIlXmlNamespaceInfoProvider>()));
                return null;
            }
        }

        class XamlTypeResolver : IXamlTypeResolver
        {
            private readonly IUixmlPortXamlIlXmlNamespaceInfoProvider _nsInfo;

            public XamlTypeResolver(IUixmlPortXamlIlXmlNamespaceInfoProvider nsInfo)
            {
                _nsInfo = nsInfo;
            }
            
            public Type Resolve(string qualifiedTypeName)
            {
                var sp = qualifiedTypeName.Split(new[] {':'}, 2);
                var (ns, name) = sp.Length == 1 ? ("", qualifiedTypeName) : (sp[0], sp[1]);
                var namespaces = _nsInfo.XmlNamespaces;
                var dic = (Dictionary<string, IReadOnlyList<UixmlPortXamlIlXmlNamespaceInfo>>)namespaces;
                if (!namespaces.TryGetValue(ns, out var lst))
                    throw new ArgumentException("Unable to resolve namespace for type " + qualifiedTypeName);
                foreach (var entry in lst)
                {
                    var asm = Assembly.Load(new AssemblyName(entry.ClrAssemblyName));
                    var resolved = asm.GetType(entry.ClrNamespace + "." + name);
                    if (resolved != null)
                        return resolved;
                }

                throw new ArgumentException(
                    $"Unable to resolve type {qualifiedTypeName} from any of the following locations: " +
                    string.Join(",", lst.Select(e => $"`{e.ClrAssemblyName}:{e.ClrNamespace}.{name}`")));
            }
        }

        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        [Obsolete("Don't use", true)]
        public static readonly IServiceProvider RootServiceProviderV1 = new RootServiceProvider(/*null*/);

        #line hidden
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// Don't emit debug symbols for this code so debugger will be forced to step into XAML instead
        /// </summary>
        public static IServiceProvider CreateRootServiceProviderV2()
        {
            return new RootServiceProvider(/*new NameScope()*/);
        }
        #line default
        
        class RootServiceProvider : IServiceProvider, IUixmlPortXamlIlParentStackProvider
        {
            //private readonly INameScope _nameScope;

            public RootServiceProvider(/*INameScope nameScope*/)
            {
                //_nameScope = nameScope;
            }
            
            public object GetService(Type serviceType)
            {
                if (serviceType == typeof(INameScope))
                    return null;
                    //throw new Exception();
                    //return _nameScope;
                if (serviceType == typeof(IUixmlPortXamlIlParentStackProvider))
                    return this;
                return null;
            }

            public IEnumerable<object> Parents
            {
                get
                {
                    throw new Exception();
                    //if (Application.Current != null)
                    //    yield return Application.Current;
                }
            }
        }
    }
}
