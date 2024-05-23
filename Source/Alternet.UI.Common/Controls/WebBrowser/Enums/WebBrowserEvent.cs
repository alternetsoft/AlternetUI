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
        Unknown,

        Navigating,

        Navigated,

        Loaded,

        Error,

        NewWindow,

        TitleChanged,

        FullScreenChanged,

        ScriptMessageReceived,

        ScriptResult,

        BeforeBrowserCreate,
    }
}
