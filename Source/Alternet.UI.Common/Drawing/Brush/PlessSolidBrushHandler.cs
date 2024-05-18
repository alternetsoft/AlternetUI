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

        public void Initialize(Color color)
        {
            this.color = color;
        }
    }
}
