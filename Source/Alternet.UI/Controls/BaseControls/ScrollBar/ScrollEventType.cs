using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the type of action used to raise the <see cref="Control.Scroll" /> event.
    /// </summary>
    public enum ScrollEventType
    {
        /// <summary>
        /// The scroll thumb was moved a small distance.
        /// The user clicked the left or top scroll arrow,
        /// or pressed the <see cref="Key.Up"/>key.</summary>
        SmallDecrement = 0,

        /// <summary>
        /// The scroll thumb was moved a small distance.
        /// The user clicked the right or bottom scroll arrow,
        /// or pressed the <see cref="Key.Down"/> key.
        /// </summary>
        SmallIncrement = 1,

        /// <summary>
        /// The scroll thumb was moved a large distance.
        /// The user clicked the scroll bar to the left or above the scroll thumb,
        /// or pressed the <see cref="Key.PageUp"/> key.</summary>
        LargeDecrement = 2,

        /// <summary>
        /// The scroll thumb was moved a large distance.
        /// The user clicked the scroll bar to the right or below
        /// the scroll thumb, or pressed the <see cref="Key.PageDown"/> key.
        /// </summary>
        LargeIncrement = 3,

        /// <summary>
        /// The scroll thumb was moved.
        /// </summary>
        ThumbPosition = 4,

        /// <summary>
        /// The scroll thumb is currently being moved.
        /// </summary>
        ThumbTrack = 5,

        /// <summary>
        /// The scroll thumb was moved to the min position.
        /// </summary>
        First = 6,

        /// <summary>
        /// The scroll thumb was moved to the max position.
        /// </summary>
        Last = 7,

        /// <summary>
        /// The scroll thumb has stopped moving.
        /// </summary>
        EndScroll = 8,
    }
}
