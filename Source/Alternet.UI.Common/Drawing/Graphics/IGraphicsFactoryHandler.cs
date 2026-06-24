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
    public partial interface IGraphicsFactoryHandler : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether OpenGL is available on the current system.
        /// </summary>
        public bool IsOpenGLAvailable { get; }

        /// <summary>
        /// Creates <see cref="IFontFactoryHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IFontFactoryHandler CreateFontFactoryHandler();

        /// <summary>
        /// Creates <see cref="IPenHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IPenHandler CreatePenHandler(Pen pen);

        /// <summary>
        /// Creates <see cref="IRegionHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IRegionHandler CreateRegionHandler();

        /// <summary>
        /// Creates <see cref="IRegionHandler"/> provider and initializes it with the rectangle.
        /// </summary>
        /// <returns></returns>
        IRegionHandler CreateRegionHandler(RectI rect);

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
        IRegionHandler CreateRegionHandler(
            ReadOnlySpan<PointD> points,
            FillMode fillMode = FillMode.Alternate,
            float scaleFactor = 1.0f);

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
        /// Creates <see cref="IImageHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IImageHandler CreateImageHandler();

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
        /// Creates <see cref="IImageListHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IImageListHandler? CreateImageListHandler();

        /// <summary>
        /// Creates <see cref="IconSet"/> handler.
        /// </summary>
        /// <returns></returns>
        IImageContainer? CreateIconSetHandler();

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <param name="width">Image width.</param>
        /// <param name="height">Image height.</param>
        /// <param name="data">Array with image pixels.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(
            int width,
            int height,
            SKColor[] data);
    }
}
