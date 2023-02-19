// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class PrintDialog : NativeObject
    {
        static PrintDialog()
        {
        }
        
        public PrintDialog()
        {
            SetNativePointer(NativeApi.PrintDialog_Create_());
        }
        
        public PrintDialog(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool AllowSomePages
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDialog_GetAllowSomePages_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDialog_SetAllowSomePages_(NativePointer, value);
            }
        }
        
        public bool AllowSelection
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDialog_GetAllowSelection_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDialog_SetAllowSelection_(NativePointer, value);
            }
        }
        
        public bool AllowPrintToFile
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDialog_GetAllowPrintToFile_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDialog_SetAllowPrintToFile_(NativePointer, value);
            }
        }
        
        public bool ShowHelp
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDialog_GetShowHelp_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDialog_SetShowHelp_(NativePointer, value);
            }
        }
        
        public PrintDocument? Document
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDialog_GetDocument_(NativePointer);
                var m = NativeObject.GetFromNativePointer<PrintDocument>(n, p => new PrintDocument(p));
                ReleaseNativeObjectPointer(n);
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDialog_SetDocument_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            var n = NativeApi.PrintDialog_ShowModal_(NativePointer, owner?.NativePointer ?? IntPtr.Zero);
            var m = n;
            return m;
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintDialog_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PrintDialog_GetAllowSomePages_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDialog_SetAllowSomePages_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PrintDialog_GetAllowSelection_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDialog_SetAllowSelection_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PrintDialog_GetAllowPrintToFile_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDialog_SetAllowPrintToFile_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PrintDialog_GetShowHelp_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDialog_SetShowHelp_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintDialog_GetDocument_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDialog_SetDocument_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ModalResult PrintDialog_ShowModal_(IntPtr obj, IntPtr owner);
            
        }
    }
}
