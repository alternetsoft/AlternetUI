#pragma warning disable
#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;
using Alternet.UI.Markup;

namespace Alternet.UI.Port
{
    internal static class ServiceProviderExt
    {
        public static T GetService<T>(this IServiceProvider sp) => (T)sp?.GetService(typeof(T));
        
        public static Uri GetContextBaseUri(this IServiceProvider ctx) => ctx.GetService<IUixmlUriContext>().BaseUri;

        public static T GetFirstParent<T>(this IServiceProvider ctx) where T : class 
            => ctx.GetService<IUixmlPortXamlIlParentStackProvider>().Parents.OfType<T>().FirstOrDefault();

        public static T GetLastParent<T>(this IServiceProvider ctx) where T : class 
            => ctx.GetService<IUixmlPortXamlIlParentStackProvider>().Parents.OfType<T>().LastOrDefault();

        public static IEnumerable<T> GetParents<T>(this IServiceProvider sp)
        {
            return sp.GetService<IUixmlPortXamlIlParentStackProvider>().Parents.OfType<T>();
        }

        public static Type ResolveType(this IServiceProvider ctx, string namespacePrefix, string type)
        {
            var tr = ctx.GetService<IXamlTypeResolver>();
            string name = string.IsNullOrEmpty(namespacePrefix) ? type : $"{namespacePrefix}:{type}";
            return tr?.Resolve(name);
        }

        public static object GetDefaultAnchor(this IServiceProvider provider)
        {
            object anchor = provider.GetFirstParent<IControl>();

            if (anchor is null)
            {
                anchor = provider.GetFirstParent<FrameworkElement>();
            }

            return anchor;
        }
    }
}
