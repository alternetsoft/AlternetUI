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
    public interface IGraphicsFactoryHandler : IDisposable
    {
        GenericImageLoadFlags GenericImageDefaultLoadFlags { get; set; }

        Graphics CreateMemoryCanvas(double scaleFactor);

        Graphics CreateMemoryCanvas(Image image);

        BitmapType GetDefaultBitmapType();

        IFontFactoryHandler CreateFontFactoryHandler();

        /// <summary>
        /// Creates transparent brush handler.
        /// </summary>
        /// <returns></returns>
        IBrushHandler CreateTransparentBrushHandler(Brush brush);

        /// <summary>
        /// Creates hatch brush handler.
        /// </summary>
        /// <returns></returns>
        IHatchBrushHandler CreateHatchBrushHandler(HatchBrush brush);

        /// <summary>
        /// Creates linear gradient brush handler.
        /// </summary>
        /// <returns></returns>
        ILinearGradientBrushHandler CreateLinearGradientBrushHandler(LinearGradientBrush brush);

        /// <summary>
        /// Creates radial gradient brush handler.
        /// </summary>
        /// <returns></returns>
        IRadialGradientBrushHandler CreateRadialGradientBrushHandler(RadialGradientBrush brush);

        IPenHandler CreatePenHandler(Pen pen);

        /// <summary>
        /// Creates solid brush handler.
        /// </summary>
        /// <returns></returns>
        ISolidBrushHandler CreateSolidBrushHandler(SolidBrush brush);

        /// <summary>
        /// Creates texture brush handler.
        /// </summary>
        /// <returns></returns>
        ITextureBrushHandler CreateTextureBrushHandler(TextureBrush brush);

        Graphics CreateGraphicsFromScreen();

        IRegionHandler CreateRegionHandler();

        IRegionHandler CreateRegionHandler(RectD rect);

        IRegionHandler CreateRegionHandler(Region region);

        IRegionHandler CreateRegionHandler(PointD[] points, FillMode fillMode = FillMode.Alternate);

        IGraphicsPathHandler CreateGraphicsPathHandler();

        ImageBitsFormat GetImageBitsFormat(ImageBitsFormatKind kind);

        IGraphicsPathHandler CreateGraphicsPathHandler(Graphics drawingContext);

        Graphics CreateGraphicsFromImage(Image image);

        IImageHandler CreateImageHandler(ImageSet imageSet, SizeI size);

        IImageHandler CreateImageHandler();

        IImageHandler CreateImageHandler(GenericImage genericImage, int depth = -1);

        IImageHandler CreateImageHandler(int width, int height, Graphics dc);

        IImageHandler CreateImageHandler(ImageSet imageSet, IControl control);

        IImageHandler CreateImageHandler(GenericImage genericImage, Graphics dc);

        IImageHandler CreateImageHandler(Image image);

        IImageHandler CreateImageHandler(Image original, SizeI newSize);

        IImageHandler CreateImageHandler(SizeI size, int depth = 32);

        IImageHandler CreateImageHandlerFromScreen();

        IImageHandler CreateImageHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color = null);

        IImageHandler CreateImageHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color = null);

        IImageSetHandler? CreateImageSetHandler();

        IImageSetHandler CreateImageSetHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color = null);

        IImageSetHandler CreateImageSetHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color = null);

        IImageListHandler? CreateImageListHandler();

        IIconSetHandler? CreateIconSetHandler();

        bool CanReadGenericImage(Stream stream);

        string GetGenericImageExtWildcard();

        int GetGenericImageCount(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any);

        IGenericImageHandler CreateGenericImageHandler();

        IGenericImageHandler CreateGenericImageHandler(int width, int height, bool clear = false);

        IGenericImageHandler CreateGenericImageHandler(SizeI size, bool clear = false);

        IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            string mimeType,
            int index = -1);

        IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data);

        IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            SKColor[] data);

        IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data,
            byte[] alpha);
    }
}
