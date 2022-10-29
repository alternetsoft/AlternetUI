// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class PrintPreviewDialog : NativeObject
    {
        static PrintPreviewDialog()
        {
        }
        
        public PrintPreviewDialog()
        {
            SetNativePointer(NativeApi.PrintPreviewDialog_Create_());
        }
        
        public PrintPreviewDialog(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public PrintDocument? Document
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintPreviewDialog_GetDocument_(NativePointer);
                var m = NativeObject.GetFromNativePointer<PrintDocument>(n, p => new PrintDocument(p));
                ReleaseNativeObjectPointer(n);
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintPreviewDialog_SetDocument_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            var n = NativeApi.PrintPreviewDialog_ShowModal_(NativePointer, owner?.NativePointer ?? IntPtr.Zero);
            var m = n;
            return m;
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintPreviewDialog_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintPreviewDialog_GetDocument_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintPreviewDialog_SetDocument_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ModalResult PrintPreviewDialog_ShowModal_(IntPtr obj, IntPtr owner);
            
        }
    }
}
