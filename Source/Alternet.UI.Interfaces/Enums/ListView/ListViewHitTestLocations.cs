using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defines constants that represent areas of a list view control or list view item.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum ListViewHitTestLocations
    {
        /// <summary>
        /// A position in the client area of the control, but not on a node or a portion of a
        /// node.
        /// </summary>
        None = 1 << 1,

        /// <summary>
        /// A position above the client portion of a control.
        /// </summary>
        AboveClientArea = 1 << 2,

        /// <summary>
        /// A position below the client portion of a control.
        /// </summary>
        BelowClientArea = 1 << 3,

        /// <summary>
        /// A position to the left of the client area of a control.
        /// </summary>
        LeftOfClientArea = 1 << 4,

        /// <summary>
        /// A position to the right of the client area of the control.
        /// </summary>
        RightOfClientArea = 1 << 5,

        /// <summary>
        /// A position within the bounds of an image contained on an item.
        /// </summary>
        ItemImage = 1 << 6,

        /// <summary>
        /// A position on the text portion of an item.
        /// </summary>
        ItemLabel = 1 << 7,

        /// <summary>
        /// A position to the right of the bounds of an item.
        /// </summary>
        RightOfItem = 1 << 8,
    }
}