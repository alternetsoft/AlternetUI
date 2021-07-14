// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Image : NativeObject
    {
        public Image()
        {
            SetNativePointer(NativeApi.Image_Create());
        }
        
        public Image(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public System.Drawing.SizeF Size
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetSize(NativePointer);
            }
            
        }
        
        public System.Drawing.Size PixelSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetPixelSize(NativePointer);
            }
            
        }
        
        public void LoadFromStream(InputStream stream)
        {
            CheckDisposed();
            NativeApi.Image_LoadFromStream(NativePointer, stream.NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Image_Create();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.SizeF Image_GetSize(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Size Image_GetPixelSize(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_LoadFromStream(IntPtr obj, IntPtr stream);
            
        }
    }
}
