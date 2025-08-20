using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a null or empty image source.
    /// Implements <see cref="IImageSource"/> with default or null values.
    /// </summary>
    public class NullImageSource : BaseObject, IImageSource
    {
        bool IImageSource.IsEmpty => GetImageSourceIsEmpty();

        ImageSourceKind IImageSource.Kind => GetImageSourceKind();

        Image? IImageSource.Image => GetImageSourceImage();

        ImageList? IImageSource.ImageList => GetImageSourceImageList();

        int IImageSource.ImageIndex => GetImageSourceImageIndex();

        ImageSet? IImageSource.ImageSet => GetImageSourceImageSet();

        SvgImage? IImageSource.SvgImage => GetImageSourceSvgImage();

        int? IImageSource.SvgSize => GetImageSourceSvgSize();

        /// <summary>
        /// Returns the kind of the image source.
        /// </summary>
        protected virtual ImageSourceKind GetImageSourceKind()
        {
            return ImageSourceKind.None;
        }

        /// <summary>
        /// Returns the image associated with this image source.
        /// </summary>
        protected virtual Image? GetImageSourceImage()
        {
            return null;
        }

        /// <summary>
        /// Returns the image list associated with this image source.
        /// </summary>
        protected virtual ImageList? GetImageSourceImageList()
        {
            return null;
        }

        /// <summary>
        /// Returns the image index in the image list.
        /// </summary>
        protected virtual int GetImageSourceImageIndex()
        {
            return -1;
        }

        /// <summary>
        /// Returns the image set associated with this image source.
        /// </summary>
        protected virtual ImageSet? GetImageSourceImageSet()
        {
            return null;
        }

        /// <summary>
        /// Returns the SVG image associated with this image source.
        /// </summary>
        protected virtual SvgImage? GetImageSourceSvgImage()
        {
            return null;
        }

        /// <summary>
        /// Returns the SVG size associated with this image source.
        /// </summary>
        protected virtual int? GetImageSourceSvgSize()
        {
            return null;
        }

        /// <summary>
        /// Determines whether the image source is empty.
        /// </summary>
        /// <returns><see langword="true"/> if the image source is empty;
        /// otherwise, <see langword="false"/>.</returns>
        protected virtual bool GetImageSourceIsEmpty()
        {
            return true;
        }
    }
}