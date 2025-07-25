// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class TreeView : Control
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
        
        public long CreateStyle
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetCreateStyle_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetCreateStyle_(NativePointer, value);
            }
        }
        
        public bool HideRoot
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetHideRoot_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetHideRoot_(NativePointer, value);
            }
        }
        
        public bool VariableRowHeight
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetVariableRowHeight_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetVariableRowHeight_(NativePointer, value);
            }
        }
        
        public bool TwistButtons
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetTwistButtons_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetTwistButtons_(NativePointer, value);
            }
        }
        
        public uint StateImageSpacing
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetStateImageSpacing_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetStateImageSpacing_(NativePointer, value);
            }
        }
        
        public uint Indentation
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetIndentation_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetIndentation_(NativePointer, value);
            }
        }
        
        public bool RowLines
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetRowLines_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetRowLines_(NativePointer, value);
            }
        }
        
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetHasBorder_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetHasBorder_(NativePointer, value);
            }
        }
        
        public ImageList? ImageList
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.TreeView_GetImageList_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<ImageList>(_nnn, p => new ImageList(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
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
                return NativeApi.TreeView_GetRootItem_(NativePointer);
            }
            
        }
        
        public Alternet.UI.TreeViewSelectionMode SelectionMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetSelectionMode_(NativePointer);
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
                return NativeApi.TreeView_GetShowLines_(NativePointer);
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
                return NativeApi.TreeView_GetShowRootLines_(NativePointer);
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
                return NativeApi.TreeView_GetShowExpandButtons_(NativePointer);
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
                return NativeApi.TreeView_GetTopItem_(NativePointer);
            }
            
        }
        
        public bool FullRowSelect
        {
            get
            {
                CheckDisposed();
                return NativeApi.TreeView_GetFullRowSelect_(NativePointer);
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
                return NativeApi.TreeView_GetAllowLabelEdit_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TreeView_SetAllowLabelEdit_(NativePointer, value);
            }
        }
        
        public static void SetItemBold(System.IntPtr handle, System.IntPtr item, bool bold)
        {
            NativeApi.TreeView_SetItemBold_(handle, item, bold);
        }
        
        public static Alternet.Drawing.Color GetItemTextColor(System.IntPtr handle, System.IntPtr item)
        {
            return NativeApi.TreeView_GetItemTextColor_(handle, item);
        }
        
        public static Alternet.Drawing.Color GetItemBackgroundColor(System.IntPtr handle, System.IntPtr item)
        {
            return NativeApi.TreeView_GetItemBackgroundColor_(handle, item);
        }
        
        public static void SetItemTextColor(System.IntPtr handle, System.IntPtr item, Alternet.Drawing.Color color)
        {
            NativeApi.TreeView_SetItemTextColor_(handle, item, color);
        }
        
        public static void SetItemBackgroundColor(System.IntPtr handle, System.IntPtr item, Alternet.Drawing.Color color)
        {
            NativeApi.TreeView_SetItemBackgroundColor_(handle, item, color);
        }
        
        public static void ResetItemTextColor(System.IntPtr handle, System.IntPtr item)
        {
            NativeApi.TreeView_ResetItemTextColor_(handle, item);
        }
        
        public static void ResetItemBackgroundColor(System.IntPtr handle, System.IntPtr item)
        {
            NativeApi.TreeView_ResetItemBackgroundColor_(handle, item);
        }
        
        public void SetNodeUniqueId(System.IntPtr node, long uniqueId)
        {
            CheckDisposed();
            NativeApi.TreeView_SetNodeUniqueId_(NativePointer, node, uniqueId);
        }
        
        public long GetNodeUniqueId(System.IntPtr node)
        {
            CheckDisposed();
            return NativeApi.TreeView_GetNodeUniqueId_(NativePointer, node);
        }
        
        public void MakeAsListBox()
        {
            CheckDisposed();
            NativeApi.TreeView_MakeAsListBox_(NativePointer);
        }
        
        public int GetItemCount(System.IntPtr parentItem)
        {
            CheckDisposed();
            return NativeApi.TreeView_GetItemCount_(NativePointer, parentItem);
        }
        
        public System.IntPtr InsertItem(System.IntPtr parentItem, System.IntPtr insertAfter, string text, int imageIndex, bool parentIsExpanded)
        {
            CheckDisposed();
            return NativeApi.TreeView_InsertItem_(NativePointer, parentItem, insertAfter, text, imageIndex, parentIsExpanded);
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
        
        public System.IntPtr ItemHitTest(Alternet.Drawing.PointD point)
        {
            CheckDisposed();
            return NativeApi.TreeView_ItemHitTest_(NativePointer, point);
        }
        
        public Alternet.UI.TreeViewHitTestLocations GetHitTestResultLocations(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            return NativeApi.TreeView_GetHitTestResultLocations_(NativePointer, hitTestResult);
        }
        
        public System.IntPtr GetHitTestResultItem(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            return NativeApi.TreeView_GetHitTestResultItem_(NativePointer, hitTestResult);
        }
        
        public void FreeHitTestResult(System.IntPtr hitTestResult)
        {
            CheckDisposed();
            NativeApi.TreeView_FreeHitTestResult_(NativePointer, hitTestResult);
        }
        
        public bool IsItemSelected(System.IntPtr item)
        {
            CheckDisposed();
            return NativeApi.TreeView_IsItemSelected_(NativePointer, item);
        }
        
        public void SetFocused(System.IntPtr item, bool value)
        {
            CheckDisposed();
            NativeApi.TreeView_SetFocused_(NativePointer, item, value);
        }
        
        public bool IsItemFocused(System.IntPtr item)
        {
            CheckDisposed();
            return NativeApi.TreeView_IsItemFocused_(NativePointer, item);
        }
        
        public void SetItemText(System.IntPtr item, string text)
        {
            CheckDisposed();
            NativeApi.TreeView_SetItemText_(NativePointer, item, text);
        }
        
        public string GetItemText(System.IntPtr item)
        {
            CheckDisposed();
            return NativeApi.TreeView_GetItemText_(NativePointer, item);
        }
        
        public void SetItemImageIndex(System.IntPtr item, int imageIndex)
        {
            CheckDisposed();
            NativeApi.TreeView_SetItemImageIndex_(NativePointer, item, imageIndex);
        }
        
        public int GetItemImageIndex(System.IntPtr item)
        {
            CheckDisposed();
            return NativeApi.TreeView_GetItemImageIndex_(NativePointer, item);
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
        public static TreeView? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TreeViewEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<TreeView>(obj, p => new TreeView(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
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
                    OnPlatformEventSelectionChanged(); return IntPtr.Zero;
                }
                case NativeApi.TreeViewEvent.ControlRecreated:
                {
                    OnPlatformEventControlRecreated(); return IntPtr.Zero;
                }
                case NativeApi.TreeViewEvent.ItemExpanded:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    OnPlatformEventItemExpanded(ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.ItemCollapsed:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    OnPlatformEventItemCollapsed(ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.ItemExpanding:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    OnPlatformEventItemExpanding(ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.ItemCollapsing:
                {
                    var ea = new NativeEventArgs<TreeViewItemEventData>(MarshalEx.PtrToStructure<TreeViewItemEventData>(parameter));
                    OnPlatformEventItemCollapsing(ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.BeforeItemLabelEdit:
                {
                    var ea = new NativeEventArgs<TreeViewItemLabelEditEventData>(MarshalEx.PtrToStructure<TreeViewItemLabelEditEventData>(parameter));
                    OnPlatformEventBeforeItemLabelEdit(ea); return ea.Result;
                }
                case NativeApi.TreeViewEvent.AfterItemLabelEdit:
                {
                    var ea = new NativeEventArgs<TreeViewItemLabelEditEventData>(MarshalEx.PtrToStructure<TreeViewItemLabelEditEventData>(parameter));
                    OnPlatformEventAfterItemLabelEdit(ea); return ea.Result;
                }
                default: throw new Exception("Unexpected TreeViewEvent value: " + e);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
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
            public static extern long TreeView_GetCreateStyle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetCreateStyle_(IntPtr obj, long value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetHideRoot_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetHideRoot_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetVariableRowHeight_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetVariableRowHeight_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetTwistButtons_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetTwistButtons_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint TreeView_GetStateImageSpacing_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetStateImageSpacing_(IntPtr obj, uint value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint TreeView_GetIndentation_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetIndentation_(IntPtr obj, uint value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetRowLines_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetRowLines_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_GetHasBorder_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetHasBorder_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TreeView_GetImageList_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetImageList_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetRootItem_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.TreeViewSelectionMode TreeView_GetSelectionMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetSelectionMode_(IntPtr obj, Alternet.UI.TreeViewSelectionMode value);
            
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
            public static extern void TreeView_SetItemBold_(System.IntPtr handle, System.IntPtr item, bool bold);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color TreeView_GetItemTextColor_(System.IntPtr handle, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color TreeView_GetItemBackgroundColor_(System.IntPtr handle, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetItemTextColor_(System.IntPtr handle, System.IntPtr item, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetItemBackgroundColor_(System.IntPtr handle, System.IntPtr item, NativeApiTypes.Color color);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ResetItemTextColor_(System.IntPtr handle, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_ResetItemBackgroundColor_(System.IntPtr handle, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetNodeUniqueId_(IntPtr obj, System.IntPtr node, long uniqueId);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long TreeView_GetNodeUniqueId_(IntPtr obj, System.IntPtr node);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_MakeAsListBox_(IntPtr obj);
            
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
            public static extern System.IntPtr TreeView_ItemHitTest_(IntPtr obj, Alternet.Drawing.PointD point);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.TreeViewHitTestLocations TreeView_GetHitTestResultLocations_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TreeView_GetHitTestResultItem_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_FreeHitTestResult_(IntPtr obj, System.IntPtr hitTestResult);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_IsItemSelected_(IntPtr obj, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TreeView_SetFocused_(IntPtr obj, System.IntPtr item, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TreeView_IsItemFocused_(IntPtr obj, System.IntPtr item);
            
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
