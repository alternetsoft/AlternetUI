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

        /// <include file="IWebBrowser.xml" path='doc/Navigate/*'/>
        void Navigate(Uri source);

        /// <include file="IWebBrowser.xml" path='doc/Navigate/*'/>
        void Navigate(string urlString);

        /// <include file="IWebBrowser.xml" path='doc/ToInvokeScriptArg/*'/>
        string? ToInvokeScriptArg(object arg);

        /// <include file="IWebBrowser.xml" path='doc/InvokeScriptAsync/*'/>
        void InvokeScriptAsync(string scriptName, IntPtr clientData, params object[] args);
    }

    /// <summary>
    ///     Subset of the <see cref="WebBrowser"/> control's methods and properties.
    /// </summary>
    public interface IWebBrowserLite
    {
        /// <include file="IWebBrowser.xml" path='doc/PreferredColorScheme/*'/>
        WebBrowserPreferredColorScheme PreferredColorScheme { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/Editable/*'/>
        bool Editable { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/CanGoBack/*'/>
        bool CanGoBack { get; }

        /// <include file="IWebBrowser.xml" path='doc/CanGoForward/*'/>
        bool CanGoForward { get; }

        /// <include file="IWebBrowser.xml" path='doc/CanCut/*'/>
        bool CanCut { get; }

        /// <include file="IWebBrowser.xml" path='doc/CanCopy/*'/>
        bool CanCopy { get; }

        /// <include file="IWebBrowser.xml" path='doc/CanUndo/*'/>
        bool CanUndo { get; }

        /// <include file="IWebBrowser.xml" path='doc/CanRedo/*'/>
        bool CanRedo { get; }

        /// <include file="IWebBrowser.xml" path='doc/IsBusy/*'/>
        bool IsBusy { get; }

        /// <include file="IWebBrowser.xml" path='doc/CanPaste/*'/>
        bool CanPaste { get; }

        /// <include file="IWebBrowser.xml" path='doc/ZoomFactor/*'/>
        float ZoomFactor { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/ZoomType/*'/>
        WebBrowserZoomType ZoomType { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/Zoom/*'/>
        WebBrowserZoom Zoom { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/HasSelection/*'/>
        bool HasSelection { get; }

        /// <include file="IWebBrowser.xml" path='doc/SelectedSource/*'/>
        string SelectedSource { get; }

        /// <include file="IWebBrowser.xml" path='doc/SelectedText/*'/>
        string SelectedText { get; }

        /// <include file="IWebBrowser.xml" path='doc/PageSource/*'/>
        string PageSource { get; }

        /// <include file="IWebBrowser.xml" path='doc/PageText/*'/>
        string PageText { get; }

        /// <include file="IWebBrowser.xml" path='doc/AccessToDevToolsEnabled/*'/>
        bool AccessToDevToolsEnabled { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/UserAgent/*'/>
        string UserAgent { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/ContextMenuEnabled/*'/>
        bool ContextMenuEnabled { get; set; }

        /// <include file="IWebBrowser.xml" path='doc/Backend/*'/>
        WebBrowserBackend Backend { get; }

        /// <include file="IWebBrowser.xml" path='doc/GetNativeBackend/*'/>
        IntPtr GetNativeBackend();

        /// <include file="IWebBrowser.xml" path='doc/DoCommand/*'/>
        string DoCommand(string cmdName, params object?[] args);

        /// <include file="IWebBrowser.xml" path='doc/AddUserScript/*'/>
        bool AddUserScript(string javascript, bool injectDocStart);

        /// <include file="IWebBrowser.xml" path='doc/LoadURL/*'/>
        void LoadURL(string url);

        /// <include file="IWebBrowser.xml" path='doc/CanSetZoomType/*'/>
        bool CanSetZoomType(WebBrowserZoomType zoomType);

        /// <include file="IWebBrowser.xml" path='doc/GetCurrentTitle/*'/>
        string GetCurrentTitle();

        /// <include file="IWebBrowser.xml" path='doc/GetCurrentURL/*'/>
        string GetCurrentURL();

        /// <include file="IWebBrowser.xml" path='doc/NavigateToString/*'/>
        void NavigateToString(string html, string? baseUrl = null);

        /// <include file="IWebBrowser.xml" path='doc/GoBack/*'/>
        bool GoBack();

        /// <include file="IWebBrowser.xml" path='doc/GoForward/*'/>
        bool GoForward();

        /// <include file="IWebBrowser.xml" path='doc/Stop/*'/>
        void Stop();

        /// <include file="IWebBrowser.xml" path='doc/ClearSelection/*'/>
        void ClearSelection();

        /// <include file="IWebBrowser.xml" path='doc/Copy/*'/>
        void Copy();

        /// <include file="IWebBrowser.xml" path='doc/Paste/*'/>
        void Paste();

        /// <include file="IWebBrowser.xml" path='doc/Cut/*'/>
        void Cut();

        /// <include file="IWebBrowser.xml" path='doc/SetVirtualHostNameToFolderMapping/*'/>
        public void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind);

        /// <include file="IWebBrowser.xml" path='doc/ClearHistory/*'/>
        void ClearHistory();

        /// <include file="IWebBrowser.xml" path='doc/EnableHistory/*'/>
        void EnableHistory(bool enable = true);

        /// <include file="IWebBrowser.xml" path='doc/Reload/*'/>
        void Reload();

        /// <include file="IWebBrowser.xml" path='doc/Reload_noCache/*'/>
        void Reload(bool noCache);

        /// <include file="IWebBrowser.xml" path='doc/SelectAll/*'/>
        void SelectAll();

        /// <include file="IWebBrowser.xml" path='doc/DeleteSelection/*'/>
        void DeleteSelection();

        /// <include file="IWebBrowser.xml" path='doc/Undo/*'/>
        void Undo();

        /// <include file="IWebBrowser.xml" path='doc/Redo/*'/>
        void Redo();

        /// <include file="IWebBrowser.xml" path='doc/Find/*'/>
        int Find(string text, WebBrowserFindParams? prm = null);

        /// <include file="IWebBrowser.xml" path='doc/FindClearResult/*'/>
        void FindClearResult();

        /// <include file="IWebBrowser.xml" path='doc/Print/*'/>
        void Print();

        /// <include file="IWebBrowser.xml" path='doc/RemoveAllUserScripts/*'/>
        void RemoveAllUserScripts();

        /// <include file="IWebBrowser.xml" path='doc/AddScriptMessageHandler/*'/>
        bool AddScriptMessageHandler(string name);

        /// <include file="IWebBrowser.xml" path='doc/RemoveScriptMessageHandler/*'/>
        bool RemoveScriptMessageHandler(string name);

        /// <include file="IWebBrowser.xml" path='doc/RunScriptAsync/*'/>
        void RunScriptAsync(string javascript, IntPtr? clientData);
    }

    internal interface IWebBrowserFactory
    {
        bool IsBackendAvailable(WebBrowserBackend value);

        void SetBackend(WebBrowserBackend value);

        string GetLibraryVersionString();

        string GetBackendVersionString(WebBrowserBackend value);

        void SetDefaultPage(string url);

        void SetLatestBackend();
    }
}