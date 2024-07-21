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

        /// <summary>
        /// Gets or sets image load flags.
        /// </summary>
        GenericImageLoadFlags LoadFlags { get; set; }

        /// <inheritdoc cref="GenericImage.BestStrategy"/>
        GenericImage.PixelStrategy BestStrategy { get; }

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

        /// <inheritdoc cref="GenericImage.SetMask"/>
        void SetMask(bool hasMask = true);

        /// <inheritdoc cref="GenericImage.SetMaskColor"/>
        void SetMaskColor(RGBValue rgb);

        /// <inheritdoc cref="GenericImage.SetMaskFromImage"/>
        bool SetMaskFromImage(GenericImage image, RGBValue mask);

        /// <inheritdoc cref="GenericImage.SetOptionAsString"/>
        void SetOptionAsString(string name, string value);

        /// <inheritdoc cref="GenericImage.SetOptionAsInt"/>
        void SetOptionAsInt(string name, int value);

        /// <inheritdoc cref="GenericImage.SetRGB"/>
        void SetRGB(int x, int y, RGBValue rgb);

        /// <inheritdoc cref="GenericImage.SetRGBRect"/>
        void SetRGBRect(RGBValue rgb, RectI? rect = null);

        /// <inheritdoc cref="GenericImage.SetImageType"/>
        void SetImageType(BitmapType type);

        /// <inheritdoc cref="GenericImage.Copy"/>
        GenericImage Copy();

        /// <inheritdoc cref="GenericImage.Reset(int, int, bool)"/>
        bool Reset(int width, int height, bool clear = false);

        /// <inheritdoc cref="GenericImage.Clear"/>
        void Clear(byte value = 0);

        /// <inheritdoc cref="GenericImage.Reset()"/>
        void Reset();

        /// <inheritdoc cref="GenericImage.FindFirstUnusedColor"/>
        Color FindFirstUnusedColor(RGBValue? startRGB = null);

        /// <inheritdoc cref="GenericImage.InitAlpha"/>
        void InitAlpha();

        /// <inheritdoc cref="GenericImage.Blur"/>
        GenericImage Blur(int blurRadius);

        /// <inheritdoc cref="GenericImage.BlurHorizontal"/>
        GenericImage BlurHorizontal(int blurRadius);

        /// <inheritdoc cref="GenericImage.BlurVertical"/>
        GenericImage BlurVertical(int blurRadius);

        /// <inheritdoc cref="GenericImage.Mirror"/>
        GenericImage Mirror(bool horizontally = true);

        /// <inheritdoc cref="GenericImage.Paste"/>
        void Paste(
            GenericImage image,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite);

        /// <inheritdoc cref="GenericImage.Replace"/>
        void Replace(RGBValue r1, RGBValue r2);

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

        /// <inheritdoc cref="GenericImage.SizeNoScale"/>
        GenericImage SizeNoScale(
            SizeI size,
            PointI pos = default,
            RGBValue? color = null);

        /// <inheritdoc cref="GenericImage.Rotate90"/>
        GenericImage Rotate90(bool clockwise = true);

        /// <inheritdoc cref="GenericImage.Rotate180"/>
        GenericImage Rotate180();

        /// <inheritdoc cref="GenericImage.RotateHue"/>
        void RotateHue(double angle);

        /// <inheritdoc cref="GenericImage.ChangeSaturation"/>
        void ChangeSaturation(double factor);

        /// <inheritdoc cref="GenericImage.ChangeBrightness"/>
        void ChangeBrightness(double factor);

        /// <inheritdoc cref="GenericImage.ChangeHSV"/>
        void ChangeHSV(double angleH, double factorS, double factorV);

        /// <inheritdoc cref="GenericImage.Scale"/>
        GenericImage Scale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal);

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

        /// <inheritdoc cref="GenericImage.ConvertToMono"/>
        GenericImage ConvertToMono(RGBValue rgb);

        /// <inheritdoc cref="GenericImage.GetAlpha"/>
        byte GetAlpha(int x, int y);

        /// <inheritdoc cref="GenericImage.GetRGB"/>
        RGBValue GetRGB(int x, int y);

        /// <inheritdoc cref="GenericImage.GetPixel"/>
        Color GetPixel(int x, int y, bool withAlpha = false);

        /// <inheritdoc cref="GenericImage.SetPixel"/>
        void SetPixel(int x, int y, Color color, bool withAlpha = false);

        /// <inheritdoc cref="GenericImage.GetRed"/>
        byte GetRed(int x, int y);

        /// <inheritdoc cref="GenericImage.GetGreen"/>
        byte GetGreen(int x, int y);

        /// <inheritdoc cref="GenericImage.GetBlue"/>
        byte GetBlue(int x, int y);

        /// <inheritdoc cref="GenericImage.GetMaskRGB"/>
        RGBValue GetMaskRGB();

        /// <inheritdoc cref="GenericImage.GetMaskRed"/>
        byte GetMaskRed();

        /// <inheritdoc cref="GenericImage.GetMaskGreen"/>
        byte GetMaskGreen();

        /// <inheritdoc cref="GenericImage.GetMaskBlue"/>
        byte GetMaskBlue();

        /// <inheritdoc cref="GenericImage.GetOptionAsString"/>
        string GetOptionAsString(string name);

        /// <inheritdoc cref="GenericImage.GetOptionAsInt"/>
        int GetOptionAsInt(string name);

        /// <inheritdoc cref="GenericImage.GetSubImage"/>
        GenericImage GetSubImage(RectI rect);

        /// <inheritdoc cref="GenericImage.GetImageType"/>
        BitmapType GetImageType();

        /// <inheritdoc cref="GenericImage.HasOption"/>
        bool HasOption(string name);

        /// <inheritdoc cref="GenericImage.IsTransparent"/>
        bool IsTransparent(int x, int y, byte threshold);

        /// <inheritdoc cref="GenericImage.LoadFromStream(Stream, BitmapType, int)"/>
        bool LoadFromStream(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        /// <inheritdoc cref="GenericImage.LoadFromStream(Stream, string, int)"/>
        bool LoadFromStream(
            Stream stream,
            string mimetype,
            int index = -1);

        /// <inheritdoc cref="GenericImage.SaveToStream(Stream, string)"/>
        bool SaveToStream(Stream stream, string mimetype);

        /// <inheritdoc cref="GenericImage.SaveToStream(Stream, BitmapType)"/>
        bool SaveToStream(Stream stream, BitmapType type);
    }
}
