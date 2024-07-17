using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains methods and properties which allow to work with hatch brush.
    /// </summary>
    public interface IHatchBrushHandler : IBrushHandler
    {
        /// <summary>
        /// Updates this brush properties using the specified brush properties.
        /// </summary>
        /// <param name="brush">Source brush.</param>
        void Update(HatchBrush brush);
    }
}
