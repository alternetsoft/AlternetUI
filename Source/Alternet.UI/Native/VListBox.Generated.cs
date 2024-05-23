// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class VListBox : Control
    {
        static VListBox()
        {
            SetEventCallback();
        }
        
        public VListBox()
        {
            SetNativePointer(NativeApi.VListBox_Create_());
        }
        
        public VListBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool VScrollBarVisible
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetVScrollBarVisible_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.VListBox_SetVScrollBarVisible_(NativePointer, value);
            }
        }
        
        public bool HScrollBarVisible
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetHScrollBarVisible_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.VListBox_SetHScrollBarVisible_(NativePointer, value);
            }
        }
        
        public System.IntPtr EventDcHandle
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetEventDcHandle_(NativePointer);
            }
            
        }
        
        public DrawingContext EventDc
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.VListBox_GetEventDc_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<DrawingContext>(_nnn, p => new DrawingContext(p))!;
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
        }
        
        public Alternet.Drawing.RectI EventRect
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetEventRect_(NativePointer);
            }
            
        }
        
        public int EventItem
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetEventItem_(NativePointer);
            }
            
        }
        
        public int EventHeight
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetEventHeight_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.VListBox_SetEventHeight_(NativePointer, value);
            }
        }
        
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetHasBorder_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.VListBox_SetHasBorder_(NativePointer, value);
            }
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetItemsCount_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.VListBox_SetItemsCount_(NativePointer, value);
            }
        }
        
        public Alternet.UI.ListBoxSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.VListBox_GetSelectionMode_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.VListBox_SetSelectionMode_(NativePointer, value);
            }
        }
        
        public Alternet.Drawing.RectI GetItemRectI(int index)
        {
            CheckDisposed();
            return NativeApi.VListBox_GetItemRectI_(NativePointer, index);
        }
        
        public bool ScrollRows(int rows)
        {
            CheckDisposed();
            return NativeApi.VListBox_ScrollRows_(NativePointer, rows);
        }
        
        public bool ScrollRowPages(int pages)
        {
            CheckDisposed();
            return NativeApi.VListBox_ScrollRowPages_(NativePointer, pages);
        }
        
        public void RefreshRow(int row)
        {
            CheckDisposed();
            NativeApi.VListBox_RefreshRow_(NativePointer, row);
        }
        
        public void RefreshRows(int from, int to)
        {
            CheckDisposed();
            NativeApi.VListBox_RefreshRows_(NativePointer, from, to);
        }
        
        public int GetVisibleEnd()
        {
            CheckDisposed();
            return NativeApi.VListBox_GetVisibleEnd_(NativePointer);
        }
        
        public int GetVisibleBegin()
        {
            CheckDisposed();
            return NativeApi.VListBox_GetVisibleBegin_(NativePointer);
        }
        
        public int GetRowHeight(int line)
        {
            CheckDisposed();
            return NativeApi.VListBox_GetRowHeight_(NativePointer, line);
        }
        
        public bool IsSelected(int line)
        {
            CheckDisposed();
            return NativeApi.VListBox_IsSelected_(NativePointer, line);
        }
        
        public bool IsVisible(int line)
        {
            CheckDisposed();
            return NativeApi.VListBox_IsVisible_(NativePointer, line);
        }
        
        public static System.IntPtr CreateEx(long styles)
        {
            return NativeApi.VListBox_CreateEx_(styles);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.VListBox_ClearItems_(NativePointer);
        }
        
        public void ClearSelected()
        {
            CheckDisposed();
            NativeApi.VListBox_ClearSelected_(NativePointer);
        }
        
        public void SetSelected(int index, bool value)
        {
            CheckDisposed();
            NativeApi.VListBox_SetSelected_(NativePointer, index, value);
        }
        
        public int GetFirstSelected()
        {
            CheckDisposed();
            return NativeApi.VListBox_GetFirstSelected_(NativePointer);
        }
        
        public int GetNextSelected()
        {
            CheckDisposed();
            return NativeApi.VListBox_GetNextSelected_(NativePointer);
        }
        
        public int GetSelectedCount()
        {
            CheckDisposed();
            return NativeApi.VListBox_GetSelectedCount_(NativePointer);
        }
        
        public int GetSelection()
        {
            CheckDisposed();
            return NativeApi.VListBox_GetSelection_(NativePointer);
        }
        
        public void EnsureVisible(int itemIndex)
        {
            CheckDisposed();
            NativeApi.VListBox_EnsureVisible_(NativePointer, itemIndex);
        }
        
        public int ItemHitTest(Alternet.Drawing.PointD position)
        {
            CheckDisposed();
            return NativeApi.VListBox_ItemHitTest_(NativePointer, position);
        }
        
        public void SetSelection(int selection)
        {
            CheckDisposed();
            NativeApi.VListBox_SetSelection_(NativePointer, selection);
        }
        
        public void SetSelectionBackground(Alternet.Drawing.Color color)
        {
            CheckDisposed();
            NativeApi.VListBox_SetSelectionBackground_(NativePointer, color);
        }
        
        public bool IsCurrent(int current)
        {
            CheckDisposed();
            return NativeApi.VListBox_IsCurrent_(NativePointer, current);
        }
        
        public bool DoSetCurrent(int current)
        {
            CheckDisposed();
            return NativeApi.VListBox_DoSetCurrent_(NativePointer, current);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.VListBoxEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<VListBox>(obj, p => new VListBox(p));
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.VListBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.VListBoxEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.VListBoxEvent.SelectionChanged:
                {
                    SelectionChanged?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.VListBoxEvent.MeasureItem:
                {
                    MeasureItem?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.VListBoxEvent.DrawItem:
                {
                    DrawItem?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.VListBoxEvent.DrawItemBackground:
                {
                    DrawItemBackground?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.VListBoxEvent.ControlRecreated:
                {
                    ControlRecreated?.Invoke(); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected VListBoxEvent value: " + e);
            }
        }
        
        public Action? SelectionChanged;
        public Action? MeasureItem;
        public Action? DrawItem;
        public Action? DrawItemBackground;
        public Action? ControlRecreated;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr VListBoxEventCallbackType(IntPtr obj, VListBoxEvent e, IntPtr param);
            
            public enum VListBoxEvent
            {
                SelectionChanged,
                MeasureItem,
                DrawItem,
                DrawItemBackground,
                ControlRecreated,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetEventCallback_(VListBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr VListBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_GetVScrollBarVisible_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetVScrollBarVisible_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_GetHScrollBarVisible_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetHScrollBarVisible_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr VListBox_GetEventDcHandle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr VListBox_GetEventDc_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectI VListBox_GetEventRect_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetEventItem_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetEventHeight_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetEventHeight_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_GetHasBorder_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetHasBorder_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetItemsCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetItemsCount_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.ListBoxSelectionMode VListBox_GetSelectionMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetSelectionMode_(IntPtr obj, Alternet.UI.ListBoxSelectionMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectI VListBox_GetItemRectI_(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_ScrollRows_(IntPtr obj, int rows);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_ScrollRowPages_(IntPtr obj, int pages);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_RefreshRow_(IntPtr obj, int row);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_RefreshRows_(IntPtr obj, int from, int to);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetVisibleEnd_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetVisibleBegin_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetRowHeight_(IntPtr obj, int line);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_IsSelected_(IntPtr obj, int line);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_IsVisible_(IntPtr obj, int line);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr VListBox_CreateEx_(long styles);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_ClearItems_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_ClearSelected_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetSelected_(IntPtr obj, int index, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetFirstSelected_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetNextSelected_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetSelectedCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_GetSelection_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_EnsureVisible_(IntPtr obj, int itemIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int VListBox_ItemHitTest_(IntPtr obj, Alternet.Drawing.PointD position);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetSelection_(IntPtr obj, int selection);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void VListBox_SetSelectionBackground_(IntPtr obj, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_IsCurrent_(IntPtr obj, int current);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool VListBox_DoSetCurrent_(IntPtr obj, int current);
            
        }
    }
}
