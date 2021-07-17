// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class NumericUpDown : Control
    {
        public NumericUpDown()
        {
            SetNativePointer(NativeApi.NumericUpDown_Create_());
            SetEventCallback();
        }
        
        public NumericUpDown(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int Minimum
        {
            get
            {
                CheckDisposed();
                return NativeApi.NumericUpDown_GetMinimum(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.NumericUpDown_SetMinimum(NativePointer, value);
            }
        }
        
        public int Maximum
        {
            get
            {
                CheckDisposed();
                return NativeApi.NumericUpDown_GetMaximum(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.NumericUpDown_SetMaximum(NativePointer, value);
            }
        }
        
        public int Value
        {
            get
            {
                CheckDisposed();
                return NativeApi.NumericUpDown_GetValue(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.NumericUpDown_SetValue(NativePointer, value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.NumericUpDownEventCallbackType((obj, e, param) =>
                {
                    var w = NativeObject.GetFromNativePointer<NumericUpDown>(obj, p => new NumericUpDown(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.NumericUpDown_SetEventCallback(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.NumericUpDownEvent e)
        {
            switch (e)
            {
                case NativeApi.NumericUpDownEvent.ValueChanged:
                ValueChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                default: throw new Exception("Unexpected NumericUpDownEvent value: " + e);
            }
        }
        
        public event EventHandler? ValueChanged;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr NumericUpDownEventCallbackType(IntPtr obj, NumericUpDownEvent e, IntPtr param);
            
            public enum NumericUpDownEvent
            {
                ValueChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NumericUpDown_SetEventCallback(NumericUpDownEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NumericUpDown_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int NumericUpDown_GetMinimum(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NumericUpDown_SetMinimum(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int NumericUpDown_GetMaximum(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NumericUpDown_SetMaximum(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int NumericUpDown_GetValue(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void NumericUpDown_SetValue(IntPtr obj, int value);
            
        }
    }
}
