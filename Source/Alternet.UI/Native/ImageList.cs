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
        
        public System.Drawing.SizeF ImageSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.ImageList_GetImageSize(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ImageList_SetImageSize(NativePointer, value);
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
            public static extern NativeApiTypes.SizeF ImageList_GetImageSize(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ImageList_SetImageSize(IntPtr obj, NativeApiTypes.SizeF value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ImageList_AddImage(IntPtr obj, IntPtr image);
            
        }
    }
}
