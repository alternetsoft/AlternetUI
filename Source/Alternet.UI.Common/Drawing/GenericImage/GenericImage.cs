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
    public class GenericImage : HandledObject<IGenericImageHandler>
    {
        /// <summary>
        /// Gets an empty generic image.
        /// </summary>
        public static readonly GenericImage Empty = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an empty image without an alpha channel.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image with the given size and clears it if requested.
        /// Does not create an alpha channel.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="clear">If true, initialize the image to black.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, bool clear = false)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(width, height, clear);
        }

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
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(width, height, data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericImage"/> class.
        /// Creates an image from data in memory.
        /// </summary>
        /// <param name="width">Specifies the width of the image.</param>
        /// <param name="height">Specifies the height of the image.</param>
        /// <param name="data">RGB data.</param>
        /// <param name="alpha">Alpha-channel data.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GenericImage(int width, int height, RGBValue[] data, byte[] alpha)
        {
            Handler = GraphicsFactory.Handler.CreateGenericImageHandler(
                width,
                height,
                data,
                alpha);
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
        /// Gets or sets pixels using array of <see cref="RGBValue"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AlphaData"/> to get alpha component of the pixels.
        /// </remarks>
        public virtual RGBValue[] RgbData
        {
            get => Handler.RgbData;
            set => Handler.RgbData = value;
        }

        /// <summary>
        /// Gets or sets alpha component of the pixels using array of <see cref="byte"/>.
        /// </summary>
        /// <remarks>
        /// Use <see cref="RgbData"/> to get alpha component of the pixels.
        /// </remarks>
        public virtual byte[] AlphaData
        {
            get => Handler.AlphaData;
            set => Handler.AlphaData = value;
        }

        /// <summary>
        /// Returns <c>true</c> if image data is present.
        /// </summary>
        public virtual bool IsOk
        {
            get => Handler.IsOk;
        }

        /// <summary>
        /// Converts <see cref="GenericImage"/> to <see cref="SKBitmap"/>. Optionally
        /// copies pixel data.
        /// </summary>
        /// <param name="bitmap">Image.</param>
        /// <param name="assignPixels">Whether to copy pixel data. Optional. Default is <c>true</c>.</param>
        /// <returns></returns>
        public static SKBitmap ToSkia(GenericImage bitmap, bool assignPixels = true)
        {
            return ToSkia(bitmap.Handler, assignPixels);
        }

        /// <summary>
        /// Converts image specified with <see cref="IGenericImageHandler"/> to
        /// <see cref="SKBitmap"/>. Optionally copies pixel data.
        /// </summary>
        /// <param name="bitmap">Image.</param>
        /// <param name="assignPixels">Whether to copy pixel data. Optional. Default is <c>true</c>.</param>
        /// <returns></returns>
        public static SKBitmap ToSkia(IGenericImageHandler bitmap, bool assignPixels = true)
        {
            var result = DrawingUtils.CreateSkiaBitmapForImage(bitmap.Width, bitmap.Height, bitmap.HasAlpha);

            if (assignPixels)
                result.Pixels = bitmap.Pixels;
            return result;
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
            return GraphicsFactory.Handler.CreateGenericImageHandler();
        }
    }
}