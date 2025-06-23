using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a picker control for selecting font names.
    /// </summary>
    /// <remarks>The <see cref="FontNamePicker"/> class provides functionality
    /// to display a list of available
    /// font names  and allows the user to select one.
    /// By default, the control initializes with a predefined set of font names.</remarks>
    public partial class FontNamePicker : ListPicker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FontNamePicker"/> class.
        /// </summary>
        public FontNamePicker()
        {
            AddFontNamesAndSelect(true);
        }
    }
}
