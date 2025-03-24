using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies constants that define which touch/mouse action took place.
    /// </summary>
    public enum TouchAction
    {
        /// <summary>
        /// The touch/mouse entered the control.
        /// </summary>
        Entered,

        /// <summary>
        /// A finger or pen was touched on the screen, or a mouse button was pressed.
        /// </summary>
        Pressed,

        /// <summary>
        /// The touch (while down) or mouse (pressed or released) moved in the control.
        /// </summary>
        Moved,

        /// <summary>
        /// A finger or pen was lifted off the screen, or a mouse button was released.
        /// </summary>
        Released,

        /// <summary>
        /// The touch/mouse operation was cancelled.
        /// </summary>
        Cancelled,

        /// <summary>
        /// The touch/mouse exited the control.
        /// </summary>
        Exited,

        /// <summary>
        /// The mouse wheel was scrolled.
        /// </summary>
        WheelChanged,
    }
}