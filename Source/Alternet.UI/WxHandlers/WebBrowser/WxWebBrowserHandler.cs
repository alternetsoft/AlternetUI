using System;

namespace Alternet.UI
{
    internal partial class WxWebBrowserHandler :
        WxControlHandler<WebBrowser, Native.WebBrowser>, IWebBrowserLite
    {
        public bool IsEdgeBackend
        {
            get => NativeControl.IsEdge;
        }

        public override bool HasBorder
        {
            get => NativeControl.HasBorder;
            set => NativeControl.HasBorder = value;
        }

        public WebBrowserPreferredColorScheme PreferredColorScheme
        {
            get => (WebBrowserPreferredColorScheme)Enum.ToObject(
                typeof(WebBrowserPreferredColorScheme),
                NativeControl.PreferredColorScheme);
            set => NativeControl.PreferredColorScheme = (int)value;
        }

        public bool AccessToDevToolsEnabled
        {
            get => NativeControl.AccessToDevToolsEnabled;
            set => NativeControl.AccessToDevToolsEnabled = value;
        }

        public string UserAgent
        {
            get => NativeControl.UserAgent;
            set => NativeControl.UserAgent = value;
        }

        public bool ContextMenuEnabled
        {
            get => NativeControl.ContextMenuEnabled;
            set => NativeControl.ContextMenuEnabled = value;
        }

        public WebBrowserBackend Backend
        {
            get
            {
                int v = WxWebBrowserHandlerApi.WebBrowser_GetBackend_(NativePointer);
                return (WebBrowserBackend)Enum.ToObject(typeof(WebBrowserBackend), v);
            }
        }

        public bool CanGoBack => NativeControl.CanGoBack;

        public bool CanGoForward => NativeControl.CanGoForward;

        public bool HasSelection => NativeControl.HasSelection;

        public string SelectedText => NativeControl.SelectedText;

        public string SelectedSource => NativeControl.SelectedSource;

        public bool CanCut => NativeControl.CanCut;

        public bool CanCopy => NativeControl.CanCopy;

        public bool CanPaste => NativeControl.CanPaste;

        public bool CanUndo => NativeControl.CanUndo;

        public bool CanRedo => NativeControl.CanRedo;

        public bool IsBusy => NativeControl.IsBusy;

        public string PageSource => NativeControl.PageSource;

        public string PageText => NativeControl.PageText;

        public float ZoomFactor
        {
            get => NativeControl.ZoomFactor;
            set => NativeControl.ZoomFactor = value;
        }

        public bool Editable
        {
            get => NativeControl.Editable;
            set => NativeControl.Editable = value;
        }

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

        public WebBrowserZoomType ZoomType
        {
            get
            {
                var zoomType = WxWebBrowserHandlerApi.WebBrowser_GetZoomType_(NativeControl.NativePointer);
                WebBrowserZoomType result =
                    (WebBrowserZoomType)Enum.ToObject(typeof(WebBrowserZoomType), zoomType);
                return result;
            }

            set
            {
                if (WxWebBrowserHandlerApi.WebBrowser_CanSetZoomType_(NativeControl.NativePointer, (int)value))
                    WxWebBrowserHandlerApi.WebBrowser_SetZoomType_(NativeControl.NativePointer, (int)value);
            }
        }

        private IntPtr NativePointer => NativeControl.NativePointer;

        public static string? DoCommandGlobal(string cmdName, params object?[] args)
        {
            ArgsToDoCommandParams(args, out string? cmdParam1, out string? cmdParam2);
            return Native.WebBrowser.DoCommandGlobal(cmdName, cmdParam1!, cmdParam2!);
        }

        public void LoadURL(string url) => NativeControl.LoadURL(url);

        public string GetCurrentTitle() => NativeControl.GetCurrentTitle();

        public string GetCurrentURL() => NativeControl.GetCurrentURL();

        public IntPtr GetNativeBackend() => NativeControl.GetNativeBackend();

        public string? DoCommand(string cmdName, params object?[] args)
        {
            ArgsToDoCommandParams(args, out string? cmdParam1, out string? cmdParam2);
            return NativeControl.DoCommand(cmdName, cmdParam1!, cmdParam2!);
        }

        public bool GoBack()
        {
            NativeControl.GoBack();
            return true;
        }

        public bool GoForward()
        {
            NativeControl.GoForward();
            return true;
        }

        public void Stop() => NativeControl.Stop();

        public void ClearHistory() => NativeControl.ClearHistory();

        public void SelectAll() => NativeControl.SelectAll();

        public void EnableHistory(bool enable = true) => NativeControl.EnableHistory(enable);

        public void Reload() =>
            NativeControl.ReloadDefault();

        public void Reload(bool noCache) =>
            NativeControl.Reload(noCache);

        public bool IsIEBackend()
        {
            return Backend == WebBrowserBackend.IE ||
                Backend == WebBrowserBackend.IELatest;
        }

        public bool RunScript(string javascript, out string result)
        {
            const string magicResult = "3140D0CF550442968B792551E6DCBEC1";

            result = NativeControl.RunScript(javascript);
            if (result.StartsWith(magicResult))
            {
                result = result.Remove(0, magicResult.Length);
                return false;
            }

            return true;
        }

        public virtual void SetVirtualHostNameToFolderMapping(
            string hostName,
            string folderPath,
            WebBrowserHostResourceAccessKind accessKind)
        {
            NativeControl.SetVirtualHostNameToFolderMapping(
                hostName,
                folderPath,
                (int)accessKind);
        }

        public void RunScriptAsync(string javascript, IntPtr? clientData = null)
        {
            if (IsIEBackend())
            {
                var ok = RunScript(javascript, out string result);
                WebBrowserEventArgs e = new (WebBrowserEvent.ScriptResult.ToString())
                {
                    Text = result,
                    IsError = !ok,
                };

                IntPtr data = IntPtr.Zero;
                if (clientData != null)
                    data = (IntPtr)clientData;

                e.ClientData = data;
                Control?.RaiseScriptResult(e);
                return;
            }

            if (clientData == null)
                NativeControl.RunScriptAsync(javascript, IntPtr.Zero);
            else
                NativeControl.RunScriptAsync(javascript, (IntPtr)clientData);
        }

        public void NavigateToString(string html, string? baseUrl = null)
        {
            NativeControl.SetPage(html, baseUrl!);
        }

        public bool CanSetZoomType(WebBrowserZoomType value)
        {
            return WxWebBrowserHandlerApi.WebBrowser_CanSetZoomType_(NativeControl.NativePointer, (int)value);
        }

        public void Undo() => NativeControl.Undo();

        public void Redo() => NativeControl.Redo();

        public void Cut() => NativeControl.Cut();

        public void Copy() => NativeControl.Copy();

        public void Paste() => NativeControl.Paste();

        public void DeleteSelection() => NativeControl.DeleteSelection();

        public void ClearSelection() => NativeControl.ClearSelection();

        public void FindClearResult()
        {
            Find(string.Empty, null);
        }

        public int Find(string text, WebBrowserFindParams? prm = null)
        {
            prm ??= new ();

            var flags = (WebBrowserSearchFlags)prm;
            int result = WxWebBrowserHandlerApi.WebBrowser_Find_(NativePointer, text, (int)flags);
            return result;
        }

        public void Print() => NativeControl.Print();

        public void RemoveAllUserScripts()
        {
            NativeControl.RemoveAllUserScripts();
        }

        public bool AddScriptMessageHandler(string name)
        {
            return NativeControl.AddScriptMessageHandler(name);
        }

        public bool RemoveScriptMessageHandler(string name)
        {
            return NativeControl.RemoveScriptMessageHandler(name);
        }

        public bool AddUserScript(string javascript, bool injectDocStart)
        {
            const int wxWEBVIEW_INJECT_AT_DOCUMENT_START = 0;
            const int wxWEBVIEW_INJECT_AT_DOCUMENT_END = 1;

            var injectionTime = wxWEBVIEW_INJECT_AT_DOCUMENT_END;

            if (injectDocStart)
                injectionTime = wxWEBVIEW_INJECT_AT_DOCUMENT_START;

            return NativeControl.AddUserScript(javascript, injectionTime);
        }

        internal static bool IsBackendIEAvailable()
            => WxWebBrowserHandlerApi.WebBrowser_IsBackendIEAvailable_();

        internal static bool IsBackendEdgeAvailable()
            => WxWebBrowserHandlerApi.WebBrowser_IsBackendEdgeAvailable_();

        internal static bool IsBackendWebKitAvailable()
            => WxWebBrowserHandlerApi.WebBrowser_IsBackendWebKitAvailable_();

        internal override Native.Control CreateNativeControl()
        {
            var browser = new NativeWebBrowser(Control?.DefaultUrl ?? WebBrowser.AboutBlankUrl);
            return browser;
        }
 
        private static void ArgsToDoCommandParams(
           object?[] args,
           out string? cmdParam1,
           out string? cmdParam2)
        {
            cmdParam1 = string.Empty;
            cmdParam2 = string.Empty;
            if (args.Length > 0 && args[0] != null)
                cmdParam1 = args[0]?.ToString();
            if (args.Length > 1 && args[1] != null)
                cmdParam2 = args[1]?.ToString();
        }

        internal class NativeWebBrowser : Native.WebBrowser
        {
            public NativeWebBrowser(string url)
                 : base()
            {
                SetNativePointer(NativeApi.WebBrowser_CreateWebBrowser_(url));
            }
        }
    }
}