// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>
#nullable enable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class TreeView : Control
    {
        static TreeView()
        {
            SetEventCallback();
        }
        
        public TreeView()
        {
            SetNativePointer(NativeApi.TreeView_Create_());
        }
        
        public TreeView(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public ImageList? ImageList
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetImageList_(NativePointer);
                var m = NativeObject.GetFromNativePointer<ImageList>(n, p => new ImageList(p));
                ReleaseNativeObjectPointer(n);
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetImageList_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public System.IntPtr RootItem
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetRootItem_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public TreeViewSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetSelectionMode_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetSelectionMode_(NativePointer, value);
            }
        }
        
        public System.IntPtr[] SelectedItems
        {
            get
            {
                CheckDisposed();
                var array = NativeApi.TreeView_OpenSelectedItemsArray_(NativePointer);
                try
                {
                    var count = NativeApi.TreeView_GetSelectedItemsItemCount_(NativePointer, array);
                    var result = new System.Collections.Generic.List<System.IntPtr>(count);
                    for (int i = 0; i < count; i++)
                    {
                        var n = NativeApi.TreeView_GetSelectedItemsItemAt_(NativePointer, array, i);
                        var item = n;
                        result.Add(item);
                    }
                    return result.ToArray();
                }
                finally
                {
                    NativeApi.TreeView_CloseSelectedItemsArray_(NativePointer, array);
                }
            }
            
        }
        
        public bool ShowLines
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetShowLines_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetShowLines_(NativePointer, value);
            }
        }
        
        public bool ShowRootLines
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetShowRootLines_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetShowRootLines_(NativePointer, value);
            }
        }
        
        public bool ShowExpandButtons
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetShowExpandButtons_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetShowExpandButtons_(NativePointer, value);
            }
        }
        
        public System.IntPtr TopItem
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetTopItem_(NativePointer);
                var m = n;
                return m;
            }
            
        }
        
        public bool FullRowSelect
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetFullRowSelect_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetFullRowSelect_(NativePointer, value);
            }
        }
        
        public bool AllowLabelEdit
        {
            get
            {
                CheckDisposed();
                var n = NativeApi.TreeView_GetAllowLabelEdit_(NativePointer);
                var m = n;
                return m;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetAllowLabelEdit_(NativePointer, value);
            }
        }
        
        public int GetItemCount(System.IntPtr parentItem)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_GetItemCount_(NativePointer, parentItem);
            var m = n;
            return m;
        }
        
        public System.IntPtr InsertItem(System.IntPtr parentItem, System.IntPtr insertAfter, string text, int imageIndex, bool parentIsExpanded)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_InsertItem_(NativePointer, parentItem, insertAfter, text, imageIndex, parentIsExpanded);
            var m = n;
            return m;
        }
        
        public void RemoveItem(System.IntPtr item)
        {
            CheckDisposed();
            NativeApi.TreeView_RemoveItem_(NativePointer, item);
        }
        
        public void ClearItems(System.IntPtr parentItem)
        {
            CheckDisposed();
            NativeApi.TreeView_ClearItems_(NativePointer, parentItem);
        }
        
        public void ClearSelected()
        {
            CheckDisposed();
            NativeApi.TreeView_ClearSelected_(NativePointer);
        }
        
        public void SetSelected(System.IntPtr item, bool value)
        {
            CheckDisposed();
            NativeApi.TreeView_SetSelected_(NativePointer, item, value);
        }
        
        public void ExpandAll()
        {
            CheckDisposed();
            NativeApi.TreeView_ExpandAll_(NativePointer);
        }
        
        public void CollapseAll()
        {
            CheckDisposed();
            NativeApi.TreeView_CollapseAll_(NativePointer);
        }
        
        public System.IntPtr ItemHitTest(Alternet.Drawing.Point point)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_ItemHitTest_(NativePointer, point);
            var m = n;
            return m;
        }
        
        public TreeViewHitTestLocations GetHitTestResultLocations(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_GetHitTestResultLocations_(NativePointer, hitTestResult);
            var m = n;
            return m;
        }
        
        public System.IntPtr GetHitTestResultItem(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_GetHitTestResultItem_(NativePointer, hitTestResult);
            var m = n;
            return m;
        }
        
        public void FreeHitTestResult(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            NativeApi.TreeView_FreeHitTestResult_(NativePointer, hitTestResult);
        }
        
        public bool IsItemSelected(System.IntPtr item)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_IsItemSelected_(NativePointer, item);
            var m = n;
            return m;
        }
        
        public void SetFocused(System.IntPtr item, bool value)
        {
            CheckDisposed();
            NativeApi.TreeView_SetFocused_(NativePointer, item, value);
        }
        
        public bool IsFocused(System.IntPtr item)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_IsFocused_(NativePointer, item);
            var m = n;
            return m;
        }
        
        public void SetItemText(System.IntPtr item, string text)
        {
            CheckDisposed();
            NativeApi.TreeView_SetItemText_(NativePointer, item, text);
        }
        
        public string GetItemText(System.IntPtr item)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_GetItemText_(NativePointer, item);
            var m = n;
            return m;
        }
        
        public void SetItemImageIndex(System.IntPtr item, int imageIndex)
        {
            CheckDisposed();
            NativeApi.TreeView_SetItemImageIndex_(NativePointer, item, imageIndex);
        }
        
        public int GetItemImageIndex(System.IntPtr item)
        {
            CheckDisposed();
            var n = NativeApi.TreeView_GetItemImageIndex_(NativePointer, item);
            var m = n;
            return m;
        }
        
        public void BeginLabelEdit(System.IntPtr item)
        {
            CheckDisposed();
            NativeApi.TreeView_BeginLabelEdit_(NativePointer, item);
        }
        
        public void EndLabelEdit(System.IntPtr item, bool cancel)
        {
            CheckDisposed();
            NativeApi.TreeView_EndLabelEdit_(NativePointer, item, cancel);
        }
        
        public void ExpandAllChildren(System.IntPtr item)
        {
            CheckDisposed();
            NativeApi.TreeView_ExpandAllChildren_(NativePointer, item);
        }
        
        public void CollapseAllChildren(System.IntPtr item)
        {
            CheckDisposed();
            NativeApi.TreeView_CollapseAllChildren_(NativePointer, item);
        }
        
        public void EnsureVisible(System.IntPtr item)
        {
            CheckDisposed();
            NativeApi.TreeView_EnsureVisible_(NativePointer, item);
        }
        
        public void ScrollIntoView(System.IntPtr item)
        {
            CheckDisposed();
            NativeApi.TreeView_ScrollIntoView_(NativePointer, item);
        }
        
        static GCHandle eventCallbackGCHandle;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TreeViewEventCallbackType((obj, e, parameter) =>
                UI.Application.HandleThreadExceptions(() =>
                {
                    var w = NativeObject.GetFromNativePointer<TreeView>(obj, p => new TreeView(p));
                    if (w == null) return IntPtr.Zero;
                    return w.OnEvent(e, parameter);
                }
                ));
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.TreeView_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.TreeViewEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.TreeViewEvent.SelectionChanged:
                {
                    SelectionChanged?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                case NativeApi.TreeViewEvent.ControlRecreated:
                {
                    ControlRecreated?.Invoke(this, EventArgs.Empty); return IntPtr.Zero;
                }
                case NativeApi.TreeViewEvent.ItemExpanded:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    ItemExpanded?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.ItemCollapsed:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    ItemCollapsed?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.ItemExpanding:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    ItemExpanding?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.ItemCollapsing:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    ItemCollapsing?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.BeforeItemLabelEdit:
                {
                    var ea = new NativeEventArgs<TreeViewItemLabelEditEventData>(MarshalEx.PtrToStructure<TreeViewItemLabelEditEventData>(parameter));
                    BeforeItemLabelEdit?.Invoke(this, ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.AfterItemLabelEdit:
                {
                    var ea = new NativeEventArgs<TreeViewItemLabelEditEventData>(MarshalEx.PtrToStructure<TreeViewItemLabelEditEventData>(parameter));
                    AfterItemLabelEdit?.Invoke(this, ea); return ea.Result;
                }
                default: throw new Exception("Unexpected TreeViewEvent value: " + e);
            }
        }
        
        public event EventHandler? SelectionChanged;
        public event EventHandler? ControlRecreated;
        public event NativeEventHandler<TreeViewItemEventData>? ItemExpanded;
        public event NativeEventHandler<TreeViewItemEventData>? ItemCollapsed;
        public event NativeEventHandler<TreeViewItemEventData>? ItemExpanding;
        public event NativeEventHandler<TreeViewItemEventData>? ItemCollapsing;
        public event NativeEventHandler<TreeViewItemLabelEditEventData>? BeforeItemLabelEdit;
        public event NativeEventHandler<TreeViewItemLabelEditEventData>? AfterItemLabelEdit;
        
        [SuppressUnmanagedCodeSecurity]
        private class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr TreeViewEventCallbackType(IntPtr obj, TreeViewEvent e, IntPtr param);
            
            public enum TreeViewEvent
            {
                SelectionChanged,
                ControlRecreated,
                ItemExpanded,
                ItemCollapsed,
                ItemExpanding,
                ItemCollapsing,
                BeforeItemLabelEdit,
                AfterItemLabelEdit,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetEventCallback_(TreeViewEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TreeView_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TreeView_GetImageList_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetImageList_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetRootItem_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern TreeViewSelectionMode TreeView_GetSelectionMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetSelectionMode_(IntPtr obj, TreeViewSelectionMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetShowLines_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetShowLines_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetShowRootLines_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetShowRootLines_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetShowExpandButtons_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetShowExpandButtons_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetTopItem_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetFullRowSelect_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetFullRowSelect_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetAllowLabelEdit_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetAllowLabelEdit_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_OpenSelectedItemsArray_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TreeView_GetSelectedItemsItemCount_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetSelectedItemsItemAt_(IntPtr obj, System.IntPtr array, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_CloseSelectedItemsArray_(IntPtr obj, System.IntPtr array);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TreeView_GetItemCount_(IntPtr obj, System.IntPtr parentItem);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_InsertItem_(IntPtr obj, System.IntPtr parentItem, System.IntPtr insertAfter, string text, int imageIndex, bool parentIsExpanded);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_RemoveItem_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ClearItems_(IntPtr obj, System.IntPtr parentItem);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ClearSelected_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetSelected_(IntPtr obj, System.IntPtr item, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ExpandAll_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_CollapseAll_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_ItemHitTest_(IntPtr obj, NativeApiTypes.Point point);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern TreeViewHitTestLocations TreeView_GetHitTestResultLocations_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetHitTestResultItem_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_FreeHitTestResult_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_IsItemSelected_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetFocused_(IntPtr obj, System.IntPtr item, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_IsFocused_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetItemText_(IntPtr obj, System.IntPtr item, string text);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TreeView_GetItemText_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetItemImageIndex_(IntPtr obj, System.IntPtr item, int imageIndex);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TreeView_GetItemImageIndex_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_BeginLabelEdit_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_EndLabelEdit_(IntPtr obj, System.IntPtr item, bool cancel);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ExpandAllChildren_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_CollapseAllChildren_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_EnsureVisible_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ScrollIntoView_(IntPtr obj, System.IntPtr item);
            
        }
    }
}
