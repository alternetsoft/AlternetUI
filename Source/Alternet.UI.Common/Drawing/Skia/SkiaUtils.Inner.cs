using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public static partial class SkiaUtils
    {
        /// <summary>
        /// Represents a cached bitmap canvas, including its graphics context, size,
        /// scale factor, and transparency.
        /// </summary>
        public class BitmapCanvasCached : DisposableObject
        {
            /// <summary>
            /// Gets or sets the <see cref="SkiaGraphics"/> instance used for drawing on the bitmap.
            /// </summary>
            public SkiaGraphics? Graphics;

            /// <summary>
            /// Gets or sets the size of the bitmap canvas in device-independent units.
            /// </summary>
            public SizeD Size;

            /// <summary>
            /// Gets or sets the scale factor used for the bitmap canvas.
            /// </summary>
            public Coord ScaleFactor;

            /// <summary>
            /// Gets or sets a value indicating whether the bitmap canvas is transparent.
            /// </summary>
            public bool IsTransparent;

            /// <summary>
            /// Gets or sets a value indicating whether the <see cref="Graphics"/>
            /// instance should be disposed when this instance is disposed.
            /// </summary>
            public bool DisposeGraphics = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="BitmapCanvasCached"/> class with default values.
            /// </summary>
            public BitmapCanvasCached()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="BitmapCanvasCached"/> class
            /// with the specified size, scale factor, and transparency.
            /// </summary>
            /// <param name="size">The size of the bitmap canvas.</param>
            /// <param name="scaleFactor">The scale factor for the bitmap canvas.</param>
            /// <param name="isTransparent">Indicates whether the bitmap canvas is transparent.</param>
            public BitmapCanvasCached(SizeD size, Coord scaleFactor, bool isTransparent = true)
            {
                Size = size;
                ScaleFactor = scaleFactor;
                IsTransparent = isTransparent;
            }

            /// <summary>
            /// Determines whether the specified specified size, scale factor,
            /// and transparency flag are equal to those used in the current instance.
            /// </summary>
            /// <param name="size">The size to compare with the value stored in the current instance.</param>
            /// <param name="scaleFactor">The scale factor to compare with the value
            /// stored in the current instance.</param>
            /// <param name="isTransparent">A value indicating whether the transparency flag to compare
            /// matches the value stored in the current instance.</param>
            /// <returns><see langword="true"/> if the specified <see cref="SizeD"/>, <see cref="Coord"/>,
            /// and transparency flag  are equal to the values stored in the current instance;
            /// otherwise, <see langword="false"/>.</returns>
            public bool Equals(SizeD size, Coord scaleFactor, bool isTransparent)
            {
                return Size == size && ScaleFactor == scaleFactor && IsTransparent == isTransparent;
            }

            /// <summary>
            /// Releases resources used by the <see cref="BitmapCanvasCached"/> class,
            /// optionally disposing the <see cref="Graphics"/> instance.
            /// </summary>
            protected override void DisposeManaged()
            {
                if (DisposeGraphics)
                {
                    SafeDispose(ref Graphics);
                }

                base.DisposeManaged();
            }
        }
    }
}
