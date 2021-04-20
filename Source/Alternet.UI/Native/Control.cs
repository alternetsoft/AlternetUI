// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

using System;
using System.Runtime.InteropServices;
using System.Security;
namespace Alternet.UI.Native
{
    internal abstract class Control : NativeObject
    {
        protected Control()
        {
            SetEventCallback();
        }
        
        public Control(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public System.Drawing.SizeF Size
        {
            get
            {
                CheckDisposed();
                return NativeApi.Control_GetSize(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Control_SetSize(NativePointer, value);
            }
        }
        
        public System.Drawing.PointF Location
        {
            get
            {
                CheckDisposed();
                return NativeApi.Control_GetLocation(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Control_SetLocation(NativePointer, value);
            }
        }
        
        public System.Drawing.RectangleF Bounds
        {
            get
            {
                CheckDisposed();
                return NativeApi.Control_GetBounds(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Control_SetBounds(NativePointer, value);
            }
        }
        
        public bool Visible
        {
            get
            {
                CheckDisposed();
                return NativeApi.Control_GetVisible(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Control_SetVisible(NativePointer, value);
            }
        }
        
        public bool IsMouseOver
        {
            get
            {
                CheckDisposed();
                return NativeApi.Control_GetIsMouseOver(NativePointer);
            }
            
        }
        
        public System.Drawing.Color BackgroundColor
        {
            get
            {
                CheckDisposed();
                return NativeApi.Control_GetBackgroundColor(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.Control_SetBackgroundColor(NativePointer, value);
            }
        }
        
        public void SetMouseCapture(bool value)
        {
            CheckDisposed();
            NativeApi.Control_SetMouseCapture(NativePointer, value);
        }
        
        public void AddChild(Control control)
        {
            CheckDisposed();
            NativeApi.Control_AddChild(NativePointer, control.NativePointer);
        }
        
        public void RemoveChild(Control control)
        {
            CheckDisposed();
            NativeApi.Control_RemoveChild(NativePointer, control.NativePointer);
        }
        
        public void Update()
        {
            CheckDisposed();
            NativeApi.Control_Update(NativePointer);
        }
        
        public System.Drawing.SizeF GetPreferredSize(System.Drawing.SizeF availableSize)
        {
            CheckDisposed();
            return NativeApi.Control_GetPreferredSize(NativePointer, availableSize);
        }
        
        public DrawingContext OpenPaintDrawingContext()
        {
            CheckDisposed();
            return NativeObject.GetFromNativePointer<DrawingContext>(NativeApi.Control_OpenPaintDrawingContext(NativePointer), p => new DrawingContext(p))!;
        }
        
        public DrawingContext OpenClientDrawingContext()
        {
            CheckDisposed();
            return NativeObject.GetFromNativePointer<DrawingContext>(NativeApi.Control_OpenClientDrawingContext(NativePointer), p => new DrawingContext(p))!;
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ControlEventCallbackType((obj, e) =>
                {
                    var w = NativeObject.GetFromNativePointer<Control>(obj, null);
                    if (w == null) return;
                    w.OnEvent(e);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Control_SetEventCallback(sink);
            }
        }
        
        void OnEvent(NativeApi.ControlEvent e)
        {
            switch (e)
            {
                case NativeApi.ControlEvent.Paint: Paint?.Invoke(this, EventArgs.Empty); break;
                case NativeApi.ControlEvent.MouseEnter: MouseEnter?.Invoke(this, EventArgs.Empty); break;
                case NativeApi.ControlEvent.MouseLeave: MouseLeave?.Invoke(this, EventArgs.Empty); break;
                case NativeApi.ControlEvent.MouseMove: MouseMove?.Invoke(this, EventArgs.Empty); break;
                case NativeApi.ControlEvent.MouseLeftButtonDown: MouseLeftButtonDown?.Invoke(this, EventArgs.Empty); break;
                case NativeApi.ControlEvent.MouseLeftButtonUp: MouseLeftButtonUp?.Invoke(this, EventArgs.Empty); break;
                default: throw new Exception("Unexpected ControlEvent value: " + e);
            }
        }
        
        public event EventHandler? Paint;
        public event EventHandler? MouseEnter;
        public event EventHandler? MouseLeave;
        public event EventHandler? MouseMove;
        public event EventHandler? MouseLeftButtonDown;
        public event EventHandler? MouseLeftButtonUp;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate void ControlEventCallbackType(IntPtr obj, ControlEvent e);
            
            public enum ControlEvent
            {
                Paint,
                MouseEnter,
                MouseLeave,
                MouseMove,
                MouseLeftButtonDown,
                MouseLeftButtonUp,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_SetEventCallback(ControlEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.SizeF Control_GetSize(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_SetSize(IntPtr obj, NativeApiTypes.SizeF value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.PointF Control_GetLocation(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_SetLocation(IntPtr obj, NativeApiTypes.PointF value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.RectangleF Control_GetBounds(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_SetBounds(IntPtr obj, NativeApiTypes.RectangleF value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Control_GetVisible(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_SetVisible(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Control_GetIsMouseOver(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color Control_GetBackgroundColor(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_SetBackgroundColor(IntPtr obj, NativeApiTypes.Color value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_SetMouseCapture(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_AddChild(IntPtr obj, IntPtr control);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_RemoveChild(IntPtr obj, IntPtr control);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Control_Update(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.SizeF Control_GetPreferredSize(IntPtr obj, NativeApiTypes.SizeF availableSize);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Control_OpenPaintDrawingContext(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Control_OpenClientDrawingContext(IntPtr obj);
            
        }
    }
}
