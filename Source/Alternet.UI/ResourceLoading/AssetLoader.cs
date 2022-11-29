using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Loads resources compiled into the application binary.
    /// </summary>
    public class ResourceLoader
    {
        private static IAssemblyDescriptorResolver s_assemblyDescriptorResolver = new AssemblyDescriptorResolver();

        private AssemblyDescriptor? _defaultEmbresAssembly;

        /// <remarks>
        /// Introduced for tests.
        /// </remarks>
        internal static void SetAssemblyDescriptorResolver(IAssemblyDescriptorResolver resolver) =>
            s_assemblyDescriptorResolver = resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceLoader"/> class.
        /// </summary>
        /// <param name="assembly">
        /// The default assembly from which to load embres: assets for which no assembly is specified.
        /// </param>
        public ResourceLoader(Assembly? assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetEntryAssembly();
            if (assembly != null)
                _defaultEmbresAssembly = new AssemblyDescriptor(assembly);
        }

        /// <summary>
        /// Sets the default assembly from which to load assets for which no assembly is specified.
        /// </summary>
        /// <param name="assembly">The default assembly.</param>
        public void SetDefaultAssembly(Assembly assembly)
        {
            _defaultEmbresAssembly = new AssemblyDescriptor(assembly);
        }

        /// <summary>
        /// Checks if an asset with the specified URI exists.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">
        /// A base URI to use if <paramref name="uri"/> is relative.
        /// </param>
        /// <returns>True if the asset could be found; otherwise false.</returns>
        public bool Exists(Uri uri, Uri? baseUri = null)
        {
            return GetAsset(uri, baseUri) != null;
        }

        /// <summary>
        /// Opens the asset with the requested URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">
        /// A base URI to use if <paramref name="uri"/> is relative.
        /// </param>
        /// <returns>A stream containing the asset contents.</returns>
        /// <exception cref="FileNotFoundException">
        /// The asset could not be found.
        /// </exception>
        public Stream Open(Uri uri, Uri? baseUri = null) => OpenAndGetAssembly(uri, baseUri).Item1;

        /// <summary>
        /// Opens the asset with the requested URI and returns the asset stream and the
        /// assembly containing the asset.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">
        /// A base URI to use if <paramref name="uri"/> is relative.
        /// </param>
        /// <returns>
        /// The stream containing the resource contents together with the assembly.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// The asset could not be found.
        /// </exception>
        public (Stream stream, Assembly assembly) OpenAndGetAssembly(Uri uri, Uri? baseUri = null)
        {
            var asset = GetAsset(uri, baseUri);

            if (asset == null)
            {
                throw new FileNotFoundException($"The resource {uri} could not be found.");
            }

            return (asset.GetStream(), asset.Assembly);
        }

        internal Assembly? GetAssembly(Uri uri, Uri? baseUri)
        {
            if (!uri.IsAbsoluteUri && baseUri != null)
                uri = new Uri(baseUri, uri);
            return GetAssembly(uri)?.Assembly;
        }

        /// <summary>
        /// Gets all assets of a folder and subfolders that match specified uri.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">Base URI that is used if <paramref name="uri"/> is relative.</param>
        /// <returns>All matching assets as a tuple of the absolute path to the asset and the assembly containing the asset</returns>
        public IEnumerable<Uri> GetAssets(Uri uri, Uri? baseUri)
        {
            if (uri.IsAbsoluteEmbres())
            {
                var assembly = GetAssembly(uri);

                return assembly?.Resources?
                           .Where(x => x.Key.IndexOf(uri.GetUnescapeAbsolutePath(), StringComparison.Ordinal) >= 0)
                           .Select(x => new Uri($"embres:{x.Key}?assembly={assembly.Name}")) ??
                       Enumerable.Empty<Uri>();
            }

            uri = uri.EnsureAbsolute(baseUri);
            if (uri.IsUires())
            {
                var (asm, path) = GetResAsmAndPath(uri);
                if (asm == null)
                {
                    throw new ArgumentException(
                        "No default assembly, entry assembly or explicit assembly specified; " +
                        "don't know where to look up for the resource, try specifying assembly explicitly.");
                }

                if (asm.UIResources == null)
                    return Enumerable.Empty<Uri>();

                if (path[path.Length - 1] != '/')
                    path += '/';

                return asm.UIResources
                    .Where(r => r.Key.StartsWith(path, StringComparison.Ordinal))
                    .Select(x => new Uri($"uires://{asm.Name}{x.Key}"));
            }

            return Enumerable.Empty<Uri>();
        }

        private IAssetDescriptor? GetAsset(Uri uri, Uri? baseUri)
        {
            if (uri.IsAbsoluteEmbres())
            {
                var asm = GetAssembly(uri) ?? GetAssembly(baseUri) ?? _defaultEmbresAssembly;

                if (asm == null)
                {
                    throw new ArgumentException(
                        "No default assembly, entry assembly or explicit assembly specified; " +
                        "don't know where to look up for the resource, try specifying assembly explicitly.");
                }

                var resourceKey = uri.AbsolutePath;
                IAssetDescriptor? rv = null;
                asm.Resources?.TryGetValue(resourceKey, out rv);
                return rv;
            }

            uri = uri.EnsureAbsolute(baseUri);

            if (uri.IsUires())
            {
                var (asm, path) = GetResAsmAndPath(uri);
                if (asm.UIResources == null)
                    return null;
                asm.UIResources.TryGetValue(path, out var desc);
                return desc;
            }

            throw new ArgumentException($"Unsupported url type: " + uri.Scheme, nameof(uri));
        }

        private (IAssemblyDescriptor asm, string path) GetResAsmAndPath(Uri uri)
        {
            var asm = s_assemblyDescriptorResolver.GetAssembly(uri.Authority);
            return (asm, uri.GetUnescapeAbsolutePath());
        }

        private IAssemblyDescriptor? GetAssembly(Uri? uri)
        {
            if (uri != null)
            {
                if (!uri.IsAbsoluteUri)
                    return null;
                if (uri.IsUires())
                    return GetResAsmAndPath(uri).asm;

                if (uri.IsEmbres())
                {
                    var assemblyName = uri.GetAssemblyNameFromQuery();
                    if (assemblyName.Length > 0)
                        return s_assemblyDescriptorResolver.GetAssembly(assemblyName);
                }
            }

            return null;
        }

        internal static void RegisterResUriParsers()
        {
            if (!UriParser.IsKnownScheme("uires"))
                UriParser.Register(new GenericUriParser(
                    GenericUriParserOptions.GenericAuthority |
                    GenericUriParserOptions.NoUserInfo |
                    GenericUriParserOptions.NoPort |
                    GenericUriParserOptions.NoQuery |
                    GenericUriParserOptions.NoFragment), "uires", -1);
        }
    }
}