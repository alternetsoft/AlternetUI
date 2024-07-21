using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless hatch brush handler.
    /// </summary>
    public class PlessHatchBrushHandler : PlessBrushHandler, IHatchBrushHandler
    {
        private BrushHatchStyle hatchStyle;
        private Color color = Color.Black;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessHatchBrushHandler"/> class.
        /// </summary>
        /// <param name="brush">Brush which owns this handler.</param>
        public PlessHatchBrushHandler(HatchBrush brush)
            : base(brush)
        {
        }

        /// <inheritdoc cref="HatchBrush.HatchStyle"/>
        public BrushHatchStyle HatchStyle => hatchStyle;

        /// <inheritdoc cref="HatchBrush.Color"/>
        public Color Color => color;

        /// <inheritdoc cref="IHatchBrushHandler.Update"/>
        public virtual void Update(HatchBrush brush)
        {
            hatchStyle = brush.HatchStyle;
            color = brush.Color;
        }
    }
}
