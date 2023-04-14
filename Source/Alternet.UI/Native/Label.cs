// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Label : Control
    {
        static Label()
        {
        }
        
        public Label()
        {
            SetNativePointer(NativeApi.Label_Create_());
        }
        
        public Label(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string Text
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.Label_GetText_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Label_SetText_(NativePointer, value);
            }
        }
        
        public bool IsEllipsized()
        {
            CheckDisposed();
            var n = NativeApi.Label_IsEllipsized_(NativePointer);
            var m = n;
            return m;
        }
        
        public void Wrap(int width)
        {
            CheckDisposed();
            NativeApi.Label_Wrap_(NativePointer, width);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Label_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Label_GetText_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Label_SetText_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Label_IsEllipsized_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Label_Wrap_(IntPtr obj, int width);
            
        }
    }
}
