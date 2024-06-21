using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the combo box control style.
    /// </summary>
    public enum ComboBoxStyle
    {
/*
        /// <summary>
        /// Specifies that the list is always visible and that the text portion is editable.
        /// This means that the user can enter a new value and is not limited to selecting
        /// an existing value in the list.</summary>
        Simple = 0,
*/

        /// <summary>
        /// Specifies that the list is displayed by clicking the down arrow and that the text
        /// portion is editable. This means that the user can enter a new value and is not
        /// limited to selecting an existing value in the list.</summary>
        DropDown = 1,

        /// <summary>
        /// Specifies that the list is displayed by clicking the down arrow and that the text
        /// portion is not editable. This means that the user cannot enter a new value. Only
        /// values already in the list can be selected.</summary>
        DropDownList = 2,
    }
}
