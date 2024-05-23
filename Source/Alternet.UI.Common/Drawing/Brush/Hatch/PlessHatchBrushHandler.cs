using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    public class PlessHatchBrushHandler : PlessBrushHandler, IHatchBrushHandler
    {
        private BrushHatchStyle hatchStyle;
        private Color color = Color.Black;

        public PlessHatchBrushHandler(HatchBrush brush)
            : base(brush)
        {
        }

        public BrushHatchStyle HatchStyle => hatchStyle;

        public Color Color => color;

        public virtual void Update(HatchBrush brush)
        {
            hatchStyle = brush.HatchStyle;
            color = brush.Color;
        }
    }
}
