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
        /// Get the size of the bitmap represented by this bundle in default resolution
        /// or, equivalently, at 100% scaling.
        /// </summary>
        /// <remarks>
        /// When creating the bundle from a number of bitmaps, this will be just the
        /// size of the smallest bitmap in it. Note that this function is mostly used by
        /// library itself and not the application.
        /// </remarks>
        public SizeI DefaultSize
        {
            get
            {
                return NativeImageSet.DefaultSize;
            }
        }

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
        /// Converts the specified <see cref='Image'/> to a <see cref='ImageSet'/>.
        /// </summary>
        public static explicit operator ImageSet(Image image) => new(image);

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
        /// <see cref="Control.GetDPI"/> and <see cref="ToolBar.GetDefaultImageSize(double)"/>
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
            var imageSize = ToolBar.GetDefaultImageSize(control);
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
            using var inputStream = new UI.Native.InputStream(stream, false);
            nativeImage.LoadSvgFromStream(inputStream, width, height, color ?? Color.Black);
            var result = new ImageSet(nativeImage);
            return result;
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified <see cref="Stream"/> which contains svg data.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="color1">Svg fill color 1.</param>
        /// <param name="color2">Svg fill color 2.</param>
        /// <returns></returns>
        public static (ImageSet, ImageSet) FromSvgStream(
            Stream stream,
            SizeI size,
            Color? color1,
            Color? color2)
        {
            var image1 = ImageSet.FromSvgStream(stream, size.Width, size.Height, color1);
            var image2 = ImageSet.FromSvgStream(stream, size.Width, size.Height, color2);
            return (image1, image2);
        }

        /// <summary>
        /// Initializes a tuple with two instances of the <see cref="ImageSet"/> class
        /// from the specified <see cref="Stream"/> which contains svg data. Images are loaded
        /// for the normal and disabled states using <see cref="Control.GetSvgColor"/>.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="control">Control which <see cref="Control.GetSvgColor"/>
        /// method is called to get color information.</param>
        /// <returns></returns>
        public static (ImageSet, ImageSet) GetNormalAndDisabledSvg(
            Stream stream,
            SizeI size,
            Control control)
        {
            var image = ImageSet.FromSvgStream(
                stream,
                32,
                control.GetSvgColor(KnownSvgColor.Normal),
                control.GetSvgColor(KnownSvgColor.Disabled));
            return image;
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
        public Image AsImage(SizeI size) => new(this, size);

        /// <summary>
        /// Gets first image with size equal to <see cref="DefaultSize"/>.
        /// </summary>
        public Image AsImage() => new(this, DefaultSize);

        /// <summary>
        /// Get bitmap of the size appropriate for the DPI scaling used by the given control.
        /// </summary>
        /// <remarks>
        /// This helper function simply combines <see cref="GetPreferredBitmapSizeFor"/> and
        /// <see cref="AsImage(SizeI)"/>, i.e.it returns a (normally unscaled) bitmap
        /// from the <see cref="ImageSet"/> of the closest size to the size that should
        /// be used at the DPI scaling of the provided control.
        /// </remarks>
        /// <param name="control"></param>
        public Image AsImageFor(Control control) => new(this, control);

        /// <summary>
        /// Get the size that would be best to use for this <see cref="ImageSet"/> at
        /// the given DPI scaling factor.
        /// </summary>
        /// <remarks>
        /// Passing a size returned by this function to <see cref="AsImage(SizeI)"/> ensures that bitmap
        /// doesn't need to be rescaled, which typically significantly lowers its quality.
        /// </remarks>
        /// <remarks>
        /// For <see cref="ImageSet"/> containing some number of the fixed-size bitmaps, this
        /// function returns the size of an existing bitmap closest to the ideal size at the given
        /// scale, i.e. <see cref="DefaultSize"/> multiplied by scale.
        /// </remarks>
        /// <param name="scale"></param>
        /// <returns></returns>
        public SizeI GetPreferredBitmapSizeAtScale(double scale)
        {
            return NativeImageSet.GetPreferredBitmapSizeAtScale(scale);
        }

        /// <summary>
        /// Get the size that would be best to use for this <see cref="ImageSet"/> at the DPI
        /// scaling factor used by the given control.
        /// </summary>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        /// <returns></returns>
        /// <remarks>
        /// This is just a convenient wrapper for <see cref="GetPreferredBitmapSizeAtScale"/> calling
        /// that function with the result of <see cref="Control.GetPixelScaleFactor"/>.
        /// </remarks>
        public SizeI GetPreferredBitmapSizeFor(Control control)
        {
            return NativeImageSet.GetPreferredBitmapSizeFor(control.WxWidget);
        }

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