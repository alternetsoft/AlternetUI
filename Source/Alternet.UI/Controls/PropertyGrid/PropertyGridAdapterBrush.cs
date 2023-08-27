using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Helper class for using <see cref="Brush"/> properties in the <see cref="PropertyGrid"/>.
    /// </summary>
    public class PropertyGridAdapterBrush : PropertyGridAdapterGeneric
    {
        /// <summary>
        /// Returns <see cref="PropertyGridAdapterGeneric.Value"/> as <see cref="Brush"/>.
        /// </summary>
        public Brush? Brush
        {
            get => Value as Brush;
            set => Value = value;
        }
/*      SmartBrush?
        Point LinearGradientStart;
        Point LinearGradientEnd;
        Point RadialGradientCenter;
        Point RadialGradientOrigin;
        double RadialGradientRadius;
        BrushType BrushType;
        GradientStop[] GradientStops; //(Color color, double offset)
        Color Color;
        BrushHatchStyle HatchStyle;
*/
    }
}