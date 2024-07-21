using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Base class for primitive painters.
    /// </summary>
    public class PrimitivePainter : BaseObject
    {
        /// <summary>
        /// Gets or sets whether this object is visible.
        /// </summary>
        public bool Visible = true;
    }
}
