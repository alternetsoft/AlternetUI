// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class PageSetupDialog : NativeObject
    {
        static PageSetupDialog()
        {
        }
        
        public PageSetupDialog()
        {
            SetNativePointer(NativeApi.PageSetupDialog_Create_());
        }
        
        public PageSetupDialog(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public PrintDocument? Document
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.PageSetupDialog_GetDocument_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<PrintDocument>(_nnn, p => new PrintDocument(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PageSetupDialog_SetDocument_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public Alternet.UI.Thickness MinMargins
        {
            get
            {
                CheckDisposed();
                return NativeApi.PageSetupDialog_GetMinMargins_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PageSetupDialog_SetMinMargins_(NativePointer, value);
            }
        }
        
        public bool MinMarginsValueSet
        {
            get
            {
                CheckDisposed();
                return NativeApi.PageSetupDialog_GetMinMarginsValueSet_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PageSetupDialog_SetMinMarginsValueSet_(NativePointer, value);
            }
        }
        
        public bool AllowMargins
        {
            get
            {
                CheckDisposed();
                return NativeApi.PageSetupDialog_GetAllowMargins_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PageSetupDialog_SetAllowMargins_(NativePointer, value);
            }
        }
        
        public bool AllowOrientation
        {
            get
            {
                CheckDisposed();
                return NativeApi.PageSetupDialog_GetAllowOrientation_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PageSetupDialog_SetAllowOrientation_(NativePointer, value);
            }
        }
        
        public bool AllowPaper
        {
            get
            {
                CheckDisposed();
                return NativeApi.PageSetupDialog_GetAllowPaper_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PageSetupDialog_SetAllowPaper_(NativePointer, value);
            }
        }
        
        public bool AllowPrinter
        {
            get
            {
                CheckDisposed();
                return NativeApi.PageSetupDialog_GetAllowPrinter_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PageSetupDialog_SetAllowPrinter_(NativePointer, value);
            }
        }
        
        public ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            return NativeApi.PageSetupDialog_ShowModal_(NativePointer, owner?.NativePointer ?? IntPtr.Zero);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PageSetupDialog_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PageSetupDialog_GetDocument_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PageSetupDialog_SetDocument_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.Thickness PageSetupDialog_GetMinMargins_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PageSetupDialog_SetMinMargins_(IntPtr obj, Alternet.UI.Thickness value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PageSetupDialog_GetMinMarginsValueSet_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PageSetupDialog_SetMinMarginsValueSet_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PageSetupDialog_GetAllowMargins_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PageSetupDialog_SetAllowMargins_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PageSetupDialog_GetAllowOrientation_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PageSetupDialog_SetAllowOrientation_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PageSetupDialog_GetAllowPaper_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PageSetupDialog_SetAllowPaper_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PageSetupDialog_GetAllowPrinter_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PageSetupDialog_SetAllowPrinter_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ModalResult PageSetupDialog_ShowModal_(IntPtr obj, IntPtr owner);
            
        }
    }
}
