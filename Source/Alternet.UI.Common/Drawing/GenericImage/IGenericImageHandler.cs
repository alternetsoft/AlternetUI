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
    /// Contains methods and properties which allow to work with generic platform independent image.
    /// </summary>
    public interface IGenericImageHandler : IDisposable, ILockImageBits
    {
        /// <inheritdoc cref="GenericImage.Pixels"/>
        SKColor[] Pixels { get; set; }

        /// <inheritdoc cref="GenericImage.RgbData"/>
        RGBValue[] RgbData { get; set; }

        /// <inheritdoc cref="GenericImage.AlphaData"/>
        byte[] AlphaData { get; set; }

        /// <inheritdoc cref="GenericImage.HasAlpha"/>
        bool HasAlpha { get; }

        /// <inheritdoc cref="GenericImage.HasMask"/>
        bool HasMask { get; }

        /// <inheritdoc cref="GenericImage.IsOk"/>
        bool IsOk { get; }

        /// <summary>
        /// Sets alpha component of the pixel.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="alpha"></param>
        void SetAlpha(int x, int y, byte alpha);

        /// <inheritdoc cref="GenericImage.ClearAlpha"/>
        void ClearAlpha();

        /// <inheritdoc cref="GenericImage.InitAlpha"/>
        void InitAlpha();

        /// <inheritdoc cref="GenericImage.Rescale"/>
        void Rescale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal);

        /// <inheritdoc cref="GenericImage.ResizeNoScale"/>
        void ResizeNoScale(
            SizeI size,
            PointI pos,
            RGBValue? color = null);

        /// <summary>
        /// Converts alpha components of the image to mask. Mask color is choosen automatically.
        /// </summary>
        /// <param name="threshold">Alpha component treshold.</param>
        /// <returns></returns>
        bool ConvertAlphaToMask(byte threshold);

        /// <summary>
        /// Converts alpha components of the image to mask.
        /// </summary>
        /// <param name="rgb">Mask color.</param>
        /// <param name="threshold">Alpha component treshold.</param>
        /// <returns></returns>
        bool ConvertAlphaToMask(RGBValue rgb, byte threshold);

        /// <summary>
        /// Converts image to grayscaled.
        /// </summary>
        /// <param name="weightR">Weight of the R component.</param>
        /// <param name="weightG">Weight of the G component.</param>
        /// <param name="weightB">Weight of the B component.</param>
        /// <returns></returns>
        GenericImage ConvertToGreyscale(double weightR, double weightG, double weightB);

        /// <summary>
        /// Converts this image to the new grey-scaled image.
        /// </summary>
        /// <returns></returns>
        GenericImage ConvertToGreyscale();
    }
}
