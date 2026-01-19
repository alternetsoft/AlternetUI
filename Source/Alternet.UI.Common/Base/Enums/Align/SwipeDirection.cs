using System;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates swipe directions.
    /// </summary>
    [Flags]
    public enum SwipeDirection
    {
        /// <summary>
        /// Indicates a presence of rightward swipe.
        /// </summary>
        Right = 1,

        /// <summary>
        /// Indicates a presence of leftward swipe.
        /// </summary>
        Left = 2,

        /// <summary>
        /// Indicates a presence of upward swipe.
        /// </summary>
        Up = 4,

        /// <summary>
        /// Indicates a presence of downward swipe.
        /// </summary>
        Down = 8,

        /// <summary>
        /// Indicates a presence of all swipe directions.
        /// </summary>
        All = Right | Left | Up | Down,

        /// <summary>
        /// Indicates a presence of horizontal swipe directions.
        /// </summary>
        LeftRight = Left | Right,

        /// <summary>
        /// Indicates a presence of vertical swipe directions.
        /// </summary>
        UpDown = Up | Down,
    }
}