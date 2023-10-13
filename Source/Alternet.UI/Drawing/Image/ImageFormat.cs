using System;
using Alternet.Base.Collections;

namespace Alternet.Drawing
{
    /// <summary>Specifies the file format of the image. Not inheritable.</summary>
    public sealed class ImageFormat
    {
        private readonly Guid guid;

        /// <summary>Initializes a new instance of the
        /// <see cref="System.Drawing.Imaging.ImageFormat" /> class by using
        /// the specified <see cref="System.Guid" /> structure.</summary>
        /// <param name="guid">The <see cref="System.Guid" /> structure that
        /// specifies a particular image format. </param>
        public ImageFormat(Guid guid)
        {
            this.guid = guid;
        }

        /// <summary>Gets the bitmap (BMP) image format.</summary>
        /// <returns>An <see cref="System.Drawing.Imaging.ImageFormat" />
        /// object that indicates the bitmap image format.</returns>
        public static ImageFormat Bmp { get; } =
            new(new Guid("{64F9239E-23B9-494C-995E-18D24A46A4AA}"));

        /// <summary>Gets the Graphics Interchange Format (GIF) image format.</summary>
        /// <returns>An <see cref="System.Drawing.Imaging.ImageFormat" /> object
        /// that indicates the GIF image format.</returns>
        public static ImageFormat Gif { get; } =
            new(new Guid("{8EE85100-C665-4C12-911C-320D5F6B67B4}"));

        /// <summary>Gets the Joint Photographic Experts Group (JPEG) image
        /// format.</summary>
        /// <returns>An <see cref="System.Drawing.Imaging.ImageFormat" /> object
        /// that indicates the JPEG image format.</returns>
        public static ImageFormat Jpeg { get; } =
            new(new Guid("{2BE18081-AD85-4A7E-9079-6437A36A88CD}"));

        /// <summary>Gets the W3C Portable Network Graphics (PNG) image
        /// format.</summary>
        /// <returns>An <see cref="System.Drawing.Imaging.ImageFormat" /> object
        /// that indicates the PNG image format.</returns>
        public static ImageFormat Png { get; } =
            new(new Guid("{862D7492-9F65-4F93-8494-BC5AA054A8BC}"));

        /// <summary>Gets the Tagged Image File Format (TIFF) image format.</summary>
        /// <returns>An <see cref="System.Drawing.Imaging.ImageFormat" /> object
        /// that indicates the TIFF image format.</returns>
        public static ImageFormat Tiff { get; } =
            new(new Guid("{4DED97F2-0F49-4CA7-9A00-06804EB55097}"));

        /// <summary>Gets a <see cref="System.Guid" /> structure that represents
        /// this <see cref="System.Drawing.Imaging.ImageFormat" /> object.</summary>
        /// <returns>A <see cref="System.Guid" /> structure that represents this
        /// <see cref="System.Drawing.Imaging.ImageFormat" /> object.</returns>
        public Guid Guid
        {
            get
            {
                return this.guid;
            }
        }

        /// <summary>Returns a value that indicates whether the specified object
        /// is an <see cref="System.Drawing.Imaging.ImageFormat" /> object
        /// that is equivalent to this
        /// <see cref="System.Drawing.Imaging.ImageFormat" />
        /// object.</summary>
        /// <returns>true if <paramref name="o" /> is an
        /// <see cref="System.Drawing.Imaging.ImageFormat" /> object that is
        /// equivalent to this <see cref="System.Drawing.Imaging.ImageFormat" />
        /// object; otherwise, false.</returns>
        /// <param name="o">The object to test. </param>
        public override bool Equals(object? o)
        {
            return o is ImageFormat imageFormat && this.guid == imageFormat.guid;
        }

        /// <summary>Returns a hash code value that represents this object.</summary>
        /// <returns>A hash code that represents this object.</returns>
        public override int GetHashCode()
        {
            return this.guid.GetHashCode();
        }

        /// <summary>Converts this <see cref="T:System.Drawing.Imaging.ImageFormat" />
        /// object to a human-readable string.</summary>
        /// <returns>A string that represents this
        /// <see cref="System.Drawing.Imaging.ImageFormat" /> object.</returns>
        public override string ToString()
        {
            if (this == ImageFormat.Bmp)
                return "Bmp";
            if (this == ImageFormat.Gif)
                return "Gif";
            if (this == ImageFormat.Jpeg)
                return "Jpeg";
            if (this == ImageFormat.Png)
                return "Png";
            if (this == ImageFormat.Tiff)
                return "Tiff";
            return "[ImageFormat: " + this.guid + "]";
        }
    }
}