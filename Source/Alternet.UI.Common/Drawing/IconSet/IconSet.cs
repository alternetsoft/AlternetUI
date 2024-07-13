using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Allows to use icons in the application.
    /// </summary>
    [TypeConverter(typeof(IconSetConverter))]
    public class IconSet : ImageContainer<IIconSetHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> with <see cref="Image"/>.
        /// </summary>
        /// <param name="image">This image will be converted to icon and added
        /// to the set of icons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IconSet(Image image)
            : base(false)
        {
            Add(image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> class from the specified
        /// data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the icon.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IconSet(Stream stream)
            : base(false)
        {
            Add(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> class from the specified
        /// url. See <see cref="FromUrl"/> for the details.
        /// </summary>
        /// <param name="url">The file or embedded resource url used to load the image.</param>
        /// <remarks>
        /// No exceptions are raised if error occurs during icon load.
        /// If DEBUG is defined, exception info is logged.
        /// </remarks>
        /// <param name="baseUri">Base url. Optional. Used if <paramref name="url"/> is relative.</param>
        public IconSet(string? url, Uri? baseUri = null)
            : base(false)
        {
            if (string.IsNullOrEmpty(url))
                return;

            try
            {
                using var stream = ResourceLoader.StreamFromUrl(url!, baseUri);
                if (stream is null)
                {
                    App.LogError($"Image not loaded from: {url}");
                    return;
                }

                Add(stream);
            }
            catch (Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> with default values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IconSet()
            : base(false)
        {
        }

        /// <summary>
        /// Creates <see cref="IconSet"/> instance from
        /// the <see cref="Image"/> instance.
        /// </summary>
        /// <param name="image">The image used to load the data.</param>
        /// <returns></returns>
        public static IconSet? FromImage(Image? image)
        {
            if (image == null)
                return null;

            IconSet result = new();
            result.Add(image);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> class
        /// from the specified url.
        /// </summary>
        /// <param name="url">The file or embedded resource url used to load the image.
        /// </param>
        /// <example>
        /// <code>
        /// var ResPrefix = $"embres:ControlsTest.Resources.Icons.";
        /// var url = $"{ResPrefix}default.ico";
        /// var iconSet = IconSet.FromUrl(url); // can raise an exception if file not found
        /// var iconSet2 = IconSet.FromUrlOrNull(url); // return null instead of exception
        /// if file not found
        /// </code>
        /// </example>
        /// <remarks>
        /// <paramref name="url"/> can include assembly name. Example:
        /// "embres:Alternet.UI.Resources.Svg.IconName.ico?assembly=Alternet.UI"
        /// </remarks>
        public static IconSet FromUrl(string url)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            return new IconSet(stream);
        }

        /// <inheritdoc cref="FromUrl"/>
        /// <remarks>
        /// Returns null if error occurs during icon load.
        /// No exceptions are raised.
        /// If DEBUG is defined, exception info is logged.
        /// </remarks>
        public static IconSet? FromUrlOrNull(string url)
        {
            try
            {
                var stream = ResourceLoader.StreamFromUrl(url);
                if (stream is not null)
                    return new IconSet(stream);
                else
                {
                    App.LogErrorIfDebug($"Image not loaded from: {url}");
                    return null;
                }
            }
            catch(Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
                return null;
            }
        }

        /// <inheritdoc cref="FromUrl"/>
        /// <remarks>
        /// Returns <paramref name="defaultIcon"/> if error occurs during icon load.
        /// No exceptions are raised.
        /// </remarks>
        public static IconSet? FromUrlOrDefault(string url, IconSet? defaultIcon)
        {
            try
            {
                var stream = ResourceLoader.StreamFromUrl(url);
                if (stream is not null)
                    return new IconSet(stream);
                else
                    return defaultIcon;
            }
            catch
            {
                return defaultIcon;
            }
        }

        /// <inheritdoc/>
        protected override IIconSetHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateIconSetHandler() ?? DummyIconSetHandler.Default;
        }
    }
}