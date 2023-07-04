using System;
using System.IO;

namespace Alternet.Drawing
{
    /// <summary>
    /// Encapsulates a bitmap, which consists of the pixel data for a graphics image and its attributes.
    /// A <see cref="Bitmap"/> is an object used to work with images defined by pixel data.
    /// </summary>
    public class Bitmap : Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class.
        /// </summary>
        public Bitmap() : this(new Size())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class with the specified size.
        /// </summary>
        /// <param name="size">The size, in device independent units, of the new <see cref="Bitmap"/>.</param>
        public Bitmap(Size size) : base(size)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified existing image.
        /// </summary>
        /// <param name="image">The <see cref="Image"/> from which to create the new <see cref="Bitmap"/>.</param>
        public Bitmap(Image image) : base()
        {
            NativeImage.CopyFrom(image.NativeImage);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap"/> class from the specified data stream.
        /// </summary>
        /// <param name="stream">The data stream used to load the bitmap.</param>
        public Bitmap(Stream stream) : base(stream)
        {
        }

        internal Bitmap(UI.Native.Image nativeImage) : base(nativeImage)
        {
        }
    }
}