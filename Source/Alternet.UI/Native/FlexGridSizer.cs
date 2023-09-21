// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class FlexGridSizer : GridSizer
    {
        static FlexGridSizer()
        {
        }
        
        public FlexGridSizer()
        {
            SetNativePointer(NativeApi.FlexGridSizer_Create_());
        }
        
        public FlexGridSizer(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr FlexGridSizer_Create_();
            
        }
    }
}
