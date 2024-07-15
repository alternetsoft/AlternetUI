using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="ICursorHandler"/> object without any functionality.
    /// </summary>
    public class PlessCursorHandler : DisposableObject, ICursorHandler
    {
        /// <inheritdoc/>
        public virtual bool IsOk
        {
            get => true;
        }

        /// <inheritdoc/>
        public virtual PointI GetHotSpot()
        {
            return PointI.MinusOne;
        }
    }
}
