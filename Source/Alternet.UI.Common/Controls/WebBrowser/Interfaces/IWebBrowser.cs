using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    ///     All methods and properties of the <see cref="WebBrowser"/> control
    ///     are defined in this interface.
    /// </summary>
    public interface IWebBrowser : IWebBrowserLite
    {
        event EventHandler<WebBrowserEventArgs> Navigated;

        event EventHandler<WebBrowserEventArgs> Navigating;

        event EventHandler<WebBrowserEventArgs> Loaded;

        event EventHandler<WebBrowserEventArgs> Error;

        event EventHandler<WebBrowserEventArgs> NewWindow;

        event EventHandler<WebBrowserEventArgs> DocumentTitleChanged;

        event EventHandler<WebBrowserEventArgs> FullScreenChanged;

        event EventHandler<WebBrowserEventArgs> ScriptMessageReceived;

        event EventHandler<WebBrowserEventArgs> ScriptResult;

        Uri Source { get; set; }

        string Url { get; set; }

        bool CanZoomIn { get; }

        bool CanZoomOut { get; }

        void ZoomIn();

        void ZoomOut();

        void NavigateToStream(Stream stream);

        void Navigate(Uri source);

        void Navigate(string urlString);

        string? ToInvokeScriptArg(object arg);

        void InvokeScriptAsync(string scriptName, IntPtr clientData, params object[] args);
    }
}