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
            SetEventCallback();
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
        
        public ListViewSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetSelectionMode(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetSelectionMode(NativePointer, value);
            }
        }
        
        public System.Int32[] SelectedIndices
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.ListView_OpenSelectedIndicesArray(NativePointer);
                try
                {
                    var count = NativeApi.ListView_GetSelectedIndicesItemCount(NativePointer, array);
                    var result = new System.Collections.Generic.List<int>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var item = NativeApi.ListView_GetSelectedIndicesItemAt(NativePointer, array, i);
                        result.Add(item);
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.ListView_CloseSelectedIndicesArray(NativePointer, array);
                }
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
        
        public void ClearSelected()
        {
            CheckDisposed();
            NativeApi.ListView_ClearSelected(NativePointer);
        }
        
        public void SetSelected(int index, bool value)
        {
            CheckDisposed();
            NativeApi.ListView_SetSelected(NativePointer, index, value);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ListViewEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<ListView>(obj, p => new ListView(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ListView_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ListViewEvent e)
        {
            switch (e)
            {
                case NativeApi.ListViewEvent.SelectionChanged:
                SelectionChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected ListViewEvent value: " + e);
            }
        }
        
        public event EventHandler? SelectionChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ListViewEventCallbackType(IntPtr obj, ListViewEvent e, IntPtr param);
            
            public enum ListViewEvent
            {
                SelectionChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetEventCallback(ListViewEventCallbackType callback);
            
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
            public static extern ListViewSelectionMode ListView_GetSelectionMode(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetSelectionMode(IntPtr obj, ListViewSelectionMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ListView_OpenSelectedIndicesArray(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListView_GetSelectedIndicesItemCount(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListView_GetSelectedIndicesItemAt(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_CloseSelectedIndicesArray(IntPtr obj, System.IntPtr array);
            
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
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_ClearSelected(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetSelected(IntPtr obj, int index, bool value);
            
        }
    }
}
