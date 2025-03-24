using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known actions which can be performed when window is closed.
    /// </summary>
    public enum WindowCloseAction
    {
        /// <summary>
        /// Window is disposed.
        /// </summary>
        Dispose,

        /// <summary>
        /// Window is hidden.
        /// </summary>
        Hide,

        /// <summary>
        /// Nothing is done.
        /// </summary>
        None,
    }
}
