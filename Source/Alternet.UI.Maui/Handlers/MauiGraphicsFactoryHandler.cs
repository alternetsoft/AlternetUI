using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using SkiaSharp;

namespace Alternet.UI
{
    internal partial class MauiGraphicsFactoryHandler : DisposableObject, IGraphicsFactoryHandler
    {
        GenericImageLoadFlags IGraphicsFactoryHandler.GenericImageDefaultLoadFlags { get; set; }

        /// <inheritdoc/>
        IPenHandler IGraphicsFactoryHandler.CreatePenHandler(Pen pen)
        {
            return new PlessPenHandler(pen);
        }

        /// <inheritdoc/>
        IFontFactoryHandler IGraphicsFactoryHandler.CreateFontFactoryHandler()
        {
            return new SkiaFontFactoryHandler();
        }

        /// <inheritdoc/>
        IBrushHandler IGraphicsFactoryHandler.CreateTransparentBrushHandler(Brush brush)
        {
            return new PlessBrushHandler(brush);
        }

        /// <inheritdoc/>
        ISolidBrushHandler IGraphicsFactoryHandler.CreateSolidBrushHandler(SolidBrush brush)
        {
            return new PlessSolidBrushHandler(brush);
        }

        /// <inheritdoc/>
        ITextureBrushHandler IGraphicsFactoryHandler.CreateTextureBrushHandler(TextureBrush brush)
        {
            return new PlessTextureBrushHandler(brush);
        }

        /// <inheritdoc/>
        IHatchBrushHandler IGraphicsFactoryHandler.CreateHatchBrushHandler(HatchBrush brush)
        {
            return new PlessHatchBrushHandler(brush);
        }

        /// <inheritdoc/>
        ILinearGradientBrushHandler IGraphicsFactoryHandler.CreateLinearGradientBrushHandler(
            LinearGradientBrush brush)
        {
            return new PlessLinearGradientBrushHandler(brush);
        }

        /// <inheritdoc/>
        IRadialGradientBrushHandler IGraphicsFactoryHandler.CreateRadialGradientBrushHandler(
            RadialGradientBrush brush)
        {
            return new PlessRadialGradientBrushHandler(brush);
        }

        bool IGraphicsFactoryHandler.CanReadGenericImage(Stream stream)
        {
            throw new NotImplementedException();
        }

        IGenericImageHandler IGraphicsFactoryHandler.CreateGenericImageHandler()
        {
            throw new NotImplementedException();
        }

        IGenericImageHandler IGraphicsFactoryHandler.CreateGenericImageHandler(
            int width,
            int height,
            bool clear)
        {
            throw new NotImplementedException();
        }

        IGenericImageHandler IGraphicsFactoryHandler.CreateGenericImageHandler(
            SizeI size,
            bool clear)
        {
            throw new NotImplementedException();
        }

        IGenericImageHandler IGraphicsFactoryHandler.CreateGenericImageHandler(
            Stream stream,
            BitmapType bitmapType,
            int index)
        {
            throw new NotImplementedException();
        }

        IGenericImageHandler IGraphicsFactoryHandler.CreateGenericImageHandler(
            Stream stream,
            string mimeType,
            int index)
        {
            throw new NotImplementedException();
        }

        IGenericImageHandler IGraphicsFactoryHandler.CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data)
        {
            throw new NotImplementedException();
        }

        IGenericImageHandler IGraphicsFactoryHandler.CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data,
            byte[] alpha)
        {
            throw new NotImplementedException();
        }

        Graphics IGraphicsFactoryHandler.CreateGraphicsFromImage(Image image)
        {
            if (image.Handler is not SkiaImageHandler skiaImageHandler)
                throw new Exception("SkiaImageHandler is required as handler");
            var bitmap = skiaImageHandler.Bitmap;
            var result = SkiaUtils.CreateBitmapCanvas(bitmap, image.ScaleFactor);
            return result;
        }

        Graphics IGraphicsFactoryHandler.CreateGraphicsFromScreen()
        {
            throw new NotImplementedException();
        }

        IGraphicsPathHandler IGraphicsFactoryHandler.CreateGraphicsPathHandler()
        {
            throw new NotImplementedException();
        }

        IGraphicsPathHandler IGraphicsFactoryHandler.CreateGraphicsPathHandler(Graphics drawingContext)
        {
            throw new NotImplementedException();
        }

        IIconSetHandler? IGraphicsFactoryHandler.CreateIconSetHandler()
        {
            return null;
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(ImageSet imageSet, SizeI size)
        {
            return new SkiaImageHandler(imageSet, size);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler()
        {
            return new SkiaImageHandler();
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(
            GenericImage genericImage,
            int depth)
        {
            return new SkiaImageHandler(genericImage, depth);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(int width, int height, Graphics dc)
        {
            return new SkiaImageHandler(width, height, dc);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(ImageSet imageSet, IControl control)
        {
            return new SkiaImageHandler(imageSet, control);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(GenericImage genericImage, Graphics dc)
        {
            return new SkiaImageHandler(genericImage, dc);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(Image image)
        {
            return new SkiaImageHandler(image);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(Image original, SizeI newSize)
        {
            return new SkiaImageHandler(original, newSize);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandler(
            SizeI size,
            int depth)
        {
            return new SkiaImageHandler(size, depth);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandlerFromScreen()
        {
            // This is a dummy implementation
            return new SkiaImageHandler();
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color)
        {
            return SkiaImageHandler.CreateFromSvg(stream, width, height, color);
        }

        IImageHandler IGraphicsFactoryHandler.CreateImageHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color)
        {
            return SkiaImageHandler.CreateFromSvg(s, width, height, color);
        }

        IImageListHandler? IGraphicsFactoryHandler.CreateImageListHandler()
        {
            return null;
        }

        IImageSetHandler? IGraphicsFactoryHandler.CreateImageSetHandler()
        {
            return null;
        }

        IImageSetHandler IGraphicsFactoryHandler.CreateImageSetHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color)
        {
            var result = new PlessImageSetHandler();
            result.DefaultSize = (width, height);
            var handler = SkiaImageHandler.CreateFromSvg(stream, width, height, color);
            result.Add(new Bitmap(handler));
            return result;
        }

        IImageSetHandler IGraphicsFactoryHandler.CreateImageSetHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color)
        {
            var result = new PlessImageSetHandler();
            result.DefaultSize = (width, height);
            var handler = SkiaImageHandler.CreateFromSvg(s, width, height, color);
            result.Add(new Bitmap(handler));
            return result;
        }

        IRegionHandler IGraphicsFactoryHandler.CreateRegionHandler()
        {
            return new SkiaRegionHandler();
        }

        IRegionHandler IGraphicsFactoryHandler.CreateRegionHandler(RectD rect)
        {
            return new SkiaRegionHandler(rect);
        }

        IRegionHandler IGraphicsFactoryHandler.CreateRegionHandler(Region region)
        {
            return new SkiaRegionHandler(region);
        }

        IRegionHandler IGraphicsFactoryHandler.CreateRegionHandler(PointD[] points, FillMode fillMode)
        {
            return new SkiaRegionHandler(points, fillMode);
        }

        BitmapType IGraphicsFactoryHandler.GetDefaultBitmapType()
        {
            throw new NotImplementedException();
        }

        int IGraphicsFactoryHandler.GetGenericImageCount(
            Stream stream,
            BitmapType bitmapType)
        {
            throw new NotImplementedException();
        }

        string IGraphicsFactoryHandler.GetGenericImageExtWildcard()
        {
            throw new NotImplementedException();
        }

        public Graphics CreateMemoryCanvas(Graphics.CanvasCreateParams createParams)
        {
            return SkiaUtils.CreateMeasureCanvas(createParams.ScaleFactor);
        }

        public IGenericImageHandler CreateGenericImageHandler(int width, int height, SKColor[] data)
        {
            throw new NotImplementedException();
        }

        public ImageBitsFormat GetImageBitsFormat(ImageBitsFormatKind kind)
        {
            throw new NotImplementedException();
        }

        public Graphics CreateMemoryCanvas(Image image)
        {
            if (image.Handler is not SkiaImageHandler skiaHandler)
                throw new Exception("SkiaImageHandler is required in CreateMemoryCanvas(Image)");
            SKCanvas canvas = new(skiaHandler.Bitmap);
            return new SkiaGraphics(canvas);
        }
    }
}
