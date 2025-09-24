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
    /// <summary>
    /// Provides a factory for creating graphics-related handlers and objects using SkiaSharp.
    /// </summary>
    /// <remarks>This class serves as an implementation of <see cref="IGraphicsFactoryHandler"/>
    /// that leverages SkiaSharp for rendering and graphics operations.
    /// It provides methods to create various graphics
    /// handlers, such as pens, brushes, images, and regions,
    /// as well as utilities for working with graphics contexts
    /// and canvases.</remarks>
    public partial class SkiaGraphicsFactoryHandler : DisposableObject, IGraphicsFactoryHandler
    {
        /// <inheritdoc/>
        public virtual GenericImageLoadFlags GenericImageDefaultLoadFlags { get; set; }

        /// <inheritdoc/>
        public virtual bool IsOpenGLAvailable => false;

        /// <inheritdoc/>
        public virtual IPenHandler CreatePenHandler(Pen pen)
        {
            return new PlessPenHandler(pen);
        }

        /// <inheritdoc/>
        public virtual IFontFactoryHandler CreateFontFactoryHandler()
        {
            return new SkiaFontFactoryHandler();
        }

        /// <inheritdoc/>
        public virtual IBrushHandler CreateTransparentBrushHandler(Brush brush)
        {
            return new PlessBrushHandler(brush);
        }

        /// <inheritdoc/>
        public virtual ISolidBrushHandler CreateSolidBrushHandler(SolidBrush brush)
        {
            return new PlessSolidBrushHandler(brush);
        }

        /// <inheritdoc/>
        public virtual ITextureBrushHandler CreateTextureBrushHandler(TextureBrush brush)
        {
            return new PlessTextureBrushHandler(brush);
        }

        /// <inheritdoc/>
        public virtual IHatchBrushHandler CreateHatchBrushHandler(HatchBrush brush)
        {
            return new PlessHatchBrushHandler(brush);
        }

        /// <inheritdoc/>
        public virtual ILinearGradientBrushHandler CreateLinearGradientBrushHandler(
            LinearGradientBrush brush)
        {
            return new PlessLinearGradientBrushHandler(brush);
        }

        /// <inheritdoc/>
        public virtual IRadialGradientBrushHandler CreateRadialGradientBrushHandler(
            RadialGradientBrush brush)
        {
            return new PlessRadialGradientBrushHandler(brush);
        }

        /// <inheritdoc/>
        public virtual bool CanReadGenericImage(Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            bool clear)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler(
            SizeI size,
            bool clear)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            BitmapType bitmapType,
            int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            string mimeType,
            int index)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data,
            byte[] alpha)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual Graphics CreateGraphicsFromImage(Image image)
        {
            if (image.Handler is not SkiaImageHandler skiaImageHandler)
                throw new Exception("SkiaImageHandler is required as handler");
            var bitmap = skiaImageHandler.Bitmap;
            var result = SkiaUtils.CreateBitmapCanvas(bitmap, image.ScaleFactor);
            return result;
        }

        /// <inheritdoc/>
        public virtual Graphics CreateGraphicsFromScreen()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGraphicsPathHandler CreateGraphicsPathHandler()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IGraphicsPathHandler CreateGraphicsPathHandler(Graphics drawingContext)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual IIconSetHandler? CreateIconSetHandler()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(ImageSet imageSet, SizeI size)
        {
            return new SkiaImageHandler(imageSet, size);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler()
        {
            return new SkiaImageHandler();
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(
            GenericImage genericImage,
            int depth)
        {
            return new SkiaImageHandler(genericImage, depth);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(int width, int height, Graphics dc)
        {
            return new SkiaImageHandler(width, height, dc);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(ImageSet imageSet, IControl control)
        {
            return new SkiaImageHandler(imageSet, control);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(GenericImage genericImage, Graphics dc)
        {
            return new SkiaImageHandler(genericImage, dc);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(Image image)
        {
            return new SkiaImageHandler(image);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(Image original, SizeI newSize)
        {
            return new SkiaImageHandler(original, newSize);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(
            SizeI size,
            int depth)
        {
            return new SkiaImageHandler(size, depth);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandlerFromScreen()
        {
            // This is a dummy implementation
            return new SkiaImageHandler();
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color)
        {
            return SkiaImageHandler.CreateFromSvg(stream, width, height, color);
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color)
        {
            return SkiaImageHandler.CreateFromSvg(s, width, height, color);
        }

        /// <inheritdoc/>
        public virtual IImageListHandler? CreateImageListHandler()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IImageSetHandler? CreateImageSetHandler()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IImageSetHandler CreateImageSetHandlerFromSvg(
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

        /// <inheritdoc/>
        public virtual IImageSetHandler CreateImageSetHandlerFromSvg(
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

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler()
        {
            return new SkiaRegionHandler();
        }

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler(RectD rect)
        {
            return new SkiaRegionHandler(rect);
        }

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler(Region region)
        {
            return new SkiaRegionHandler(region);
        }

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler(PointD[] points, FillMode fillMode)
        {
            return new SkiaRegionHandler(points, fillMode);
        }

        /// <inheritdoc/>
        public virtual BitmapType GetDefaultBitmapType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual int GetGenericImageCount(
            Stream stream,
            BitmapType bitmapType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual string GetGenericImageExtWildcard()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual Graphics CreateMemoryCanvas(Graphics.CanvasCreateParams createParams)
        {
            return SkiaUtils.CreateMeasureCanvas(createParams.ScaleFactor);
        }

        /// <inheritdoc/>
        public virtual IGenericImageHandler CreateGenericImageHandler(int width, int height, SKColor[] data)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual ImageBitsFormat GetImageBitsFormat(ImageBitsFormatKind kind)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public virtual Graphics CreateMemoryCanvas(Image image)
        {
            if (image.Handler is not SkiaImageHandler skiaHandler)
                throw new Exception("SkiaImageHandler is required in CreateMemoryCanvas(Image)");
            SKCanvas canvas = new(skiaHandler.Bitmap);
            return new SkiaGraphics(canvas);
        }
    }
}
