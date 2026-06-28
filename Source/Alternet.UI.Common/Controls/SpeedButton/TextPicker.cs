using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a generic control that allows to edit text.
    /// This is different from <see cref="TextBox"/> control, which is a native control of the operating system.
    /// <see cref="TextPicker"/> is derived from <see cref="SpeedButton"/> and creates a text editing interface
    /// when it is clicked or activated. This allows to use <see cref="TextPicker"/> in toolbars
    /// and other places where a native control cannot be used.
    /// </summary>
    /// <remarks>
    /// This control also has a drop down list box, which allows to select items
    /// from the list or enter a new value. The list box is shown as a popup window when
    /// the user clicks on the drop down button which is hidden by default.
    /// </remarks>
    public partial class TextPicker : EditableListPicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextPicker"/> class.
        /// </summary>
        public TextPicker()
        {
            ImageVisible = false;
        }
    }
}
