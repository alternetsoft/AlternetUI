using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Represents a dynamically resizable bitmap that supports transparency and scaling.
    /// </summary>
    /// <remarks>This class provides functionality for creating and managing a bitmap with dynamic size, scale
    /// factor, and transparency settings. It extends the <see cref="DynamicBitmap{Bitmap}"/> class,
    /// specializing it for use with <see cref="Bitmap"/>.</remarks>
    public class DynamicBitmap : DynamicBitmap<Bitmap>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicBitmap"/> class.
        /// </summary>
        public DynamicBitmap()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicBitmap"/> class.
        /// </summary>
        /// <param name="size"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="isTransparent"></param>
        public DynamicBitmap(SizeD size, Coord scaleFactor, bool isTransparent)
            : base(size, scaleFactor, isTransparent)
        {
        }

        /// <summary>
        /// Updates the properties of an existing <see cref="DynamicBitmap"/> instance or creates
        /// a new instance if the reference is null.
        /// </summary>
        /// <remarks>If the <paramref name="bitmap"/> parameter is null, a new <see cref="DynamicBitmap"/>
        /// instance is created with the specified properties.
        /// If the <paramref name="bitmap"/> parameter is not null,
        /// its properties are updated to match the specified values.</remarks>
        /// <param name="bitmap">A reference to the <see cref="DynamicBitmap"/> instance to update.
        /// If null, a new instance is created and assigned to this reference.</param>
        /// <param name="size">The dimensions of the bitmap, specified as a <see cref="SizeD"/> structure.</param>
        /// <param name="scaleFactor">The scaling factor to apply to the bitmap, specified
        /// as a <see cref="Coord"/>.</param>
        /// <param name="isTransparent">A value indicating whether the bitmap should support transparency.
        /// <see langword="true"/> if transparency is
        /// enabled; otherwise, <see langword="false"/>.</param>
        public static void CreateOrUpdate(
            [NotNull] ref DynamicBitmap? bitmap,
            SizeD size,
            Coord scaleFactor,
            bool isTransparent)
        {
            if (bitmap == null)
                bitmap = new DynamicBitmap(size, scaleFactor, isTransparent);
            else
                bitmap.SetDynamicProperties(size, scaleFactor, isTransparent);
        }

        /// <inheritdoc/>
        public override Bitmap CreateBitmap()
        {
            var sizeI = SizeInPixels;
            var bmp = new Drawing.Bitmap(sizeI.Width, sizeI.Height);
            bmp.HasAlpha = IsTransparent;
            bmp.ScaleFactor = ScaleFactor;
            return bmp;
        }
    }
}
