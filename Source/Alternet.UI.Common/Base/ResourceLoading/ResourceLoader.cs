using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Loads resources compiled into the application binary.
    /// </summary>
    public class ResourceLoader
    {
        private static ResourceLoader? defaultLoader;
        private static IAssemblyDescriptorResolver assemblyDescriptorResolver =
            new AssemblyDescriptorResolver();

        private AssemblyDescriptor? defaultEmbresAssembly;

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
                defaultEmbresAssembly = new AssemblyDescriptor(assembly);
        }

        /// <summary>
        /// Occurs when <see cref="StreamFromUrl"/> is called. You can implement
        /// <see cref="CustomStreamFromUrl"/> event handler in order to perform custom url processing.
        /// </summary>
        public static event EventHandler<StreamFromUrlEventArgs>? CustomStreamFromUrl;

        /// <summary>
        /// Gets or sets default <see cref="ResourceLoader"/>.
        /// </summary>
        public static ResourceLoader Default
        {
            get
            {
                defaultLoader ??= new();
                return defaultLoader;
            }

            set
            {
                defaultLoader = value;
            }
        }

        /// <summary>
        /// Loads <see cref="Stream"/> from the specified url.
        /// </summary>
        /// <param name="url">Url used to load the data. By default "file" and "embres"
        /// protocols are supported but you can extend it with <see cref="CustomStreamFromUrl"/>
        /// event.
        /// </param>
        /// <example>
        /// <code>
        /// var ImageSize = 16;
        /// var ResPrefix = $"embres:ControlsTest.resources.Png._{ImageSize}.";
        /// var url = $"{ResPrefix}arrow-left-{ImageSize}.png"
        /// using var stream = ResourceLoader.StreamFromUrl(url);
        /// return new Bitmap(stream);
        /// </code>
        /// </example>
        /// <remarks>
        /// <param name="baseUri">Specifies base url if <paramref name="url"/> is not absolute.</param>
        /// <paramref name="url"/> with "embres" protocol can include assembly name. Example:
        /// "embres:Alternet.UI.Resources.Svg.ImageName.svg?assembly=Alternet.UI"
        /// </remarks>
        public static Stream StreamFromUrl(string url, Uri? baseUri = null)
        {
            if(CustomStreamFromUrl is not null)
            {
                StreamFromUrlEventArgs e = new(url);

                var list = CustomStreamFromUrl.GetInvocationList();

                foreach(var item in list)
                {
                    ((EventHandler<StreamFromUrlEventArgs>)item)(null, e);
                    if (e.Handled && e.Result is not null)
                        return e.Result;
                }
            }

            return DefaultStreamFromUrl(url, baseUri);
        }

        /// <summary>
        /// Loads a string from the specified resource URL or returns null if failed.
        /// </summary>
        /// <param name="url">The resource URL used to load the data.</param>
        /// <param name="encoding">The encoding to use for reading the stream.
        /// Optional. If not specified, <see cref="Encoding.UTF8"/> is used.</param>
        /// <returns>The string loaded from the URL, or <c>null</c> if an error occurs.</returns>
        public static string? StringFromUrlOrNull(string url, Encoding? encoding = null)
        {
            try
            {
                var stream = StreamFromUrlOrDefault(url);
                var str = StreamUtils.StringFromStreamOrNull(stream);
                return str;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Calls <see cref="StreamFromUrlOrDefault"/> and if it returns <c>null</c>,
        /// calls <paramref name="func"/>.
        /// </summary>
        /// <param name="url">Url used to load the data. By default "file" and "embres"
        /// protocols are supported but you can extend it with <see cref="CustomStreamFromUrl"/>
        /// event.
        /// </param>
        /// <param name="func">Function used to get stream from url in case if
        /// <see cref="StreamFromUrl"/> returns <c>null</c>.</param>
        /// <returns></returns>
        public static Stream? StreamFromUrlOrDefault(string url, Func<Stream?>? func = null)
        {
            var result = BaseObject.InsideTryCatch(() => StreamFromUrl(url));
            result ??= func?.Invoke();
            return result;
        }

        /// <summary>
        /// Default implementation of <see cref="StreamFromUrl"/>.
        /// See <see cref="StreamFromUrl"/> for details.
        /// </summary>
        /// <param name="url">Url used to load the data. By default "file" and "embres"
        /// protocols are supported but you can extend it with <see cref="CustomStreamFromUrl"/>
        /// event.
        /// </param>
        /// <param name="baseUri">Specifies base url if <paramref name="url"/> is not absolute.</param>
        /// <returns></returns>
        public static Stream DefaultStreamFromUrl(string url, Uri? baseUri = null)
        {
            var s = url.Trim();

            var isFile = s.StartsWith("file:");
            var isEmbres = s.StartsWith("embres:");
            var isUires = s.StartsWith("uires:");

            var hasScheme = isFile || isEmbres || isUires;

            if (hasScheme)
            {
                var uri = new Uri(s, UriKind.Absolute);
                return DefaultStreamFromUri(uri, baseUri);
            }
            else
            {
                string path;

                if (baseUri is null)
                {
                    path = PathUtils.GetFullPath(s, PathUtils.GetAppFolder());
                }
                else
                {
                    path = PathUtils.GetFullPath(s, baseUri.LocalPath);
                }

                var stream = FileSystem.Default.OpenRead(path);
                return stream;
            }
        }

        /// <summary>
        /// Default implementation of the open stream from the <see cref="Uri"/>.
        /// Used in <see cref="DefaultStreamFromUrl"/>.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="baseUri"></param>
        /// <returns></returns>
        public static Stream DefaultStreamFromUri(Uri uri, Uri? baseUri = null)
        {
            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                var stream = FileSystem.OpenRead(uri.LocalPath);
                return stream;
            }

            var result = ResourceLoader.Default.Open(uri, baseUri);
            return result;
        }

        /// <summary>
        /// Gets all assets of a folder and subfolders that match specified uri.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">Base URI that is used if <paramref name="uri"/> is relative.</param>
        /// <returns>All matching assets as a tuple of the absolute path to the asset and the
        /// assembly containing the asset</returns>
        public static IEnumerable<Uri> GetAssets(Uri uri, Uri? baseUri)
        {
            if (uri.IsAbsoluteEmbres())
            {
                var assembly = GetAssembly(uri);

                return assembly?.Resources?
                           .Where(x => x.Key.Contains(uri.GetUnescapeAbsolutePath()))
                           .Select(x => new Uri($"embres:{x.Key}?assembly={assembly.Name}")) ??
                       Enumerable.Empty<Uri>();
            }

            uri = uri.EnsureAbsolute(baseUri);
            if (uri.IsUires())
            {
                var (asm, path) = GetResAsmAndPath(uri);
                if (asm == null)
                {
                    throw new ArgumentException("Assembly is not specified");
                }

                if (asm.UIResources == null)
                    return Enumerable.Empty<Uri>();

#pragma warning disable
                if (path[path.Length - 1] != '/')
                    path += '/';
#pragma warning restore

                return asm.UIResources
                    .Where(r => r.Key.StartsWith(path, StringComparison.Ordinal))
                    .Select(x => new Uri($"uires://{asm.Name}{x.Key}"));
            }

            return Enumerable.Empty<Uri>();
        }

        /// <summary>
        /// Sets the default assembly from which to load assets for which
        /// no assembly is specified.
        /// </summary>
        /// <param name="assembly">The default assembly.</param>
        public virtual void SetDefaultAssembly(Assembly assembly)
        {
            defaultEmbresAssembly = new AssemblyDescriptor(assembly);
        }

        /// <summary>
        /// Checks if an asset with the specified URI exists.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="baseUri">
        /// A base URI to use if <paramref name="uri"/> is relative.
        /// </param>
        /// <returns>True if the asset could be found; otherwise false.</returns>
        public virtual bool Exists(Uri uri, Uri? baseUri = null)
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
        public virtual Stream Open(Uri uri, Uri? baseUri = null)
            => OpenAndGetAssembly(uri, baseUri).Stream;

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
        public virtual (Stream Stream, Assembly Assembly) OpenAndGetAssembly(
            Uri uri,
            Uri? baseUri = null)
        {
            var asset = GetAsset(uri, baseUri)
                ?? throw new FileNotFoundException($"The resource {uri} could not be found.");
            return (asset.GetStream(), asset.Assembly);
        }

        /// <remarks>
        /// Introduced for tests.
        /// </remarks>
        internal static void SetAssemblyDescriptorResolver(IAssemblyDescriptorResolver resolver) =>
            assemblyDescriptorResolver = resolver;

        internal static void RegisterResUriParsers()
        {
            if (!UriParser.IsKnownScheme("uires"))
            {
                UriParser.Register(
                    new GenericUriParser(
                        GenericUriParserOptions.GenericAuthority |
                        GenericUriParserOptions.NoUserInfo |
                        GenericUriParserOptions.NoPort |
                        GenericUriParserOptions.NoQuery |
                        GenericUriParserOptions.NoFragment),
                    "uires",
                    -1);
            }
        }

        internal static Assembly? GetAssembly(Uri uri, Uri? baseUri)
        {
            if (!uri.IsAbsoluteUri && baseUri != null)
                uri = new Uri(baseUri, uri);
            return GetAssembly(uri)?.Assembly;
        }

        private static (IAssemblyDescriptor Asm, string Path) GetResAsmAndPath(Uri uri)
        {
            var asm = assemblyDescriptorResolver.GetAssembly(uri.Authority);
            return (asm, uri.GetUnescapeAbsolutePath());
        }

        private static IAssemblyDescriptor? GetAssembly(Uri? uri)
        {
            if (uri != null)
            {
                if (!uri.IsAbsoluteUri)
                    return null;
                if (uri.IsUires())
                    return GetResAsmAndPath(uri).Asm;

                if (uri.IsEmbres())
                {
                    var assemblyName = uri.GetAssemblyNameFromQuery();
                    if (!string.IsNullOrEmpty(assemblyName))
                        return assemblyDescriptorResolver.GetAssembly(assemblyName);
                    else
                        return assemblyDescriptorResolver.GetAssemblyFromUrl(uri);
                }
            }

            return null;
        }

        private IAssetDescriptor? GetAsset(Uri uri, Uri? baseUri)
        {
            if (uri.IsAbsoluteEmbres())
            {
                var asm = (GetAssembly(uri) ?? GetAssembly(baseUri) ?? defaultEmbresAssembly)
                    ?? throw new ArgumentException("Assembly is not specified");
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
    }
}