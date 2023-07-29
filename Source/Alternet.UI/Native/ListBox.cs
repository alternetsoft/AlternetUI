// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ListBox : Control
    {
        static ListBox()
        {
            SetEventCallback();
        }
        
        public ListBox()
        {
            SetNativePointer(NativeApi.ListBox_Create_());
        }
        
        public ListBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.ListBox_GetHasBorder_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListBox_SetHasBorder_(NativePointer, value);
            }
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.ListBox_GetItemsCount_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public ListBoxSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.ListBox_GetSelectionMode_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListBox_SetSelectionMode_(NativePointer, value);
            }
        }
        
        public System.Int32[] SelectedIndices
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.ListBox_OpenSelectedIndicesArray_(NativePointer);
                try
                {
                    var count = NativeApi.ListBox_GetSelectedIndicesItemCount_(NativePointer, array);
                    var result = new System.Collections.Generic.List<int>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var n = NativeApi.ListBox_GetSelectedIndicesItemAt_(NativePointer, array, i);
                        var item = n;
                        result.Add(item);
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.ListBox_CloseSelectedIndicesArray_(NativePointer, array);
                }
            }
            
        }
        
        public static System.IntPtr CreateEx(long styles)
        {
            var n = NativeApi.ListBox_CreateEx_(styles);
            var m = n;
            return m;
        }
        
        public void InsertItem(int index, string value)
        {
            CheckDisposed();
            NativeApi.ListBox_InsertItem_(NativePointer, index, value);
        }
        
        public void RemoveItemAt(int index)
        {
            CheckDisposed();
            NativeApi.ListBox_RemoveItemAt_(NativePointer, index);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.ListBox_ClearItems_(NativePointer);
        }
        
        public void ClearSelected()
        {
            CheckDisposed();
            NativeApi.ListBox_ClearSelected_(NativePointer);
        }
        
        public void SetSelected(int index, bool value)
        {
            CheckDisposed();
            NativeApi.ListBox_SetSelected_(NativePointer, index, value);
        }
        
        public void EnsureVisible(int itemIndex)
        {
            CheckDisposed();
            NativeApi.ListBox_EnsureVisible_(NativePointer, itemIndex);
        }
        
        public int ItemHitTest(Alternet.Drawing.Point position)
        {
            CheckDisposed();
            var n = NativeApi.ListBox_ItemHitTest_(NativePointer, position);
            var m = n;
            return m;
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ListBoxEventCallbackType((obj, e, parameter) =>
                UI.Application.HandleThreadExceptions(() =>
                {
                    var w = NativeObject.GetFromNativePointer<ListBox>(obj, p => new ListBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                ));
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ListBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ListBoxEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.ListBoxEvent.SelectionChanged:
                {
                    SelectionChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                case NativeApi.ListBoxEvent.ControlRecreated:
                {
                    ControlRecreated?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected ListBoxEvent value: " + e);
            }
        }
        
        public event EventHandler? SelectionChanged;
        public event EventHandler? ControlRecreated;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ListBoxEventCallbackType(IntPtr obj, ListBoxEvent e, IntPtr param);
            
            public enum ListBoxEvent
            {
                SelectionChanged,
                ControlRecreated,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_SetEventCallback_(ListBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ListBox_GetHasBorder_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_SetHasBorder_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_GetItemsCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListBoxSelectionMode ListBox_GetSelectionMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_SetSelectionMode_(IntPtr obj, ListBoxSelectionMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ListBox_OpenSelectedIndicesArray_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_GetSelectedIndicesItemCount_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_GetSelectedIndicesItemAt_(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_CloseSelectedIndicesArray_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ListBox_CreateEx_(long styles);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_InsertItem_(IntPtr obj, int index, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_RemoveItemAt_(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_ClearItems_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_ClearSelected_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_SetSelected_(IntPtr obj, int index, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListBox_EnsureVisible_(IntPtr obj, int itemIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListBox_ItemHitTest_(IntPtr obj, Alternet.Drawing.Point position);
            
        }
    }
}
