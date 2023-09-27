// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class ListView : Control
    {
        static ListView()
        {
            SetEventCallback();
        }
        
        public ListView()
        {
            SetNativePointer(NativeApi.ListView_Create_());
        }
        
        public ListView(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetHasBorder_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetHasBorder_(NativePointer, value);
            }
        }
        
        public long ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetItemsCount_(NativePointer);
            }
            
        }
        
        public ImageList? SmallImageList
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.ListView_GetSmallImageList_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<ImageList>(_nnn, p => new ImageList(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetSmallImageList_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public ImageList? LargeImageList
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.ListView_GetLargeImageList_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<ImageList>(_nnn, p => new ImageList(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetLargeImageList_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public ListViewView CurrentView
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetCurrentView_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetCurrentView_(NativePointer, value);
            }
        }
        
        public ListViewSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetSelectionMode_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetSelectionMode_(NativePointer, value);
            }
        }
        
        public System.Int64[] SelectedIndices
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.ListView_OpenSelectedIndicesArray_(NativePointer);
                try
                {
                    var count = NativeApi.ListView_GetSelectedIndicesItemCount_(NativePointer, array);
                    var result = new System.Collections.Generic.List<long>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var n = NativeApi.ListView_GetSelectedIndicesItemAt_(NativePointer, array, i);
                        var item = n;
                        result.Add(item);
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.ListView_CloseSelectedIndicesArray_(NativePointer, array);
                }
            }
            
        }
        
        public bool AllowLabelEdit
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetAllowLabelEdit_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetAllowLabelEdit_(NativePointer, value);
            }
        }
        
        public long TopItemIndex
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetTopItemIndex_(NativePointer);
            }
            
        }
        
        public ListViewGridLinesDisplayMode GridLinesDisplayMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetGridLinesDisplayMode_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetGridLinesDisplayMode_(NativePointer, value);
            }
        }
        
        public ListViewSortMode SortMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetSortMode_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetSortMode_(NativePointer, value);
            }
        }
        
        public bool ColumnHeaderVisible
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetColumnHeaderVisible_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetColumnHeaderVisible_(NativePointer, value);
            }
        }
        
        public long FocusedItemIndex
        {
            get
            {
                CheckDisposed();
                return NativeApi.ListView_GetFocusedItemIndex_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ListView_SetFocusedItemIndex_(NativePointer, value);
            }
        }
        
        public void InsertItemAt(long index, string text, long columnIndex, int imageIndex)
        {
            CheckDisposed();
            NativeApi.ListView_InsertItemAt_(NativePointer, index, text, columnIndex, imageIndex);
        }
        
        public void RemoveItemAt(long index)
        {
            CheckDisposed();
            NativeApi.ListView_RemoveItemAt_(NativePointer, index);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.ListView_ClearItems_(NativePointer);
        }
        
        public void InsertColumnAt(long index, string header, double width, ListViewColumnWidthMode widthMode)
        {
            CheckDisposed();
            NativeApi.ListView_InsertColumnAt_(NativePointer, index, header, width, widthMode);
        }
        
        public void RemoveColumnAt(long index)
        {
            CheckDisposed();
            NativeApi.ListView_RemoveColumnAt_(NativePointer, index);
        }
        
        public void ClearSelected()
        {
            CheckDisposed();
            NativeApi.ListView_ClearSelected_(NativePointer);
        }
        
        public void SetSelected(long index, bool value)
        {
            CheckDisposed();
            NativeApi.ListView_SetSelected_(NativePointer, index, value);
        }
        
        public System.IntPtr ItemHitTest(Alternet.Drawing.Point point)
        {
            CheckDisposed();
            return NativeApi.ListView_ItemHitTest_(NativePointer, point);
        }
        
        public ListViewHitTestLocations GetHitTestResultLocations(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            return NativeApi.ListView_GetHitTestResultLocations_(NativePointer, hitTestResult);
        }
        
        public long GetHitTestResultItemIndex(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            return NativeApi.ListView_GetHitTestResultItemIndex_(NativePointer, hitTestResult);
        }
        
        public long GetHitTestResultColumnIndex(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            return NativeApi.ListView_GetHitTestResultColumnIndex_(NativePointer, hitTestResult);
        }
        
        public void FreeHitTestResult(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            NativeApi.ListView_FreeHitTestResult_(NativePointer, hitTestResult);
        }
        
        public void BeginLabelEdit(long itemIndex)
        {
            CheckDisposed();
            NativeApi.ListView_BeginLabelEdit_(NativePointer, itemIndex);
        }
        
        public Alternet.Drawing.Rect GetItemBounds(long itemIndex, ListViewItemBoundsPortion portion)
        {
            CheckDisposed();
            return NativeApi.ListView_GetItemBounds_(NativePointer, itemIndex, portion);
        }
        
        public void Clear()
        {
            CheckDisposed();
            NativeApi.ListView_Clear_(NativePointer);
        }
        
        public void EnsureItemVisible(long itemIndex)
        {
            CheckDisposed();
            NativeApi.ListView_EnsureItemVisible_(NativePointer, itemIndex);
        }
        
        public void SetItemText(long itemIndex, long columnIndex, string text)
        {
            CheckDisposed();
            NativeApi.ListView_SetItemText_(NativePointer, itemIndex, columnIndex, text);
        }
        
        public void SetItemImageIndex(long itemIndex, long columnIndex, int imageIndex)
        {
            CheckDisposed();
            NativeApi.ListView_SetItemImageIndex_(NativePointer, itemIndex, columnIndex, imageIndex);
        }
        
        public void SetColumnWidth(long columnIndex, double fixedWidth, ListViewColumnWidthMode widthMode)
        {
            CheckDisposed();
            NativeApi.ListView_SetColumnWidth_(NativePointer, columnIndex, fixedWidth, widthMode);
        }
        
        public void SetColumnTitle(long columnIndex, string text)
        {
            CheckDisposed();
            NativeApi.ListView_SetColumnTitle_(NativePointer, columnIndex, text);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ListViewEventCallbackType((obj, e, parameter) =>
                {
                    var w = NativeObject.GetFromNativePointer<ListView>(obj, p => new ListView(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ListView_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ListViewEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.ListViewEvent.ControlRecreated:
                {
                    ControlRecreated?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                case NativeApi.ListViewEvent.SelectionChanged:
                {
                    SelectionChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                case NativeApi.ListViewEvent.CompareItemsForCustomSort:
                {
                    var ea = new NativeEventArgs<CompareListViewItemsEventData>(MarshalEx.PtrToStructure<CompareListViewItemsEventData>(parameter));
                    CompareItemsForCustomSort?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.ListViewEvent.ColumnClick:
                {
                    var ea = new NativeEventArgs<ListViewColumnEventData>(MarshalEx.PtrToStructure<ListViewColumnEventData>(parameter));
                    ColumnClick?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.ListViewEvent.BeforeItemLabelEdit:
                {
                    var ea = new NativeEventArgs<ListViewItemLabelEditEventData>(MarshalEx.PtrToStructure<ListViewItemLabelEditEventData>(parameter));
                    BeforeItemLabelEdit?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.ListViewEvent.AfterItemLabelEdit:
                {
                    var ea = new NativeEventArgs<ListViewItemLabelEditEventData>(MarshalEx.PtrToStructure<ListViewItemLabelEditEventData>(parameter));
                    AfterItemLabelEdit?.Invoke(this, ea); return ea.Result;
                }
                default: throw new Exception("Unexpected ListViewEvent value: " + e);
            }
        }
        
        public event EventHandler? ControlRecreated;
        public event EventHandler? SelectionChanged;
        public event NativeEventHandler<CompareListViewItemsEventData>? CompareItemsForCustomSort;
        public event NativeEventHandler<ListViewColumnEventData>? ColumnClick;
        public event NativeEventHandler<ListViewItemLabelEditEventData>? BeforeItemLabelEdit;
        public event NativeEventHandler<ListViewItemLabelEditEventData>? AfterItemLabelEdit;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ListViewEventCallbackType(IntPtr obj, ListViewEvent e, IntPtr param);
            
            public enum ListViewEvent
            {
                ControlRecreated,
                SelectionChanged,
                CompareItemsForCustomSort,
                ColumnClick,
                BeforeItemLabelEdit,
                AfterItemLabelEdit,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetEventCallback_(ListViewEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListView_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ListView_GetHasBorder_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetHasBorder_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long ListView_GetItemsCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListView_GetSmallImageList_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetSmallImageList_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ListView_GetLargeImageList_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetLargeImageList_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListViewView ListView_GetCurrentView_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetCurrentView_(IntPtr obj, ListViewView value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListViewSelectionMode ListView_GetSelectionMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetSelectionMode_(IntPtr obj, ListViewSelectionMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ListView_GetAllowLabelEdit_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetAllowLabelEdit_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long ListView_GetTopItemIndex_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListViewGridLinesDisplayMode ListView_GetGridLinesDisplayMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetGridLinesDisplayMode_(IntPtr obj, ListViewGridLinesDisplayMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListViewSortMode ListView_GetSortMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetSortMode_(IntPtr obj, ListViewSortMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ListView_GetColumnHeaderVisible_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetColumnHeaderVisible_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long ListView_GetFocusedItemIndex_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetFocusedItemIndex_(IntPtr obj, long value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ListView_OpenSelectedIndicesArray_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ListView_GetSelectedIndicesItemCount_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long ListView_GetSelectedIndicesItemAt_(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_CloseSelectedIndicesArray_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_InsertItemAt_(IntPtr obj, long index, string text, long columnIndex, int imageIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_RemoveItemAt_(IntPtr obj, long index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_ClearItems_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_InsertColumnAt_(IntPtr obj, long index, string header, double width, ListViewColumnWidthMode widthMode);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_RemoveColumnAt_(IntPtr obj, long index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_ClearSelected_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetSelected_(IntPtr obj, long index, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ListView_ItemHitTest_(IntPtr obj, Alternet.Drawing.Point point);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern ListViewHitTestLocations ListView_GetHitTestResultLocations_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long ListView_GetHitTestResultItemIndex_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long ListView_GetHitTestResultColumnIndex_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_FreeHitTestResult_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_BeginLabelEdit_(IntPtr obj, long itemIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Rect ListView_GetItemBounds_(IntPtr obj, long itemIndex, ListViewItemBoundsPortion portion);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_Clear_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_EnsureItemVisible_(IntPtr obj, long itemIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetItemText_(IntPtr obj, long itemIndex, long columnIndex, string text);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetItemImageIndex_(IntPtr obj, long itemIndex, long columnIndex, int imageIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetColumnWidth_(IntPtr obj, long columnIndex, double fixedWidth, ListViewColumnWidthMode widthMode);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ListView_SetColumnTitle_(IntPtr obj, long columnIndex, string text);
            
        }
    }
}
