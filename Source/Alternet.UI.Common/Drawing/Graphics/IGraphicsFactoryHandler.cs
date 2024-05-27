using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IGraphicsFactoryHandler : IDisposable
    {
        BitmapType GetDefaultBitmapType();

        Graphics CreateGraphicsFromScreen();

        IRegionHandler CreateRegionHandler();

        IRegionHandler CreateRegionHandler(RectD rect);

        IRegionHandler CreateRegionHandler(Region region);

        IRegionHandler CreateRegionHandler(PointD[] points, FillMode fillMode = FillMode.Alternate);

        IGraphicsPathHandler CreateGraphicsPathHandler();

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

        IImageSetHandler CreateImageSetHandler();

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

        IImageListHandler CreateImageListHandler();

        IIconSetHandler CreateIconSetHandler();
    }
}
