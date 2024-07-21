using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Extends <see cref="IBrushHandler"/> with properties and methods specific
    /// to linear gradient brush.
    /// </summary>
    public interface ILinearGradientBrushHandler : IBrushHandler
    {
        /// <summary>
        /// Update native brush properties from the managed brush properties.
        /// </summary>
        /// <param name="brush">Managed brush.</param>
        void Update(LinearGradientBrush brush);
    }
}
