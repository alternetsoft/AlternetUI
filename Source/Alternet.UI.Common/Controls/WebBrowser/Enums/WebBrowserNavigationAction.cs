using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    ///     Navigation action types for the <see cref="WebBrowser"/> control.
    /// </summary>
    public enum WebBrowserNavigationAction
    {
        /// <summary>
        ///     No navigation action.
        /// </summary>
        None,

        /// <summary>
        ///     The navigation was started by the user.
        /// </summary>
        User,

        /// <summary>
        ///     The navigation was started but not by the user.
        /// </summary>
        Other,
    }
}
