using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class PlessSolidBrushHandler : PlessBrushHandler, ISolidBrushHandler
    {
        private Color color = Color.Black;

        public PlessSolidBrushHandler(SolidBrush brush)
            : base(brush)
        {
        }

        public Color Color => color;

        public virtual void Update(SolidBrush brush)
        {
            this.color = brush?.Color ?? Color.Black;
        }
    }
}
