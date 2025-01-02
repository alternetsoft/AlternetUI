// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class LinkLabel : Control
    {
        static LinkLabel()
        {
            SetEventCallback();
        }
        
        public LinkLabel()
        {
            SetNativePointer(NativeApi.LinkLabel_Create_());
        }
        
        public LinkLabel(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public Alternet.Drawing.Color HoverColor
        {
            get
            {
                CheckDisposed();
                return NativeApi.LinkLabel_GetHoverColor_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.LinkLabel_SetHoverColor_(NativePointer, value);
            }
        }
        
        public Alternet.Drawing.Color NormalColor
        {
            get
            {
                CheckDisposed();
                return NativeApi.LinkLabel_GetNormalColor_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.LinkLabel_SetNormalColor_(NativePointer, value);
            }
        }
        
        public Alternet.Drawing.Color VisitedColor
        {
            get
            {
                CheckDisposed();
                return NativeApi.LinkLabel_GetVisitedColor_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.LinkLabel_SetVisitedColor_(NativePointer, value);
            }
        }
        
        public bool Visited
        {
            get
            {
                CheckDisposed();
                return NativeApi.LinkLabel_GetVisited_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.LinkLabel_SetVisited_(NativePointer, value);
            }
        }
        
        public string Url
        {
            get
            {
                CheckDisposed();
                return NativeApi.LinkLabel_GetUrl_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.LinkLabel_SetUrl_(NativePointer, value);
            }
        }
        
        public static bool UseGenericControl
        {
            get
            {
                return NativeApi.LinkLabel_GetUseGenericControl_();
            }
            
            set
            {
                NativeApi.LinkLabel_SetUseGenericControl_(value);
            }
        }
        
        static GCHandle eventCallbackGCHandle;
        public static LinkLabel? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.LinkLabelEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<LinkLabel>(obj, p => new LinkLabel(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.LinkLabel_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.LinkLabelEvent e, IntPtr parameter)
        {
            {
                if(HyperlinkClick is not null)
                {
                var cea = new CancelEventArgs();
                HyperlinkClick.Invoke(this, cea);
                return cea.Cancel ? IntPtrUtils.One : IntPtr.Zero;
                }
                else return IntPtr.Zero;
            }
        }
        
        public event EventHandler<CancelEventArgs>? HyperlinkClick;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr LinkLabelEventCallbackType(IntPtr obj, LinkLabelEvent e, IntPtr param);
            
            public enum LinkLabelEvent
            {
                HyperlinkClick,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinkLabel_SetEventCallback_(LinkLabelEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr LinkLabel_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color LinkLabel_GetHoverColor_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinkLabel_SetHoverColor_(IntPtr obj, NativeApiTypes.Color value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color LinkLabel_GetNormalColor_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinkLabel_SetNormalColor_(IntPtr obj, NativeApiTypes.Color value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color LinkLabel_GetVisitedColor_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinkLabel_SetVisitedColor_(IntPtr obj, NativeApiTypes.Color value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool LinkLabel_GetVisited_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinkLabel_SetVisited_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string LinkLabel_GetUrl_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinkLabel_SetUrl_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool LinkLabel_GetUseGenericControl_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void LinkLabel_SetUseGenericControl_(bool value);
            
        }
    }
}
