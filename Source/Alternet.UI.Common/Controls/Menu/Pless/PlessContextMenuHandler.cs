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
        /// <summary>
        /// Gets the <see cref="ContextMenu"/> associated with this component, if any.
        /// </summary>
        public new ContextMenu? Control
        {
            get
            {
                return (ContextMenu?)base.Control;
            }
        }

        /// <inheritdoc/>
        public virtual void Show(AbstractControl control, PointD? position = null)
        {
        }
    }
}
