using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines known property editors in the property grid control.
    /// </summary>
    public enum PropertyGridKnownEditors
    {
        /// <summary>
        /// Use <c>CheckBox</c> as property editor.
        /// </summary>
        CheckBox,

        /// <summary>
        /// Use choice property editor.
        /// </summary>
        Choice,

        /// <summary>
        /// Use <c>TextBox</c> as property editor.
        /// </summary>
        TextCtrl,

        /// <summary>
        /// Use choice property editor with ellipsis button.
        /// </summary>
        ChoiceAndButton,

        /// <summary>
        /// Use <c>ComboBox</c> as property editor.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Use <c>TextBox</c> with up/down buttons as property editor.
        /// </summary>
        SpinCtrl,

        /// <summary>
        /// Use <c>TextBox</c> with ellipsis button as property editor.
        /// </summary>
        TextCtrlAndButton,

        /// <summary>
        /// Uses date-picker control as property editor.
        /// </summary>
        DatePickerCtrl,
    }
}
