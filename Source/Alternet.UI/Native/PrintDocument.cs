// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class PrintDocument : NativeObject
    {
        static PrintDocument()
        {
            SetEventCallback();
        }
        
        public PrintDocument()
        {
            SetNativePointer(NativeApi.PrintDocument_Create_());
        }
        
        public PrintDocument(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool OriginAtMargins
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetOriginAtMargins_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDocument_SetOriginAtMargins_(NativePointer, value);
            }
        }
        
        public string DocumentName
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetDocumentName_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDocument_SetDocumentName_(NativePointer, value);
            }
        }
        
        public PrinterSettings PrinterSettings
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrinterSettings_(NativePointer);
                var m = NativeObject.GetFromNativePointer<PrinterSettings>(n, p => new PrinterSettings(p))!;
                ReleaseNativeObjectPointer(n);
                return m;
            }
            
        }
        
        public PageSettings PageSettings
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPageSettings_(NativePointer);
                var m = NativeObject.GetFromNativePointer<PageSettings>(n, p => new PageSettings(p))!;
                ReleaseNativeObjectPointer(n);
                return m;
            }
            
        }
        
        public DrawingContext PrintPage_DrawingContext
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrintPage_DrawingContext_(NativePointer);
                var m = NativeObject.GetFromNativePointer<DrawingContext>(n, p => new DrawingContext(p))!;
                ReleaseNativeObjectPointer(n);
                return m;
            }
            
        }
        
        public bool PrintPage_HasMorePages
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrintPage_HasMorePages_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.PrintDocument_SetPrintPage_HasMorePages_(NativePointer, value);
            }
        }
        
        public Alternet.Drawing.Rect PrintPage_MarginBounds
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrintPage_MarginBounds_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public Alternet.Drawing.Rect PrintPage_PhysicalPageBounds
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrintPage_PhysicalPageBounds_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public Alternet.Drawing.Rect PrintPage_PageBounds
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrintPage_PageBounds_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public Alternet.Drawing.Rect PrintPage_PrintablePageBounds
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrintPage_PrintablePageBounds_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public int PrintPage_PageNumber
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.PrintDocument_GetPrintPage_PageNumber_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public void Print()
        {
            CheckDisposed();
            NativeApi.PrintDocument_Print_(NativePointer);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.PrintDocumentEventCallbackType((obj, e, parameter) =>
                UI.Application.HandleThreadExceptions(() =>
                {
                    var w = NativeObject.GetFromNativePointer<PrintDocument>(obj, p => new PrintDocument(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                ));
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.PrintDocument_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.PrintDocumentEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.PrintDocumentEvent.PrintPage:
                {
                    {
                        var cea = new CancelEventArgs();
                        PrintPage?.Invoke(this, cea);
                        return cea.Cancel ? new IntPtr(1) : IntPtr.Zero;
                    }
                }
                case NativeApi.PrintDocumentEvent.BeginPrint:
                {
                    {
                        var cea = new CancelEventArgs();
                        BeginPrint?.Invoke(this, cea);
                        return cea.Cancel ? new IntPtr(1) : IntPtr.Zero;
                    }
                }
                case NativeApi.PrintDocumentEvent.EndPrint:
                {
                    {
                        var cea = new CancelEventArgs();
                        EndPrint?.Invoke(this, cea);
                        return cea.Cancel ? new IntPtr(1) : IntPtr.Zero;
                    }
                }
                default: throw new Exception("Unexpected PrintDocumentEvent value: " + e);
            }
        }
        
        public event EventHandler<CancelEventArgs>? PrintPage;
        public event EventHandler<CancelEventArgs>? BeginPrint;
        public event EventHandler<CancelEventArgs>? EndPrint;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr PrintDocumentEventCallbackType(IntPtr obj, PrintDocumentEvent e, IntPtr param);
            
            public enum PrintDocumentEvent
            {
                PrintPage,
                BeginPrint,
                EndPrint,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDocument_SetEventCallback_(PrintDocumentEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintDocument_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PrintDocument_GetOriginAtMargins_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDocument_SetOriginAtMargins_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string PrintDocument_GetDocumentName_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDocument_SetDocumentName_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintDocument_GetPrinterSettings_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintDocument_GetPageSettings_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr PrintDocument_GetPrintPage_DrawingContext_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool PrintDocument_GetPrintPage_HasMorePages_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDocument_SetPrintPage_HasMorePages_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Rect PrintDocument_GetPrintPage_MarginBounds_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Rect PrintDocument_GetPrintPage_PhysicalPageBounds_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Rect PrintDocument_GetPrintPage_PageBounds_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Rect PrintDocument_GetPrintPage_PrintablePageBounds_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int PrintDocument_GetPrintPage_PageNumber_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void PrintDocument_Print_(IntPtr obj);
            
        }
    }
}
