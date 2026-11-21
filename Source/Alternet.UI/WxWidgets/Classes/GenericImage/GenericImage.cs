using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements platform independent image.
    /// </summary>
    internal class GenericImage : HandledObject<IGenericImageHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">Image data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, SKColor[] data)
        {
            Handler = new WxGenericImageHandler(width, height, data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(IGenericImageHandler handle)
        {
            Handler = handle;
        }

        /// <summary>
        /// Returns <c>true</c> if this image has alpha channel, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasAlpha
        {
            get
            {
                return Handler.HasAlpha;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if there is a mask active, <c>false</c> otherwise.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual bool HasMask
        {
            get
            {
                return Handler.HasMask;
            }
        }

        /// <summary>
        /// Gets the width of the image in pixels.
        /// </summary>
        public virtual int Width
        {
            get => Handler.Width;
        }

        /// <summary>
        /// Gets the height of the image in pixels.
        /// </summary>
        public virtual int Height
        {
            get => Handler.Height;
        }

        /// <summary>
        /// Gets or sets pixels using array of <see cref="SKColor"/>.
        /// </summary>
        public virtual SKColor[] Pixels
        {
            get => Handler.Pixels;
            set => Handler.Pixels = value;
        }

        /// <summary>
        /// Returns <c>true</c> if image data is present.
        /// </summary>
        public virtual bool IsOk
        {
            get => Handler.IsOk;
        }

        /// <summary>
        /// Converts <see cref="SKBitmap"/> to <see cref="GenericImage"/>.
        /// </summary>
        /// <param name="bitmap">Image to convert.</param>
        /// <returns></returns>
        public static GenericImage FromSkia(SKBitmap bitmap)
        {
            var result = new GenericImage(bitmap.Width, bitmap.Height, bitmap.Pixels);
            return result;
        }

        /// <inheritdoc/>
        protected override IGenericImageHandler CreateHandler()
        {
            return new WxGenericImageHandler();
        }
    }
}