// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class AnimationControl : Control
    {
        static AnimationControl()
        {
        }
        
        public AnimationControl()
        {
            SetNativePointer(NativeApi.AnimationControl_Create_());
        }
        
        public AnimationControl(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool UseGeneric
        {
            get
            {
                CheckDisposed();
                return NativeApi.AnimationControl_GetUseGeneric_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.AnimationControl_SetUseGeneric_(NativePointer, value);
            }
        }
        
        public int GetDelay(uint i)
        {
            CheckDisposed();
            return NativeApi.AnimationControl_GetDelay_(NativePointer, i);
        }
        
        public System.IntPtr GetFrame(uint i)
        {
            CheckDisposed();
            return NativeApi.AnimationControl_GetFrame_(NativePointer, i);
        }
        
        public uint GetFrameCount()
        {
            CheckDisposed();
            return NativeApi.AnimationControl_GetFrameCount_(NativePointer);
        }
        
        public Alternet.Drawing.SizeI GetSize()
        {
            CheckDisposed();
            return NativeApi.AnimationControl_GetSize_(NativePointer);
        }
        
        public bool IsOk()
        {
            CheckDisposed();
            return NativeApi.AnimationControl_IsOk_(NativePointer);
        }
        
        public bool Play()
        {
            CheckDisposed();
            return NativeApi.AnimationControl_Play_(NativePointer);
        }
        
        public void Stop()
        {
            CheckDisposed();
            NativeApi.AnimationControl_Stop_(NativePointer);
        }
        
        public bool IsPlaying()
        {
            CheckDisposed();
            return NativeApi.AnimationControl_IsPlaying_(NativePointer);
        }
        
        public bool LoadFile(string filename, int type)
        {
            CheckDisposed();
            return NativeApi.AnimationControl_LoadFile_(NativePointer, filename, type);
        }
        
        public bool Load(InputStream stream, int type)
        {
            CheckDisposed();
            return NativeApi.AnimationControl_Load_(NativePointer, stream.NativePointer, type);
        }
        
        public void SetInactiveBitmap(ImageSet? bitmapBundle)
        {
            CheckDisposed();
            NativeApi.AnimationControl_SetInactiveBitmap_(NativePointer, bitmapBundle?.NativePointer ?? IntPtr.Zero);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr AnimationControl_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AnimationControl_GetUseGeneric_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AnimationControl_SetUseGeneric_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int AnimationControl_GetDelay_(IntPtr obj, uint i);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AnimationControl_GetFrame_(IntPtr obj, uint i);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint AnimationControl_GetFrameCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeI AnimationControl_GetSize_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AnimationControl_IsOk_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AnimationControl_Play_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AnimationControl_Stop_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AnimationControl_IsPlaying_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AnimationControl_LoadFile_(IntPtr obj, string filename, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AnimationControl_Load_(IntPtr obj, IntPtr stream, int type);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AnimationControl_SetInactiveBitmap_(IntPtr obj, IntPtr bitmapBundle);
            
        }
    }
}
