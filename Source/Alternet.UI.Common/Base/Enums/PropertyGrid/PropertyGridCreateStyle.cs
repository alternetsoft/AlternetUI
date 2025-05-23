﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines create options for a property grid control.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum PropertyGridCreateStyle
    {
        /// <summary>
        /// This will cause Sort() automatically after an item is added.
        /// When inserting a lot of items in this mode, it may make sense to
        /// use Freeze() before operations and Thaw() afterwards to increase
        /// performance.
        /// </summary>
        AutoSort = 0x00000010,

        /// <summary>
        /// Categories are not initially shown (even if added).
        /// IMPORTANT NOTE: If you do not plan to use categories, then this
        /// style will waste resources.
        /// This flag can also be changed using wxPropertyGrid::EnableCategories method.
        /// </summary>
        HideCategories = 0x00000020,

        /// <summary>
        /// This style combines non-categoric mode and automatic sorting.
        /// </summary>
        AlphabeticMode = HideCategories | AutoSort,

        /// <summary>
        /// Modified values are shown in bold font. Changing this requires Refresh()
        /// to show changes.
        /// </summary>
        BoldModified = 0x00000040,

        /// <summary>
        /// When PropertyGrid is resized, splitter moves to the center. This
        /// behavior stops once the user manually moves the splitter.
        /// </summary>
        SplitterAutoCenter = 0x00000080,

        /// <summary>
        /// Display tooltips for cell text that cannot be shown completely.
        /// </summary>
        Tooltips = 0x00000100,

        /// <summary>
        /// Disables margin and hides all expand/collapse buttons that would appear
        /// outside the margin (for sub-properties). Toggling this style automatically
        /// expands all collapsed items.
        /// </summary>
        HideMargin = 0x00000200,

        /// <summary>
        /// This style prevents user from moving the splitter.
        /// </summary>
        StaticSplitter = 0x00000400,

        /// <summary>
        /// Combination of other styles that make it impossible for user to modify
        /// the layout.
        /// </summary>
        StaticLayout = HideMargin | StaticSplitter,

        /// <summary>
        /// Disables TextBox based editors for properties which
        /// can be edited in another way.
        /// Equals calling PropertyGrid.LimitPropertyEditing for all added
        /// properties.
        /// </summary>
        LimitedEditing = 0x00000800,

        /// <summary>
        /// Default style of the property grid.
        /// </summary>
        DefaultStyle = Tooltips,
    }
}