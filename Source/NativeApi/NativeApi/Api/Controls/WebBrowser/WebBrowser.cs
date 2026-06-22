#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    public class WebBrowser : Control
    {
        public int Find(NativeStringSpan text, int flags) => default;

        public static void CrtSetDbgFlag(int value) => throw new Exception();

        public int GetBackend() => default;

        public void SetZoomType(int zoomType) => throw new Exception();

        public bool CanSetZoomType(int type) => default;

        internal static NativeStringSpan GetBackendVersionString(int backend) => default;

        internal static bool IsBackendIEAvailable() => default;

        internal static bool IsBackendEdgeAvailable() => default;

        internal static bool IsBackendWebKitAvailable() => default;

        internal static void SetBackend(int value) {}

        internal static NativeStringSpan GetLibraryVersionString() => default;

        public void SetDefaultPage(NativeStringSpan value) { }

        public int GetZoomType() => default;

        public static IntPtr CreateWebBrowser(NativeStringSpan url) => default;

        public static bool IsEdgeBackendEnabled { get; set; }

        public bool HasBorder { get; set; }

        public static void SetDefaultUserAgent(NativeStringSpan value) => 
            throw new Exception();
        public static void SetDefaultScriptMesageName(NativeStringSpan value) 
            => throw new Exception();
        public static void SetDefaultFSNameMemory(NativeStringSpan value) => 
            throw new Exception();
        public static void SetDefaultFSNameArchive(NativeStringSpan value) => 
            throw new Exception();

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
        public NativeStringSpan GetSelectedSource() => throw new Exception();
        public NativeStringSpan GetSelectedText() => throw new Exception();
        public NativeStringSpan GetPageSource() => throw new Exception();
        public NativeStringSpan GetPageText() => throw new Exception();

        public void SetSelectedSource(NativeStringSpan value) => throw new Exception();
        public void SetSelectedText(NativeStringSpan value) => throw new Exception();
        public void SetPageSource(NativeStringSpan value) => throw new Exception();
        public void SetPageText(NativeStringSpan value) => throw new Exception();

        public bool AccessToDevToolsEnabled { get; set; }
        public int PreferredColorScheme { get; set; }
        
        public NativeStringSpan GetUserAgent() => throw new Exception();
        public void SetUserAgent(NativeStringSpan value) => throw new Exception();

        public bool ContextMenuEnabled { get; set; }

        public NativeStringSpan DoCommand(NativeStringSpan cmdName, NativeStringSpan cmdParam1, 
            NativeStringSpan cmdParam2) => throw new Exception();
        public static NativeStringSpan DoCommandGlobal(NativeStringSpan cmdName, NativeStringSpan cmdParam1, 
            NativeStringSpan cmdParam2) => throw new Exception();

        public void SetVirtualHostNameToFolderMapping(
            NativeStringSpan hostName, 
            NativeStringSpan folderPath, 
            int accessKind) => throw new Exception();

        public IntPtr GetNativeBackend() => throw new Exception();
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
        public bool AddScriptMessageHandler(NativeStringSpan name) => throw new Exception();
        public bool RemoveScriptMessageHandler(NativeStringSpan name) => throw new Exception();
        public void RunScriptAsync(NativeStringSpan javascript, IntPtr clientData) 
            => throw new Exception();
        public void CreateBackend() => throw new Exception();

        public static int GetBackendOS() => throw new Exception();
        public static void SetEdgePath(NativeStringSpan path) => throw new Exception();

        public bool Editable { get; set; }
        public int Zoom { get; set; }

        public bool IsEdge { get; }

        public NativeStringSpan GetCurrentTitle() => throw new Exception();
        public NativeStringSpan GetCurrentURL() => throw new Exception();
        public void LoadURL(NativeStringSpan url) => throw new Exception();
        public NativeStringSpan RunScript(NativeStringSpan javascript) => throw new Exception();
        public void SetPage(NativeStringSpan text, NativeStringSpan baseUrl) => throw new Exception();
        public bool AddUserScript(NativeStringSpan javascript, int injectionTime) 
            => throw new Exception();

        public event NativeEventHandler<WebBrowserEventData>? Navigating;
        public event NativeEventHandler<WebBrowserEventData>? Navigated;
        public event NativeEventHandler<WebBrowserEventData>? Loaded;
        public event NativeEventHandler<WebBrowserEventData>? Error;
        public event NativeEventHandler<WebBrowserEventData>? NewWindow;
        public event NativeEventHandler<WebBrowserEventData>? TitleChanged;
        public event NativeEventHandler<WebBrowserEventData>? FullScreenChanged;
        public event NativeEventHandler<WebBrowserEventData>? ScriptMessageReceived;
        public event NativeEventHandler<WebBrowserEventData>? ScriptResult;
        public event NativeEventHandler<WebBrowserEventData>? BeforeBrowserCreate; 
    }
}
