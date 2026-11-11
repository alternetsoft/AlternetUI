using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a specialized button control that displays a popup list when clicked.
    /// </summary>
    /// <remarks>This class is a non-generic version of <see cref="SpeedButtonWithListPopup{T}"/>
    /// and uses <see cref="VirtualListBox"/> as the default type for the popup list.
    /// It provides functionality for scenarios
    /// where a quick selection from a list is required.</remarks>
    [ControlCategory(KnownControlCategory.MenusAndToolbars)]
    public partial class SpeedButtonWithListPopup : SpeedButtonWithListPopup<VirtualListBox>
    {
    }

    /// <summary>
    /// Represents a specialized button control that displays a popup list when clicked,
    /// without any visible text.
    /// </summary>
    public partial class NoTextSpeedButtonWithListPopup : SpeedButtonWithListPopup<VirtualListBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoTextSpeedButtonWithListPopup"/> class.
        /// </summary>
        public NoTextSpeedButtonWithListPopup()
        {
            ImageVisible = true;
            HasVisibleTextOverride = false;
            ImageHorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}
