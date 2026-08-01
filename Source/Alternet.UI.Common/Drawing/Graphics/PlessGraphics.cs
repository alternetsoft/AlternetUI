using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements dummy drawing context which does no drawing.
    /// </summary>
    public class PlessGraphics : SkiaGraphics
    {
        /// <summary>
        /// Gets default dummy drawing context.
        /// </summary>
        public static readonly Graphics Default = new PlessGraphics();
    }
}
