// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Font : NativeObject
    {
        public Font()
        {
            SetNativePointer(NativeApi.Font_Create_());
        }
        
        public Font(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public System.String[] Families
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.Font_OpenFamiliesArray_(NativePointer);
                try
                {
                    var count = NativeApi.Font_GetFamiliesItemCount_(NativePointer, array);
                    var result = new System.Collections.Generic.List<string>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var item = NativeApi.Font_GetFamiliesItemAt_(NativePointer, array, i);
                        result.Add(item);
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.Font_CloseFamiliesArray_(NativePointer, array);
                }
            }
            
        }
        
        public void Initialize(GenericFontFamily genericFamily, string? familyName, float emSize)
        {
            CheckDisposed();
            NativeApi.Font_Initialize_(NativePointer, genericFamily, familyName, emSize);
        }
        
        public void InitializeWithDefaultFont()
        {
            CheckDisposed();
            NativeApi.Font_InitializeWithDefaultFont_(NativePointer);
        }
        
        public static bool IsFamilyValid(string fontFamily)
        {
            return NativeApi.Font_IsFamilyValid_(fontFamily);
        }
        
        public static string GetGenericFamilyName(GenericFontFamily genericFamily)
        {
            return NativeApi.Font_GetGenericFamilyName_(genericFamily);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Font_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Font_OpenFamiliesArray_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Font_GetFamiliesItemCount_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Font_GetFamiliesItemAt_(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Font_CloseFamiliesArray_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Font_Initialize_(IntPtr obj, GenericFontFamily genericFamily, string? familyName, float emSize);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Font_InitializeWithDefaultFont_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Font_IsFamilyValid_(string fontFamily);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Font_GetGenericFamilyName_(GenericFontFamily genericFamily);
            
        }
    }
}
