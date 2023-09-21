// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class MainMenu : Control
    {
        static MainMenu()
        {
        }
        
        public MainMenu()
        {
            SetNativePointer(NativeApi.MainMenu_Create_());
        }
        
        public MainMenu(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.MainMenu_GetItemsCount_(NativePointer);
            }
            
        }
        
        public void InsertItemAt(int index, Menu menu, string text)
        {
            CheckDisposed();
            NativeApi.MainMenu_InsertItemAt_(NativePointer, index, menu.NativePointer, text);
        }
        
        public void RemoveItemAt(int index)
        {
            CheckDisposed();
            NativeApi.MainMenu_RemoveItemAt_(NativePointer, index);
        }
        
        public void SetItemText(int index, string text)
        {
            CheckDisposed();
            NativeApi.MainMenu_SetItemText_(NativePointer, index, text);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr MainMenu_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int MainMenu_GetItemsCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MainMenu_InsertItemAt_(IntPtr obj, int index, IntPtr menu, string text);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MainMenu_RemoveItemAt_(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MainMenu_SetItemText_(IntPtr obj, int index, string text);
            
        }
    }
}
