// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ListView : Control
    {
        public ListView()
        {
            SetNativePointer(NativeApi.ListView_Create());
        }
        
        public ListView(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetItemsCount(NativePointer);
            }
            
        }
        
        public ListViewView CurrentView
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetCurrentView(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetCurrentView(NativePointer, value);
            }
        }
        
        public void InsertItemAt(int index, string value)
        {
            CheckDisposed();
            NativeApi.ListView_InsertItemAt(NativePointer, index, value);
        }
        
        public void RemoveItemAt(int index)
        {
            CheckDisposed();
            NativeApi.ListView_RemoveItemAt(NativePointer, index);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.ListView_ClearItems(NativePointer);
        }
        
        public void InsertColumnAt(int index, string header)
        {
            CheckDisposed();
            NativeApi.ListView_InsertColumnAt(NativePointer, index, header);
        }
        
        public void RemoveColumnAt(int index)
        {
            CheckDisposed();
            NativeApi.ListView_RemoveColumnAt(NativePointer, index);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListView_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListView_GetItemsCount(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListViewView ListView_GetCurrentView(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetCurrentView(IntPtr obj, ListViewView value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_InsertItemAt(IntPtr obj, int index, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_RemoveItemAt(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_ClearItems(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_InsertColumnAt(IntPtr obj, int index, string header);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_RemoveColumnAt(IntPtr obj, int index);
            
        }
    }
}
