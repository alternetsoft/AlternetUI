// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class AuiNotebookPage : NativeObject
    {
        static AuiNotebookPage()
        {
        }
        
        public AuiNotebookPage()
        {
            SetNativePointer(NativeApi.AuiNotebookPage_Create_());
        }
        
        public AuiNotebookPage(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr AuiNotebookPage_Create_();
            
        }
    }
}
