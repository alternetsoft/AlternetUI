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
    internal partial class SkiaGraphicsFactoryHandler : DisposableObject
    {
        /// <inheritdoc/>
        public virtual bool IsOpenGLAvailable => false;

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
        public virtual IImageContainer? CreateIconSetHandler()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler()
        {
            return new SkiaImageHandler();
        }

        /// <inheritdoc/>
        public virtual IImageHandler CreateImageHandler(int width, int height, Graphics dc)
        {
            return new SkiaImageHandler(width, height, dc);
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
        public virtual IImageListHandler? CreateImageListHandler()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler()
        {
            return new SkiaRegionHandler();
        }

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler(RectI rect)
        {
            return new SkiaRegionHandler(rect);
        }

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler(Region region)
        {
            return new SkiaRegionHandler(region);
        }

        /// <inheritdoc/>
        public virtual IRegionHandler CreateRegionHandler(
            ReadOnlySpan<PointD> points,
            FillMode fillMode,
            float scaleFactor)
        {
            return new SkiaRegionHandler(points, fillMode, scaleFactor);
        }

        /// <inheritdoc/>
        public virtual Graphics CreateMemoryCanvas(Graphics.CanvasCreateParams createParams)
        {
            return SkiaUtils.CreateMeasureCanvas(createParams.ScaleFactor);
        }

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="data">Array with image pixels.</param>
        /// <returns></returns>
        public virtual IImageHandler CreateImageHandler(
            int width,
            int height,
            SKColor[] data)
        {
            return new SkiaImageHandler(width, height, data);
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
