using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the type of action used to raise the <see cref="ScrollBar.Scroll" /> event.
    /// </summary>
    public enum ScrollEventType
    {
        /// <summary>
        /// The scroll thumb was moved a small distance.
        /// The user clicked the left or top scroll arrow,
        /// or pressed the <see cref="Key.Up"/>key.</summary>
        SmallDecrement,

        /// <summary>
        /// The scroll thumb was moved a small distance.
        /// The user clicked the right or bottom scroll arrow,
        /// or pressed the <see cref="Key.Down"/> key.
        /// </summary>
        SmallIncrement,

        /// <summary>
        /// The scroll thumb was moved a large distance.
        /// The user clicked the scroll bar to the left or above the scroll thumb,
        /// or pressed the <see cref="Key.PageUp"/> key.</summary>
        LargeDecrement,

        /// <summary>
        /// The scroll thumb was moved a large distance.
        /// The user clicked the scroll bar to the right or below
        /// the scroll thumb, or pressed the <see cref="Key.PageDown"/> key.
        /// </summary>
        LargeIncrement,

        /// <summary>
        /// The scroll thumb was moved.
        /// </summary>
        ThumbPosition,

        /// <summary>
        /// The scroll thumb is currently being moved.
        /// </summary>
        ThumbTrack,

        /// <summary>
        /// The scroll thumb was moved to the min position.
        /// </summary>
        First,

        /// <summary>
        /// The scroll thumb was moved to the max position.
        /// </summary>
        Last,

        /// <summary>
        /// The scroll box has stopped moving.
        /// </summary>
        EndScroll,
    }
}
