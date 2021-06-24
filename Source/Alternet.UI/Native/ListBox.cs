// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ListBox : Control
    {
        public ListBox()
        {
            SetNativePointer(NativeApi.ListBox_Create());
        }
        
        public ListBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListBox_GetItemsCount(NativePointer);
            }
            
        }
        
        public void InsertItem(int index, string? value)
        {
            CheckDisposed();
            NativeApi.ListBox_InsertItem(NativePointer, index, value);
        }
        
        public void RemoveItemAt(int index)
        {
            CheckDisposed();
            NativeApi.ListBox_RemoveItemAt(NativePointer, index);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.ListBox_ClearItems(NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListBox_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_GetItemsCount(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_InsertItem(IntPtr obj, int index, string? value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_RemoveItemAt(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_ClearItems(IntPtr obj);
            
        }
    }
}
