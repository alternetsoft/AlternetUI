using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial interface IGraphicsFactoryHandler
    {
        /// <summary>
        /// Creates <see cref="IImageSetHandler"/> provider.
        /// </summary>
        /// <returns></returns>
        IImageSetHandler? CreateImageSetHandler();

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
        /// Creates <see cref="IImageHandler"/> for the specified <see cref="ImageSet"/>
        /// and image size.
        /// </summary>
        /// <param name="imageSet">Image set.</param>
        /// <param name="size">Image size.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(ImageSet imageSet, SizeI size);

        /// <summary>
        /// Creates <see cref="IImageHandler"/> provider using the specified parameters.
        /// </summary>
        /// <returns></returns>
        /// <param name="imageSet">Image set.</param>
        /// <param name="control">Control to get dpi from.</param>
        /// <returns></returns>
        IImageHandler CreateImageHandler(ImageSet imageSet, IControl control);


    }
}
