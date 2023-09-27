// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class MenuItem : Control
    {
        static MenuItem()
        {
            SetEventCallback();
        }
        
        public MenuItem()
        {
            SetNativePointer(NativeApi.MenuItem_Create_());
        }
        
        public MenuItem(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string ManagedCommandId
        {
            get
            {
                CheckDisposed();
                return NativeApi.MenuItem_GetManagedCommandId_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.MenuItem_SetManagedCommandId_(NativePointer, value);
            }
        }
        
        public string Text
        {
            get
            {
                CheckDisposed();
                return NativeApi.MenuItem_GetText_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.MenuItem_SetText_(NativePointer, value);
            }
        }
        
        public string Role
        {
            get
            {
                CheckDisposed();
                return NativeApi.MenuItem_GetRole_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.MenuItem_SetRole_(NativePointer, value);
            }
        }
        
        public bool Checked
        {
            get
            {
                CheckDisposed();
                return NativeApi.MenuItem_GetChecked_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.MenuItem_SetChecked_(NativePointer, value);
            }
        }
        
        public Menu? Submenu
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.MenuItem_GetSubmenu_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<Menu>(_nnn, p => new Menu(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.MenuItem_SetSubmenu_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public void SetShortcut(Key key, ModifierKeys modifierKeys)
        {
            CheckDisposed();
            NativeApi.MenuItem_SetShortcut_(NativePointer, key, modifierKeys);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.MenuItemEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<MenuItem>(obj, p => new MenuItem(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.MenuItem_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.MenuItemEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.MenuItemEvent.Click:
                {
                    Click?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected MenuItemEvent value: " + e);
            }
        }
        
        public event EventHandler? Click;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr MenuItemEventCallbackType(IntPtr obj, MenuItemEvent e, IntPtr param);
            
            public enum MenuItemEvent
            {
                Click,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MenuItem_SetEventCallback_(MenuItemEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr MenuItem_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string MenuItem_GetManagedCommandId_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MenuItem_SetManagedCommandId_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string MenuItem_GetText_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MenuItem_SetText_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string MenuItem_GetRole_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MenuItem_SetRole_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool MenuItem_GetChecked_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MenuItem_SetChecked_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr MenuItem_GetSubmenu_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MenuItem_SetSubmenu_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void MenuItem_SetShortcut_(IntPtr obj, Key key, ModifierKeys modifierKeys);
            
        }
    }
}
