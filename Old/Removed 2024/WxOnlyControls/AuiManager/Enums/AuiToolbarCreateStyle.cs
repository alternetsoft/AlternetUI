using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the appearance of a <see cref="AuiToolbar"/>.
    /// </summary>
    [Flags]
    internal enum AuiToolbarCreateStyle
    {
        /// <summary>
        /// Shows the text in the toolbar buttons; by default only icons are shown.
        /// </summary>
        Text = 1 << 0,

        /// <summary>
        /// Don't show tooltips on toolbar items.
        /// </summary>
        NoTooltips = 1 << 1,

        /// <summary>
        /// Do not auto-resize the toolbar.
        /// </summary>
        NoAutoResize = 1 << 2,

        /// <summary>
        /// Shows a gripper on the toolbar.
        /// </summary>
        Gripper = 1 << 3,

        /// <summary>
        /// Toolbar can contain overflow items.
        /// </summary>
        Overflow = 1 << 4,

        /// <summary>
        /// Using this style forces the toolbar to be vertical and be only
        /// dockable to the left or right sides of the window whereas by
        /// default it can be horizontal or vertical and be docked anywhere.
        /// </summary>
        Vertical = 1 << 5,

        /// <summary>
        /// Shows the text and the icons alongside, not vertically stacked.
        /// </summary>
        HorzLayout = 1 << 6,

        /// <summary>
        /// Forces the toolbar to be horizontal, docking to the top or bottom
        /// of the window.
        /// </summary>
        Horizontal = 1 << 7,

        /// <summary>
        /// Draw a plain background (based on parent) instead of the default
        /// gradient background.
        /// </summary>
        PlainBackground = 1 << 8,

        /// <summary>
        /// Shows the text alongside the icons, not vertically stacked.
        /// </summary>
        HorzText = HorzLayout | Text,

        /// <summary>
        /// Default style of the toolbar.
        /// </summary>
        DefaultStyle = 0,
    }
}