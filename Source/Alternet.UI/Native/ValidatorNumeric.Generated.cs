// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class ValidatorNumeric : Validator
    {
        static ValidatorNumeric()
        {
        }
        
        public ValidatorNumeric()
        {
            SetNativePointer(NativeApi.ValidatorNumeric_Create_());
        }
        
        public ValidatorNumeric(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ValidatorNumeric_Create_();
            
        }
    }
}
