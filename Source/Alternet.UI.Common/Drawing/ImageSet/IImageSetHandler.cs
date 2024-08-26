using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with image sets.
    /// </summary>
    public interface IImageSetHandler : IImageContainer
    {
        /// <summary>
        /// Gets default image size.
        /// </summary>
        SizeI DefaultSize { get; }

        /// <summary>
        /// Gets preferred image size for the specified control.
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        SizeI GetPreferredBitmapSizeFor(IControl control);

        /// <summary>
        /// Gets preferred image size for the specified scale factor.
        /// </summary>
        /// <param name="scale">Scale factor.</param>
        /// <returns></returns>
        SizeI GetPreferredBitmapSizeAtScale(Coord scale);

        /// <summary>
        /// Adds image from the specified stream with the image data.
        /// </summary>
        /// <param name="stream">Stream with the image data.</param>
        /// <returns></returns>
        bool LoadFromStream(Stream stream);

        /// <summary>
        /// Gets as image with the specified size.
        /// </summary>
        /// <param name="size">Image size.</param>
        /// <returns></returns>
        Image AsImage(SizeI size);
    }
}
