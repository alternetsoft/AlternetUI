using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known touch events mask values. This is used when touch events
    /// are enabled/disabled in the controls.
    /// </summary>
    /// <remarks>
    /// The values other than <see cref="None"/> and <see cref="All"/> can be
    /// combined together to request enabling events for the specified gestures and
    /// for them only.
    /// </remarks>
    [Flags]
    public enum TouchEventsMask
    {
        /// <summary>
        /// Don't generate any touch events.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Generate gesture events for vertical pans. Note that under macOS horizontal
        /// pan events are also enabled when this flag is specified.
        /// </summary>
        VerticalPan = 0x0001,

        /// <summary>
        /// Generate gesture events for horizontal pans. Note that under macOS vertical
        /// pan events are also enabled when this flag is specified.
        /// </summary>
        HorizontalPan = 0x0002,

        /// <summary>
        /// Generate gesture events for any pans. This is just a convenient combination
        /// of <see cref="VerticalPan"/> and <see cref="HorizontalPan"/>.
        /// </summary>
        Pan = VerticalPan | HorizontalPan,

        /// <summary>
        /// Generate zoom gesture events.
        /// </summary>
        Zoom = 0x0004,

        /// <summary>
        /// Generate rotate gesture events.
        /// </summary>
        Rotate = 0x0008,

        /// <summary>
        /// Generate events for press or tap gestures such as "two finger tap" event,
        /// "long press" event and "press and tap" event.
        /// </summary>
        Press = 0x0010,

        /// <summary>
        /// Enable all supported gesture events.
        /// </summary>
        All = 0x001f,
    }
}