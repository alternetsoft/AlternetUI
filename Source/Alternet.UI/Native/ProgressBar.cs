// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ProgressBar : Control
    {
        public ProgressBar()
        {
            SetNativePointer(NativeApi.ProgressBar_Create());
        }
        
        public ProgressBar(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int Minimum
        {
            get
            {
                CheckDisposed();
                return NativeApi.ProgressBar_GetMinimum(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ProgressBar_SetMinimum(NativePointer, value);
            }
        }
        
        public int Maximum
        {
            get
            {
                CheckDisposed();
                return NativeApi.ProgressBar_GetMaximum(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ProgressBar_SetMaximum(NativePointer, value);
            }
        }
        
        public int Value
        {
            get
            {
                CheckDisposed();
                return NativeApi.ProgressBar_GetValue(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ProgressBar_SetValue(NativePointer, value);
            }
        }
        
        public bool IsIndeterminate
        {
            get
            {
                CheckDisposed();
                return NativeApi.ProgressBar_GetIsIndeterminate(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ProgressBar_SetIsIndeterminate(NativePointer, value);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ProgressBar_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ProgressBar_GetMinimum(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ProgressBar_SetMinimum(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ProgressBar_GetMaximum(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ProgressBar_SetMaximum(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ProgressBar_GetValue(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ProgressBar_SetValue(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ProgressBar_GetIsIndeterminate(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ProgressBar_SetIsIndeterminate(IntPtr obj, bool value);
            
        }
    }
}
