using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates <see cref="WebBrowser"/> event types.
    /// </summary>
    public enum WebBrowserEvent
    {
        /// <summary>
        /// Unknown event.
        /// </summary>
        Unknown,

        /// <summary>
        /// 'Navigating' event.
        /// </summary>
        Navigating,

        /// <summary>
        /// 'Navigated' event.
        /// </summary>
        Navigated,

        /// <summary>
        /// 'Loaded' event.
        /// </summary>
        Loaded,

        /// <summary>
        /// 'Error' event.
        /// </summary>
        Error,

        /// <summary>
        /// 'NewWindow' event.
        /// </summary>
        NewWindow,

        /// <summary>
        /// 'TitleChanged' event.
        /// </summary>
        TitleChanged,

        /// <summary>
        /// 'FullScreenChanged' event.
        /// </summary>
        FullScreenChanged,

        /// <summary>
        /// 'ScriptMessageReceived' event.
        /// </summary>
        ScriptMessageReceived,

        /// <summary>
        /// 'ScriptResult' event.
        /// </summary>
        ScriptResult,

        /// <summary>
        /// 'BeforeBrowserCreate' event.
        /// </summary>
        BeforeBrowserCreate,
    }
}
