using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines parameters for <see cref="DrawingUtils.DrawImageSliced(Graphics, NinePatchImagePaintParams)"/>.
    /// </summary>
    public class NinePatchImagePaintParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NinePatchImagePaintParams"/> class.
        /// </summary>
        /// <param name="image">Image to draw.</param>
        public NinePatchImagePaintParams(Image image)
        {
            Image = image;
        }

        /// <summary>
        /// Gets or sets image that will be painted.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets rectangle inside the image that will be used for painting.
        /// </summary>
        public RectI SourceRect { get; set; }

        /// <summary>
        /// Gets or sets destination rectangle. It supposed to be larger than
        /// <see cref="SourceRect"/>. In this case all parts except top-left, top-right,
        /// bottom-right, bottom-left parts will be filled with <see cref="TextureBrush"/>
        /// constructed with correspoding parts of the image.
        /// </summary>
        public RectI DestRect { get; set; }

        /// <summary>
        /// Rectangle that slices image into 9 parts.
        /// </summary>
        public RectI PatchRect { get; set; }

        internal bool Tile { get; set; } = true;
    }
}
