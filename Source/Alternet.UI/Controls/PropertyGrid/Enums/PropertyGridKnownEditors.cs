using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines known <see cref="PropertyGrid"/> property editors.
    /// </summary>
    public enum PropertyGridKnownEditors
    {
        /// <summary>
        /// Use <see cref="CheckBox"/> as property editor.
        /// </summary>
        CheckBox,

        /// <summary>
        /// Use choice property editor.
        /// </summary>
        Choice,

        /// <summary>
        /// Use <see cref="TextBox"/> as property editor.
        /// </summary>
        TextCtrl,

        /// <summary>
        /// Use choice property editor with ellipsis button.
        /// </summary>
        ChoiceAndButton,

        /// <summary>
        /// Use <see cref="ComboBox"/> as property editor.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Use <see cref="TextBox"/> with up/down buttons as property editor.
        /// </summary>
        SpinCtrl,

        /// <summary>
        /// Use <see cref="TextBox"/> with ellipsis button as property editor.
        /// </summary>
        TextCtrlAndButton,

        /// <summary>
        /// Uses <see cref="DateTimePicker"/> as property editor.
        /// </summary>
        DatePickerCtrl,
    }
}
