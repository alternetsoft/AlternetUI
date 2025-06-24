using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a picker control for selecting font sizes.
    /// </summary>
    /// <remarks>The <see cref="FontSizePicker"/> class provides functionality
    /// to display a list of available
    /// font sizes and allows the user to select one.
    /// By default, the control initializes with a predefined set of font sizes.</remarks>
    public partial class FontSizePicker : ListPicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontSizePicker"/> class.
        /// </summary>
        public FontSizePicker()
        {
            PopupWindowTitle = CommonStrings.Default.WindowTitleSelectFontSize;
            AddFontSizesAndSelect(true);
        }
    }
}
