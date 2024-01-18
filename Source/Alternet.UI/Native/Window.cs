// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Window : Control
    {
        static Window()
        {
            SetEventCallback();
        }
        
        public Window()
        {
            SetNativePointer(NativeApi.Window_Create_());
        }
        
        public Window(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public string Title
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetTitle_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetTitle_(NativePointer, value);
            }
        }
        
        public WindowStartLocation WindowStartLocation
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetWindowStartLocation_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetWindowStartLocation_(NativePointer, value);
            }
        }
        
        public bool ShowInTaskbar
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetShowInTaskbar_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetShowInTaskbar_(NativePointer, value);
            }
        }
        
        public bool MinimizeEnabled
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetMinimizeEnabled_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetMinimizeEnabled_(NativePointer, value);
            }
        }
        
        public bool MaximizeEnabled
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetMaximizeEnabled_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetMaximizeEnabled_(NativePointer, value);
            }
        }
        
        public bool CloseEnabled
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetCloseEnabled_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetCloseEnabled_(NativePointer, value);
            }
        }
        
        public bool AlwaysOnTop
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetAlwaysOnTop_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetAlwaysOnTop_(NativePointer, value);
            }
        }
        
        public bool IsToolWindow
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetIsToolWindow_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetIsToolWindow_(NativePointer, value);
            }
        }
        
        public bool Resizable
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetResizable_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetResizable_(NativePointer, value);
            }
        }
        
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetHasBorder_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetHasBorder_(NativePointer, value);
            }
        }
        
        public bool HasTitleBar
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetHasTitleBar_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetHasTitleBar_(NativePointer, value);
            }
        }
        
        public bool HasSystemMenu
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetHasSystemMenu_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetHasSystemMenu_(NativePointer, value);
            }
        }
        
        public bool IsPopupWindow
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetIsPopupWindow_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetIsPopupWindow_(NativePointer, value);
            }
        }
        
        public ModalResult ModalResult
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetModalResult_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetModalResult_(NativePointer, value);
            }
        }
        
        public bool Modal
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetModal_(NativePointer);
            }
            
        }
        
        public static Window ActiveWindow
        {
            get
            {
                var _nnn = NativeApi.Window_GetActiveWindow_();
                var _mmm = NativeObject.GetFromNativePointer<Window>(_nnn, p => new Window(p))!;
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
        }
        
        public Window[] OwnedWindows
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.Window_OpenOwnedWindowsArray_(NativePointer);
                try
                {
                    var count = NativeApi.Window_GetOwnedWindowsItemCount_(NativePointer, array);
                    var result = new System.Collections.Generic.List<Window>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var n = NativeApi.Window_GetOwnedWindowsItemAt_(NativePointer, array, i);
                        var item = NativeObject.GetFromNativePointer<Window>(n, p => new Window(p));
                        ReleaseNativeObjectPointer(n);
                        result.Add(item ?? throw new System.Exception());
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.Window_CloseOwnedWindowsArray_(NativePointer, array);
                }
            }
            
        }
        
        public WindowState State
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetState_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetState_(NativePointer, value);
            }
        }
        
        public IconSet? Icon
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.Window_GetIcon_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<IconSet>(_nnn, p => new IconSet(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetIcon_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public MainMenu? Menu
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.Window_GetMenu_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<MainMenu>(_nnn, p => new MainMenu(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetMenu_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public Toolbar? Toolbar
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.Window_GetToolbar_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<Toolbar>(_nnn, p => new Toolbar(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetToolbar_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public System.IntPtr WxStatusBar
        {
            get
            {
                CheckDisposed();
                return NativeApi.Window_GetWxStatusBar_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Window_SetWxStatusBar_(NativePointer, value);
            }
        }
        
        public static System.IntPtr CreateEx(int kind)
        {
            return NativeApi.Window_CreateEx_(kind);
        }
        
        public static void SetDefaultBounds(Alternet.Drawing.RectD bounds)
        {
            NativeApi.Window_SetDefaultBounds_(bounds);
        }
        
        public static void SetParkingWindowFont(Font? font)
        {
            NativeApi.Window_SetParkingWindowFont_(font?.NativePointer ?? IntPtr.Zero);
        }
        
        public void ShowModal()
        {
            CheckDisposed();
            NativeApi.Window_ShowModal_(NativePointer);
        }
        
        public void Close()
        {
            CheckDisposed();
            NativeApi.Window_Close_(NativePointer);
        }
        
        public void Activate()
        {
            CheckDisposed();
            NativeApi.Window_Activate_(NativePointer);
        }
        
        public void AddInputBinding(string managedCommandId, Key key, ModifierKeys modifiers)
        {
            CheckDisposed();
            NativeApi.Window_AddInputBinding_(NativePointer, managedCommandId, key, modifiers);
        }
        
        public void RemoveInputBinding(string managedCommandId)
        {
            CheckDisposed();
            NativeApi.Window_RemoveInputBinding_(NativePointer, managedCommandId);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.WindowEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<Window>(obj, p => new Window(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Window_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.WindowEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.WindowEvent.Closing:
                {
                    {
                        var cea = new CancelEventArgs();
                        Closing?.Invoke(this, cea);
                        return cea.Cancel ? new IntPtr(1) : IntPtr.Zero;
                    }
                }
                case NativeApi.WindowEvent.StateChanged:
                {
                    StateChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                case NativeApi.WindowEvent.SizeChanged:
                {
                    SizeChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                case NativeApi.WindowEvent.InputBindingCommandExecuted:
                {
                    var ea = new NativeEventArgs<CommandEventData>(MarshalEx.PtrToStructure<CommandEventData>(parameter));
                    InputBindingCommandExecuted?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.WindowEvent.LocationChanged:
                {
                    LocationChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected WindowEvent value: " + e);
            }
        }
        
        public event EventHandler<CancelEventArgs>? Closing;
        public event EventHandler? StateChanged;
        public event EventHandler? SizeChanged;
        public event NativeEventHandler<CommandEventData>? InputBindingCommandExecuted;
        public event EventHandler? LocationChanged;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr WindowEventCallbackType(IntPtr obj, WindowEvent e, IntPtr param);
            
            public enum WindowEvent
            {
                Closing,
                StateChanged,
                SizeChanged,
                InputBindingCommandExecuted,
                LocationChanged,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetEventCallback_(WindowEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string Window_GetTitle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetTitle_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern WindowStartLocation Window_GetWindowStartLocation_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetWindowStartLocation_(IntPtr obj, WindowStartLocation value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetShowInTaskbar_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetShowInTaskbar_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetMinimizeEnabled_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetMinimizeEnabled_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetMaximizeEnabled_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetMaximizeEnabled_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetCloseEnabled_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetCloseEnabled_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetAlwaysOnTop_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetAlwaysOnTop_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetIsToolWindow_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetIsToolWindow_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetResizable_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetResizable_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetHasBorder_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetHasBorder_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetHasTitleBar_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetHasTitleBar_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetHasSystemMenu_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetHasSystemMenu_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetIsPopupWindow_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetIsPopupWindow_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ModalResult Window_GetModalResult_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetModalResult_(IntPtr obj, ModalResult value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Window_GetModal_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_GetActiveWindow_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern WindowState Window_GetState_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetState_(IntPtr obj, WindowState value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_GetIcon_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetIcon_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_GetMenu_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetMenu_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_GetToolbar_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetToolbar_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Window_GetWxStatusBar_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetWxStatusBar_(IntPtr obj, System.IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Window_OpenOwnedWindowsArray_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Window_GetOwnedWindowsItemCount_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Window_GetOwnedWindowsItemAt_(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_CloseOwnedWindowsArray_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Window_CreateEx_(int kind);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetDefaultBounds_(Alternet.Drawing.RectD bounds);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_SetParkingWindowFont_(IntPtr font);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_ShowModal_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_Close_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_Activate_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_AddInputBinding_(IntPtr obj, string managedCommandId, Key key, ModifierKeys modifiers);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Window_RemoveInputBinding_(IntPtr obj, string managedCommandId);
            
        }
    }
}
