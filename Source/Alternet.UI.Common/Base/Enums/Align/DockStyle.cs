using System;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the position and manner in which a control is docked inside the container.
    /// </summary>
    public enum DockStyle
    {
        /// <summary>
        /// The control is not docked and is positioned absolutely using its
        /// 'Bounds' property.
        /// </summary>
        None = 0,

        /// <summary>
        /// The control's top edge is docked to the top of its containing control.
        /// </summary>
        Top = 1,

        /// <summary>
        /// The control's bottom edge is docked to the bottom of
        /// its containing control.
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// The control's left edge is docked to the left edge of
        /// its containing control.
        /// </summary>
        Left = 3,

        /// <summary>
        /// The control's right edge is docked to the right edge
        /// of its containing control.
        /// </summary>
        Right = 4,

        /// <summary>
        /// All the control's edges are docked to the all edges of
        /// its containing control and sized appropriately.
        /// </summary>
        Fill = 5,

        /// <summary>
        /// The control's right edge is docked to the right edge
        /// of its containing control. Size of the control is calculated using it's
        /// preferred size.
        /// </summary>
        RightAutoSize = 6,
    }
}
