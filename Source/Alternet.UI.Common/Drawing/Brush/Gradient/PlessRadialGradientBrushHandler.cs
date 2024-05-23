using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class PlessRadialGradientBrushHandler : PlessBrushHandler, IRadialGradientBrushHandler
    {
        private PointD center;
        private double radius;
        private PointD gradientOrigin;
        private GradientStop[]? gradientStops;

        public PlessRadialGradientBrushHandler(RadialGradientBrush brush)
            : base(brush)
        {
        }

        public PointD Center => center;

        public double Radius => radius;

        public PointD GradientOrigin => gradientOrigin;

        public GradientStop[]? GradientStops => gradientStops;

        public virtual void Update(RadialGradientBrush brush)
        {
            center = brush.Center;
            radius = brush.Radius;
            gradientOrigin = brush.GradientOrigin;
            gradientStops = brush.GradientStops;
        }
    }
}
