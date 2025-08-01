#pragma warning disable
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public class TreeView : Control
    {
        public static void SetItemBold(IntPtr handle, IntPtr item, bool bold = true) { }

        public static Color GetItemTextColor(IntPtr handle, IntPtr item) => default;

        public static Color GetItemBackgroundColor(IntPtr handle, IntPtr item) => default;

        public static void SetItemTextColor(IntPtr handle, IntPtr item, Color color) {}

        public static void SetItemBackgroundColor(IntPtr handle, IntPtr item, Color color) { }

        public static void ResetItemTextColor(IntPtr handle, IntPtr item) { }

        public static void ResetItemBackgroundColor(IntPtr handle, IntPtr item) { }

        public void SetNodeUniqueId(IntPtr node, long uniqueId) { }
        public long GetNodeUniqueId(IntPtr node) => default;

        public void MakeAsListBox() { }

        public long CreateStyle { get; set; }

        public bool HideRoot { get; set; }

        public bool VariableRowHeight { get; set; }

        public bool TwistButtons { get; set; }

        public uint StateImageSpacing { get; set; }

        public uint Indentation { get; set; }

        public bool RowLines { get; set; }

        public bool HasBorder { get; set; }

        public ImageList? ImageList { get; set; }

        public IntPtr RootItem { get; }

        public int GetItemCount(IntPtr parentItem) => throw new Exception();

        public IntPtr InsertItem(IntPtr parentItem, IntPtr insertAfter,
            string text, int imageIndex, bool parentIsExpanded) =>
            throw new Exception();

        public void RemoveItem(IntPtr item) => throw new Exception();

        public void ClearItems(IntPtr parentItem) => throw new Exception();

        public event EventHandler? SelectionChanged
        {
            add => throw new Exception(); remove => throw new Exception();
        }

        public event EventHandler? ControlRecreated
        {
            add => throw new Exception(); remove => throw new Exception();
        }

        public TreeViewSelectionMode SelectionMode { get; set; }

        public IntPtr[] SelectedItems { get => throw new Exception(); }

        public void ClearSelected() => throw new Exception();

        public void SetSelected(IntPtr item, bool value) => throw new Exception();

        public bool ShowLines { get; set; }

        public bool ShowRootLines { get; set; }

        public bool ShowExpandButtons { get; set; }

        public IntPtr TopItem { get; }

        public bool FullRowSelect { get; set; }

        public bool AllowLabelEdit { get; set; }

        public void ExpandAll() => throw new Exception();

        public void CollapseAll() => throw new Exception();

        public IntPtr ItemHitTest(PointD point) => throw new Exception();
        public TreeViewHitTestLocations GetHitTestResultLocations(
            IntPtr hitTestResult) => throw new Exception();
        public IntPtr GetHitTestResultItem(IntPtr hitTestResult) =>
            throw new Exception();
        public void FreeHitTestResult(IntPtr hitTestResult) => throw new Exception();

        public bool IsItemSelected(IntPtr item) => throw new Exception();

        public void SetFocused(IntPtr item, bool value) => throw new Exception();
        public new bool IsItemFocused(IntPtr item) => throw new Exception();

        public void SetItemText(IntPtr item, string text) => throw new Exception();
        public string GetItemText(IntPtr item) => throw new Exception();

        public void SetItemImageIndex(IntPtr item, int imageIndex) =>
            throw new Exception();
        public int GetItemImageIndex(IntPtr item) => throw new Exception();

        public void BeginLabelEdit(IntPtr item) => throw new Exception();
        public void EndLabelEdit(IntPtr item, bool cancel) => throw new Exception();
        public void ExpandAllChildren(IntPtr item) => throw new Exception();
        public void CollapseAllChildren(IntPtr item) => throw new Exception();
        public void EnsureVisible(IntPtr item) => throw new Exception();
        public void ScrollIntoView(IntPtr item) => throw new Exception();

        public event NativeEventHandler<TreeViewItemEventData>? ItemExpanded
        { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<TreeViewItemEventData>? ItemCollapsed
        { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<TreeViewItemEventData>? ItemExpanding
        { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<TreeViewItemEventData>? ItemCollapsing
        { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<TreeViewItemLabelEditEventData>?
            BeforeItemLabelEdit
        {
            add => throw new Exception();
            remove => throw new Exception();
        }
        public event NativeEventHandler<TreeViewItemLabelEditEventData>?
            AfterItemLabelEdit
        {
            add => throw new Exception();
            remove => throw new Exception();
        }
    }
}