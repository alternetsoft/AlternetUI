// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ImageList : NativeObject
    {
        public ImageList()
        {
            SetNativePointer(NativeApi.ImageList_Create_());
        }
        
        public ImageList(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public System.Drawing.Size PixelImageSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.ImageList_GetPixelImageSize(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ImageList_SetPixelImageSize(NativePointer, value);
            }
        }
        
        public void AddImage(Image image)
        {
            CheckDisposed();
            NativeApi.ImageList_AddImage(NativePointer, image.NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ImageList_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Size ImageList_GetPixelImageSize(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ImageList_SetPixelImageSize(IntPtr obj, NativeApiTypes.Size value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ImageList_AddImage(IntPtr obj, IntPtr image);
            
        }
    }
}
