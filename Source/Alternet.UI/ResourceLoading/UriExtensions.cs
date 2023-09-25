using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal static class UriExtensions
    {
        public static bool IsAbsoluteEmbres(this Uri uri) =>
            uri.IsAbsoluteUri && uri.IsEmbres();

        public static bool IsEmbres(this Uri uri) => uri.Scheme == "embres";

        public static bool IsUires(this Uri uri) => uri.Scheme == "uires";

        public static Uri EnsureAbsolute(this Uri uri, Uri? baseUri)
        {
            if (uri.IsAbsoluteUri)
                return uri;
            if (baseUri == null)
                throw new ArgumentException($"Relative uri {uri} without base url");
            if (!baseUri.IsAbsoluteUri)
                throw new ArgumentException($"Base uri {baseUri} is relative");
            if (baseUri.IsEmbres())
            {
                throw new ArgumentException(
                    $"Relative uris for 'embres' scheme aren't supported; {baseUri} uses embres");
            }

            return new Uri(baseUri, uri);
        }

        public static string GetUnescapeAbsolutePath(this Uri uri) =>
            Uri.UnescapeDataString(uri.AbsolutePath);

        public static string GetUnescapeAbsoluteUri(this Uri uri) =>
            Uri.UnescapeDataString(uri.AbsoluteUri);

        public static string GetAssemblyNameFromQuery(this Uri uri)
        {
            const string assembly = "assembly";

            var query = Uri.UnescapeDataString(uri.Query);

            // Skip the '?'
            var currentIndex = 1;
            while (currentIndex < query.Length)
            {
                var isFind = false;
                for (var i = 0; i < assembly.Length; ++currentIndex, ++i)
                {
                    if (query[currentIndex] == assembly[i])
                    {
                        isFind = i == assembly.Length - 1;
                    }
                    else
                    {
                        break;
                    }
                }

                // Skip the '='
                ++currentIndex;

                var beginIndex = currentIndex;
                while (currentIndex < query.Length && query[currentIndex] != '&')
                    ++currentIndex;

                if (isFind)
#pragma warning disable
                    return query.Substring(beginIndex, currentIndex - beginIndex);
#pragma warning restore

                ++currentIndex;
            }

            return string.Empty;
        }
    }
}