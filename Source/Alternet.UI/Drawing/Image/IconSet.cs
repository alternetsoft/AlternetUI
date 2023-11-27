using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Allows to use icons in the application.
    /// </summary>
    public class IconSet : DisposableObject
    {
        private UI.Native.IconSet? iconSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> with <see cref="Image"/>.
        /// </summary>
        /// <param name="image">This image will be converted to icon and added to the set of icons.</param>
        public IconSet(Image image)
            : this()
        {
            Add(image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> class from the specified
        /// data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the icon.</param>
        public IconSet(Stream stream)
            : this()
        {
            Add(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconSet"/> class from the specified
        /// url. See <see cref="FromUrl"/> for the details.
        /// </summary>
        /// <param name="url">The file or embedded resource url used to load the image.
        /// </param>
        /// <remarks>
        /// No exceptions are raised if error occurs during icon load.
        /// If DEBUG is defined, exception info is logged.
        /// </remarks>
        public IconSet(string url)
            : this()
        {
            try
            {
                using var stream = ResourceLoader.StreamFromUrl(url);
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
        public IconSet()
            : base()
        {
            iconSet = new UI.Native.IconSet();
        }

        /// <summary>
        /// Gets whether object is ok.
        /// </summary>
        public bool IsOk => (iconSet is not null) && iconSet.IsOk();

        internal UI.Native.IconSet? NativeIconSet => iconSet;

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
        /// var iconSet2 = IconSet.FromUrlOrNull(url); // return null instead of exception if file not found
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
                var result = IconSet.FromUrl(url);
                return result;
            }
            catch(Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
                return null;
            }
        }

        /// <summary>
        /// Adds image.
        /// </summary>
        /// <param name="image"></param>
        public void Add(Image image)
        {
            iconSet?.AddImage(image.NativeImage);
        }

        /// <summary>
        /// Adds image from stream.
        /// </summary>
        /// <param name="stream">Stream with image.</param>
        public void Add(Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            iconSet?.LoadFromStream(inputStream);
        }

        /// <summary>
        /// Removes all icons from the <see cref="IconSet"/>.
        /// </summary>
        public void Clear()
        {
            iconSet?.Clear();
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            iconSet?.Dispose();
            iconSet = null;
        }
    }
}
