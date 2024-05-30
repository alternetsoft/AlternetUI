using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class MauiGraphicsFactoryHandler : DisposableObject, IGraphicsFactoryHandler
    {
        public GenericImageLoadFlags GenericImageDefaultLoadFlags { get; set; }

        public bool CanReadGenericImage(string filename)
        {
            throw new NotImplementedException();
        }

        public bool CanReadGenericImage(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void CleanUpGenericImageHandlers()
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler()
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(int width, int height, bool clear = false)
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(SizeI size, bool clear = false)
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(string fileName, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(string name, string mimetype, int index = -1)
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(Stream stream, BitmapType bitmapType = BitmapType.Any, int index = -1)
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(Stream stream, string mimeType, int index = -1)
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(int width, int height, nint data, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public IGenericImageHandler CreateGenericImageHandler(int width, int height, nint data, nint alpha, bool staticData = false)
        {
            throw new NotImplementedException();
        }

        public Graphics CreateGraphicsFromImage(Image image)
        {
            throw new NotImplementedException();
        }

        public Graphics CreateGraphicsFromScreen()
        {
            throw new NotImplementedException();
        }

        public IGraphicsPathHandler CreateGraphicsPathHandler()
        {
            throw new NotImplementedException();
        }

        public IGraphicsPathHandler CreateGraphicsPathHandler(Graphics drawingContext)
        {
            throw new NotImplementedException();
        }

        public IIconSetHandler CreateIconSetHandler()
        {
            throw new NotImplementedException();
        }

        public IImageHandler CreateImageHandler(ImageSet imageSet, SizeI size)
        {
            return new SkiaImageHandler(imageSet, size);
        }

        public IImageHandler CreateImageHandler()
        {
            return new SkiaImageHandler();
        }

        public IImageHandler CreateImageHandler(GenericImage genericImage, int depth = -1)
        {
            return new SkiaImageHandler(genericImage, depth);
        }

        public IImageHandler CreateImageHandler(int width, int height, Graphics dc)
        {
            return new SkiaImageHandler(width, height, dc);
        }

        public IImageHandler CreateImageHandler(ImageSet imageSet, IControl control)
        {
            return new SkiaImageHandler(imageSet, control);
        }

        public IImageHandler CreateImageHandler(GenericImage genericImage, Graphics dc)
        {
            return new SkiaImageHandler(genericImage, dc);
        }

        public IImageHandler CreateImageHandler(Image image)
        {
            return new SkiaImageHandler(image);
        }

        public IImageHandler CreateImageHandler(Image original, SizeI newSize)
        {
            return new SkiaImageHandler(original, newSize);
        }

        public IImageHandler CreateImageHandler(SizeI size, int depth = 32)
        {
            return new SkiaImageHandler(size, depth);
        }

        public IImageHandler CreateImageHandlerFromScreen()
        {
            throw new NotImplementedException();
        }

        public IImageHandler CreateImageHandlerFromSvg(Stream stream, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public IImageHandler CreateImageHandlerFromSvg(string s, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public IImageListHandler CreateImageListHandler()
        {
            throw new NotImplementedException();
        }

        public IImageSetHandler CreateImageSetHandler()
        {
            throw new NotImplementedException();
        }

        public IImageSetHandler CreateImageSetHandlerFromSvg(Stream stream, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public IImageSetHandler CreateImageSetHandlerFromSvg(string s, int width, int height, Color? color = null)
        {
            throw new NotImplementedException();
        }

        public IRegionHandler CreateRegionHandler()
        {
            throw new NotImplementedException();
        }

        public IRegionHandler CreateRegionHandler(RectD rect)
        {
            throw new NotImplementedException();
        }

        public IRegionHandler CreateRegionHandler(Region region)
        {
            throw new NotImplementedException();
        }

        public IRegionHandler CreateRegionHandler(PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            throw new NotImplementedException();
        }

        public BitmapType GetDefaultBitmapType()
        {
            throw new NotImplementedException();
        }

        public int GetGenericImageCount(string filename, BitmapType bitmapType = BitmapType.Any)
        {
            throw new NotImplementedException();
        }

        public int GetGenericImageCount(Stream stream, BitmapType bitmapType = BitmapType.Any)
        {
            throw new NotImplementedException();
        }

        public string GetGenericImageExtWildcard()
        {
            throw new NotImplementedException();
        }

        public bool RemoveGenericImageHandler(string name)
        {
            throw new NotImplementedException();
        }
    }
}
