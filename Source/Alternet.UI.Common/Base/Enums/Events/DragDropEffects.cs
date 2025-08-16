using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the possible effects of a drag-and-drop operation.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum DragDropEffects
    {
        /// <summary>
        /// The drop target does not accept the data.
        /// </summary>
        None = 0,

        /// <summary>
        /// The data is copied to the drop target.
        /// </summary>
        Copy = 1 << 0,

        /// <summary>
        /// The data from the drag source is moved to the drop target.
        /// </summary>
        Move = 1 << 1,

        /// <summary>
        /// The data from the drag source is linked to the drop target.
        /// </summary>
        Link = 1 << 2,

        /// <summary>
        /// The target can be scrolled while dragging to locate a drop position that
        /// is not currently visible in the target.</summary>
        Scroll = int.MinValue,

        /// <summary>
        /// The combination of the <see cref="DragDropEffects.Copy" />,
        /// <see cref="DragDropEffects.Move" />, and
        /// <see cref="DragDropEffects.Scroll" /> effects.</summary>
        All = -2147483645,
    }
}