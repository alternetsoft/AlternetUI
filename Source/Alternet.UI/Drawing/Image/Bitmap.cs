using System;
using System.IO;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates a bitmap, which consists of the pixel data for a graphics image and
    /// its attributes.
    /// A <see cref="Bitmap"/> is an object used to work with images defined by pixel data.
    /// </summary>
    public class Bitmap : Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from
        /// the specified <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="genericImage">Generic image.</param>
        /// <param name="depth">Specifies the depth of the bitmap.
        /// Some platforms only support (1) for monochrome and (-1) for the current color setting.
        /// A depth of 32 including an alpha channel is supported under MSW, Mac and Linux.
        /// If this parameter is omitted
        /// (= -1), the display depth of the screen is used.</param>
        public Bitmap(GenericImage genericImage, int depth = -1)
            : base(genericImage, depth)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class
        /// with the specified size in device pixels.
        /// </summary>
        /// <param name="width">The width in pixels used to create the image.</param>
        /// <param name="height">The height in pixels used to create the image.</param>
        public Bitmap(int width, int height)
            : base(width, height)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class.
        /// </summary>
        public Bitmap()
            : base(Drawing.Int32Size.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class with the specified size.
        /// </summary>
        /// <param name="size">The size, in device pixels, of the new <see cref="Bitmap"/>.</param>
        public Bitmap(Int32Size size)
            : base(size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class with the image from
        /// <see cref="ImageSet"/>.
        /// </summary>
        /// <param name="imageSet">Source of the image.</param>
        /// <param name="size">Size of the image in device pixels.</param>
        public Bitmap(ImageSet imageSet, Int32Size size)
            : base(imageSet, size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap" /> class from the specified
        /// existing image, scaled to the specified size.
        /// </summary>
        /// <param name="original">The <see cref="Image" /> from which to create the
        /// new <see cref="Bitmap" />.</param>
        /// <param name="newSize">The <see cref="Int32Size" /> structure that represent the
        /// size of the new <see cref="Bitmap" />.</param>
        public Bitmap(Image original, Int32Size newSize)
            : base(original, newSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified
        /// existing image.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> from which to create the
        /// new <see cref="Bitmap"/>.</param>
        /// <remarks>
        /// Full image data is copied from the original image.
        /// </remarks>
        public Bitmap(Image image)
            : base()
        {
            NativeImage.CopyFrom(image.NativeImage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified
        /// data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the bitmap.</param>
        public Bitmap(Stream stream)
            : base(stream)
        {
        }

        internal Bitmap(UI.Native.Image nativeImage)
            : base(nativeImage)
        {
        }

        /// <summary>
        /// Creates a clone of this image with fully copied image data.
        /// </summary>
        /// <returns></returns>
        public new Bitmap Clone()
        {
            return new Bitmap(this);
        }
    }
}