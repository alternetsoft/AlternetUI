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
    public class SpeedButtonWithListPopup : SpeedButtonWithListPopup<VirtualListBox>
    {
    }
}
