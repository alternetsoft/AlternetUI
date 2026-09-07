using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the method used to invalidate a control.
    /// </summary>
    public enum InvalidateMethod
    {
        /// <summary>
        /// Indicates that the control should not be invalidated.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the control should be invalidated, causing it to be redrawn
        /// when the message loop processes paint messages. This is the default behavior for most controls.
        /// </summary>
        Invalidate,

        /// <summary>
        /// Indicates that the control should be refreshed, causing it to be redrawn immediately.
        /// </summary>
        Refresh,

        /// <summary>
        /// Indicates that the control should be laid out and invalidated,
        /// causing it to be redrawn when the message loop processes paint messages.
        /// </summary>
        LayoutAndInvalidate,

        /// <summary>
        /// Indicates that the control should be laid out and refreshed, causing it to be redrawn immediately.
        /// </summary>
        LayoutAndRefresh,
    }
}
