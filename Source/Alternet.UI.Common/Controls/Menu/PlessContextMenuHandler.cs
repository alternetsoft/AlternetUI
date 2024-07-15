using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements dummy <see cref="IContextMenuHandler"/> provider.
    /// </summary>
    public class PlessContextMenuHandler : PlessControlHandler, IContextMenuHandler
    {
        /// <inheritdoc/>
        public virtual void Show(IControl control, PointD? position = null)
        {
        }
    }
}
