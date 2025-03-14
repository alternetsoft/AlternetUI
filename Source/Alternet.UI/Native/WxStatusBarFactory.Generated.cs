// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class WxStatusBarFactory : NativeObject
    {
        static WxStatusBarFactory()
        {
        }
        
        public WxStatusBarFactory()
        {
            SetNativePointer(NativeApi.WxStatusBarFactory_Create_());
        }
        
        public WxStatusBarFactory(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public static void DeleteStatusBar(System.IntPtr handle)
        {
            NativeApi.WxStatusBarFactory_DeleteStatusBar_(handle);
        }
        
        public static System.IntPtr CreateStatusBar(System.IntPtr window, long style)
        {
            return NativeApi.WxStatusBarFactory_CreateStatusBar_(window, style);
        }
        
        public static int GetFieldsCount(System.IntPtr handle)
        {
            return NativeApi.WxStatusBarFactory_GetFieldsCount_(handle);
        }
        
        public static void SetStatusText(System.IntPtr handle, string text, int number)
        {
            NativeApi.WxStatusBarFactory_SetStatusText_(handle, text, number);
        }
        
        public static string GetStatusText(System.IntPtr handle, int number)
        {
            return NativeApi.WxStatusBarFactory_GetStatusText_(handle, number);
        }
        
        public static void PushStatusText(System.IntPtr handle, string text, int number)
        {
            NativeApi.WxStatusBarFactory_PushStatusText_(handle, text, number);
        }
        
        public static void PopStatusText(System.IntPtr handle, int number)
        {
            NativeApi.WxStatusBarFactory_PopStatusText_(handle, number);
        }
        
        public static void SetStatusWidths(System.IntPtr handle, System.Int32[] widths)
        {
            NativeApi.WxStatusBarFactory_SetStatusWidths_(handle, widths, widths.Length);
        }
        
        public static void SetFieldsCount(System.IntPtr handle, int number)
        {
            NativeApi.WxStatusBarFactory_SetFieldsCount_(handle, number);
        }
        
        public static int GetStatusWidth(System.IntPtr handle, int n)
        {
            return NativeApi.WxStatusBarFactory_GetStatusWidth_(handle, n);
        }
        
        public static int GetStatusStyle(System.IntPtr handle, int n)
        {
            return NativeApi.WxStatusBarFactory_GetStatusStyle_(handle, n);
        }
        
        public static void SetStatusStyles(System.IntPtr handle, System.Int32[] styles)
        {
            NativeApi.WxStatusBarFactory_SetStatusStyles_(handle, styles, styles.Length);
        }
        
        public static Alternet.Drawing.RectI GetFieldRect(System.IntPtr handle, int i)
        {
            return NativeApi.WxStatusBarFactory_GetFieldRect_(handle, i);
        }
        
        public static void SetMinHeight(System.IntPtr handle, int height)
        {
            NativeApi.WxStatusBarFactory_SetMinHeight_(handle, height);
        }
        
        public static int GetBorderX(System.IntPtr handle)
        {
            return NativeApi.WxStatusBarFactory_GetBorderX_(handle);
        }
        
        public static int GetBorderY(System.IntPtr handle)
        {
            return NativeApi.WxStatusBarFactory_GetBorderY_(handle);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr WxStatusBarFactory_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_DeleteStatusBar_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr WxStatusBarFactory_CreateStatusBar_(System.IntPtr window, long style);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WxStatusBarFactory_GetFieldsCount_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_SetStatusText_(System.IntPtr handle, string text, int number);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string WxStatusBarFactory_GetStatusText_(System.IntPtr handle, int number);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_PushStatusText_(System.IntPtr handle, string text, int number);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_PopStatusText_(System.IntPtr handle, int number);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_SetStatusWidths_(System.IntPtr handle, System.Int32[] widths, int widthsCount);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_SetFieldsCount_(System.IntPtr handle, int number);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WxStatusBarFactory_GetStatusWidth_(System.IntPtr handle, int n);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WxStatusBarFactory_GetStatusStyle_(System.IntPtr handle, int n);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_SetStatusStyles_(System.IntPtr handle, System.Int32[] styles, int stylesCount);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectI WxStatusBarFactory_GetFieldRect_(System.IntPtr handle, int i);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void WxStatusBarFactory_SetMinHeight_(System.IntPtr handle, int height);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WxStatusBarFactory_GetBorderX_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int WxStatusBarFactory_GetBorderY_(System.IntPtr handle);
            
        }
    }
}
