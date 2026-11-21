using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Declares methods and properties which allow to work with <see cref="WebBrowser"/> control.
    /// </summary>
    public interface IWebBrowser : IWebBrowserLite
    {
        /// <inheritdoc cref="WebBrowser.Navigated"/>
        event EventHandler<WebBrowserEventArgs> Navigated;

        /// <inheritdoc cref="WebBrowser.Navigating"/>
        event EventHandler<WebBrowserEventArgs> Navigating;

        /// <inheritdoc cref="WebBrowser.Loaded"/>
        event EventHandler<WebBrowserEventArgs> Loaded;

        /// <inheritdoc cref="WebBrowser.Error"/>
        event EventHandler<WebBrowserEventArgs> Error;

        /// <inheritdoc cref="WebBrowser.NewWindow"/>
        event EventHandler<WebBrowserEventArgs> NewWindow;

        /// <inheritdoc cref="WebBrowser.DocumentTitleChanged"/>
        event EventHandler<WebBrowserEventArgs> DocumentTitleChanged;

        /// <inheritdoc cref="WebBrowser.FullScreenChanged"/>
        event EventHandler<WebBrowserEventArgs> FullScreenChanged;

        /// <inheritdoc cref="WebBrowser.ScriptMessageReceived"/>
        event EventHandler<WebBrowserEventArgs> ScriptMessageReceived;

        /// <inheritdoc cref="WebBrowser.ScriptResult"/>
        event EventHandler<WebBrowserEventArgs> ScriptResult;

        /// <inheritdoc cref="WebBrowser.Source"/>
        Uri Source { get; set; }

        /// <inheritdoc cref="WebBrowser.Url"/>
        string Url { get; set; }

        /// <inheritdoc cref="WebBrowser.CanZoomIn"/>
        bool CanZoomIn { get; }

        /// <inheritdoc cref="WebBrowser.CanZoomOut"/>
        bool CanZoomOut { get; }

        /// <inheritdoc cref="WebBrowser.ZoomIn"/>
        void ZoomIn();

        /// <inheritdoc cref="WebBrowser.ZoomOut"/>
        void ZoomOut();

        /// <inheritdoc cref="WebBrowser.NavigateToStream(Stream)"/>
        void NavigateToStream(Stream stream);

        /// <inheritdoc cref="WebBrowser.Navigate(Uri)"/>
        void Navigate(Uri source);

        /// <inheritdoc cref="WebBrowser.Navigate(string)"/>
        void Navigate(string urlString);

        /// <inheritdoc cref="WebBrowser.ToInvokeScriptArg"/>
        string? ToInvokeScriptArg(object arg);

        /// <inheritdoc cref="WebBrowser.InvokeScriptAsync"/>
        void InvokeScriptAsync(string scriptName, IntPtr clientData, params object[] args);
    }
}