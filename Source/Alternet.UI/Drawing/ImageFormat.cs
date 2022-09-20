using System;
using Alternet.Base.Collections;

namespace Alternet.Drawing
{
    /// <summary>Specifies the file format of the image. Not inheritable.</summary>
    public sealed class ImageFormat
    {
        private static ImageFormat bmp = new ImageFormat(new Guid("{64F9239E-23B9-494C-995E-18D24A46A4AA}"));

        private static ImageFormat jpeg = new ImageFormat(new Guid("{2BE18081-AD85-4A7E-9079-6437A36A88CD}"));

        private static ImageFormat png = new ImageFormat(new Guid("{862D7492-9F65-4F93-8494-BC5AA054A8BC}"));

        private static ImageFormat gif = new ImageFormat(new Guid("{8EE85100-C665-4C12-911C-320D5F6B67B4}"));

        private static ImageFormat tiff = new ImageFormat(new Guid("{4DED97F2-0F49-4CA7-9A00-06804EB55097}"));

        private Guid guid;

        /// <summary>Gets a <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
        /// <returns>A <see cref="T:System.Guid" /> structure that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
        public Guid Guid
        {
            get
            {
                return this.guid;
            }
        }

        /// <summary>Gets the bitmap (BMP) image format.</summary>
        /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the bitmap image format.</returns>
        public static ImageFormat Bmp
        {
            get
            {
                return ImageFormat.bmp;
            }
        }

        /// <summary>Gets the Graphics Interchange Format (GIF) image format.</summary>
        /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the GIF image format.</returns>
        public static ImageFormat Gif
        {
            get
            {
                return ImageFormat.gif;
            }
        }

        /// <summary>Gets the Joint Photographic Experts Group (JPEG) image format.</summary>
        /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the JPEG image format.</returns>
        public static ImageFormat Jpeg
        {
            get
            {
                return ImageFormat.jpeg;
            }
        }

        /// <summary>Gets the W3C Portable Network Graphics (PNG) image format.</summary>
        /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the PNG image format.</returns>
        public static ImageFormat Png
        {
            get
            {
                return ImageFormat.png;
            }
        }

        /// <summary>Gets the Tagged Image File Format (TIFF) image format.</summary>
        /// <returns>An <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that indicates the TIFF image format.</returns>
        public static ImageFormat Tiff
        {
            get
            {
                return ImageFormat.tiff;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.ImageFormat" /> class by using the specified <see cref="T:System.Guid" /> structure.</summary>
        /// <param name="guid">The <see cref="T:System.Guid" /> structure that specifies a particular image format. </param>
        public ImageFormat(Guid guid)
        {
            this.guid = guid;
        }

        /// <summary>Returns a value that indicates whether the specified object is an <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that is equivalent to this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</summary>
        /// <returns>true if <paramref name="o" /> is an <see cref="T:System.Drawing.Imaging.ImageFormat" /> object that is equivalent to this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object; otherwise, false.</returns>
        /// <param name="o">The object to test. </param>
        public override bool Equals(object? o)
        {
            var imageFormat = o as ImageFormat;
            return imageFormat != null && this.guid == imageFormat.guid;
        }

        /// <summary>Returns a hash code value that represents this object.</summary>
        /// <returns>A hash code that represents this object.</returns>
        public override int GetHashCode()
        {
            return this.guid.GetHashCode();
        }

        /// <summary>Converts this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object to a human-readable string.</summary>
        /// <returns>A string that represents this <see cref="T:System.Drawing.Imaging.ImageFormat" /> object.</returns>
        public override string ToString()
        {
            if (this == ImageFormat.bmp)
            {
                return "Bmp";
            }
            if (this == ImageFormat.gif)
            {
                return "Gif";
            }
            if (this == ImageFormat.jpeg)
            {
                return "Jpeg";
            }
            if (this == ImageFormat.png)
            {
                return "Png";
            }
            if (this == ImageFormat.tiff)
            {
                return "Tiff";
            }
            return "[ImageFormat: " + this.guid + "]";
        }
    }
}