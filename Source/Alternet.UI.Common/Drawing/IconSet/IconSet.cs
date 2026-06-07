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
    /// Represents a set of icons, typically loaded from an .ico file, containing multiple icon
    /// images of various sizes and color depths.
    /// </summary>
    [TypeConverter(typeof(IconSetConverter))]
    public partial class IconSet : AttachedImageContainer<IImageContainer>
    {
        private string? url;

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> with <see cref="Image"/>.
        /// </summary>
        /// <param name="image">This image will be converted to icon and added
        /// to the set of icons.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IconSet(Image? image)
        {
            if (image is not null)
                Add(image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> class from the specified
        /// data stream. Data stream must contain icon data in .ico format.
        /// </summary>
        /// <param name="stream">The data stream used to load the icon.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IconSet(Stream? stream)
        {
            if (stream is not null)
                Add(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> class from the specified
        /// url. See <see cref="FromUrl"/> for the details.
        /// Data stream must contain icon data in .ico format.
        /// </summary>
        /// <param name="url">The file or embedded resource url used to load the image.</param>
        /// <remarks>
        /// No exceptions are raised if error occurs during icon load.
        /// If DEBUG is defined, exception info is logged.
        /// </remarks>
        /// <param name="baseUri">Base url. Optional. Used if <paramref name="url"/> is relative.</param>
        public IconSet(string? url, Uri? baseUri = null)
        {
            if (string.IsNullOrEmpty(url))
                return;

            Url = ResourceLoader.ToAbsoluteUrl(url, baseUri);

            try
            {
                using var stream = ResourceLoader.StreamFromUrlOr(url, baseUri);
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
        {
        }

        /// <summary>
        /// Gets width of the icon from the system settings.
        /// </summary>
        public static int SystemIconWidth => SystemSettings.GetMetric(SystemSettingsMetric.IconX);

        /// <summary>
        /// Gets height of the icon from the system settings.
        /// </summary>
        public static int SystemIconHeight => SystemSettings.GetMetric(SystemSettingsMetric.IconY);

        /// <summary>
        /// Gets size of the icon from the system settings.
        /// </summary>
        public static SizeI SystemIconSize => new (SystemIconWidth, SystemIconHeight);

        /// <summary>
        /// Gets width of the small icon from the system settings (used in window captions and small icon view).
        /// </summary>
        public static int SmallSystemIconWidth => SystemSettings.GetMetric(SystemSettingsMetric.SmallIconX);

        /// <summary>
        /// Gets height of the small icon from the system settings (used in window captions and small icon view).
        /// </summary>
        public static int SmallSystemIconHeight => SystemSettings.GetMetric(SystemSettingsMetric.SmallIconY);

        /// <summary>
        /// Gets size of the small icon from the system settings (used in window captions and small icon view).
        /// </summary>
        public static SizeI SmallSystemIconSize => new (SmallSystemIconWidth, SmallSystemIconHeight);

        /// <summary>
        /// Gets or sets an override value for the system small icon width.
        /// If set to a value greater than zero, this value will be used instead of the system icon width
        /// in <see cref="EffectiveSmallSystemIconWidth"/>.
        /// </summary>
        public static int? SmallSystemIconWidthOverride;

        /// <summary>
        /// Gets or sets an override value for the system small icon height.
        /// If set to a value greater than zero, this value will be used instead of the system icon height
        /// in <see cref="EffectiveSmallSystemIconHeight"/>.
        /// </summary>
        public static int? SmallSystemIconHeightOverride;

        /// <summary>
        /// Gets or sets an override value for the system icon width.
        /// If set to a value greater than zero, this value will be used instead of the system icon width
        /// in <see cref="EffectiveSystemIconWidth"/>.
        /// </summary>
        public static int? SystemIconWidthOverride;

        /// <summary>
        /// Gets or sets an override value for the system icon height.
        /// If set to a value greater than zero, this value will be used instead of the system icon height
        /// in <see cref="EffectiveSystemIconHeight"/>.
        /// </summary>
        public static int? SystemIconHeightOverride;

        /// <summary>
        /// Gets effective system small icon width, taking into account the <see cref="SmallSystemIconWidthOverride"/>.
        /// </summary>
        public static int EffectiveSmallSystemIconWidth => SmallSystemIconWidthOverride ?? SmallSystemIconWidth;

        /// <summary>
        /// Gets effective system small icon height, taking into account the <see cref="SmallSystemIconHeightOverride"/>.
        /// </summary>
        public static int EffectiveSmallSystemIconHeight => SmallSystemIconHeightOverride ?? SmallSystemIconHeight;

        /// <summary>
        /// Gets effective system icon width, taking into account the <see cref="SystemIconWidthOverride"/>.
        /// </summary>
        public static int EffectiveSystemIconWidth => SystemIconWidthOverride ?? SystemIconWidth;

        /// <summary>
        /// Gets effective system icon height, taking into account the <see cref="SystemIconHeightOverride"/>.
        /// </summary>
        public static int EffectiveSystemIconHeight => SystemIconHeightOverride ?? SystemIconHeight;

        /// <summary>
        /// Gets effective system small icon size, taking into account
        /// the <see cref="SmallSystemIconWidthOverride"/> and <see cref="SmallSystemIconHeightOverride"/>.
        /// </summary>
        public static SizeI EffectiveSmallSystemIconSize => new(EffectiveSmallSystemIconWidth, EffectiveSmallSystemIconHeight);

        /// <summary>
        /// Gets effective system icon size, taking into account
        /// the <see cref="SystemIconWidthOverride"/> and <see cref="SystemIconHeightOverride"/>.
        /// </summary>
        public static SizeI EffectiveSystemIconSize => new(EffectiveSystemIconWidth, EffectiveSystemIconHeight);

        /// <summary>
        /// Gets url of the icon if it is loaded from a url,
        /// or null if the icon is not loaded from a url.
        /// </summary>
        public string? Url
        {
            get
            {
                return url;
            }

            private set
            {
                url = value;
            }
        }

        /// <summary>
        /// Creates <see cref="IconSet"/> instance from
        /// the <see cref="Image"/> instance.
        /// </summary>
        /// <param name="image">The image used to load the data.</param>
        /// <returns>An <see cref="IconSet"/> instance or null if the image is null.</returns>
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
        /// from the specified url. Data stream must contain icon data in .ico format.
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
        public static IconSet? FromUrl(string url)
        {
            using var stream = ResourceLoader.StreamFromUrlOrDefault(url);
            var result = new IconSet(stream);
            result.Url = url;
            return result;
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
                var stream = ResourceLoader.StreamFromUrlOrDefault(url);
                if (stream is not null)
                {
                    var result = new IconSet(stream);
                    result.Url = url;
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
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
                var stream = ResourceLoader.StreamFromUrlOrDefault(url);
                if (stream is not null)
                {
                    var result = new IconSet(stream);
                    result.Url = url;
                    return result;
                }
                else
                    return defaultIcon;
            }
            catch
            {
                return defaultIcon;
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Data stream must contain icon data in .ico format.
        /// </remarks>
        public override bool Add(Stream? stream)
        {
            if (IsReadOnly || stream is null)
                return false;

            var result = false;

            try
            {
                using IconStream iconStream = new(stream);

                foreach (var entry in iconStream.Entries)
                {
                    if (entry.Image is null)
                        continue;

                    var image = (Image)entry.Image;

                    Images.Add(image);
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                App.DebugLogError($"Error adding icon image from stream: {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        protected override IImageContainer? CreateHandler()
        {
            return GraphicsFactory.Handler.CreateIconSetHandler();
        }
    }
}