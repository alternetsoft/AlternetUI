using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alternet.Base.Collections;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods to manage a collection of <see cref="Image"/> objects.
    /// </summary>
    /// <remarks>
    /// <see cref="ImageList"/> is used by some controls, such as the tree view and list view.
    /// You can add images to the <see cref="ImageList"/>, and the controls are
    /// able to use the images as they require.
    /// </remarks>
    public class ImageList : AttachedImageContainer<IImageListHandler>
    {
        /// <summary>
        /// Gets an empty <see cref="ImageList"/>.
        /// </summary>
        public static readonly ImageList Empty = new(immutable: true);

        private SizeI size = DefaultImageSize;

        /// <summary>
        /// Initializes a new empty instance of the <c>ImageList</c> class with
        /// the specified image size.
        /// </summary>
        public ImageList(SizeI size)
            : this()
        {
            ImageSize = size;
        }

        /// <summary>
        /// Initializes a new instance of the <c>ImageList</c> class with specified image list.
        /// </summary>
        /// <param name="images">Collection of images to be stored in this new instance.</param>
        public ImageList(IEnumerable<Image> images)
            : this()
        {
            var firstImage = images.FirstOrDefault();
            if (firstImage == null)
            {
                ImageSize = new(DefaultImageSize, DefaultImageSize);
                return;
            }

            Initialize(images, firstImage.Size);
        }

        /// <summary>
        /// Initializes a new instance of the <c>ImageList</c> class with specified parameters.
        /// </summary>
        /// <param name="images">Collection of images to be stored in this new instance.</param>
        /// <param name="size">New size of the images.</param>
        public ImageList(IEnumerable<Image> images, SizeI size)
            : this()
        {
            Initialize(images, size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> class using an image strip loaded from the specified URL.
        /// </summary>
        /// <param name="url">The URL of the image to load. Can be a relative or absolute path. Cannot be null or empty.</param>
        /// <param name="baseUri">An optional base URI to resolve relative URLs. If null, relative URLs are resolved based on the
        /// application's context.</param>
        public ImageList(string? url, Uri? baseUri = null)
            : this(new Bitmap(url, baseUri))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ImageList</c> class with specified image strip.
        /// </summary>
        /// <param name="stripImage">List of images concatenated in a single image strip.</param>
        public ImageList(Image stripImage)
            : this(stripImage, stripImage.Height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>ImageList</c> class with specified image strip.
        /// </summary>
        /// <param name="stripImage">List of images concatenated in a single image strip.</param>
        /// <param name="imageWidth">Width of the particular image in the strip.</param>
        public ImageList(Image stripImage, int imageWidth)
            : this()
        {
            ImageSize = new(imageWidth, stripImage.Height);
            AddImageStrip(stripImage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList(bool immutable)
        {
            SetImmutable(immutable);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList()
        {
        }

        /// <summary>
        /// Gets or sets the size of the images in the image container, in pixels.
        /// </summary>
        /// <value>
        /// The <see cref="SizeI"/> that defines the height and width, in pixels, of the images
        /// in the container. The default size is 16 by 16.
        /// </value>
        /// <remarks>
        /// Setting the <see cref="ImageSize"/> to a different value than the actual size
        /// of the images in the image collection causes the images to be resized to the size
        /// specified.
        /// </remarks>
        public virtual SizeI ImageSize
        {
            get
            {
                return size;
            }

            set
            {
                if (size == value)
                    return;
                size = value;
                if (Handler != null)
                    Handler.Size = value;
                RaiseChanged();
            }
        }

        /// <summary>
        /// Adds the image to the image list, resizing it if necessary.
        /// </summary>
        /// <param name="image">The image to be added.</param>
        /// <returns>Returns true if the image was added successfully.</returns>
        public virtual bool AddResized(Image image)
        {
            return AddResized(image, ImageSize);
        }

        /// <summary>
        /// Adds image strip to the image container. The image strip is a single image
        /// that contains multiple images concatenated horizontally.
        /// This method divides the image strip into individual images based on the <see cref="ImageSize"/>
        /// property and adds them to the container.
        /// </summary>
        /// <param name="strip">The image strip containing multiple images.</param>
        public virtual void AddImageStrip(Image strip)
        {
            AddImageStrip(strip, ImageSize);
        }

        /// <summary>
        /// Creates a single image strip by combining all images in the collection horizontally.
        /// </summary>
        /// <remarks>Each image in the collection is placed side by side in the resulting strip,
        /// maintaining its original size and order. The width of the strip is the sum of the widths of all images, and
        /// the height matches the individual image height. This method is useful for generating sprite sheets or
        /// preview strips from a sequence of images.</remarks>
        /// <returns>An <see cref="Image"/> containing the combined image strip, or <see langword="null"/> if the collection
        /// contains no images.</returns>
        public virtual Image? AsImageStrip()
        {
            return AsImageStrip(ImageSize);
        }

        /// <summary>
        /// Creates a single horizontal bitmap strip by concatenating all images in the collection.
        /// </summary>
        /// <remarks>The resulting bitmap arranges each image side by side in the order they appear in the
        /// collection. The width of the strip is the sum of the widths of all images, and the height matches the
        /// individual image height. The caller is responsible for disposing the returned <see cref="SKBitmap"/> when it
        /// is no longer needed.</remarks>
        /// <returns>An <see cref="SKBitmap"/> representing the concatenated image strip, or <see langword="null"/> if the
        /// collection contains no images.</returns>
        public virtual SKBitmap? AsSkiaStrip()
        {
            return AsSkiaStrip(ImageSize);
        }

        /// <summary>
        /// Adds svg to the image list with the default normal color for the specified theme.
        /// </summary>
        /// <param name="svg">Svg to add.</param>
        /// <param name="isDarkTheme">Whether theme is dark.</param>
        /// <returns>The added <see cref="Image"/> if successful; otherwise, <see langword="null"/>.</returns>
        public virtual Image? AddSvg(SvgImage svg, bool isDarkTheme)
        {
            return AddSvg(svg, ImageSize, isDarkTheme);
        }

        /// <summary>
        /// Adds svg to the image list with the default normal color for the specified theme.
        /// </summary>
        /// <param name="svg">Svg to add.</param>
        /// <returns>The added <see cref="Image"/> if successful; otherwise, <see langword="null"/>.</returns>
        /// <param name="color">Svg color. Optional. If not specified, svg colors
        /// are not changed.</param>
        public virtual Image? AddSvg(SvgImage svg, Color? color = null)
        {
            return AddSvg(svg, ImageSize, color);
        }

        /// <summary>
        /// Creates an new <see cref="ImageList"/> from this object
        /// with all pixels converted using
        /// the specified function.
        /// </summary>
        /// <param name="func">Function used to convert color of the pixel.</param>
        /// <returns>Image list with all pixels converted using the specified function.</returns>
        public virtual ImageList WithConvertedColors(Func<ColorStruct, ColorStruct> func)
        {
            ImageList result = new();
            result.ImageSize = ImageSize;

            foreach (var image in Images)
            {
                var converted = image.WithConvertedColors(func);
                result.Add(converted);
            }

            return result;
        }

        /// <summary>
        /// Creates an new <see cref="ImageList"/> from this <see cref="ImageList"/>
        /// with all pixels lighter
        /// (this method makes 2x lighter than <see cref="WithLightColors"/>).
        /// </summary>
        /// <returns>Image list with all pixels 2x lighter.</returns>
        public virtual ImageList WithLightLightColors()
        {
            return WithConvertedColors(ControlPaint.LightLight);
        }

        /// <summary>
        /// Creates an new <see cref="ImageList"/> from this <see cref="ImageList"/>
        /// with all pixels lighter.
        /// </summary>
        /// <returns>Image list with all pixels lighter.</returns>
        public virtual ImageList WithLightColors()
        {
            return WithConvertedColors(ControlPaint.Light);
        }

        /// <inheritdoc/>
        protected override IImageListHandler? CreateHandler()
        {
            return App.Handler.CreateImageListHandler();
        }

        /// <summary>
        /// Initializes the image list with the specified images and size.
        /// </summary>
        /// <param name="images">The images to initialize with.</param>
        /// <param name="size">The size to set for the images.</param>
        protected virtual void Initialize(IEnumerable<Image> images, SizeI size)
        {
            ImageSize = size;
            Images.AddRange(images);
        }
    }
}