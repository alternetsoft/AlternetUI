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
            get => NativeControl.GetUserAgent();
            set
            {
                NativeStringSpan.Invoke(value, span =>
                {
                    NativeControl.SetUserAgent(span);
                });
            }
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
                int v = NativeControl.GetBackend();
                return (WebBrowserBackend)Enum.ToObject(typeof(WebBrowserBackend), v);
            }
        }

        public bool CanGoBack => NativeControl.CanGoBack;

        public bool CanGoForward => NativeControl.CanGoForward;

        public bool HasSelection => NativeControl.HasSelection;

        public string SelectedText => NativeControl.GetSelectedText();

        public string SelectedSource => NativeControl.GetSelectedSource();

        public bool CanCut => NativeControl.CanCut;

        public bool CanCopy => NativeControl.CanCopy;

        public bool CanPaste => NativeControl.CanPaste;

        public bool CanUndo => NativeControl.CanUndo;

        public bool CanRedo => NativeControl.CanRedo;

        public bool IsBusy => NativeControl.IsBusy;

        public string PageSource => NativeControl.GetPageSource();

        public string PageText => NativeControl.GetPageText();

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
                var zoomType = NativeControl.GetZoomType();
                WebBrowserZoomType result =
                    (WebBrowserZoomType)Enum.ToObject(typeof(WebBrowserZoomType), zoomType);
                return result;
            }

            set
            {
                if (NativeControl.CanSetZoomType((int)value))
                    NativeControl.SetZoomType((int)value);
            }
        }

        private IntPtr NativePointer => NativeControl.NativePointer;

        public static string? DoCommandGlobal(string cmdName, params object?[] args)
        {
            ArgsToDoCommandParams(args, out string? cmdParam1, out string? cmdParam2);
            return Native.WebBrowser.DoCommandGlobal(cmdName, cmdParam1!, cmdParam2!);
        }

        public void LoadURL(string url)
        {
            NativeStringSpan.Invoke(url, urlSpan => NativeControl.LoadURL(urlSpan));
        }

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

            result = NativeStringSpan.InvokeWithResult(javascript, span => NativeControl.RunScript(span));

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
            NativeStringSpan.Invoke(hostName, folderPath, (hostSpan, folderSpan) =>
            {
                NativeControl.SetVirtualHostNameToFolderMapping(
                    hostSpan,
                    folderSpan,
                    (int)accessKind);
            });
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

            NativeStringSpan.Invoke(javascript, span =>
            {
                if (clientData == null)
                    NativeControl.RunScriptAsync(span, IntPtr.Zero);
                else
                    NativeControl.RunScriptAsync(span, (IntPtr)clientData);
            });
        }

        public void NavigateToString(string html, string? baseUrl = null)
        {
            NativeStringSpan.Invoke(html, baseUrl, (htmlSpan, baseUrlSpan) =>
            {
                NativeControl.SetPage(htmlSpan, baseUrlSpan);
            });
        }

        public bool CanSetZoomType(WebBrowserZoomType value)
        {
            return NativeControl.CanSetZoomType((int)value);
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

            int result = NativeStringSpan.InvokeWithResult(text, (span) =>
            {
                return NativeControl.Find(span, (int)flags);
            });

            return result;
        }

        public void Print() => NativeControl.Print();

        public void RemoveAllUserScripts()
        {
            NativeControl.RemoveAllUserScripts();
        }

        public bool AddScriptMessageHandler(string name)
        {
            return NativeStringSpan.InvokeWithResult(name, (span) =>
            {
                return NativeControl.AddScriptMessageHandler(span);
            });
        }

        public bool RemoveScriptMessageHandler(string name)
        {
            return NativeStringSpan.InvokeWithResult(name, (span) =>
            {
                return NativeControl.RemoveScriptMessageHandler(span);
            });
        }

        public bool AddUserScript(string javascript, bool injectDocStart)
        {
            const int wxWEBVIEW_INJECT_AT_DOCUMENT_START = 0;
            const int wxWEBVIEW_INJECT_AT_DOCUMENT_END = 1;

            var injectionTime = wxWEBVIEW_INJECT_AT_DOCUMENT_END;

            if (injectDocStart)
                injectionTime = wxWEBVIEW_INJECT_AT_DOCUMENT_START;

            return NativeStringSpan.InvokeWithResult(javascript, (span) =>
            {
                return NativeControl.AddUserScript(span, injectionTime);
            });
        }

        internal static bool IsBackendIEAvailable()
            => Native.WebBrowser.IsBackendIEAvailable();

        internal static bool IsBackendEdgeAvailable()
            => Native.WebBrowser.IsBackendEdgeAvailable();

        internal static bool IsBackendWebKitAvailable()
            => Native.WebBrowser.IsBackendWebKitAvailable();

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
                NativeStringSpan.Invoke(url, urlSpan =>
                {
                    SetNativePointer(Native.WebBrowser.CreateWebBrowser(urlSpan));
                });
            }
        }
    }
}