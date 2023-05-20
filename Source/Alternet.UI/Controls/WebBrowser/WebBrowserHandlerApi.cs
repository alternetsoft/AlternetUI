using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using Alternet.UI.Native;

namespace Alternet.UI
{
    //-------------------------------------------------
    internal partial class WebBrowserHandler
    {
        private const string ModuleName = Native.NativeApiProvider.NativeModuleName;

        //-------------------------------------------------
        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long WebBrowser_Find_(IntPtr obj, string text, int flags);

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WebBrowser_CrtSetDbgFlag_(int value);

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WebBrowser_GetBackend_(IntPtr obj);
        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WebBrowser_SetZoomType_(IntPtr obj, int zoomType);

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool WebBrowser_CanSetZoomType_(IntPtr obj, int type);

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern string WebBrowser_GetBackendVersionString_(int backend);

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool WebBrowser_IsBackendIEAvailable_();

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool WebBrowser_IsBackendEdgeAvailable_();

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool WebBrowser_IsBackendWebKitAvailable_();

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WebBrowser_SetBackend_(int value);

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern string WebBrowser_GetLibraryVersionString_();

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WebBrowser_SetDefaultPage_(string value);

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WebBrowser_GetZoomType_(IntPtr obj);

        //-------------------------------------------------
        /*
                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_AddUserScript_(IntPtr obj, string javascript,
                    int injectionTime);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Stop_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_CanGoBack_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_CanGoForward_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_GoBack_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_GoForward_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_ClearHistory_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_EnableHistory_(IntPtr obj, bool enable = true);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_SetPage_(IntPtr obj, string html, string baseUrl);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_ReloadDefault_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Reload_(IntPtr obj, bool noCache);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_SetZoomFactor_(IntPtr obj, float zoom);


                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern float WebBrowser_GetZoomFactor_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_SelectAll_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_HasSelection_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_DeleteSelection_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern string WebBrowser_GetSelectedText_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern string WebBrowser_GetSelectedSource_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_ClearSelection_(IntPtr objj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_CanCut_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_CanCopy_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_CanPaste_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Cut_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Copy_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Paste_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_CanUndo_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_CanRedo_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Undo_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Redo_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_IsBusy_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_IsContextMenuEnabled_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_IsAccessToDevToolsEnabled_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_Print_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_SetUserAgent_(IntPtr obj, string userAgent);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern string WebBrowser_GetPageSource_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern string WebBrowser_GetPageText_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern string WebBrowser_GetUserAgent_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_EnableContextMenu_(IntPtr obj, bool enable);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_EnableAccessToDevTools_(IntPtr obj, bool enable);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern void WebBrowser_RemoveAllUserScripts_(IntPtr obj);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_AddScriptMessageHandler_(IntPtr obj, string name);

                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern bool WebBrowser_RemoveScriptMessageHandler_(IntPtr obj, string name);


                [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
                internal static extern string WebBrowser_RunScript_(IntPtr obj, string javascript);
        */
        //-------------------------------------------------
    }
}
//-------------------------------------------------

