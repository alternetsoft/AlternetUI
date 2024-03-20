// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class CheckListBox : ListBox
    {
        static CheckListBox()
        {
            SetEventCallback();
        }
        
        public CheckListBox()
        {
            SetNativePointer(NativeApi.CheckListBox_Create_());
        }
        
        public CheckListBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public System.Int32[] CheckedIndices
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.CheckListBox_OpenCheckedIndicesArray_(NativePointer);
                try
                {
                    var count = NativeApi.CheckListBox_GetCheckedIndicesItemCount_(NativePointer, array);
                    var result = new System.Collections.Generic.List<int>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var n = NativeApi.CheckListBox_GetCheckedIndicesItemAt_(NativePointer, array, i);
                        var item = n;
                        result.Add(item);
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.CheckListBox_CloseCheckedIndicesArray_(NativePointer, array);
                }
            }
            
        }
        
        public void ClearChecked()
        {
            CheckDisposed();
            NativeApi.CheckListBox_ClearChecked_(NativePointer);
        }
        
        public void SetChecked(int index, bool value)
        {
            CheckDisposed();
            NativeApi.CheckListBox_SetChecked_(NativePointer, index, value);
        }
        
        public bool IsChecked(int item)
        {
            CheckDisposed();
            return NativeApi.CheckListBox_IsChecked_(NativePointer, item);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.CheckListBoxEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<CheckListBox>(obj, p => new CheckListBox(p));
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.CheckListBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.CheckListBoxEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.CheckListBoxEvent.SelectionChanged:
                {
                    SelectionChanged?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.CheckListBoxEvent.CheckedChanged:
                {
                    CheckedChanged?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.CheckListBoxEvent.ControlRecreated:
                {
                    ControlRecreated?.Invoke(); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected CheckListBoxEvent value: " + e);
            }
        }
        
        public Action? SelectionChanged;
        public Action? CheckedChanged;
        public Action? ControlRecreated;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr CheckListBoxEventCallbackType(IntPtr obj, CheckListBoxEvent e, IntPtr param);
            
            public enum CheckListBoxEvent
            {
                SelectionChanged,
                CheckedChanged,
                ControlRecreated,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckListBox_SetEventCallback_(CheckListBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CheckListBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr CheckListBox_OpenCheckedIndicesArray_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int CheckListBox_GetCheckedIndicesItemCount_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int CheckListBox_GetCheckedIndicesItemAt_(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckListBox_CloseCheckedIndicesArray_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckListBox_ClearChecked_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void CheckListBox_SetChecked_(IntPtr obj, int index, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool CheckListBox_IsChecked_(IntPtr obj, int item);
            
        }
    }
}
