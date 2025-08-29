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
    public class PlessContextMenuHandler : DisposableObject, IContextMenuHandler
    {
        private readonly ContextMenu contextMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessContextMenuHandler"/> class.
        /// </summary>
        /// <param name="menu"></param>
        public PlessContextMenuHandler(ContextMenu menu)
        {
            contextMenu = menu;
        }

        /// <summary>
        /// Gets the <see cref="ContextMenu"/> associated with this component, if any.
        /// </summary>
        public ContextMenu? Control
        {
            get
            {
                return contextMenu;
            }
        }

        /// <inheritdoc/>
        public virtual void Show(
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null)
        {
        }
    }
}
