// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class FontDialog : NativeObject
    {
        static FontDialog()
        {
        }
        
        public FontDialog()
        {
            SetNativePointer(NativeApi.FontDialog_Create_());
        }
        
        public FontDialog(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool AllowSymbols
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetAllowSymbols_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.FontDialog_SetAllowSymbols_(NativePointer, value);
            }
        }
        
        public bool ShowHelp
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetShowHelp_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.FontDialog_SetShowHelp_(NativePointer, value);
            }
        }
        
        public bool EnableEffects
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetEnableEffects_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.FontDialog_SetEnableEffects_(NativePointer, value);
            }
        }
        
        public int RestrictSelection
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetRestrictSelection_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.FontDialog_SetRestrictSelection_(NativePointer, value);
            }
        }
        
        public Alternet.Drawing.Color Color
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetColor_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.FontDialog_SetColor_(NativePointer, value);
            }
        }
        
        public string ResultFontName
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetResultFontName_(NativePointer);
            }
            
        }
        
        public double ResultFontSizeInPoints
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetResultFontSizeInPoints_(NativePointer);
            }
            
        }
        
        public Alternet.Drawing.FontStyle ResultFontStyle
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetResultFontStyle_(NativePointer);
            }
            
        }
        
        public string? Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.FontDialog_GetTitle_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.FontDialog_SetTitle_(NativePointer, value);
            }
        }
        
        public Alternet.UI.ModalResult ShowModal(Window? owner)
        {
            CheckDisposed();
            return NativeApi.FontDialog_ShowModal_(NativePointer, owner?.NativePointer ?? IntPtr.Zero);
        }
        
        public void SetRange(int minRange, int maxRange)
        {
            CheckDisposed();
            NativeApi.FontDialog_SetRange_(NativePointer, minRange, maxRange);
        }
        
        public void SetInitialFont(Alternet.Drawing.GenericFontFamily genericFamily, string? familyName, double emSizeInPoints, Alternet.Drawing.FontStyle style)
        {
            CheckDisposed();
            NativeApi.FontDialog_SetInitialFont_(NativePointer, genericFamily, familyName, emSizeInPoints, style);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr FontDialog_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool FontDialog_GetAllowSymbols_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetAllowSymbols_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool FontDialog_GetShowHelp_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetShowHelp_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool FontDialog_GetEnableEffects_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetEnableEffects_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int FontDialog_GetRestrictSelection_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetRestrictSelection_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color FontDialog_GetColor_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetColor_(IntPtr obj, NativeApiTypes.Color value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string FontDialog_GetResultFontName_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern double FontDialog_GetResultFontSizeInPoints_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.FontStyle FontDialog_GetResultFontStyle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string? FontDialog_GetTitle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetTitle_(IntPtr obj, string? value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.ModalResult FontDialog_ShowModal_(IntPtr obj, IntPtr owner);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetRange_(IntPtr obj, int minRange, int maxRange);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void FontDialog_SetInitialFont_(IntPtr obj, Alternet.Drawing.GenericFontFamily genericFamily, string? familyName, double emSizeInPoints, Alternet.Drawing.FontStyle style);
            
        }
    }
}
