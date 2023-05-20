using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    public class WebBrowser : Control
    { 
        //WebBrowserZoomType ZoomType { get; set; }
        //WebBrowserZoom Zoom { get; set; }
        //WebBrowserBackend Backend { get; }
        //bool CanSetZoomType(WebBrowserZoomType value);

        public bool CanGoBack { get; }
        public bool CanGoForward { get; }
        public bool CanCut { get; }
        public bool CanCopy { get; }
        public bool CanUndo { get; }
        public bool CanRedo { get; }
        public bool IsBusy { get; }
        public bool CanPaste { get; }
        public float ZoomFactor { get; set; }
        public bool HasSelection { get; }
        public string SelectedSource { get => throw new Exception(); }
        public string SelectedText { get => throw new Exception(); }
        public string PageSource { get => throw new Exception(); }
        public string PageText { get => throw new Exception(); }
        public bool AccessToDevToolsEnabled { get; set; }
        public string UserAgent { get => throw new Exception(); set => throw new Exception(); }
        public bool ContextMenuEnabled { get; set; }

        public void GoBack() => throw new Exception();
        public void GoForward() => throw new Exception();
        public void Stop() => throw new Exception();
        public void ClearSelection() => throw new Exception();
        public void Copy() => throw new Exception();
        public void Paste() => throw new Exception();
        public void Cut() => throw new Exception();
        public void ClearHistory() => throw new Exception();
        public void EnableHistory(bool enable = true) => throw new Exception();
        public void ReloadDefault() => throw new Exception();
        public void Reload(bool noCache) => throw new Exception();
        public void SelectAll() => throw new Exception();
        public void DeleteSelection() => throw new Exception();
        public void Undo() => throw new Exception();
        public void Redo() => throw new Exception();
        public void Print() => throw new Exception();
        public void RemoveAllUserScripts() => throw new Exception();
        public bool AddScriptMessageHandler(string name) => throw new Exception();
        public bool RemoveScriptMessageHandler(string name) => throw new Exception();
        public void RunScriptAsync(string javascript, IntPtr clientData) => throw new Exception();

        public bool Editable { get; set; }
        public int Zoom { get; set; }

        public string GetCurrentTitle() => throw new Exception();
        public string GetCurrentURL() => throw new Exception();
        public void LoadURL(string url) => throw new Exception();
        public string RunScript(string javascript) => throw new Exception();
        public void SetPage(string text, string baseUrl) => throw new Exception();
        public bool AddUserScript(string javascript, int injectionTime) => throw new Exception();

        public event EventHandler Navigating { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler Navigated { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler Loaded { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler Error { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler NewWindow { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler TitleChanged { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler FullScreenChanged { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler ScriptMessageReceived { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler ScriptResult { add => throw new Exception(); remove => throw new Exception(); }
    }
}
