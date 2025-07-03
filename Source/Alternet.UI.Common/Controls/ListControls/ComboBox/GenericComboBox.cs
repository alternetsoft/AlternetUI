using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a text box with an associated list popup that provides
    /// a generic combo box functionality.
    /// </summary>
    /// <remarks>The <see cref="GenericComboBox"/> class extends
    /// <see cref="TextBoxWithListPopup"/> to allow
    /// users  to input text and select items from a dropdown list.
    /// This control is suitable for scenarios where both
    /// free-form text input and predefined item selection are required.</remarks>
    public class GenericComboBox : TextBoxWithListPopup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericComboBox"/> class.
        /// </summary>
        public GenericComboBox()
        {
            SyncTextAndComboButton();
        }
    }
}
