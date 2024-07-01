using System;

namespace Alternet.UI
{
    /// <summary>
    /// Defines constants that represent areas of a tree view control.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum TreeViewHitTestLocations
    {
        /// <summary>
        /// A position in the client area of a control,
        /// but not on a node or a portion of a node.
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
        /// A position to the right of the client area of a control.
        /// </summary>
        RightOfClientArea = 1 << 5,

        /// <summary>
        /// A position on the expand button of an item.
        /// </summary>
        ItemExpandButton = 1 << 6,

        /// <summary>
        /// A position within the bounds of an image contained on an item.
        /// </summary>
        ItemImage = 1 << 7,

        /// <summary>
        /// A position in the indentation area for an item.
        /// </summary>
        ItemIndent = 1 << 8,

        /// <summary>
        /// A position on the text portion of an item.
        /// </summary>
        ItemLabel = 1 << 9,

        /// <summary>
        /// A position to the right of the text area of an item.
        /// </summary>
        RightOfItemLabel = 1 << 10,

        /// <summary>
        /// A position within the bounds of a state image for an item.
        /// </summary>
        ItemStateImage = 1 << 11,

        /// <summary>
        /// A position on the upper part portion of an item.
        /// </summary>
        ItemUpperPart = 1 << 12,

        /// <summary>
        /// A position on the lower part portion of an item.
        /// </summary>
        ItemLowerPart = 1 << 13,
    }
}