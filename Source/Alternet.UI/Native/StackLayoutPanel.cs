// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.Security;
namespace Alternet.UI.Native
{
    internal class StackLayoutPanel : Control
    {
        public StackLayoutPanel()
        {
            SetNativePointer(NativeApi.StackLayoutPanel_Create());
        }
        
        public StackLayoutOrientation Orientation
        {
            get
            {
                CheckDisposed();
                return NativeApi.StackLayoutPanel_GetOrientation(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.StackLayoutPanel_SetOrientation(NativePointer, value);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr StackLayoutPanel_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void StackLayoutPanel_Destroy(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern StackLayoutOrientation StackLayoutPanel_GetOrientation(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void StackLayoutPanel_SetOrientation(IntPtr obj, StackLayoutOrientation value);
            
        }
    }
}
