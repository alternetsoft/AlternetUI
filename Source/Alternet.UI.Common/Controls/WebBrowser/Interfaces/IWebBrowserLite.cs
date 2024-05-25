using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    ///     Subset of the <see cref="WebBrowser"/> control's methods and properties.
    /// </summary>
    public interface IWebBrowserLite
    {
        WebBrowserPreferredColorScheme PreferredColorScheme { get; set; }

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

        IntPtr GetNativeBackend();

        string? DoCommand(string cmdName, params object?[] args);

        bool AddUserScript(string javascript, bool injectDocStart);

        void LoadURL(string url);

        bool CanSetZoomType(WebBrowserZoomType zoomType);

        string GetCurrentTitle();

        string GetCurrentURL();

        void NavigateToString(string html, string? baseUrl = null);

        bool GoBack();

        bool GoForward();

        void Stop();

        void ClearSelection();

        void Copy();

        void Paste();

        void Cut();

        public void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind);

        void ClearHistory();

        void EnableHistory(bool enable = true);

        void Reload();

        void Reload(bool noCache);

        void SelectAll();

        void DeleteSelection();

        void Undo();

        void Redo();

        int Find(string text, WebBrowserFindParams? prm = null);

        void FindClearResult();

        void Print();

        void RemoveAllUserScripts();

        bool AddScriptMessageHandler(string name);

        bool RemoveScriptMessageHandler(string name);

        void RunScriptAsync(string javascript, IntPtr? clientData);
    }
}
