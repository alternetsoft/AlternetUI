using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

using SkiaSharp;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ImageSet"/> contains the same <see cref="Image"/> with different sizes.
    /// </summary>
    [TypeConverter(typeof(ImageSetConverter))]
    public class ImageSet : ImageContainer<IImageSetHandler>, IImageSource
    {
        /// <summary>
        /// Gets an empty <see cref="ImageSet"/>.
        /// </summary>
        public static readonly ImageSet Empty = new(immutable: true);

        /// <summary>
        /// Gets or sets a value indicating whether to use SkiaSharp for loading images.
        /// Default is True. If this value is False, platform specific image loading will be used.
        /// </summary>
        public static bool UseSkiaSharpForLoading = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with default values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImageSet()
            : base(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with default values.
        /// </summary>
        public ImageSet(bool immutable)
            : base(immutable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> with <see cref="Image"/>.
        /// </summary>
        /// <param name="image">This image will be added to <see cref="ImageContainer{T}.Images"/>.</param>
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
            InternalLoadFromStream(stream);
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
        /// <param name="baseUri">Base url. Optional. Used if <paramref name="url"/>
        /// is relative.</param>
        public ImageSet(string? url, Uri? baseUri = null)
            : this()
        {
            if (string.IsNullOrEmpty(url))
                return;

            using var stream = ResourceLoader.StreamFromUrl(url!, baseUri);
            if (stream is null)
            {
                App.LogError($"ImageSet not loaded from: {url}");
                return;
            }

            InternalLoadFromStream(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSet"/> class.
        /// </summary>
        /// <param name="imageSet">Native image set instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ImageSet(IImageSetHandler imageSet)
            : this()
        {
            Handler = imageSet;
        }

        /// <summary>
        /// Get the size of the bitmap represented by this bundle in default resolution
        /// or, equivalently, at 100% scaling.
        /// </summary>
        /// <remarks>
        /// When creating the bundle from a number of bitmaps, this will be just the
        /// size of the smallest bitmap in it. Note that this function is mostly used by
        /// library itself and not the application.
        /// </remarks>
        public virtual SizeI DefaultSize
        {
            get
            {
                if (Handler.IsDummy)
                {
                    if (Images.Count == 0)
                        return 16;
                    return Images[0].Size;
                }

                return Handler.DefaultSize;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsReadOnly
            => Immutable || Handler.IsReadOnly;

        Image? IImageSource.Image => null;

        ImageList? IImageSource.ImageList => null;

        int IImageSource.ImageIndex => 0;

        ImageSet? IImageSource.ImageSet => this;

        SvgImage? IImageSource.SvgImage => null;

        int? IImageSource.SvgSize => null;

        ImageSourceKind IImageSource.Kind => ImageSourceKind.ImageSet;

        /// <summary>
        /// Converts the specified <see cref='Image'/> to a <see cref='ImageSet'/>.
        /// </summary>
        public static explicit operator ImageSet(Image image) => new(image);

        /// <summary>
        /// Creates grayscale <see cref="ImageSet"/> instance from
        /// the <see cref="Image"/> instance.
        /// </summary>
        /// <param name="image">The image used to load the data.</param>
        public static ImageSet? FromImageGrayScale(Image? image)
        {
            if (image == null)
                return null;

            ImageSet result = new();
            result.Images.Add(image.ToGrayScale());
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
            var handler = GraphicsFactory.Handler.CreateImageSetHandlerFromSvg(
                stream,
                width,
                height,
                color);
            var result = new ImageSet(handler);
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
            var handler = GraphicsFactory.Handler.CreateImageSetHandlerFromSvg(
                s,
                width,
                height,
                color);
            var result = new ImageSet(handler);
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
            stream.Seek(0, SeekOrigin.Begin);
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
            catch (Exception e)
            {
                LogUtils.LogExceptionIfDebug(e);
                return null;
            }
        }

        /// <summary>
        /// Gets first image.
        /// </summary>
        public virtual Image AsImage(SizeI size)
        {
            var result = Handler.AsImage(size);
            if (Immutable)
                result.SetImmutable();
            return result;
        }

        /// <summary>
        /// Gets first image with size equal to <see cref="DefaultSize"/>.
        /// </summary>
        public virtual Image AsImage()
        {
            return AsImage(DefaultSize);
        }

        /// <summary>
        /// Get the size that would be best to use for this <see cref="ImageSet"/> at the DPI
        /// scaling factor used by the given control.
        /// </summary>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        /// <returns></returns>
        /// <remarks>
        /// This is just a convenient wrapper for
        /// <see cref="ImageSet.GetPreferredBitmapSizeAtScale"/> calling
        /// that function with the result of <see cref="AbstractControl.ScaleFactor"/>.
        /// </remarks>
        public virtual SizeI GetPreferredBitmapSizeFor(AbstractControl control)
        {
            return Handler.GetPreferredBitmapSizeFor(control);
        }

        /// <summary>
        /// Get bitmap of the size appropriate for the DPI scaling used by the given control.
        /// </summary>
        /// <remarks>
        /// This helper function returns a (normally unscaled) bitmap
        /// from the <see cref="ImageSet"/> of the closest size to the size that should
        /// be used at the DPI scaling of the provided control.
        /// </remarks>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        public virtual Image AsImageFor(AbstractControl control)
        {
            var result = new Bitmap(this, control);
            if (Immutable)
                result.SetImmutable();
            return result;
        }

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
        public virtual SizeI GetPreferredBitmapSizeAtScale(Coord scale)
        {
            return Handler.GetPreferredBitmapSizeAtScale(scale);
        }

        /// <inheritdoc/>
        protected override IImageSetHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateImageSetHandler() ?? new PlessImageSetHandler();
        }

        /// <summary>
        /// Loads image data from the specified stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the image.</param>
        protected virtual bool InternalLoadFromStream(Stream stream)
        {
            if (UseSkiaSharpForLoading)
            {
                try
                {
                    var bitmap = SKBitmap.Decode(stream);
                    Images.Add((Image)bitmap);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return Handler.LoadFromStream(stream);
            }
        }
    }
}