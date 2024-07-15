using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Subset of the <see cref="WebBrowser"/> control's methods and properties.
    /// </summary>
    public interface IWebBrowserLite
    {
        /// <inheritdoc cref="WebBrowser.PreferredColorScheme"/>
        WebBrowserPreferredColorScheme PreferredColorScheme { get; set; }

        /// <inheritdoc cref="WebBrowser.Editable"/>
        bool Editable { get; set; }

        /// <inheritdoc cref="WebBrowser.CanGoBack"/>
        bool CanGoBack { get; }

        /// <inheritdoc cref="WebBrowser.CanGoForward"/>
        bool CanGoForward { get; }

        /// <inheritdoc cref="WebBrowser.CanCut"/>
        bool CanCut { get; }

        /// <inheritdoc cref="WebBrowser.CanCopy"/>
        bool CanCopy { get; }

        /// <inheritdoc cref="WebBrowser.CanUndo"/>
        bool CanUndo { get; }

        /// <inheritdoc cref="WebBrowser.CanRedo"/>
        bool CanRedo { get; }

        /// <inheritdoc cref="WebBrowser.IsBusy"/>
        bool IsBusy { get; }

        /// <inheritdoc cref="WebBrowser.CanPaste"/>
        bool CanPaste { get; }

        /// <inheritdoc cref="WebBrowser.ZoomFactor"/>
        float ZoomFactor { get; set; }

        /// <inheritdoc cref="WebBrowser.ZoomType"/>
        WebBrowserZoomType ZoomType { get; set; }

        /// <inheritdoc cref="WebBrowser.Zoom"/>
        WebBrowserZoom Zoom { get; set; }

        /// <inheritdoc cref="WebBrowser.HasSelection"/>
        bool HasSelection { get; }

        /// <inheritdoc cref="WebBrowser.SelectedSource"/>
        string SelectedSource { get; }

        /// <inheritdoc cref="WebBrowser.SelectedText"/>
        string SelectedText { get; }

        /// <inheritdoc cref="WebBrowser.PageSource"/>
        string PageSource { get; }

        /// <inheritdoc cref="WebBrowser.PageText"/>
        string PageText { get; }

        /// <inheritdoc cref="WebBrowser.AccessToDevToolsEnabled"/>
        bool AccessToDevToolsEnabled { get; set; }

        /// <inheritdoc cref="WebBrowser.UserAgent"/>
        string UserAgent { get; set; }

        /// <inheritdoc cref="WebBrowser.ContextMenuEnabled"/>
        bool ContextMenuEnabled { get; set; }

        /// <inheritdoc cref="WebBrowser.Backend"/>
        WebBrowserBackend Backend { get; }

        /// <inheritdoc cref="WebBrowser.GetNativeBackend"/>
        IntPtr GetNativeBackend();

        /// <inheritdoc cref="WebBrowser.DoCommand"/>
        string? DoCommand(string cmdName, params object?[] args);

        /// <inheritdoc cref="WebBrowser.AddUserScript"/>
        bool AddUserScript(string javascript, bool injectDocStart);

        /// <inheritdoc cref="WebBrowser.LoadURL"/>
        void LoadURL(string url);

        /// <inheritdoc cref="WebBrowser.CanSetZoomType"/>
        bool CanSetZoomType(WebBrowserZoomType zoomType);

        /// <inheritdoc cref="WebBrowser.GetCurrentTitle"/>
        string GetCurrentTitle();

        /// <inheritdoc cref="WebBrowser.GetCurrentURL"/>
        string GetCurrentURL();

        /// <inheritdoc cref="WebBrowser.NavigateToString"/>
        void NavigateToString(string html, string? baseUrl = null);

        /// <inheritdoc cref="WebBrowser.GoBack"/>
        bool GoBack();

        /// <inheritdoc cref="WebBrowser.GoForward"/>
        bool GoForward();

        /// <inheritdoc cref="WebBrowser.Stop"/>
        void Stop();

        /// <inheritdoc cref="WebBrowser.ClearSelection"/>
        void ClearSelection();

        /// <inheritdoc cref="WebBrowser.Copy"/>
        void Copy();

        /// <inheritdoc cref="WebBrowser.Paste"/>
        void Paste();

        /// <inheritdoc cref="WebBrowser.Cut"/>
        void Cut();

        /// <inheritdoc cref="WebBrowser.SetVirtualHostNameToFolderMapping"/>
        public void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind);

        /// <inheritdoc cref="WebBrowser.ClearHistory"/>
        void ClearHistory();

        /// <inheritdoc cref="WebBrowser.EnableHistory"/>
        void EnableHistory(bool enable = true);

        /// <inheritdoc cref="WebBrowser.Reload()"/>
        void Reload();

        /// <inheritdoc cref="WebBrowser.Reload(bool)"/>
        void Reload(bool noCache);

        /// <inheritdoc cref="WebBrowser.SelectAll"/>
        void SelectAll();

        /// <inheritdoc cref="WebBrowser.DeleteSelection"/>
        void DeleteSelection();

        /// <inheritdoc cref="WebBrowser.Undo"/>
        void Undo();

        /// <inheritdoc cref="WebBrowser.Redo"/>
        void Redo();

        /// <inheritdoc cref="WebBrowser.Find"/>
        int Find(string text, WebBrowserFindParams? prm = null);

        /// <inheritdoc cref="WebBrowser.FindClearResult"/>
        void FindClearResult();

        /// <inheritdoc cref="WebBrowser.Print"/>
        void Print();

        /// <inheritdoc cref="WebBrowser.RemoveAllUserScripts"/>
        void RemoveAllUserScripts();

        /// <inheritdoc cref="WebBrowser.AddScriptMessageHandler"/>
        bool AddScriptMessageHandler(string name);

        /// <inheritdoc cref="WebBrowser.RemoveScriptMessageHandler"/>
        bool RemoveScriptMessageHandler(string name);

        /// <inheritdoc cref="WebBrowser.RunScriptAsync"/>
        void RunScriptAsync(string javascript, IntPtr? clientData);
    }
}
