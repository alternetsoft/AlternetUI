using System;
using System.Collections.Generic;
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
    public class ImageList : ImageContainer<IImageListHandler>
    {
        /// <summary>
        /// Gets an empty <see cref="ImageList"/>.
        /// </summary>
        public static readonly ImageList Empty = new(immutable: true);

        /// <summary>
        /// Gets or sets the default size of the images in the image list, in pixels.
        /// </summary>
        public static int DefaultSize = 16;

        private SizeI size = DefaultSize;

        /// <summary>
        /// Initializes a new empty instance of the <c>ImageList</c> class with
        /// the specified image size.
        /// </summary>
        public ImageList(SizeI size)
            : this(false)
        {
            ImageSize = size;
        }

        /// <summary>
        /// Initializes a new instance of the <c>ImageList</c> class with specified image list.
        /// </summary>
        /// <param name="images">Collection of images to be stored in this new instance.</param>
        public ImageList(IEnumerable<Image> images)
            : this(false)
        {
            var firstImage = images.FirstOrDefault();
            if (firstImage == null)
            {
                ImageSize = new(16, 16);
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
            : this(false)
        {
            Initialize(images, size);
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
            : this(false)
        {
            ImageSize = new(imageWidth, stripImage.Height);
            AddImageStrip(stripImage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList(bool immutable)
            : base(immutable)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> with default values.
        /// </summary>
        public ImageList()
            : this(false)
        {
        }

        /// <summary>
        /// Gets or sets the size of the images in the image list, in pixels.
        /// </summary>
        /// <value>
        /// The <see cref="SizeI"/> that defines the height and width, in pixels, of the images
        /// in the list. The default size is 16 by 16.
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
                Handler.Size = value;
                RaiseChanged();
            }
        }

        /// <summary>
        /// Gets suggested size of the image for the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">Scale factor for which to get suggested size of the image.</param>
        /// <returns></returns>
        public static int GetSuggestedSize(Coord scaleFactor)
        {
            int size = 16;

            if (scaleFactor > 1)
            {
                size = (int)(size * scaleFactor);
                if (size < 32)
                    size = 32;
            }

            return size;
        }

        /// <summary>
        /// Adds the image to the image list, resizing it if necessary.
        /// </summary>
        /// <param name="image">The image to be added.</param>
        /// <returns>Returns true if the image was added successfully.</returns>
        public virtual bool AddResized(Image image)
        {
            if (image.Size != ImageSize)
            {
                var resizedImage = new Bitmap(image, ImageSize);
                return Add(resizedImage);
            }

            return Add(image);
        }

        /// <summary>
        /// Adds image strip to the image list.
        /// </summary>
        /// <param name="strip">The image strip containing multiple images.</param>
        public virtual void AddImageStrip(Image strip)
        {
            var width = ImageSize.Width;
            int imageCount = strip.Width / width;
            for (int i = 0; i < imageCount; i++)
            {
                var sourceRectangle = new RectI(i * width, 0, width, ImageSize.Height);
                var image = strip.GetSubBitmap(sourceRectangle);
                Add(image);
            }
        }

        /// <summary>
        /// Adds svg to the image list with the default normal color for the specified theme.
        /// </summary>
        /// <param name="svg">Svg to add.</param>
        /// <param name="isDarkTheme">Whether theme is dark.</param>
        /// <returns></returns>
        public virtual bool AddSvg(SvgImage svg, bool isDarkTheme)
        {
            var color = svg.GetSvgColor(KnownSvgColor.Normal, isDarkTheme);
            var result = AddSvg(svg, color);
            return result;
        }

        /// <summary>
        /// Adds svg to the image list with the default normal color for the specified theme.
        /// </summary>
        /// <param name="svg">Svg to add.</param>
        /// <returns></returns>
        /// <param name="color">Svg color. Optional. If not specified, svg colors
        /// are not changed.</param>
        /// <returns></returns>
        public virtual bool AddSvg(SvgImage svg, Color? color = null)
        {
            if (ImageSize.SameWidthHeight)
            {
                var image = svg.ImageWithColor(size.Width, color);
                return Add(image);
            }
            else
            {
                var imageSet = svg.LoadImage(ImageSize, color);
                if (imageSet is null)
                    return false;
                var image = imageSet.AsImage(ImageSize);
                return Add(image);
            }
        }

        /// <summary>
        /// Draws an image on the specified graphic surface at the location specified
        /// by a coordinate pair.
        /// </summary>
        /// <param name="g">The Graphics object to draw on.</param>
        /// <param name="x">X-coordinate of the upper-left corner of the image to be drawn.</param>
        /// <param name="y">Y-coordinate of the upper-left corner of the image to be drawn.</param>
        /// <param name="index">Index of image to draw within image list.</param>
        public virtual bool Draw(Graphics g, Coord x, Coord y, int index)
        {
            if (index < 0 || index >= Images.Count)
                return false;

            var image = Images[index];

            if (image is null)
                return false;

            g.DrawImageUnscaled(image, new(x, y));
            return true;
        }

        /// <summary>
        /// Creates an new <see cref="ImageList"/> from this object
        /// with all pixels converted using
        /// the specified function.
        /// </summary>
        /// <param name="func">Function used to convert color of the pixel.</param>
        /// <returns></returns>
        public virtual ImageList WithConvertedColors(Func<ColorStruct, ColorStruct> func)
        {
            ImageList result = new();
            result.ImageSize = ImageSize;

            foreach(var image in Images)
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
        /// <returns></returns>
        public virtual ImageList WithLightLightColors()
        {
            return WithConvertedColors(ControlPaint.LightLight);
        }

        /// <summary>
        /// Creates an new <see cref="ImageList"/> from this <see cref="ImageList"/>
        /// with all pixels lighter.
        /// </summary>
        /// <returns></returns>
        public virtual ImageList WithLightColors()
        {
            return WithConvertedColors(ControlPaint.Light);
        }

        /// <inheritdoc/>
        protected override IImageListHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateImageListHandler() ?? new PlessImageListHandler();
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