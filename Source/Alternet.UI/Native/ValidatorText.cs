// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ValidatorText : Validator
    {
        static ValidatorText()
        {
        }
        
        public ValidatorText()
        {
            SetNativePointer(NativeApi.ValidatorText_Create_());
        }
        
        public ValidatorText(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public static void DeleteValidatorText(System.IntPtr handle)
        {
            NativeApi.ValidatorText_DeleteValidatorText_(handle);
        }
        
        public static System.IntPtr CreateValidatorText(long style)
        {
            var n = NativeApi.ValidatorText_CreateValidatorText_(style);
            return n;
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ValidatorText_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ValidatorText_DeleteValidatorText_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ValidatorText_CreateValidatorText_(long style);
            
        }
    }
}
