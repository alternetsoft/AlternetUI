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
            SetEventCallback();
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
        
        public ListBoxSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListBox_GetSelectionMode(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListBox_SetSelectionMode(NativePointer, value);
            }
        }
        
        public int SelectedIndicesCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListBox_GetSelectedIndicesCount(NativePointer);
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
        
        public int GetSelectedIndexAt(int index)
        {
            CheckDisposed();
            return NativeApi.ListBox_GetSelectedIndexAt(NativePointer, index);
        }
        
        public void ClearSelected()
        {
            CheckDisposed();
            NativeApi.ListBox_ClearSelected(NativePointer);
        }
        
        public void SetSelected(int index, bool value)
        {
            CheckDisposed();
            NativeApi.ListBox_SetSelected(NativePointer, index, value);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ListBoxEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<ListBox>(obj, p => new ListBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ListBox_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ListBoxEvent e)
        {
            switch (e)
            {
                case NativeApi.ListBoxEvent.SelectionChanged:
                SelectionChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected ListBoxEvent value: " + e);
            }
        }
        
        public event EventHandler? SelectionChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ListBoxEventCallbackType(IntPtr obj, ListBoxEvent e, IntPtr param);
            
            public enum ListBoxEvent
            {
                SelectionChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_SetEventCallback(ListBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListBox_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_GetItemsCount(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListBoxSelectionMode ListBox_GetSelectionMode(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_SetSelectionMode(IntPtr obj, ListBoxSelectionMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_GetSelectedIndicesCount(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_InsertItem(IntPtr obj, int index, string? value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_RemoveItemAt(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_ClearItems(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_GetSelectedIndexAt(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_ClearSelected(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_SetSelected(IntPtr obj, int index, bool value);
            
        }
    }
}
