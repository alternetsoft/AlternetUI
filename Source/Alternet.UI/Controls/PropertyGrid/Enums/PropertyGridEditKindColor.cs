using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates all possible <see cref="Color"/> edit kinds in <see cref="PropertyGrid"/>.
    /// </summary>
    public enum PropertyGridEditKindColor
    {
        /// <summary>
        /// Uses <see cref="TextBox"/> and ellispsis button with <see cref="ColorDialog"/>.
        /// </summary>
        Dialog,

        /// <summary>
        /// Uses <see cref="ComboBox"/> with list of system colors.
        /// </summary>
        SystemColorComboBox,

        /// <summary>
        /// Uses <see cref="ComboBox"/> with list of colors. When 'Custom' color is selected,
        /// <see cref="ColorDialog"/> is opened.
        /// </summary>
        ComboBox,
    }
}
