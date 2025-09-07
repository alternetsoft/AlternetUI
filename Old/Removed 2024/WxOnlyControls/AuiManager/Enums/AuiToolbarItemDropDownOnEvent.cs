using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies when to show a drop down menu assiciated with
    /// <see cref="AuiToolbar"/> item.
    /// </summary>
    internal enum AuiToolbarItemDropDownOnEvent
    {
        /// <summary>
        /// Do not show drop down menu.
        /// </summary>
        None,

        /// <summary>
        /// Show drop down menu when tool is clicked.
        /// </summary>
        Click,

        /// <summary>
        /// Show drop down menu when arrow on the right side of the tool is clicked.
        /// Arrow is shown next to the tool when it's kind is
        /// <see cref="AuiToolbarItemKind.Dropdown"/>.
        /// </summary>
        ClickArrow,
    }
}
