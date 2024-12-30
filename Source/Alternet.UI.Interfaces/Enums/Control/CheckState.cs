using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the state of an item or a control, such as a check box, that can be checked,
    /// unchecked, or set to an indeterminate state.
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// The check box is unchecked.
        /// </summary>
        Unchecked,

        /// <summary>
        /// The check box is checked.
        /// </summary>
        Checked,

        /// <summary>
        /// The check box is indeterminate. An indeterminate checkbox generally has a
        /// shaded appearance.
        /// </summary>
        Indeterminate,
    }
}