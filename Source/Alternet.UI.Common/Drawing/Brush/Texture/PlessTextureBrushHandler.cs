using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public class PlessTextureBrushHandler : PlessBrushHandler, ITextureBrushHandler
    {
        private Image? image;

        public Image? Image => image;

        public virtual void Update(TextureBrush brush)
        {
            image = brush.Image;
        }
    }
}
