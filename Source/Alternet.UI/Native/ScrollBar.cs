// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ScrollBar : Control
    {
        static ScrollBar()
        {
        }
        
        public ScrollBar()
        {
            SetNativePointer(NativeApi.ScrollBar_Create_());
        }
        
        public ScrollBar(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool IsVertical
        {
            get
            {
                CheckDisposed();
                return NativeApi.ScrollBar_GetIsVertical_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ScrollBar_SetIsVertical_(NativePointer, value);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ScrollBar_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ScrollBar_GetIsVertical_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ScrollBar_SetIsVertical_(IntPtr obj, bool value);
            
        }
    }
}
