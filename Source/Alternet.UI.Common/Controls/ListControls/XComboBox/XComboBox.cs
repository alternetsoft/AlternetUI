using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a text box with an associated list popup that provides
    /// a generic combo box functionality.
    /// </summary>
    /// <remarks>The <see cref="XComboBox"/> class extends
    /// <see cref="TextBoxWithListPopup"/> to allow
    /// users  to input text and select items from a dropdown list.
    /// This control is suitable for scenarios where both
    /// free-form text input and predefined item selection are required.</remarks>
    public class XComboBox : TextBoxWithListPopup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XComboBox"/> class.
        /// </summary>
        public XComboBox()
        {
            SyncTextAndComboButton();
        }
    }
}
