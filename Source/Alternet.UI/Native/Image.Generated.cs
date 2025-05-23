// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class Image : NativeObject
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
        
        public bool HasMask
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetHasMask_(NativePointer);
            }
            
        }
        
        public double ScaleFactor
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetScaleFactor_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Image_SetScaleFactor_(NativePointer, value);
            }
        }
        
        public Alternet.Drawing.SizeI DipSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetDipSize_(NativePointer);
            }
            
        }
        
        public double ScaledHeight
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetScaledHeight_(NativePointer);
            }
            
        }
        
        public Alternet.Drawing.SizeI ScaledSize
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetScaledSize_(NativePointer);
            }
            
        }
        
        public double ScaledWidth
        {
            get
            {
                CheckDisposed();
                return NativeApi.Image_GetScaledWidth_(NativePointer);
            }
            
        }
        
        public Alternet.Drawing.SizeI PixelSize
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
        
        public static int GetStaticOption(int objectId, int propId)
        {
            return NativeApi.Image_GetStaticOption_(objectId, propId);
        }
        
        public static void Log()
        {
            NativeApi.Image_Log_();
        }
        
        public bool InitializeFromDipSize(int width, int height, double scale, int depth)
        {
            CheckDisposed();
            return NativeApi.Image_InitializeFromDipSize_(NativePointer, width, height, scale, depth);
        }
        
        public bool InitializeFromScreen()
        {
            CheckDisposed();
            return NativeApi.Image_InitializeFromScreen_(NativePointer);
        }
        
        public bool LoadFromStream(InputStream stream)
        {
            CheckDisposed();
            return NativeApi.Image_LoadFromStream_(NativePointer, stream.NativePointer);
        }
        
        public bool LoadSvgFromStream(InputStream stream, int width, int height, Alternet.Drawing.Color color)
        {
            CheckDisposed();
            return NativeApi.Image_LoadSvgFromStream_(NativePointer, stream.NativePointer, width, height, color);
        }
        
        public bool LoadSvgFromString(string s, int width, int height, Alternet.Drawing.Color color)
        {
            CheckDisposed();
            return NativeApi.Image_LoadSvgFromString_(NativePointer, s, width, height, color);
        }
        
        public void Initialize(Alternet.Drawing.SizeI size, int depth)
        {
            CheckDisposed();
            NativeApi.Image_Initialize_(NativePointer, size, depth);
        }
        
        public void InitializeFromImage(Image source, Alternet.Drawing.SizeI size)
        {
            CheckDisposed();
            NativeApi.Image_InitializeFromImage_(NativePointer, source.NativePointer, size);
        }
        
        public void CopyFrom(Image otherImage)
        {
            CheckDisposed();
            NativeApi.Image_CopyFrom_(NativePointer, otherImage.NativePointer);
        }
        
        public bool SaveToStream(OutputStream stream, string format)
        {
            CheckDisposed();
            return NativeApi.Image_SaveToStream_(NativePointer, stream.NativePointer, format);
        }
        
        public bool SaveToFile(string fileName)
        {
            CheckDisposed();
            return NativeApi.Image_SaveToFile_(NativePointer, fileName);
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
        
        public bool ResetAlpha()
        {
            CheckDisposed();
            return NativeApi.Image_ResetAlpha_(NativePointer);
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
        
        public Image GetSubBitmap(Alternet.Drawing.RectI rect)
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
        
        public bool Rescale(Alternet.Drawing.SizeI sizeNeeded)
        {
            CheckDisposed();
            return NativeApi.Image_Rescale_(NativePointer, sizeNeeded);
        }
        
        public static int GetDefaultBitmapType()
        {
            return NativeApi.Image_GetDefaultBitmapType_();
        }
        
        public System.IntPtr LockBits()
        {
            CheckDisposed();
            return NativeApi.Image_LockBits_(NativePointer);
        }
        
        public int GetStride()
        {
            CheckDisposed();
            return NativeApi.Image_GetStride_(NativePointer);
        }
        
        public void UnlockBits()
        {
            CheckDisposed();
            NativeApi.Image_UnlockBits_(NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Image_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_GetHasMask_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern double Image_GetScaleFactor_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_SetScaleFactor_(IntPtr obj, double value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeI Image_GetDipSize_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern double Image_GetScaledHeight_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeI Image_GetScaledSize_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern double Image_GetScaledWidth_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeI Image_GetPixelSize_(IntPtr obj);
            
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
            public static extern int Image_GetStaticOption_(int objectId, int propId);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_Log_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_InitializeFromDipSize_(IntPtr obj, int width, int height, double scale, int depth);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_InitializeFromScreen_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_LoadFromStream_(IntPtr obj, IntPtr stream);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_LoadSvgFromStream_(IntPtr obj, IntPtr stream, int width, int height, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_LoadSvgFromString_(IntPtr obj, string s, int width, int height, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_Initialize_(IntPtr obj, Alternet.Drawing.SizeI size, int depth);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_InitializeFromImage_(IntPtr obj, IntPtr source, Alternet.Drawing.SizeI size);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_CopyFrom_(IntPtr obj, IntPtr otherImage);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_SaveToStream_(IntPtr obj, IntPtr stream, string format);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_SaveToFile_(IntPtr obj, string fileName);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Image_ConvertToGenericImage_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_LoadFromGenericImage_(IntPtr obj, System.IntPtr image, int depth);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_GrayScale_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_ResetAlpha_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_LoadFile_(IntPtr obj, string name, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_SaveFile_(IntPtr obj, string name, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_SaveStream_(IntPtr obj, IntPtr stream, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_LoadStream_(IntPtr obj, IntPtr stream, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Image_GetSubBitmap_(IntPtr obj, Alternet.Drawing.RectI rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Image_ConvertToDisabled_(IntPtr obj, byte brightness);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Image_Rescale_(IntPtr obj, Alternet.Drawing.SizeI sizeNeeded);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Image_GetDefaultBitmapType_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Image_LockBits_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Image_GetStride_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Image_UnlockBits_(IntPtr obj);
            
        }
    }
}
