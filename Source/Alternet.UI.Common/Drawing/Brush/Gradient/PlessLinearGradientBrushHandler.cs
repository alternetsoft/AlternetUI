using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless linear gradient brush handler.
    /// </summary>
    public class PlessLinearGradientBrushHandler : PlessBrushHandler, ILinearGradientBrushHandler
    {
        private PointD startPoint;
        private PointD endPoint;
        private GradientStop[]? gradientStops;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessLinearGradientBrushHandler"/> class.
        /// </summary>
        /// <param name="brush">Owner</param>
        public PlessLinearGradientBrushHandler(LinearGradientBrush brush)
            : base(brush)
        {
        }

        /// <inheritdoc cref="LinearGradientBrush.StartPoint"/>
        public PointD StartPoint => startPoint;

        /// <inheritdoc cref="LinearGradientBrush.EndPoint"/>
        public PointD EndPoint => endPoint;

        /// <inheritdoc cref="GradientBrush.GradientStops"/>
        public GradientStop[]? GradientStops => gradientStops;

        /// <inheritdoc/>
        public virtual void Update(LinearGradientBrush brush)
        {
            startPoint = brush.StartPoint;
            endPoint = brush.EndPoint;
            gradientStops = brush.GradientStops;
        }
    }
}
