// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class TreeView : Control
    {
        public TreeView()
        {
            SetNativePointer(NativeApi.TreeView_Create_());
            SetEventCallback();
        }
        
        public TreeView(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public ImageList? ImageList
        {
            get
            {
                CheckDisposed();
                return NativeObject.GetFromNativePointer<ImageList>(NativeApi.TreeView_GetImageList(NativePointer), p => new ImageList(p));
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetImageList(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public System.IntPtr RootItem
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetRootItem(NativePointer);
            }
            
        }
        
        public TreeViewSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetSelectionMode(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetSelectionMode(NativePointer, value);
            }
        }
        
        public System.IntPtr[] SelectedItems
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.TreeView_OpenSelectedItemsArray(NativePointer);
                try
                {
                    var count = NativeApi.TreeView_GetSelectedItemsItemCount(NativePointer, array);
                    var result = new System.Collections.Generic.List<System.IntPtr>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var item = NativeApi.TreeView_GetSelectedItemsItemAt(NativePointer, array, i);
                        result.Add(item);
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.TreeView_CloseSelectedItemsArray(NativePointer, array);
                }
            }
            
        }
        
        public int GetItemCount(System.IntPtr parentItem)
        {
            CheckDisposed();
            return NativeApi.TreeView_GetItemCount(NativePointer, parentItem);
        }
        
        public void InsertItemAt(System.IntPtr parentItem, int index, string text, int imageIndex)
        {
            CheckDisposed();
            NativeApi.TreeView_InsertItemAt(NativePointer, parentItem, index, text, imageIndex);
        }
        
        public void RemoveItem(System.IntPtr item)
        {
            CheckDisposed();
            NativeApi.TreeView_RemoveItem(NativePointer, item);
        }
        
        public void ClearItems(System.IntPtr parentItem)
        {
            CheckDisposed();
            NativeApi.TreeView_ClearItems(NativePointer, parentItem);
        }
        
        public void ClearSelected()
        {
            CheckDisposed();
            NativeApi.TreeView_ClearSelected(NativePointer);
        }
        
        public void SetSelected(System.IntPtr item, bool value)
        {
            CheckDisposed();
            NativeApi.TreeView_SetSelected(NativePointer, item, value);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TreeViewEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<TreeView>(obj, p => new TreeView(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.TreeView_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.TreeViewEvent e)
        {
            switch (e)
            {
                case NativeApi.TreeViewEvent.SelectionChanged:
                SelectionChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected TreeViewEvent value: " + e);
            }
        }
        
        public event EventHandler? SelectionChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr TreeViewEventCallbackType(IntPtr obj, TreeViewEvent e, IntPtr param);
            
            public enum TreeViewEvent
            {
                SelectionChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetEventCallback(TreeViewEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TreeView_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TreeView_GetImageList(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetImageList(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetRootItem(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern TreeViewSelectionMode TreeView_GetSelectionMode(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetSelectionMode(IntPtr obj, TreeViewSelectionMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_OpenSelectedItemsArray(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TreeView_GetSelectedItemsItemCount(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetSelectedItemsItemAt(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_CloseSelectedItemsArray(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TreeView_GetItemCount(IntPtr obj, System.IntPtr parentItem);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_InsertItemAt(IntPtr obj, System.IntPtr parentItem, int index, string text, int imageIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_RemoveItem(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ClearItems(IntPtr obj, System.IntPtr parentItem);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ClearSelected(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetSelected(IntPtr obj, System.IntPtr item, bool value);
            
        }
    }
}
