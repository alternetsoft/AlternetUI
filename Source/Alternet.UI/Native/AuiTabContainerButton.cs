// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class AuiTabContainerButton : NativeObject
    {
        static AuiTabContainerButton()
        {
        }
        
        public AuiTabContainerButton()
        {
            SetNativePointer(NativeApi.AuiTabContainerButton_Create_());
        }
        
        public AuiTabContainerButton(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr AuiTabContainerButton_Create_();
            
        }
    }
}
