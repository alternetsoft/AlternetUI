// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class Validator : NativeObject
    {
        static Validator()
        {
        }
        
        public Validator()
        {
            SetNativePointer(NativeApi.Validator_Create_());
        }
        
        public Validator(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public static void SuppressBellOnError(bool suppress)
        {
            NativeApi.Validator_SuppressBellOnError_(suppress);
        }
        
        public static bool IsSilent()
        {
            return NativeApi.Validator_IsSilent_();
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Validator_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Validator_SuppressBellOnError_(bool suppress);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Validator_IsSilent_();
            
        }
    }
}