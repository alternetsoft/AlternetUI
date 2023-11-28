// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Image : NativeObject
    {
        static Image()
        {
        }
        
        public Image()
        {
            SetNativePointer(NativeApi.Image_Create_());
        }
        
        public Image(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public Alternet.Drawing.Size Size
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetSize_(NativePointer);
            }
            
        }
        
        public Alternet.Drawing.Int32Size PixelSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetPixelSize_(NativePointer);
            }
            
        }
        
        public bool IsOk
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetIsOk_(NativePointer);
            }
            
        }
        
        public bool HasAlpha
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetHasAlpha_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Image_SetHasAlpha_(NativePointer, value);
            }
        }
        
        public int PixelWidth
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetPixelWidth_(NativePointer);
            }
            
        }
        
        public int PixelHeight
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetPixelHeight_(NativePointer);
            }
            
        }
        
        public int Depth
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetDepth_(NativePointer);
            }
            
        }
        
        public void LoadFromStream(InputStream stream)
        {
            CheckDisposed();
            NativeApi.Image_LoadFromStream_(NativePointer, stream.NativePointer);
        }
        
        public void LoadSvgFromStream(InputStream stream, int width, int height, Alternet.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.Image_LoadSvgFromStream_(NativePointer, stream.NativePointer, width, height, color);
        }
        
        public void Initialize(Alternet.Drawing.Size size)
        {
            CheckDisposed();
            NativeApi.Image_Initialize_(NativePointer, size);
        }
        
        public void CopyFrom(Image otherImage)
        {
            CheckDisposed();
            NativeApi.Image_CopyFrom_(NativePointer, otherImage.NativePointer);
        }
        
        public void SaveToStream(OutputStream stream, string format)
        {
            CheckDisposed();
            NativeApi.Image_SaveToStream_(NativePointer, stream.NativePointer, format);
        }
        
        public void SaveToFile(string fileName)
        {
            CheckDisposed();
            NativeApi.Image_SaveToFile_(NativePointer, fileName);
        }
        
        public System.IntPtr ConvertToGenericImage()
        {
            CheckDisposed();
            return NativeApi.Image_ConvertToGenericImage_(NativePointer);
        }
        
        public void LoadFromGenericImage(System.IntPtr image, int depth)
        {
            CheckDisposed();
            NativeApi.Image_LoadFromGenericImage_(NativePointer, image, depth);
        }
        
        public bool GrayScale()
        {
            CheckDisposed();
            return NativeApi.Image_GrayScale_(NativePointer);
        }
        
        public void ResetAlpha()
        {
            CheckDisposed();
            NativeApi.Image_ResetAlpha_(NativePointer);
        }
        
        public bool LoadFile(string name, int type)
        {
            CheckDisposed();
            return NativeApi.Image_LoadFile_(NativePointer, name, type);
        }
        
        public bool SaveFile(string name, int type)
        {
            CheckDisposed();
            return NativeApi.Image_SaveFile_(NativePointer, name, type);
        }
        
        public bool SaveStream(OutputStream stream, int type)
        {
            CheckDisposed();
            return NativeApi.Image_SaveStream_(NativePointer, stream.NativePointer, type);
        }
        
        public bool LoadStream(InputStream stream, int type)
        {
            CheckDisposed();
            return NativeApi.Image_LoadStream_(NativePointer, stream.NativePointer, type);
        }
        
        public Image GetSubBitmap(Alternet.Drawing.Int32Rect rect)
        {
            CheckDisposed();
            var _nnn = NativeApi.Image_GetSubBitmap_(NativePointer, rect);
            var _mmm = NativeObject.GetFromNativePointer<Image>(_nnn, p => new Image(p))!;
            ReleaseNativeObjectPointer(_nnn);
            return _mmm;
        }
        
        public Image ConvertToDisabled(byte brightness)
        {
            CheckDisposed();
            var _nnn = NativeApi.Image_ConvertToDisabled_(NativePointer, brightness);
            var _mmm = NativeObject.GetFromNativePointer<Image>(_nnn, p => new Image(p))!;
            ReleaseNativeObjectPointer(_nnn);
            return _mmm;
        }
        
        public void Rescale(Alternet.Drawing.Int32Size sizeNeeded)
        {
            CheckDisposed();
            NativeApi.Image_Rescale_(NativePointer, sizeNeeded);
        }
        
        public static int GetDefaultBitmapType()
        {
            return NativeApi.Image_GetDefaultBitmapType_();
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Image_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Size Image_GetSize_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Size Image_GetPixelSize_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_GetIsOk_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_GetHasAlpha_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_SetHasAlpha_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Image_GetPixelWidth_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Image_GetPixelHeight_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Image_GetDepth_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_LoadFromStream_(IntPtr obj, IntPtr stream);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_LoadSvgFromStream_(IntPtr obj, IntPtr stream, int width, int height, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_Initialize_(IntPtr obj, Alternet.Drawing.Size size);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_CopyFrom_(IntPtr obj, IntPtr otherImage);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_SaveToStream_(IntPtr obj, IntPtr stream, string format);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_SaveToFile_(IntPtr obj, string fileName);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Image_ConvertToGenericImage_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_LoadFromGenericImage_(IntPtr obj, System.IntPtr image, int depth);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_GrayScale_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_ResetAlpha_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_LoadFile_(IntPtr obj, string name, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_SaveFile_(IntPtr obj, string name, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_SaveStream_(IntPtr obj, IntPtr stream, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_LoadStream_(IntPtr obj, IntPtr stream, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Image_GetSubBitmap_(IntPtr obj, Alternet.Drawing.Int32Rect rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Image_ConvertToDisabled_(IntPtr obj, byte brightness);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_Rescale_(IntPtr obj, Alternet.Drawing.Int32Size sizeNeeded);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Image_GetDefaultBitmapType_();
            
        }
    }
}
