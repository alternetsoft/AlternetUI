using System;
using System.ComponentModel;
using System.IO;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ImageSet"/> can be used for specifying several size representations of the
    /// same picture.
    /// </summary>
    [TypeConverter(typeof(ImageSetConverter))]
    public class ImageSet : BaseComponent, IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with default values.
        /// </summary>
        public ImageSet()
            : this(new UI.Native.ImageSet())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with <see cref="Image"/>.
        /// </summary>
        /// <param name="image">This image will be added to <see cref="Images"/>.</param>
        public ImageSet(Image image)
            : this()
        {
            Images.Add(image);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class from the specified
        /// data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        public ImageSet(Stream stream)
            : this()
        {
            using var inputStream = new UI.Native.InputStream(stream);
            NativeImageSet.LoadFromStream(inputStream);
        }

        internal ImageSet(UI.Native.ImageSet imageSet)
        {
            NativeImageSet = imageSet;

            Images.ItemInserted += Images_ItemInserted;
            Images.ItemRemoved += Images_ItemRemoved;
        }

        /// <summary>
        /// Occurs when the image set is changed, i.e. an image is added to it or removed from it.
        /// </summary>
        public event EventHandler? Changed;

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public Collection<Image> Images { get; } = new() { ThrowOnNullAdd = true };

        /// <summary>
        /// Gets whether this <see cref="ImageSet"/> instance is valid and contains image(s).
        /// </summary>
        public bool IsOk => NativeImageSet.IsOk;

        /// <summary>
        /// Gets whether this <see cref="ImageSet"/> instance is readonly (no further
        /// add or load operations are allowed).
        /// </summary>
        public bool IsReadOnly => NativeImageSet.IsReadOnly;

        internal UI.Native.ImageSet NativeImageSet { get; private set; }

        /// <summary>
        /// Creates grayscaled <see cref="ImageSet"/> instance from
        /// the <see cref="Image"/> instance.
        /// </summary>
        /// <param name="image">The image used to load the data.</param>
        public static ImageSet? FromImageGrayScale(Image? image)
        {
            if (image == null)
                return null;

            ImageSet result = new();
            result.Images.Add(image!.ToGrayScale());
            return result;
        }

        /// <summary>
        /// Creates <see cref="ImageSet"/> instance from
        /// the <see cref="Image"/> instance.
        /// </summary>
        /// <param name="image">The image used to load the data.</param>
        /// <returns></returns>
        public static ImageSet? FromImage(Image? image)
        {
            if (image == null)
                return null;

            ImageSet result = new();
            result.Images.Add(image);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified url.
        /// </summary>
        /// <param name="url">The file or embedded resource url used to load the image.
        /// </param>
        /// <example>
        /// <code>
        /// var ImageSize = 16;
        /// var ResPrefix = $"embres:ControlsTest.Resources.Png._{ImageSize}.";
        /// var url = $"{ResPrefix}arrow-left-{ImageSize}.png";
        /// ImageSet imageSet = ImageSet.FromUrl(url); // can raise an exception if file not found
        /// ImageSet imageSet2 = ImageSet.FromUrlOrNull(url); // return null instead of exception if file not found
        /// </code>
        /// </example>
        /// <remarks>
        /// <paramref name="url"/> can include assembly name. Example:
        /// "embres:Alternet.UI.Resources.Svg.ImageName.png?assembly=Alternet.UI"
        /// </remarks>
        public static ImageSet FromUrl(string url)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            return new ImageSet(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <remarks>
        /// This is similar to <see cref="Image.FromSvgUrl"/> but uses
        /// <see cref="Control.GetDPI"/> and <see cref="Toolbar.GetDefaultImageSize(double)"/>
        /// to get appropriate image size which is best suitable for toolbars.
        /// </remarks>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.</param>
        /// <param name="control">Control which <see cref="Control.GetDPI"/> method
        /// is used to get DPI.</param>
        /// <returns><see cref="ImageSet"/> instance loaded from Svg data for use
        /// on the toolbars.</returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static ImageSet FromSvgUrlForToolbar(string url, Control control, Color? color = null)
        {
            var imageSize = Toolbar.GetDefaultImageSize(control);
            var result = ImageSet.FromSvgUrl(url, imageSize.Width, imageSize.Height, color);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified url which points to svg file or resource.
        /// </summary>
        /// <param name="url">The file or embedded resource url with Svg data used
        /// to load the image.
        /// </param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <remarks>
        /// <paramref name="url"/> can include assembly name. Example:
        /// "embres:Alternet.UI.Resources.Svg.ImageName.svg?assembly=Alternet.UI"
        /// </remarks>
        /// <returns><see cref="ImageSet"/> instance with svg data loaded from the specified
        /// <paramref name="url"/>. </returns>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static ImageSet FromSvgUrl(string url, int width, int height, Color? color = null)
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            var result = FromSvgStream(stream, width, height, color);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified <see cref="Stream"/> which contains svg data.
        /// </summary>
        /// <param name="stream">Stream with Svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        /// <returns><see cref="ImageSet"/> instance with svg data loaded from
        /// <paramref name="stream"/>. </returns>
        public static ImageSet FromSvgStream(Stream stream, int width, int height, Color? color = null)
        {
            var nativeImage = new UI.Native.ImageSet();
            using var inputStream = new UI.Native.InputStream(stream);
            nativeImage.LoadSvgFromStream(inputStream, width, height, color ?? Color.Black);
            var result = new ImageSet(nativeImage);
            return result;
        }

        /// <inheritdoc cref="FromUrl"/>
        /// <remarks>
        /// Returns null if error occurs during image load.
        /// No exceptions are raised.
        /// If DEBUG is defined, exception info is logged.
        /// </remarks>
        public static ImageSet? FromUrlOrNull(string url)
        {
            try
            {
                var result = ImageSet.FromUrl(url);
                return result;
            }
            catch(Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
                return null;
            }
        }

        /// <summary>
        /// Gets first image.
        /// </summary>
        public Image AsImage(Int32Size size) => new Bitmap(this, size);

        /// <summary>
        /// Releases all resources used by the <see cref="ImageList"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ImageList"/> and
        /// optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativeImageSet.Dispose();
                    NativeImageSet = null!;
                }

                isDisposed = true;
            }
        }

        private void Images_ItemInserted(object? sender, int index, Image item)
        {
            NativeImageSet.AddImage(item.NativeImage);
            OnChanged();
        }

        private void Images_ItemRemoved(object? sender, int index, Image item)
        {
            OnChanged();

            // todo
        }

        private void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}