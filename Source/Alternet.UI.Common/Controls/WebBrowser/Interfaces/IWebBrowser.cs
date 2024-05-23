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
    /// <inheritdoc cref="IWebBrowserLite"/>
    public interface IWebBrowser : IWebBrowserLite
    {
        /*/// <include file="IWebBrowser.xml" path='doc/RunScript/*'/>
        bool RunScript(string javascript);*/

        /// <include file="IWebBrowser.xml" path='doc/Events/Navigated/*'/>
        event EventHandler<WebBrowserEventArgs> Navigated;

        /// <include file="IWebBrowser.xml" path='doc/Events/Navigating/*'/>
        event EventHandler<WebBrowserEventArgs> Navigating;

        /// <include file="IWebBrowser.xml" path='doc/Events/Loaded/*'/>
        event EventHandler<WebBrowserEventArgs> Loaded;

        /// <include file="IWebBrowser.xml" path='doc/Events/Error/*'/>
        event EventHandler<WebBrowserEventArgs> Error;

        /// <include file="IWebBrowser.xml" path='doc/Events/NewWindow/*'/>
        event EventHandler<WebBrowserEventArgs> NewWindow;

        /// <include file="IWebBrowser.xml" path='doc/Events/DocumentTitleChanged/*'/>
        event EventHandler<WebBrowserEventArgs> DocumentTitleChanged;

        /// <include file="IWebBrowser.xml" path='doc/Events/FullScreenChanged/*'/>
        event EventHandler<WebBrowserEventArgs> FullScreenChanged;

        /// <include file="IWebBrowser.xml" path='doc/Events/ScriptMessageReceived/*'/>
        event EventHandler<WebBrowserEventArgs> ScriptMessageReceived;

        /// <include file="IWebBrowser.xml" path='doc/Events/ScriptResult/*'/>
        event EventHandler<WebBrowserEventArgs> ScriptResult;

        /// <include file="IWebBrowser.xml" path='doc/Source/*'/>
        Uri Source { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/Url/*'/>
        string Url { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/CanZoomIn/*'/>
        bool CanZoomIn { get; }

        /// <include file="IWebBrowser.xml" path='doc/CanZoomOut/*'/>
        bool CanZoomOut { get; }

        /// <include file="IWebBrowser.xml" path='doc/ZoomIn/*'/>
        void ZoomIn();

        /// <include file="IWebBrowser.xml" path='doc/ZoomOut/*'/>
        void ZoomOut();

        /// <include file="IWebBrowser.xml" path='doc/NavigateToStream/*'/>
        void NavigateToStream(Stream stream);

        /// <include file="IWebBrowser.xml" path='doc/Navigate_uri/*'/>
        void Navigate(Uri source);

        /// <include file="IWebBrowser.xml" path='doc/Navigate_string/*'/>
        void Navigate(string urlString);

        /// <include file="IWebBrowser.xml" path='doc/ToInvokeScriptArg/*'/>
        string? ToInvokeScriptArg(object arg);

        /// <include file="IWebBrowser.xml" path='doc/InvokeScriptAsync/*'/>
        void InvokeScriptAsync(string scriptName, IntPtr clientData, params object[] args);
    }
}