using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains properties which describe format of the image pixels.
    /// </summary>
    public struct ImageBitsFormat
    {
        /// <summary>
        /// Number of bits per pixel.
        /// </summary>
        public int BitsPerPixel;

        /// <summary>
        /// Whether alpha component is present in pixel data.
        /// </summary>
        public bool HasAlpha;

        /// <summary>
        /// Size of the pixel in bytes.
        /// </summary>
        public int SizePixel;

        /// <summary>
        /// Offset of the red component.
        /// </summary>
        public int Red;

        /// <summary>
        /// Offset of the green component.
        /// </summary>
        public int Green;

        /// <summary>
        /// Offset of the blue component.
        /// </summary>
        public int Blue;

        /// <summary>
        /// Offset of the alpha component.
        /// </summary>
        public int Alpha;

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the 'Rgba8888' images.
        /// </summary>
        public static ImageBitsFormat Rgba8888
        {
            get
            {
                var result = new ImageBitsFormat();

                result.BitsPerPixel = 32;
                result.HasAlpha = true;
                result.SizePixel = 4;
                result.Red = 0;
                result.Green = 1;
                result.Blue = 2;
                result.Alpha = 3;

                return result;
            }
        }

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the 'Rgb888' images.
        /// </summary>
        public static ImageBitsFormat Rgb888
        {
            get
            {
                var result = new ImageBitsFormat();

                result.BitsPerPixel = 24;
                result.HasAlpha = false;
                result.SizePixel = 3;
                result.Red = 0;
                result.Green = 1;
                result.Blue = 2;
                result.Alpha = -1;

                return result;
            }
        }

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the 'Bgra8888' images.
        /// </summary>
        public static ImageBitsFormat Bgra8888
        {
            get
            {
                var result = new ImageBitsFormat();

                result.BitsPerPixel = 32;
                result.HasAlpha = true;
                result.SizePixel = 4;
                result.Red = 2;
                result.Green = 1;
                result.Blue = 0;
                result.Alpha = 3;

                return result;
            }
        }

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the 'Argb8888Opaque' images.
        /// </summary>
        public static ImageBitsFormat Argb8888Opaque
        {
            get
            {
                var result = new ImageBitsFormat();

                result.BitsPerPixel = 32;
                result.HasAlpha = false;
                result.SizePixel = 4;
                result.Red = 1;
                result.Green = 2;
                result.Blue = 3;
                result.Alpha = -1;

                return result;
            }
        }

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the 'Argb8888' images.
        /// </summary>
        public static ImageBitsFormat Argb8888
        {
            get
            {
                var result = new ImageBitsFormat();

                result.BitsPerPixel = 32;
                result.HasAlpha = true;
                result.SizePixel = 4;
                result.Red = 1;
                result.Green = 2;
                result.Blue = 3;
                result.Alpha = 0;

                return result;
            }
        }

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the 'Bgr888' images.
        /// </summary>
        public static ImageBitsFormat Bgr888
        {
            get
            {
                var result = new ImageBitsFormat();

                result.BitsPerPixel = 24;
                result.HasAlpha = false;
                result.SizePixel = 3;
                result.Red = 2;
                result.Green = 1;
                result.Blue = 0;
                result.Alpha = -1;

                return result;
            }
        }

        /// <summary>
        /// Gets whether or not this image format is 'Bgr888'.
        /// </summary>
        public readonly bool IsBgr888 => Bgr888 == this;

        /// <summary>
        /// Gets whether or not this image format is 'Bgra8888'.
        /// </summary>
        public readonly bool IsBgra8888 => Bgra8888 == this;

        /// <summary>
        /// Gets whether or not this image format is 'Rgba8888'.
        /// </summary>
        public readonly bool IsRgba8888 => Rgba8888 == this;

        /// <summary>
        /// Gets whether or not this image format is 'Rgb888'.
        /// </summary>
        public readonly bool IsRgb888 => Rgb888 == this;

        /// <summary>
        /// Gets whether or not this image format is 'Argb8888'.
        /// </summary>
        public readonly bool IsArgb8888 => Argb8888 == this;

        /// <summary>
        /// Gets whether or not this image format is 'Argb8888Opaque'.
        /// </summary>
        public readonly bool IsArgb8888Opaque => Argb8888Opaque == this;

        /// <summary>
        /// Gets <see cref="SKColorType"/>.
        /// </summary>
        public readonly SKColorType ColorType
        {
            get
            {
                if (IsBgra8888)
                    return SKColorType.Bgra8888;
                if (IsRgba8888)
                    return SKColorType.Rgba8888;
                return SKColorType.Unknown;
            }
        }

        /// <summary>
        /// Tests whether two specified <see cref="ImageBitsFormat"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="ImageBitsFormat"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="ImageBitsFormat"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="ImageBitsFormat"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ImageBitsFormat left, ImageBitsFormat right)
        {
            return
                left.BitsPerPixel == right.BitsPerPixel &&
                left.HasAlpha == right.HasAlpha &&
                left.SizePixel == right.SizePixel &&
                left.Red == right.Red &&
                left.Green == right.Green &&
                left.Blue == right.Blue &&
                left.Alpha == right.Alpha;
        }

        /// <summary>
        /// Tests whether two specified <see cref="ImageBitsFormat"/> structures are not equivalent.
        /// </summary>
        /// <param name="left">The <see cref="ImageBitsFormat"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="ImageBitsFormat"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="ImageBitsFormat"/> structures
        /// are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ImageBitsFormat left, ImageBitsFormat right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Logs this object properties.
        /// </summary>
        /// <param name="sectionName">Section name. Optional.</param>
        public readonly void Log(string? sectionName = null)
        {
            var self = this;

            App.LogSection(
                () =>
                {
                    App.LogNameValue("BitsPerPixel", self.BitsPerPixel);
                    App.LogNameValue("HasAlpha", self.HasAlpha);
                    App.LogNameValue("SizePixel", self.SizePixel);
                    App.LogNameValue("Red", self.Red);
                    App.LogNameValue("Green", self.Green);
                    App.LogNameValue("Blue", self.Blue);
                    App.LogNameValue("Alpha", self.Alpha);
                },
                sectionName);
        }

        /// <summary>
        /// Compares this object with another object.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns></returns>
        public readonly override bool Equals(object? obj)
        {
            if (obj is not ImageBitsFormat value)
                return false;
            return this == value;
        }

        /// <summary>
        /// Gets hash code of this object.
        /// </summary>
        /// <returns></returns>
        public readonly override int GetHashCode()
        {
            return (BitsPerPixel, HasAlpha, SizePixel, Red, Green, Blue, Alpha).GetHashCode();
        }
    }
}