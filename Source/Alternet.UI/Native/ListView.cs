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
            SetNativePointer(NativeApi.ListView_Create_());
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
        
        public ImageList? SmallImageList
        {
            get
            {
                CheckDisposed();
                return NativeObject.GetFromNativePointer<ImageList>(NativeApi.ListView_GetSmallImageList(NativePointer), p => new ImageList(p));
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetSmallImageList(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public ImageList? LargeImageList
        {
            get
            {
                CheckDisposed();
                return NativeObject.GetFromNativePointer<ImageList>(NativeApi.ListView_GetLargeImageList(NativePointer), p => new ImageList(p));
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetLargeImageList(NativePointer, value?.NativePointer ?? IntPtr.Zero);
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
        
        public void InsertItemAt(int index, string text, int columnIndex, int imageIndex)
        {
            CheckDisposed();
            NativeApi.ListView_InsertItemAt(NativePointer, index, text, columnIndex, imageIndex);
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
            public static extern IntPtr ListView_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListView_GetItemsCount(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListView_GetSmallImageList(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetSmallImageList(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListView_GetLargeImageList(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetLargeImageList(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListViewView ListView_GetCurrentView(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetCurrentView(IntPtr obj, ListViewView value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_InsertItemAt(IntPtr obj, int index, string text, int columnIndex, int imageIndex);
            
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
