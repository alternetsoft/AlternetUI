using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    //-------------------------------------------------
    internal interface IWebBrowser : IWebBrowserLite
    {
        //-------------------------------------------------
        event EventHandler Navigated;
        event EventHandler Navigating;
        event EventHandler Loaded;
        event EventHandler Error;
        event EventHandler NewWindow;
        event EventHandler TitleChanged;
        event EventHandler FullScreenChanged;
        event EventHandler ScriptMessageReceived;
        event EventHandler ScriptResult;
        //-------------------------------------------------
        void ZoomIn();
        void ZoomOut();
        void NavigateToString(string text);
        void NavigateToStream(Stream stream);
        void Navigate(Uri source);
        string? InvokeScript(string scriptName);
        string? ToInvokeScriptArg(object arg);
        string? InvokeScript(string scriptName, params object[] args);
        bool RunScript(string javascript);
        //-------------------------------------------------
        Uri Source { get; set; }
        bool CanZoomIn { get; }
        bool CanZoomOut { get; }
        //-------------------------------------------------
    }
    //-------------------------------------------------
    internal interface IWebBrowserLite
    {
        //-------------------------------------------------
        bool Editable { get; set; }
        bool CanGoBack { get; }
        bool CanGoForward { get; }
        bool CanCut { get; }
        bool CanCopy { get; }
        bool CanUndo { get; }
        bool CanRedo { get; }
        bool IsBusy { get; }
        bool CanPaste { get; }
        float ZoomFactor { get; set; }
        WebBrowserZoomType ZoomType { get; set; }
        WebBrowserZoom Zoom { get; set; }
        bool HasSelection { get; }
        string SelectedSource { get; }
        string SelectedText { get; }
        string PageSource { get; }
        string PageText { get; }
        bool AccessToDevToolsEnabled { get; set; }
        string UserAgent { get; set; }
        bool ContextMenuEnabled { get; set; }
        WebBrowserBackend Backend { get; }
        //-------------------------------------------------
        bool AddUserScript(string javascript, bool injectDocStart);
        void LoadURL(string url);
        bool CanSetZoomType(WebBrowserZoomType value);
        string GetCurrentTitle();
        string GetCurrentURL();
        void NavigateToString(string text, string baseUrl);
        void GoBack();
        void GoForward();
        void Stop();
        void ClearSelection();
        void Copy();
        void Paste();
        void Cut();
        void ClearHistory();
        void EnableHistory(bool enable = true);
        void Reload();
        void Reload(bool noCache);
        void SelectAll();
        void DeleteSelection();
        void Undo();
        void Redo();
        long Find(string text, WebBrowserFindParams? prm = null);
        void Print();
        void RemoveAllUserScripts();
        bool AddScriptMessageHandler(string name);
        bool RemoveScriptMessageHandler(string name);
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

