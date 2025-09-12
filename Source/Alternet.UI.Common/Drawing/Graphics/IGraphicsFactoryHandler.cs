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
    /// Contains methods and properties which allow to create graphics objects and to perform
    /// graphics related operations.
    /// </summary>
    public interface IGraphicsFactoryHandler : IDisposable
    {
        /// <summary>
        /// Gets or sets <see cref="GenericImageLoadFlags"/> used to specify default load image flags.
        /// </summary>
        GenericImageLoadFlags GenericImageDefaultLoadFlags { get; set; }

        /// <summary>
        /// Creates memory drawing context.
        /// </summary>
        /// <param name="prm">The parameters for creating the measurement canvas.</param>
        /// <returns></returns>
        Graphics CreateMemoryCanvas(Graphics.CanvasCreateParams prm);

        /// <summary>
        /// Creates memory drawing context.
        /// </summary>
        /// <param name="image">Surface on which drawing is performed.</param>
        /// <returns></returns>
        Graphics CreateMemoryCanvas(Image image);

        /// <summary>
        /// Gets default bitmap type.
        /// </summary>
        /// <returns></returns>
        BitmapType GetDefaultBitmapType();

        /// <summary>
        /// Creates <see cref="IFontFactoryHandler"/> provider.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates <see cref="IPenHandler"/> provider.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates drawing context which allows to draw on screen.
        /// </summary>
        /// <returns></returns>
        Graphics CreateGraphicsFromScreen();

        /// <summary>
        /// Creates <see cref="IRegionHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IRegionHandler CreateRegionHandler();

        /// <summary>
        /// Creates <see cref="IRegionHandler"/> provider and initializes it with the rectangle.
        /// </summary>
        /// <returns></returns>
        IRegionHandler CreateRegionHandler(RectD rect);

        /// <summary>
        /// Creates <see cref="IRegionHandler"/> provider and initializes it with the region.
        /// </summary>
        /// <returns></returns>
        IRegionHandler CreateRegionHandler(Region region);

        /// <summary>
        /// Creates <see cref="IRegionHandler"/> provider and initializes it using figure
        /// specified with array of points and fill mode.
        /// </summary>
        /// <returns></returns>
        IRegionHandler CreateRegionHandler(PointD[] points, FillMode fillMode = FillMode.Alternate);

        /// <summary>
        /// Creates <see cref="IGraphicsPathHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IGraphicsPathHandler CreateGraphicsPathHandler();

        /// <summary>
        /// Gets <see cref="ImageBitsFormat"/> for the specified <see cref="ImageBitsFormatKind"/>.
        /// </summary>
        /// <param name="kind">Image format kind.</param>
        /// <returns></returns>
        ImageBitsFormat GetImageBitsFormat(ImageBitsFormatKind kind);

        /// <summary>
        /// Creates <see cref="IGraphicsPathHandler"/> provider for the specified
        /// drawing context.
        /// </summary>
        /// <param name="drawingContext">Drawing context.</param>
        /// <returns></returns>
        IGraphicsPathHandler CreateGraphicsPathHandler(Graphics drawingContext);

        /// <summary>
        /// Creates drawing context for the specified image.
        /// </summary>
        /// <param name="image">Image on which drawing will be performed.</param>
        /// <returns></returns>
        Graphics CreateGraphicsFromImage(Image image);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> for the specified <see cref="ImageSet"/>
        /// and image size.
        /// </summary>
        /// <param name="imageSet">Image set.</param>
        /// <param name="size">Image size.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(ImageSet imageSet, SizeI size);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IImageHandler CreateImageHandler();

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Source image used to get pixel data.</param>
        /// <param name="depth">Image depth.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(GenericImage genericImage, int depth = -1);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="dc">Drawing context to get dpi from.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(int width, int height, Graphics dc);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="imageSet">Image set.</param>
        /// <param name="control">Control to get dpi from.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(ImageSet imageSet, IControl control);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="genericImage">Source image used to get pixel data.</param>
        /// <param name="dc">Drawing context to get dpi from.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(GenericImage genericImage, Graphics dc);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="image">Source image used to get pixel data.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(Image image);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="original">Source image used to get pixel data.</param>
        /// <param name="newSize">Image size.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(Image original, SizeI newSize);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified image size and depth.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <param name="depth">Image depth.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(SizeI size, int depth = 32);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider for the screen.
        /// </summary>
        /// <returns></returns>
        IImageHandler CreateImageHandlerFromScreen();

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider for svg image using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Default color of the svg figures.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color = null);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider for svg image using the specified parameters.
        /// </summary>
        /// <param name="s">String with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Default color of the svg figures.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color = null);

        /// <summary>
        /// Creates <see cref="IImageSetHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IImageSetHandler? CreateImageSetHandler();

        /// <summary>
        /// Creates <see cref="IImageSetHandler"/> provider for svg image using the specified parameters.
        /// </summary>
        /// <param name="stream">Stream with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Default color of the svg figures.</param>
        /// <returns></returns>
        IImageSetHandler CreateImageSetHandlerFromSvg(
            Stream stream,
            int width,
            int height,
            Color? color = null);

        /// <summary>
        /// Creates <see cref="IImageSetHandler"/> provider for svg image using the specified parameters.
        /// </summary>
        /// <param name="s">String with svg data.</param>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="color">Default color of the svg figures.</param>
        /// <returns></returns>
        IImageSetHandler CreateImageSetHandlerFromSvg(
            string s,
            int width,
            int height,
            Color? color = null);

        /// <summary>
        /// Creates <see cref="IImageListHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IImageListHandler? CreateImageListHandler();

        /// <summary>
        /// Creates <see cref="IIconSetHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IIconSetHandler? CreateIconSetHandler();

        /// <summary>
        /// Gets whether or not stream contains image data.
        /// </summary>
        /// <param name="stream">Stream with data.</param>
        /// <returns></returns>
        bool CanReadGenericImage(Stream stream);

        /// <summary>
        /// Gets generic image wildcards for open/save dialogs.
        /// </summary>
        /// <returns></returns>
        string GetGenericImageExtWildcard();

        /// <summary>
        /// Gets number of images in the stream.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <param name="bitmapType">Type of the image.</param>
        /// <returns></returns>
        int GetGenericImageCount(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any);

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler();

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="clear">Whether or not to clear the image.</param>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler(int width, int height, bool clear = false);

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <param name="clear">Whether or not to clear the image.</param>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler(SizeI size, bool clear = false);

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <param name="bitmapType">Type of the image.</param>
        /// <param name="index">Image index.</param>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <param name="mimeType">Mime-type of the image.</param>
        /// <param name="index">Image index.</param>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler(
            Stream stream,
            string mimeType,
            int index = -1);

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="data">Array with image pixels.</param>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data);

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="data">Array with image pixels.</param>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            SKColor[] data);

        /// <summary>
        /// Creates <see cref="IGenericImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="data">Array with image pixels.</param>
        /// <param name="alpha">Array with alpha component of the pixels.</param>
        /// <returns></returns>
        IGenericImageHandler CreateGenericImageHandler(
            int width,
            int height,
            RGBValue[] data,
            byte[] alpha);
    }
}
