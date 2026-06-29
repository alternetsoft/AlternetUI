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
    internal class WxGraphicsFactoryHandler : DisposableObject
    {
        static WxGraphicsFactoryHandler()
        {
        }

        internal enum ImageStaticObjectId
        {
            NativePixelFormat = 0,
            AlphaPixelFormat = 1,
            GenericPixelFormat = 2,
        };

        internal enum ImageStaticPropertyId
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

        public static ImageBitsFormat GetImageBitsFormat(ImageBitsFormatKind kind)
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

        public IGraphicsPathHandler CreateGraphicsPathHandler(Graphics graphics)
        {
            var result = new UI.Native.GraphicsPath();
            result.Initialize((UI.Native.DrawingContext)graphics.NativeObject);
            return result;
        }

        public IRegionHandler CreateRegionHandler()
        {
            if (ControlUtils.SkiaSharpRendering)
            {
                return new SkiaRegionHandler();
            }
            else
            {
                return new UI.Native.Region();
            }
        }

        public IRegionHandler CreateRegionHandler(RectI rect)
        {
            if (ControlUtils.SkiaSharpRendering)
            {
                return new SkiaRegionHandler(rect);
            }
            else
            {
                var region = new UI.Native.Region();
                region.InitializeWithRect(rect);
                return region;
            }
        }

        public IRegionHandler CreateRegionHandler(Region region)
        {
            if (ControlUtils.SkiaSharpRendering)
            {
                return new SkiaRegionHandler(region);
            }
            else
            {
                var nativeObject = new UI.Native.Region();
                nativeObject.InitializeWithRegion((UI.Native.Region)region.Handler);
                return nativeObject;
            }
        }

        public IRegionHandler CreateRegionHandler(
            ReadOnlySpan<PointD> points,
            FillMode fillMode = FillMode.Alternate,
            float scaleFactor = 1.0f)
        {
            if (ControlUtils.SkiaSharpRendering)
            {
                return new SkiaRegionHandler(points, fillMode, scaleFactor);
            }
            else
            {
                var nativeObject = new UI.Native.Region();
                unsafe
                {
                    fixed (PointD* pointsPtr = points)
                    {
                        nativeObject.InitializeWithPolygon(pointsPtr, points.Length, fillMode, scaleFactor);
                    }
                }
                return nativeObject;
            }
        }

        public IGraphicsPathHandler CreateGraphicsPathHandler()
        {
            return new UI.Native.GraphicsPath();
        }

        public IImageListHandler? CreateImageListHandler()
        {
            return new UI.Native.ImageList();
        }

        public IImageContainer? CreateIconSetHandler()
        {
            return new UI.Native.IconSet();
        }

        public IImageHandler CreateImageHandler()
        {
            return new UI.Native.Image();
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

        public IGenericImageHandler CreateGenericImageHandler()
        {
            return new WxGenericImageHandler();
        }

        public IGenericImageHandler CreateGenericImageHandler(int width, int height, bool clear = false)
        {
            return new WxGenericImageHandler(width, height, clear);
        }

        public IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1)
        {
            return new WxGenericImageHandler(stream, bitmapType, index);
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

        public IImageHandler CreateImageHandler(int width, int height, SKColor[] data)
        {
            using var genericImage = new WxGenericImageHandler(width, height, data);
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).LoadFromGenericImage(genericImage.Handle, 32);
            return nativeImage;
        }
    }
}
