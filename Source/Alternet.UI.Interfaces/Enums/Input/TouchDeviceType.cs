using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies constants that define which touch device was used.
    /// </summary>
    public enum TouchDeviceType
    {
        /// <summary>
        /// A finger on the screen was being used when the event was raised.
        /// </summary>
        Touch,

        /// <summary>
        /// A mouse was being used when the event was raised.
        /// </summary>
        Mouse,

        /// <summary>
        /// A pen on the screen was being used when the event was raised.
        /// </summary>
        Pen,
    }
}