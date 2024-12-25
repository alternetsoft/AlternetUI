using System;

using ApiCommon;

namespace NativeApi.Api
{
    /// <summary>
    /// Defines constants that represent areas of a <see cref="TreeView"/> or <see cref="TreeViewItem"/>.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    [ManagedExternName("Alternet.UI.TreeViewHitTestLocations")]
    [ManagedName("Alternet.UI.TreeViewHitTestLocations")]
    public enum TreeViewHitTestLocations
    {
        /// <summary>
        /// A position in the client area of the <see cref="TreeView"/> control, but not on a node or a portion of a
        /// node.
        /// </summary>
        None = 1 << 1,

        /// <summary>
        /// A position above the client portion of a <see cref="TreeView"/> control.
        /// </summary>
        AboveClientArea = 1 << 2,

        /// <summary>
        /// A position below the client portion of a <see cref="TreeView"/> control.
        /// </summary>
        BelowClientArea = 1 << 3,

        /// <summary>
        /// A position to the left of the client area of a <see cref="TreeView"/> control.
        /// </summary>
        LeftOfClientArea = 1 << 4,

        /// <summary>
        /// A position to the right of the client area of the <see cref="TreeView"/> control.
        /// </summary>
        RightOfClientArea = 1 << 5,

        /// <summary>
        /// A position on the expand button of a <see cref="TreeViewItem"/>.
        /// </summary>
        ItemExpandButton = 1 << 6,

        /// <summary>
        /// A position within the bounds of an image contained on a <see cref="TreeViewItem"/>.
        /// </summary>
        ItemImage = 1 << 7,

        /// <summary>
        /// A position in the indentation area for a <see cref="TreeViewItem"/>.
        /// </summary>
        ItemIndent = 1 << 8,

        /// <summary>
        /// A position on the text portion of a <see cref="TreeViewItem"/>.
        /// </summary>
        ItemLabel = 1 << 9,

        /// <summary>
        /// A position to the right of the text area of a <see cref="TreeViewItem"/>.
        /// </summary>
        RightOfItemLabel = 1 << 10,

        /// <summary>
        /// A position within the bounds of a state image for a <see cref="TreeViewItem"/>.
        /// </summary>
        ItemStateImage = 1 << 11,

        /// <summary>
        /// A position on the upper part portion of a <see cref="TreeViewItem"/>.
        /// </summary>
        ItemUpperPart = 1 << 12,

        /// <summary>
        /// A position on the lower part portion of a <see cref="TreeViewItem"/>.
        /// </summary>
        ItemLowerPart = 1 << 13,
    }
}