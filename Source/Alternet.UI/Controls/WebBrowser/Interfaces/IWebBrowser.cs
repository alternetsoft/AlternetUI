using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    //-------------------------------------------------
    public interface IWebBrowser : IWebBrowserLite
    {
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> Navigated;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> Navigating;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> Loaded;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> Error;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> NewWindow;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> DocumentTitleChanged;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> FullScreenChanged;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> ScriptMessageReceived;
        //-------------------------------------------------
        event EventHandler<WebBrowserEventArgs> ScriptResult;
        //-------------------------------------------------
        void ZoomIn();
        //-------------------------------------------------
        void ZoomOut();
        //-------------------------------------------------
        void NavigateToString(string text);
        //-------------------------------------------------
        void NavigateToStream(Stream stream);
        //-------------------------------------------------
        void Navigate(Uri source);
        //-------------------------------------------------
        void Navigate(string urlString);
        //-------------------------------------------------
        string? InvokeScript(string scriptName);
        //-------------------------------------------------
        string? ToInvokeScriptArg(object arg);
        //-------------------------------------------------
        string? InvokeScript(string scriptName, params object[] args);
        //-------------------------------------------------
        bool RunScript(string javascript);
        //-------------------------------------------------
        Uri Source { get; set; }
        //-------------------------------------------------
        /// <include file="IWebBrowser.xml" path='doc/CanZoomIn/*'/>
        bool CanZoomIn { get; }
        //-------------------------------------------------
        bool CanZoomOut { get; }
        //-------------------------------------------------
    }
    //-------------------------------------------------
    public interface IWebBrowserLite
    {
        //-------------------------------------------------
        bool Editable { get; set; }
        //-------------------------------------------------
        bool CanGoBack { get; }
        //-------------------------------------------------
        bool CanGoForward { get; }
        //-------------------------------------------------
        bool CanCut { get; }
        //-------------------------------------------------
        bool CanCopy { get; }
        //-------------------------------------------------
        bool CanUndo { get; }
        //-------------------------------------------------
        bool CanRedo { get; }
        //-------------------------------------------------
        bool IsBusy { get; }
        //-------------------------------------------------
        bool CanPaste { get; }
        //-------------------------------------------------
        float ZoomFactor { get; set; }
        //-------------------------------------------------
        WebBrowserZoomType ZoomType { get; set; }
        //-------------------------------------------------
        WebBrowserZoom Zoom { get; set; }
        //-------------------------------------------------
        bool HasSelection { get; }
        //-------------------------------------------------
        string SelectedSource { get; }
        //-------------------------------------------------
        string SelectedText { get; }
        //-------------------------------------------------
        string PageSource { get; }
        //-------------------------------------------------
        string PageText { get; }
        //-------------------------------------------------
        bool AccessToDevToolsEnabled { get; set; }
        //-------------------------------------------------
        string UserAgent { get; set; }
        //-------------------------------------------------
        bool ContextMenuEnabled { get; set; }
        //-------------------------------------------------
        WebBrowserBackend Backend { get; }
        //-------------------------------------------------
        IntPtr GetNativeBackend();
        //-------------------------------------------------
        string DoCommand(string cmdName, params object?[] args);
        //-------------------------------------------------
        void RunScriptAsync(string javascript, IntPtr clientData);
        //-------------------------------------------------
        bool AddUserScript(string javascript, bool injectDocStart);
        //-------------------------------------------------
        void LoadURL(string url);
        //-------------------------------------------------
        bool CanSetZoomType(WebBrowserZoomType value);
        //-------------------------------------------------
        string GetCurrentTitle();
        //-------------------------------------------------
        string GetCurrentURL();
        //-------------------------------------------------
        void NavigateToString(string text, string baseUrl);
        //-------------------------------------------------
        bool GoBack();
        //-------------------------------------------------
        bool GoForward();
        //-------------------------------------------------
        void Stop();
        //-------------------------------------------------
        void ClearSelection();
        //-------------------------------------------------
        void Copy();
        //-------------------------------------------------
        void Paste();
        //-------------------------------------------------
        void Cut();
        //-------------------------------------------------
        void ClearHistory();
        //-------------------------------------------------
        void EnableHistory(bool enable = true);
        //-------------------------------------------------
        void Reload();
        //-------------------------------------------------
        void Reload(bool noCache);
        //-------------------------------------------------
        void SelectAll();
        //-------------------------------------------------
        void DeleteSelection();
        //-------------------------------------------------
        void Undo();
        //-------------------------------------------------
        void Redo();
        //-------------------------------------------------
        long Find(string text, WebBrowserFindParams? prm = null);
        //-------------------------------------------------
        void Print();
        //-------------------------------------------------
        void RemoveAllUserScripts();
        //-------------------------------------------------
        bool AddScriptMessageHandler(string name);
        //-------------------------------------------------
        bool RemoveScriptMessageHandler(string name);
        //-------------------------------------------------
        bool RunScript(string javascript, out string result);
        //-------------------------------------------------
    }
    //-------------------------------------------------
    internal interface IWebBrowserFactory
    {
        bool IsBackendAvailable(WebBrowserBackend value);
        void SetBackend(WebBrowserBackend value);
        string GetLibraryVersionString();
        string GetBackendVersionString(WebBrowserBackend value);
        void SetDefaultPage(string url);
        void SetLatestBackend();
    }
    //-------------------------------------------------
}
//-------------------------------------------------

