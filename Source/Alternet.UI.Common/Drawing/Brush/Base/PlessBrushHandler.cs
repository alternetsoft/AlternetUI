using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base class for platformless brushes.
    /// </summary>
    public class PlessBrushHandler : DisposableObject, IBrushHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlessBrushHandler"/> class.
        /// </summary>
        /// <param name="brush">Owner object.</param>
        public PlessBrushHandler(Brush brush)
        {
        }
    }
}
