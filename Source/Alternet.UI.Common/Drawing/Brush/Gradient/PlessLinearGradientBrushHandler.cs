using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public class PlessLinearGradientBrushHandler : PlessBrushHandler, ILinearGradientBrushHandler
    {
        private PointD startPoint;
        private PointD endPoint;
        private GradientStop[]? gradientStops;

        public PlessLinearGradientBrushHandler(LinearGradientBrush brush)
            : base(brush)
        {
        }

        public PointD StartPoint => startPoint;

        public PointD EndPoint => endPoint;

        public GradientStop[]? GradientStops => gradientStops;

        public virtual void Update(LinearGradientBrush brush)
        {
            startPoint = brush.StartPoint;
            endPoint = brush.EndPoint;
            gradientStops = brush.GradientStops;
        }
    }
}
