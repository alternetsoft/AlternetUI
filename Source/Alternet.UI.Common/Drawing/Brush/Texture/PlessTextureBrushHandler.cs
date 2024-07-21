using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless texture brush handler.
    /// </summary>
    public class PlessTextureBrushHandler : PlessBrushHandler, ITextureBrushHandler
    {
        private Image? image;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessTextureBrushHandler"/> class.
        /// </summary>
        /// <param name="brush"></param>
        public PlessTextureBrushHandler(TextureBrush brush)
            : base(brush)
        {
        }

        /// <summary>
        /// Gets image.
        /// </summary>
        public Image? Image => image;

        /// <inheritdoc/>
        public virtual void Update(TextureBrush brush)
        {
            image = brush.Image;
        }
    }
}
