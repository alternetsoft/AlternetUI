using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines <see cref="AuiToolbar"/> item kind.
    /// </summary>
    internal enum AuiToolbarItemKind
    {
        /// <summary>
        /// Toolbar item is separator.
        /// </summary>
        Separator = -1,

        /// <summary>
        /// Toolbar item is normal button.
        /// </summary>
        Normal,

        /// <summary>
        /// Toolbar item is checkbox.
        /// </summary>
        Check,

        /// <summary>
        /// Toolbar item is radio button.
        /// </summary>
        Radio,

        /// <summary>
        /// Toolbar item is drop down button.
        /// </summary>
        Dropdown,
    }
}
