using System;

namespace Alternet.UI
{
    //-------------------------------------------------
    internal partial class WebBrowserHandler :
        NativeControlHandler<WebBrowser, Native.WebBrowser>, IWebBrowserLite
    {
        //-------------------------------------------------
        internal static bool IsBackendIEAvailable() => WebBrowserNativeApi.WebBrowser_IsBackendIEAvailable_();
        internal static bool IsBackendEdgeAvailable() => WebBrowserNativeApi.WebBrowser_IsBackendEdgeAvailable_();
        internal static bool IsBackendWebKitAvailable() => WebBrowserNativeApi.WebBrowser_IsBackendWebKitAvailable_();
        //-------------------------------------------------
        private IntPtr NativePointer => NativeControl.NativePointer;
        public void LoadURL(string url) => NativeControl.LoadURL(url);
        public string GetCurrentTitle() => NativeControl.GetCurrentTitle();
        public string GetCurrentURL() => NativeControl.GetCurrentURL();
        public IntPtr GetNativeBackend() => NativeControl.GetNativeBackend();
        public bool CanGoBack => NativeControl.CanGoBack;
        public bool CanGoForward => NativeControl.CanGoForward;
        //-------------------------------------------------
        private static void ArgsToDoCommandParams(object?[] args,out string? cmdParam1, 
            out string? cmdParam2)
        {
            cmdParam1 = String.Empty;
            cmdParam2 = String.Empty;
            if (args.Length > 0 && args[0] != null)
                cmdParam1 = args[0]?.ToString();
            if (args.Length > 1 && args[1] != null)
                cmdParam2 = args[1]?.ToString();
        }
        //-------------------------------------------------
        public static string DoCommandGlobal(string cmdName, params object?[] args)
        {
            ArgsToDoCommandParams(args, out string? cmdParam1, out string? cmdParam2);
            return Native.WebBrowser.DoCommandGlobal(cmdName, cmdParam1!, cmdParam2!);
        }
        //-------------------------------------------------
        public string DoCommand(string cmdName,params object?[] args)
        {
            ArgsToDoCommandParams(args, out string? cmdParam1, out string? cmdParam2);
            return NativeControl.DoCommand(cmdName, cmdParam1!,cmdParam2!);
        }
        //-------------------------------------------------
        public bool GoBack() 
        { 
            NativeControl.GoBack(); 
            return true;
        }
        //-------------------------------------------------
        public bool GoForward()
        {
            NativeControl.GoForward();
            return true;
        }
        //-------------------------------------------------
        public void Stop()=> NativeControl.Stop();
        public void ClearHistory()=> NativeControl.ClearHistory();
        public void SelectAll() => NativeControl.SelectAll();
        public void EnableHistory(bool enable = true)=>NativeControl.EnableHistory(enable);
        public void Reload() =>
            NativeControl.ReloadDefault();
        public void Reload(bool noCache) =>
            NativeControl.Reload(noCache);
        //-------------------------------------------------
        public void RunScriptAsync(string javascript, IntPtr clientData)
        {
            NativeControl.RunScriptAsync(javascript, clientData);
        }
        //-------------------------------------------------
        public void NavigateToString(string text, string baseUrl)
        {
            NativeControl.SetPage(text, baseUrl);
        }
        //-------------------------------------------------
        public WebBrowserZoomType ZoomType
        {
            get
            {
                var zoomType = WebBrowserNativeApi.WebBrowser_GetZoomType_(NativeControl.NativePointer);
                WebBrowserZoomType result = 
                    (WebBrowserZoomType)Enum.ToObject(typeof(WebBrowserZoomType), zoomType);
                return result;
            }
            set
            {
                if (WebBrowserNativeApi.WebBrowser_CanSetZoomType_(NativeControl.NativePointer, (int)value))
                    WebBrowserNativeApi.WebBrowser_SetZoomType_(NativeControl.NativePointer, (int)value);
            }
        }
        //-------------------------------------------------
        public bool CanSetZoomType(WebBrowserZoomType value)
        {
            return WebBrowserNativeApi.WebBrowser_CanSetZoomType_(NativeControl.NativePointer, (int)value);
        }
        //-------------------------------------------------
        public float ZoomFactor
        {
            get => NativeControl.ZoomFactor;
            set=> NativeControl.ZoomFactor = value;
        }
        //-------------------------------------------------
        public bool Editable
        {
            get => NativeControl.Editable;
            set => NativeControl.Editable = value;
        }
        //-------------------------------------------------
        public WebBrowserZoom Zoom
        {
            get
            {
                return (WebBrowserZoom)Enum.ToObject(typeof(WebBrowserZoom), (int)NativeControl.Zoom);
            }

            set
            {
                NativeControl.Zoom = (int)value;
            }
        }
        //-------------------------------------------------
        internal override Native.Control CreateNativeControl()
        {
            return new Native.WebBrowser();
        }
        //-------------------------------------------------
        protected override void OnAttach()
        {
            base.OnAttach();

            NativeControl.Navigating += Control.OnNativeNavigating;
            NativeControl.Navigated += Control.OnNativeNavigated;
            NativeControl.Loaded += Control.OnNativeLoaded;
            NativeControl.Error += Control.OnNativeError;
            NativeControl.NewWindow += Control.OnNativeNewWindow;
            NativeControl.TitleChanged += Control.OnNativeTitleChanged;
            NativeControl.FullScreenChanged += Control.OnNativeFullScreenChanged;
            NativeControl.ScriptMessageReceived += Control.OnNativeScriptMessageReceived;
            NativeControl.ScriptResult += Control.OnNativeScriptResult;
        }
        //-------------------------------------------------
        protected override void OnDetach()
        {
            base.OnDetach();

            NativeControl.Navigating -= Control.OnNativeNavigating;
            NativeControl.Navigated -= Control.OnNativeNavigated;
            NativeControl.Loaded -= Control.OnNativeLoaded;
            NativeControl.Error -= Control.OnNativeError;
            NativeControl.NewWindow -= Control.OnNativeNewWindow;
            NativeControl.TitleChanged -= Control.OnNativeTitleChanged;
            NativeControl.FullScreenChanged -= Control.OnNativeFullScreenChanged;
            NativeControl.ScriptMessageReceived -= Control.OnNativeScriptMessageReceived;
            NativeControl.ScriptResult -= Control.OnNativeScriptResult;
        }
        //-------------------------------------------------
        public bool HasSelection => NativeControl.HasSelection;
        public void DeleteSelection() => NativeControl.DeleteSelection();
        public string SelectedText => NativeControl.SelectedText;
        public string SelectedSource => NativeControl.SelectedSource;
        public void ClearSelection()=> NativeControl.ClearSelection();
        public bool CanCut => NativeControl.CanCut;
        public bool CanCopy => NativeControl.CanCopy;
        public bool CanPaste => NativeControl.CanPaste;
        public void Cut() => NativeControl.Cut();
        public void Copy()=> NativeControl.Copy();
        public void Paste()=> NativeControl.Paste();
        public bool CanUndo=> NativeControl.CanUndo;
        public bool CanRedo=> NativeControl.CanRedo;
        public void Undo()=> NativeControl.Undo();
        public void Redo()=> NativeControl.Redo();
        //-------------------------------------------------
        public long Find(string text, WebBrowserFindParams? prm = null)
        {
            int flags = 0;
            if (prm != null)
                flags = prm.ToWebViewParams();
            return WebBrowserNativeApi.WebBrowser_Find_(NativePointer, text, flags);
        }
        //-------------------------------------------------
        public bool IsBusy=> NativeControl.IsBusy;
        public void Print()=> NativeControl.Print();
        public string PageSource => NativeControl.PageSource;
        public string PageText => NativeControl.PageText;
        //-------------------------------------------------
        public void RemoveAllUserScripts()
        {
            NativeControl.RemoveAllUserScripts();
        }
        //-------------------------------------------------
        public bool AddScriptMessageHandler(string name)
        {
            return NativeControl.AddScriptMessageHandler(name);
        }
        //-------------------------------------------------
        public bool RemoveScriptMessageHandler(string name)
        {
            return NativeControl.RemoveScriptMessageHandler(name);
        }
        //-------------------------------------------------
        public bool AccessToDevToolsEnabled
        {
            get=> NativeControl.AccessToDevToolsEnabled;
            set=> NativeControl.AccessToDevToolsEnabled=value;
        }
        //-------------------------------------------------
        public string UserAgent
        {
            get =>NativeControl.UserAgent;
            set =>NativeControl.UserAgent =value;
        }
        //-------------------------------------------------
        public bool ContextMenuEnabled
        {
            get => NativeControl.ContextMenuEnabled;
            set=> NativeControl.ContextMenuEnabled=value;
        }
        //-------------------------------------------------
        public WebBrowserBackend Backend
        {
            get
            {
                int v = WebBrowserNativeApi.WebBrowser_GetBackend_(NativePointer);
                return (WebBrowserBackend)Enum.ToObject(typeof(WebBrowserBackend), v);
            }
        }
        //-------------------------------------------------
        public bool AddUserScript(string javascript,bool injectDocStart)
        {
            const int wxWEBVIEW_INJECT_AT_DOCUMENT_START = 0;
            const int wxWEBVIEW_INJECT_AT_DOCUMENT_END = 1;

            var injectionTime = wxWEBVIEW_INJECT_AT_DOCUMENT_END;

            if (injectDocStart)
                injectionTime = wxWEBVIEW_INJECT_AT_DOCUMENT_START;

            return NativeControl.AddUserScript(javascript, injectionTime);
        }
        //-------------------------------------------------
        public bool RunScript(string javascript,out string result)
        {
            result = NativeControl.RunScript(javascript);
            if (result == "3140D0CF550442968B792551E6DCBEC1")
                return false;
            return true;
        }
        //-------------------------------------------------
    }
    //-------------------------------------------------
}