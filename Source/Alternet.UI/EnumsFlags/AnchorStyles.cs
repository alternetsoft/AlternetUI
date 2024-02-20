using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies how a control anchors to the edges of its container.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    /// <remarks>
    /// This enumeration is used in the <see cref="LayoutPanel"/>.
    /// </remarks>
    [Flags]
    public enum AnchorStyles
    {
        /// <summary>
        /// The control is not anchored to any edges of its container.
        /// </summary>
        None = 0x00000000,

        /// <summary>
        /// The control is anchored to the top edge of its container.
        /// </summary>
        Top = 0x00000001,

        /// <summary>
        /// The control is anchored to the bottom edge of its container.
        /// </summary>
        Bottom = 0x00000002,

        /// <summary>
        /// The control is anchored to the left edge of its container.
        /// </summary>
        Left = 0x00000004,

        /// <summary>
        /// The control is anchored to the right edge of its container.
        /// </summary>
        Right = 0x00000008,

        /// <summary>
        /// The control is anchored to the left and right edges of its container.
        /// </summary>
        LeftRight = Left | Right,

        /// <summary>
        /// The control is anchored to the top and bottom edges of its container.
        /// </summary>
        TopBottom = Top | Bottom,

        /// <summary>
        /// The control is anchored to the left and top edges of its container.
        /// </summary>
        LeftTop = Top | Left,
    }
}