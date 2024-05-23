using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ImageSet"/> can be used for specifying several size representations of the
    /// same picture.
    /// </summary>
    [TypeConverter(typeof(ImageSetConverter))]
    public class ImageSet : HandledObject<object>, IDisposable
    {
        /// <summary>
        /// Gets an empty <see cref="ImageSet"/>.
        /// </summary>
        public static readonly ImageSet Empty = new(immutable: true);

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with default values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImageSet()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with default values.
        /// </summary>
        public ImageSet(bool immutable)
            : base(immutable)
        {
            Images.ItemInserted += Images_ItemInserted;
            Images.ItemRemoved += Images_ItemRemoved;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with <see cref="Image"/>.
        /// </summary>
        /// <param name="image">This image will be added to <see cref="Images"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImageSet(Stream stream)
            : this()
        {
            NativeDrawing.Default.ImageSetLoadFromStream(this, stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class from the specified
        /// file or resource url.
        /// </summary>
        /// <param name="url">The file or embedded resource url used
        /// to load the image.
        /// </param>
        /// <remarks>
        /// See <see cref="ImageSet.FromUrl(string)"/> for the details.
        /// </remarks>
        public ImageSet(string url)
            : this()
        {
            using var stream = ResourceLoader.StreamFromUrl(url);
            if (stream is null)
            {
                BaseApplication.LogError($"ImageSet not loaded from: {url}");
                return;
            }

            NativeDrawing.Default.ImageSetLoadFromStream(this, stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class.
        /// </summary>
        /// <param name="imageSet">Native image set instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImageSet(object imageSet)
            : this()
        {
            Handler = imageSet;
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
                return NativeDrawing.Default.ImageSetGetDefaultSize(this);
            }
        }

        /// <summary>
        /// Gets the <see cref="Image"/> collection for this image list.
        /// </summary>
        /// <value>The collection of images.</value>
        public Collection<Image> Images { get; internal set; } = new() { ThrowOnNullAdd = true };

        /// <summary>
        /// Gets whether this <see cref="ImageSet"/> instance is valid and contains image(s).
        /// </summary>
        [Browsable(false)]
        public bool IsOk => NativeDrawing.Default.ImageSetIsOk(this);

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsReadOnly
            => Immutable || NativeDrawing.Default.ImageSetIsReadOnly(this);

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
        /// ImageSet imageSet2 = ImageSet.FromUrlOrNull(url); // return null instead of
        /// exception if file not found
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
            var nativeImage = NativeDrawing.Default.CreateImageSetFromSvgStream(
                stream,
                width,
                height,
                color);
            var result = new ImageSet(nativeImage);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class
        /// from the specified string which contains svg data.
        /// </summary>
        /// <param name="s">String with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        /// <returns><see cref="ImageSet"/> instance with svg data loaded from
        /// <paramref name="s"/>. </returns>
        public static ImageSet FromSvgString(string s, int width, int height, Color? color = null)
        {
            var nativeImage = NativeDrawing.Default.CreateImageSetFromSvgString(
                s,
                width,
                height,
                color);
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
        public static (ImageSet Normal, ImageSet Disabled) FromSvgStream(
            Stream stream,
            SizeI size,
            Color? color1,
            Color? color2)
        {
            var image1 = ImageSet.FromSvgStream(stream, size.Width, size.Height, color1);
            var image2 = ImageSet.FromSvgStream(stream, size.Width, size.Height, color2);
            return (image1, image2);
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
        /// Get the size that would be best to use for this <see cref="ImageSet"/> at the DPI
        /// scaling factor used by the given control.
        /// </summary>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        /// <returns></returns>
        /// <remarks>
        /// This is just a convenient wrapper for
        /// <see cref="ImageSet.GetPreferredBitmapSizeAtScale"/> calling
        /// that function with the result of <see cref="Control.GetPixelScaleFactor"/>.
        /// </remarks>
        /// <param name="imageSet"><see cref="ImageSet"/> instance.</param>
        public SizeI GetPreferredBitmapSizeFor(IControl control)
        {
            return NativeDrawing.Default.ImageSetGetPreferredBitmapSizeFor(this, control);
        }

        /// <summary>
        /// Get bitmap of the size appropriate for the DPI scaling used by the given control.
        /// </summary>
        /// <remarks>
        /// This helper function simply combines
        /// <see cref="GetPreferredBitmapSizeFor(ImageSet, Control)"/> and
        /// <see cref="ImageSet.AsImage(SizeI)"/>, i.e.it returns a (normally unscaled) bitmap
        /// from the <see cref="ImageSet"/> of the closest size to the size that should
        /// be used at the DPI scaling of the provided control.
        /// </remarks>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        /// <param name="imageSet"><see cref="ImageSet"/> instance.</param>
        public Image AsImageFor(IControl control) => new Bitmap(this, control);

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
            return NativeDrawing.Default.ImageSetGetPreferredBitmapSizeAtScale(this, scale);
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            return NativeDrawing.Default.CreateImageSet();
        }

        private void Images_ItemInserted(object? sender, int index, Image item)
        {
            CheckReadOnly();
            NativeDrawing.Default.ImageSetAddImage(this, index, item);
            OnChanged();
        }

        private void Images_ItemRemoved(object? sender, int index, Image item)
        {
            CheckReadOnly();
            NativeDrawing.Default.ImageSetRemoveImage(this, index, item);
            OnChanged();
        }

        private void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}