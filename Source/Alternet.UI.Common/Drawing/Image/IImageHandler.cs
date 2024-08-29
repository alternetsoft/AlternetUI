using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with image.
    /// </summary>
    public interface IImageHandler : IDisposable, ILockImageBits
    {
        /// <inheritdoc cref="Image.ScaleFactor"/>
        Coord ScaleFactor { get; set; }

        /// <inheritdoc cref="Image.DipSize"/>
        SizeI DipSize { get; }

        /// <inheritdoc cref="Image.ScaledHeight"/>
        Coord ScaledHeight { get; }

        /// <inheritdoc cref="Image.ScaledSize"/>
        SizeI ScaledSize { get; }

        /// <inheritdoc cref="Image.ScaledWidth"/>
        Coord ScaledWidth { get; }

        /// <inheritdoc cref="Image.PixelSize"/>
        SizeI PixelSize { get; }

        /// <inheritdoc cref="Image.HasMask"/>
        bool HasMask { get; }

        /// <inheritdoc cref="Image.IsOk"/>
        bool IsOk { get; }

        /// <inheritdoc cref="Image.HasAlpha"/>
        bool HasAlpha { get; set; }

        /// <inheritdoc cref="Image.Depth"/>
        int Depth { get; }

        /// <summary>
        /// Loads image from stream.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <returns></returns>
        bool LoadFromStream(Stream stream);

        /// <inheritdoc cref="Image.Load(Stream, BitmapType)"/>
        bool LoadFromStream(Stream stream, BitmapType type);

        /// <inheritdoc cref="Image.Save(Stream, BitmapType, int?)"/>
        bool SaveToStream(Stream stream, BitmapType type, int quality);

        /// <summary>
        /// Convert this image to <see cref="GenericImage"/>.
        /// </summary>
        /// <returns></returns>
        GenericImage ToGenericImage();

        /// <inheritdoc cref="Image.GetSubBitmap"/>
        IImageHandler GetSubBitmap(RectI rect);

        /// <inheritdoc cref="Image.ResetAlpha"/>
        bool ResetAlpha();

        /// <inheritdoc cref="Image.Rescale"/>
        bool Rescale(SizeI sizeNeeded);

        /// <summary>
        /// Assigns pixels of <see cref="GenericImage"/> to this image.
        /// </summary>
        /// <param name="image">Image.</param>
        void Assign(GenericImage image);

        /// <summary>
        /// Assigns pixels of <see cref="SKBitmap"/> to this image.
        /// </summary>
        /// <param name="image">Image.</param>
        void Assign(SKBitmap image);

        /// <summary>
        /// Marks the image as immutable.
        /// </summary>
        /// <remarks>
        /// Marks this image as immutable, meaning that the contents of its pixels will not change
        /// for the lifetime of the image. This state can be set, but it cannot be cleared once it is set.
        /// This state propagates to all other images that share the same pixels.
        /// </remarks>
        void SetImmutable();
    }
}
