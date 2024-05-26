using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal class WxGraphicsFactoryHandler : DisposableObject, IGraphicsFactoryHandler
    {
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

        public IImageHandler CreateImageHandler(ImageSet imageSet, SizeI size)
        {
            var image = new UI.Native.Image();
            ((UI.Native.ImageSet)imageSet.Handler).InitImage(
                image,
                size.Width,
                size.Height);
            return image;
        }

        /// <inheritdoc/>
        public IImageHandler CreateImageHandler()
        {
            return new UI.Native.Image();
        }

        /// <inheritdoc/>
        public IImageHandler CreateImageHandler(GenericImage genericImage, int depth = -1)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).LoadFromGenericImage((IntPtr)genericImage.Handler, depth);
            return nativeImage;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IImageHandler CreateImageHandler(ImageSet imageSet, IControl control)
        {
            var nativeObject = CreateImageHandler();
            ((UI.Native.ImageSet)imageSet.Handler).InitImageFor(
                (UI.Native.Image)nativeObject,
                WxApplicationHandler.WxWidget(control));
            return nativeObject;
        }

        /// <inheritdoc/>
        public IImageHandler CreateImageHandler(GenericImage genericImage, Graphics dc)
        {
            var nativeImage = CreateImageHandler();
            UI.Native.DrawingContext.ImageFromGenericImageDC(
                (UI.Native.Image)nativeImage,
                (IntPtr)genericImage.Handler,
                (UI.Native.DrawingContext)dc.NativeObject);
            return nativeImage;
        }

        /// <inheritdoc/>
        public IImageHandler CreateImageHandler(Image image)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).CopyFrom((UI.Native.Image)image.Handler);
            return nativeImage;
        }

        /// <inheritdoc/>
        public IImageHandler CreateImageHandler(Image original, SizeI newSize)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).InitializeFromImage(
                (UI.Native.Image)original.Handler,
                newSize);
            return nativeImage;
        }

        /// <inheritdoc/>
        public IImageHandler CreateImageHandler(SizeI size, int depth = 32)
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).Initialize(size, depth);
            return nativeImage;
        }

        /// <inheritdoc/>
        public IImageHandler CreateImageHandlerFromScreen()
        {
            var nativeImage = CreateImageHandler();
            ((UI.Native.Image)nativeImage).InitializeFromScreen();
            return nativeImage;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
    }
}
