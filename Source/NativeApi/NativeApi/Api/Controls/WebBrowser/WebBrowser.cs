﻿#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    public class WebBrowser : Control
    {
        public static IntPtr CreateWebBrowser(string url) => default;

        public static bool IsEdgeBackendEnabled { get; set; }

        public bool HasBorder { get; set; }

        public static void SetDefaultUserAgent(string value) => 
            throw new Exception();
        public static void SetDefaultScriptMesageName(string value) 
            => throw new Exception();
        public static void SetDefaultFSNameMemory(string value) => 
            throw new Exception();
        public static void SetDefaultFSNameArchive(string value) => 
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
        public string SelectedSource { get => throw new Exception(); }
        public string SelectedText { get => throw new Exception(); }
        public string PageSource { get => throw new Exception(); }
        public string PageText { get => throw new Exception(); }
        public bool AccessToDevToolsEnabled { get; set; }
        public int PreferredColorScheme { get; set; }
        
        public string UserAgent 
        { 
            get => throw new Exception(); 
            set => throw new Exception(); 
        }
        public bool ContextMenuEnabled { get; set; }

        public string DoCommand(string cmdName, string cmdParam1, 
            string cmdParam2) => throw new Exception();
        public static string DoCommandGlobal(string cmdName, string cmdParam1, 
            string cmdParam2) => throw new Exception();

        public void SetVirtualHostNameToFolderMapping(
            string hostName, 
            string folderPath, 
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
        public bool AddScriptMessageHandler(string name) => throw new Exception();
        public bool RemoveScriptMessageHandler(string name) => throw new Exception();
        public void RunScriptAsync(string javascript, IntPtr clientData) 
            => throw new Exception();
        public void CreateBackend() => throw new Exception();

        public static int GetBackendOS() => throw new Exception();
        public static void SetEdgePath(string path) => throw new Exception();

        public bool Editable { get; set; }
        public int Zoom { get; set; }

        public bool IsEdge { get; }

        public string GetCurrentTitle() => throw new Exception();
        public string GetCurrentURL() => throw new Exception();
        public void LoadURL(string url) => throw new Exception();
        public string RunScript(string javascript) => throw new Exception();
        public void SetPage(string text, string baseUrl) => throw new Exception();
        public bool AddUserScript(string javascript, int injectionTime) 
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
