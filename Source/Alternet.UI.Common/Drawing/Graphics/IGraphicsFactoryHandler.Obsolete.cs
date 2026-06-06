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
