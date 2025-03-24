using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates all possible color property editor kinds in the property grid control.
    /// </summary>
    public enum PropertyGridEditKindColor
    {
        /// <summary>
        /// Uses default color editor specified in 'DefaultEditKindColor' property.
        /// </summary>
        Default,

        /// <summary>
        /// Uses text box and ellispsis button with a color dialog.
        /// </summary>
        TextBoxAndButton,

        /// <summary>
        /// Uses combo box with list of system colors.
        /// </summary>
        SystemColors,

        /// <summary>
        /// Uses combo box with list of colors. When 'Custom' color is selected,
        /// color dialog is opened.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Uses choice color editor (non-editable).
        /// </summary>
        Choice,

        /// <summary>
        /// Uses choice color editor (non-editable)
        /// and ellispsis button with a color dialog.
        /// </summary>
        ChoiceAndButton,
    }
}
