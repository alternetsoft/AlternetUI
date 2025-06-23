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
        /// <summary>
        /// Adds a list of font sizes to the associated list control and optionally
        /// selects a specified font size.
        /// </summary>
        /// <remarks>This method populates the list control with a predefined set
        /// of font sizes and updates the current value to the specified font size
        /// or the default font size if none is provided.</remarks>
        /// <param name="select">A value indicating whether the specified font size
        /// should be selected in the list control. If <see
        /// langword="true"/>, the specified font size will be selected; otherwise,
        /// no selection is made.</param>
        /// <param name="size">The font size to add and optionally select.
        /// If <see langword="null"/>, the default font size is used.</param>
        public virtual void AddFontSizesAndSelect(bool select = false, Coord? size = null)
        {
            size ??= Control.DefaultFont.Size;
            ListControlUtils.AddFontSizes(ListBox, false, size);
            if(select)
                Value = size;
        }

        /// <summary>
        /// Adds font names to the associated list control and optionally selects
        /// a specified font name.
        /// </summary>
        /// <remarks>If <paramref name="select"/> is <see langword="true"/>,
        /// value property is updated to the specified font name.</remarks>
        /// <param name="select"><see langword="true"/> to select the specified font name
        /// after adding it to the list; otherwise, <see langword="false"/>.</param>
        /// <param name="fontName">The name of the font to add and optionally select.
        /// If <paramref name="fontName"/> is <see langword="null"/>,
        /// the default font name is used.</param>
        public virtual void AddFontNamesAndSelect(bool select = false, string? fontName = default)
        {
            fontName ??= Control.DefaultFont.Name;
            ListControlUtils.AddFontNames(ListBox, false, fontName);
            if (select)
                Value = fontName;
        }
    }
}
