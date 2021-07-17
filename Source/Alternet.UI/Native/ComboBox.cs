// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ComboBox : Control
    {
        public ComboBox()
        {
            SetNativePointer(NativeApi.ComboBox_Create_());
            SetEventCallback();
        }
        
        public ComboBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetItemsCount(NativePointer);
            }
            
        }
        
        public bool IsEditable
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetIsEditable(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetIsEditable(NativePointer, value);
            }
        }
        
        public int SelectedIndex
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetSelectedIndex(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetSelectedIndex(NativePointer, value);
            }
        }
        
        public string Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetText(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetText(NativePointer, value);
            }
        }
        
        public void InsertItem(int index, string value)
        {
            CheckDisposed();
            NativeApi.ComboBox_InsertItem(NativePointer, index, value);
        }
        
        public void RemoveItemAt(int index)
        {
            CheckDisposed();
            NativeApi.ComboBox_RemoveItemAt(NativePointer, index);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.ComboBox_ClearItems(NativePointer);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ComboBoxEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<ComboBox>(obj, p => new ComboBox(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ComboBox_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ComboBoxEvent e)
        {
            switch (e)
            {
                case NativeApi.ComboBoxEvent.SelectedItemChanged:
                SelectedItemChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                case NativeApi.ComboBoxEvent.TextChanged:
                TextChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected ComboBoxEvent value: " + e);
            }
        }
        
        public event EventHandler? SelectedItemChanged;
        public event EventHandler? TextChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ComboBoxEventCallbackType(IntPtr obj, ComboBoxEvent e, IntPtr param);
            
            public enum ComboBoxEvent
            {
                SelectedItemChanged,
                TextChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetEventCallback(ComboBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ComboBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetItemsCount(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ComboBox_GetIsEditable(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetIsEditable(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetSelectedIndex(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetSelectedIndex(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string ComboBox_GetText(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetText(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_InsertItem(IntPtr obj, int index, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_RemoveItemAt(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_ClearItems(IntPtr obj);
            
        }
    }
}
