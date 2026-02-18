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
    internal class WxGraphicsFactoryHandler : DisposableObject, IGraphicsFactoryHandler
    {
        static WxGraphicsFactoryHandler()
        {
        }

        enum ImageStaticObjectId
        {
            NativePixelFormat = 0,
            AlphaPixelFormat = 1,
            GenericPixelFormat = 2,
        };

        enum ImageStaticPropertyId
        {
            BitsPerPixel = 0,
            HasAlpha = 1,
            SizePixel = 2,
            Red = 3,
            Green = 4,
            Blue = 5,
            Alpha = 6,
        };

        public bool IsOpenGLAvailable
        {
            get => false;
        }

        private static ImageBitsFormat GetImageBitsFormat(ImageStaticObjectId objectId)
        {
            ImageBitsFormat result = new();

            result.BitsPerPixel = GetProp(ImageStaticPropertyId.BitsPerPixel);
            result.HasAlpha = GetProp(ImageStaticPropertyId.HasAlpha) != 0;
            result.SizePixel = GetProp(ImageStaticPropertyId.SizePixel);
            result.Red = GetProp(ImageStaticPropertyId.Red);
            result.Green = GetProp(ImageStaticPropertyId.Green);
            result.Blue = GetProp(ImageStaticPropertyId.Blue);
            result.Alpha = GetProp(ImageStaticPropertyId.Alpha);

            return result;

            int GetProp(ImageStaticPropertyId propId)
            {
                var result = UI.Native.Image.GetStaticOption((int)objectId, (int)propId);
                return result;
            }
        }

        public Graphics CreateMemoryCanvas(Graphics.CanvasCreateParams prm)
        {
            if (prm.ControlRenderingFlags.HasFlag(ControlRenderingFlags.UseSkiaSharp))
            {
                return SkiaUtils.CreateMeasureCanvas(prm.ScaleFactor);
            }
            else
            {
                return new WxGraphics(UI.Native.DrawingContext.CreateMemoryDC(prm.ScaleFactor));
            }
        }

        public Graphics CreateMemoryCanvas(Image image)
        {
            return new WxGraphics(UI.Native.DrawingContext.CreateMemoryDCFromImage(
                (UI.Native.Image)image.Handler));
        }

        public ImageBitsFormat GetImageBitsFormat(ImageBitsFormatKind kind)
        {
            switch (kind)
            {
                case ImageBitsFormatKind.Native:
                    return GetImageBitsFormat(ImageStaticObjectId.NativePixelFormat);
                default:
                case ImageBitsFormatKind.Alpha:
                    return GetImageBitsFormat(ImageStaticObjectId.AlphaPixelFormat);
                case ImageBitsFormatKind.Generic:
                    return GetImageBitsFormat(ImageStaticObjectId.GenericPixelFormat);
            }
        }

        /// <inheritdoc/>
        public IFontFactoryHandler CreateFontFactoryHandler()
        {
            return new WxFontFactoryHandler();
        }

        /// <inheritdoc/>
        public IPenHandler CreatePenHandler(Pen pen) => new UI.Native.Pen();

        /// <inheritdoc/>
        public IBrushHandler CreateTransparentBrushHandler(Brush brush)
            => new UI.Native.Brush();

        /// <inheritdoc/>
        public IHatchBrushHandler CreateHatchBrushHandler(HatchBrush brush)
            => new UI.Native.HatchBrush();

        /// <inheritdoc/>
        public ILinearGradientBrushHandler CreateLinearGradientBrushHandler(
            LinearGradientBrush brush)
            => new UI.Native.LinearGradientBrush();

        /// <inheritdoc/>
        public IRadialGradientBrushHandler CreateRadialGradientBrushHandler(
            RadialGradientBrush brush)
            => new UI.Native.RadialGradientBrush();

        /// <inheritdoc/>
        public ISolidBrushHandler CreateSolidBrushHandler(SolidBrush brush)
            => new UI.Native.SolidBrush();

        /// <inheritdoc/>
        public ITextureBrushHandler CreateTextureBrushHandler(TextureBrush brush)
            => new UI.Native.TextureBrush();

        public IRegionHandler CreateRegionHandler()
        {
            return new UI.Native.Region();
        }

        public IRegionHandler CreateRegionHandler(RectD rect)
        {
            var region = new UI.Native.Region();
            region.InitializeWithRect(rect);
            return region;
        }

        public IGraphicsPathHandler CreateGraphicsPathHandler(Graphics graphics)
        {
            var result = new UI.Native.GraphicsPath();
            result.Initialize((UI.Native.DrawingContext)graphics.NativeObject);
            return result;
        }

        public IRegionHandler CreateRegionHandler(Region region)
        {
            var nativeObject = new UI.Native.Region();
            nativeObject.InitializeWithRegion((UI.Native.Region)region.Handler);
            return nativeObject;
        }

        public IRegionHandler CreateRegionHandler(PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            var nativeObject = new UI.Native.Region();
            nativeObject.InitializeWithPolygon(points, fillMode);
            return nativeObject;
        }

        public IGraphicsPathHandler CreateGraphicsPathHandler()
        {
            return new UI.Native.GraphicsPath();
        }

        public IImageSetHandler? CreateImageSetHandler()
        {
            return new UI.Native.ImageSet();
        }

        public IImageSetHandler CreateImageSetHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = new UI.Native.ImageSet();
            using var inputStream = new UI.Native.InputStream(stream, false);
            nativeImage.LoadSvgFromStream(inputStream, width, height, color ?? Color.Black);
            return nativeImage;
        }

        public IImageSetHandler CreateImageSetHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = new UI.Native.ImageSet();
            nativeImage.LoadSvgFromString(s, width, height, color ?? Color.Black);
            return nativeImage;
        }

        public IImageListHandler? CreateImageListHandler()
        {
            return new UI.Native.ImageList();
        }

        public IIconSetHandler? CreateIconSetHandler()
        {
            return new UI.Native.IconSet();
        }

        public BitmapType GetDefaultBitmapType()
        {
            return (BitmapType)UI.Native.Image.GetDefaultBitmapType();
        }

        public Graphics CreateGraphicsFromScreen()
        {
            return new WxGraphics(UI.Native.DrawingContext.FromScreen());
        }

        public Graphics CreateGraphicsFromImage(Image image)
        {
            return new WxGraphics(
                UI.Native.DrawingContext.FromImage((UI.Native.Image)image.Handler));
        }

        public IImageHandler CreateImageHandler()
        {
            return new UI.Native.Image();
        }

        public IImageHandler CreateImageHandler(GenericImage genericImage, int depth = -1)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).LoadFromGenericImage(WxGenericImageHandler.GetPtr(genericImage), depth);
            return nativeImage;
        }

        public IImageHandler CreateImageHandler(int width, int height, Graphics dc)
        {
            var nativeImage = CreateImageHandler();
            UI.Native.DrawingContext.ImageFromDrawingContext(
                (UI.Native.Image)nativeImage,
                width,
                height,
                (UI.Native.DrawingContext)dc.NativeObject);
            return nativeImage;
        }

        public static IImageHandler CreateImageHandler(UI.Native.ImageSet imageSet, SizeI size)
        {
            var image = new UI.Native.Image();
            imageSet.InitImage(
                image,
                size.Width,
                size.Height);
            return image;
        }

        public IImageHandler CreateImageHandler(ImageSet imageSet, SizeI size)
        {
            var image = new UI.Native.Image();
            ((UI.Native.ImageSet)imageSet.Handler).InitImage(
                image,
                size.Width,
                size.Height);
            return image;
        }

        public IImageHandler CreateImageHandler(ImageSet imageSet, IControl control)
        {
            var nativeObject = CreateImageHandler();
            ((UI.Native.ImageSet)imageSet.Handler).InitImageFor(
                (UI.Native.Image)nativeObject,
                WxApplicationHandler.WxWidget(control));
            return nativeObject;
        }

        public IImageHandler CreateImageHandler(GenericImage genericImage, Graphics dc)
        {
            var nativeImage = CreateImageHandler();
            UI.Native.DrawingContext.ImageFromGenericImageDC(
                (UI.Native.Image)nativeImage,
                WxGenericImageHandler.GetPtr(genericImage),
                (UI.Native.DrawingContext)dc.NativeObject);
            return nativeImage;
        }

        public IImageHandler CreateImageHandler(Image image)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).CopyFrom((UI.Native.Image)image.Handler);
            return nativeImage;
        }

        public IImageHandler CreateImageHandler(Image original, SizeI newSize)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).InitializeFromImage(
                (UI.Native.Image)original.Handler,
                newSize);
            return nativeImage;
        }

        public IImageHandler CreateImageHandler(SizeI size, int depth = 32)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).Initialize(size, depth);
            return nativeImage;
        }

        public IImageHandler CreateImageHandlerFromScreen()
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).InitializeFromScreen();
            return nativeImage;
        }

        public IImageHandler CreateImageHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = CreateImageHandler();
            using var inputStream = new UI.Native.InputStream(stream);
            ((UI.Native.Image)nativeImage).LoadSvgFromStream(
                inputStream,
                width,
                height,
                color ?? Color.Black);
            return nativeImage;
        }

        public IImageHandler CreateImageHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color = null)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).LoadSvgFromString(s, width, height, color ?? Color.Black);
            return nativeImage;
        }

        public bool CanReadGenericImage(string filename)
        {
            return UI.Native.GenericImage.CanRead(filename);
        }

        public bool CanReadGenericImage(Stream stream)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.CanReadStream(inputStream);
        }

        public string GetGenericImageExtWildcard()
        {
            return UI.Native.GenericImage.GetImageExtWildcard();
        }

        public bool RemoveGenericImageHandler(string name)
        {
            return UI.Native.GenericImage.RemoveHandler(name);
        }

        public int GetGenericImageCount(
            string filename,
            BitmapType bitmapType = BitmapType.Any)
        {
            return UI.Native.GenericImage.GetImageCountInFile(filename, (int)bitmapType);
        }

        public int GetGenericImageCount(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any)
        {
            var inputStream = new UI.Native.InputStream(stream);
            return UI.Native.GenericImage.GetImageCountInStream(inputStream, (int)bitmapType);
        }

        public void CleanUpGenericImageHandlers()
        {
            UI.Native.GenericImage.CleanUpHandlers();
        }

        public GenericImageLoadFlags GenericImageDefaultLoadFlags
        {
            get
            {
                return (GenericImageLoadFlags)UI.Native.GenericImage.GetDefaultLoadFlags();
            }

            set
            {
                UI.Native.GenericImage.SetDefaultLoadFlags((int)value);
            }
        }

        public IGenericImageHandler CreateGenericImageHandler()
        {
            return new WxGenericImageHandler();
        }

        public IGenericImageHandler CreateGenericImageHandler(int width, int height, bool clear = false)
        {
            return new WxGenericImageHandler(width, height, clear);
        }

        public IGenericImageHandler CreateGenericImageHandler(SizeI size, bool clear = false)
        {
            return new WxGenericImageHandler(size.Width, size.Height, clear);
        }

        public IGenericImageHandler CreateGenericImageHandler(
            string fileName,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return new WxGenericImageHandler(fileName, bitmapType,index);
        }

        public IGenericImageHandler CreateGenericImageHandler(string name, string mimetype, int index = -1)
        {
            return new WxGenericImageHandler(name, mimetype, index);
        }

        public IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return new WxGenericImageHandler(stream, bitmapType, index);
        }

        public IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            string mimeType,
            int index = -1)
        {
            return new WxGenericImageHandler(stream, mimeType, index);
        }

        public IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data)
        {
            return new WxGenericImageHandler(width, height, data);
        }

        public IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            SKColor[] data)
        {
            return new WxGenericImageHandler(width, height, data);
        }

        public IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data,
            byte[] alpha)
        {
            return new WxGenericImageHandler(width, height, data, alpha);
        }
    }
}
