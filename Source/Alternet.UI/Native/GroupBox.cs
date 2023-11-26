// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class GroupBox : Control
    {
        static GroupBox()
        {
        }
        
        public GroupBox()
        {
            SetNativePointer(NativeApi.GroupBox_Create_());
        }
        
        public GroupBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string? Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.GroupBox_GetTitle_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.GroupBox_SetTitle_(NativePointer, value);
            }
        }
        
        public int GetTopBorderForSizer()
        {
            CheckDisposed();
            return NativeApi.GroupBox_GetTopBorderForSizer_(NativePointer);
        }
        
        public int GetOtherBorderForSizer()
        {
            CheckDisposed();
            return NativeApi.GroupBox_GetOtherBorderForSizer_(NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GroupBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? GroupBox_GetTitle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void GroupBox_SetTitle_(IntPtr obj, string? value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GroupBox_GetTopBorderForSizer_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int GroupBox_GetOtherBorderForSizer_(IntPtr obj);
            
        }
    }
}
