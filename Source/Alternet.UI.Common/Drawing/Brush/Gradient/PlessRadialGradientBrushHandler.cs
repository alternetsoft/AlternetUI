using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless radial gradient brush handler.
    /// </summary>
    public class PlessRadialGradientBrushHandler : PlessBrushHandler, IRadialGradientBrushHandler
    {
        private PointD center;
        private Coord radius;
        private PointD gradientOrigin;
        private GradientStop[]? gradientStops;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessRadialGradientBrushHandler"/> class.
        /// </summary>
        /// <param name="brush">Owner.</param>
        public PlessRadialGradientBrushHandler(RadialGradientBrush brush)
            : base(brush)
        {
        }

        /// <inheritdoc cref="RadialGradientBrush.Center"/>
        public PointD Center => center;

        /// <inheritdoc cref="RadialGradientBrush.Radius"/>
        public Coord Radius => radius;

        /// <inheritdoc cref="RadialGradientBrush.GradientOrigin"/>
        public PointD GradientOrigin => gradientOrigin;

        /// <inheritdoc cref="GradientBrush.GradientStops"/>
        public GradientStop[]? GradientStops => gradientStops;

        /// <inheritdoc/>
        public virtual void Update(RadialGradientBrush brush)
        {
            center = brush.Center;
            radius = brush.Radius;
            gradientOrigin = brush.GradientOrigin;
            gradientStops = brush.GradientStops;
        }
    }
}
