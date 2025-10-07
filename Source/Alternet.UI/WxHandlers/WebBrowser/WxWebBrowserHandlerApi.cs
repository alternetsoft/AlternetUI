using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Alternet.UI.Native;

namespace Alternet.UI
{
#pragma warning disable
    [SuppressUnmanagedCodeSecurity]
    internal class WxWebBrowserHandlerApi
    {
        private const string ModuleName = Native.NativeApiProvider.NativeModuleName;

        [DllImport(ModuleName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WebBrowser_Find_(IntPtr obj, string text, int flags);

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
    }
}