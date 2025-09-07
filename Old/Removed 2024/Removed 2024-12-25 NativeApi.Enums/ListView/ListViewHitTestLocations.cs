using System;

using ApiCommon;

namespace NativeApi.Api
{
    /// <summary>
    /// Defines constants that represent areas of a <see cref="ListView"/> or <see cref="ListViewItem"/>.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    [ManagedExternName("Alternet.UI.ListViewHitTestLocations")]
    [ManagedName("Alternet.UI.ListViewHitTestLocations")]
    public enum ListViewHitTestLocations
    {
        /// <summary>
        /// A position in the client area of the <see cref="ListView"/> control, but not on a node or a portion of a
        /// node.
        /// </summary>
        None = 1 << 1,

        /// <summary>
        /// A position above the client portion of a <see cref="ListView"/> control.
        /// </summary>
        AboveClientArea = 1 << 2,

        /// <summary>
        /// A position below the client portion of a <see cref="ListView"/> control.
        /// </summary>
        BelowClientArea = 1 << 3,

        /// <summary>
        /// A position to the left of the client area of a <see cref="ListView"/> control.
        /// </summary>
        LeftOfClientArea = 1 << 4,

        /// <summary>
        /// A position to the right of the client area of the <see cref="ListView"/> control.
        /// </summary>
        RightOfClientArea = 1 << 5,

        /// <summary>
        /// A position within the bounds of an image contained on a <see cref="ListViewItem"/>.
        /// </summary>
        ItemImage = 1 << 6,

        /// <summary>
        /// A position on the text portion of a <see cref="ListViewItem"/>.
        /// </summary>
        ItemLabel = 1 << 7,

        /// <summary>
        /// A position to the right of the bounds of a <see cref="ListViewItem"/>.
        /// </summary>
        RightOfItem = 1 << 8,
    }
}