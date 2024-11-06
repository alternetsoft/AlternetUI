using System;
using Alternet.Base.Collections;
using Alternet.UI;

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

        private SizeI size = 16;

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
        /// <param name="color">Svg color. Optional. If not specified, svg colors are not changed.</param>
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
        /// Draws an image on the specified graphic surface at the location specified by a coordinate pair.
        /// </summary>
        /// <param name="g">The Graphics object to draw on.</param>
        /// <param name="x">X-coordinate of the upper-left corner of the image to be drawn.</param>
        /// <param name="y">Y-coordinate of the upper-left corner of the image to be drawn.</param>
        /// <param name="index">Index of image to draw within image list.</param>
        public virtual bool Draw(Graphics g, Coord x, Coord y, int index)
        {
            if (index < 0 || index >= Images.Count)
                return false;

            g.DrawImageUnscaled(Images[index], new(x, y));
            return true;
        }

        /// <inheritdoc/>
        protected override IImageListHandler CreateHandler()
        {
            return GraphicsFactory.Handler.CreateImageListHandler() ?? DummyImageListHandler.Default;
        }
    }
}