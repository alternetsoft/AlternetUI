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
        /// Uses default color editor specified in <see cref="PropertyGrid.DefaultEditKindColor"/>.
        /// </summary>
        Default,

        /// <summary>
        /// Uses <see cref="TextBox"/> and ellispsis button with <see cref="ColorDialog"/>.
        /// </summary>
        TextBoxAndButton,

        /// <summary>
        /// Uses <see cref="UI.ComboBox"/> with list of system colors.
        /// </summary>
        SystemColors,

        /// <summary>
        /// Uses <see cref="UI.ComboBox"/> with list of colors. When 'Custom' color is selected,
        /// <see cref="ColorDialog"/> is opened.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Uses choice color editor (non-editable <see cref="UI.ComboBox"/>).
        /// </summary>
        Choice,

        /// <summary>
        /// Uses choice color editor (non-editable <see cref="UI.ComboBox"/>)
        /// and ellispsis button with <see cref="ColorDialog"/>.
        /// </summary>
        ChoiceAndButton,
    }
}
