using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the attributes of a bitmap image. The <see cref="BitmapData" /> class is
    /// used by the <see cref="Image.LockSurface" />.</summary>
    internal class BitmapData : DisposableObject, IBitmapData
    {
        private int width;
        private int height;
        private int stride;
        private PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
        private IntPtr scan0;
        private int reserved;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitmapData" /> class.
        /// </summary>
        public BitmapData()
        {
        }

        /// <summary>
        /// Gets or sets the pixel width of the <see cref="Image" /> object.
        /// This can also be thought of as the number of pixels in one scan line.
        /// </summary>
        /// <returns>The pixel width of the <see cref="Image" /> object.</returns>
        public int Width
        {
            get
            {
                return width;
            }

            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Gets or sets the pixel height of the <see cref="Image" /> object.
        /// Also sometimes referred to as the number of scan lines.
        /// </summary>
        /// <returns>The pixel height of the <see cref="Image" /> object.</returns>
        public int Height
        {
            get
            {
                return height;
            }

            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Gets or sets the stride width (also known as scan width) of the <see cref="Image" /> object.
        /// </summary>
        /// <returns>
        /// The stride width, in bytes, of the <see cref="Image" /> object.
        /// </returns>
        public int Stride
        {
            get
            {
                return stride;
            }

            set
            {
                stride = value;
            }
        }

        /// <summary>
        /// Gets or sets the format of the pixel information in the <see cref="Image" /> object
        /// that returned this <see cref="BitmapData" /> object.</summary>
        /// <returns>A <see cref="PixelFormat" /> that specifies the format of the pixel
        /// information in the associated <see cref="Bitmap" /> object.</returns>
        public PixelFormat PixelFormat
        {
            get
            {
                return pixelFormat;
            }

            set
            {
                pixelFormat = value;
            }
        }

        /// <summary>
        /// Gets or sets the address of the first pixel data in the bitmap.
        /// This can also be thought of as the first scan line in the bitmap.
        /// </summary>
        /// <returns>The address of the first pixel data in the bitmap.</returns>
        public IntPtr Scan0
        {
            get
            {
                return scan0;
            }

            set
            {
                scan0 = value;
            }
        }

        /// <summary>
        /// Reserved property. Do not use.
        /// </summary>
        public int Reserved
        {
            get
            {
                return reserved;
            }

            set
            {
                reserved = value;
            }
        }
    }
}
