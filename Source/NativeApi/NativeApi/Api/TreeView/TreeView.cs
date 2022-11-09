using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class TreeView : Control
    {
        public ImageList? ImageList { get; set; }

        public IntPtr RootItem { get; }

        public int GetItemCount(IntPtr parentItem) => throw new Exception();

        public IntPtr InsertItem(IntPtr parentItem, IntPtr insertAfter, string text, int imageIndex, bool parentIsExpanded) => throw new Exception();

        public void RemoveItem(IntPtr item) => throw new Exception();

        public void ClearItems(IntPtr parentItem) => throw new Exception();

        public event EventHandler? SelectionChanged { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? ControlRecreated { add => throw new Exception(); remove => throw new Exception(); }

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

        public IntPtr FocusedItem { get; set; }

        public IntPtr ItemHitTest(Point point) => throw new Exception();
        public TreeViewHitTestLocations GetHitTestResultLocations(IntPtr hitTestResult) => throw new Exception();
        public IntPtr GetHitTestResultItem(IntPtr hitTestResult) => throw new Exception();
        public void FreeHitTestResult(IntPtr hitTestResult) => throw new Exception();

        public bool IsItemSelected(IntPtr item) => throw new Exception();

        public void BeginLabelEdit(IntPtr item) => throw new Exception();
        public void EndLabelEdit(IntPtr item) => throw new Exception();
        public void ExpandAllChildren(IntPtr item) => throw new Exception();
        public void CollapseAllChildren(IntPtr item) => throw new Exception();
        public void EnsureVisible(IntPtr item) => throw new Exception();
        public void ScrollIntoView(IntPtr item) => throw new Exception();

        public event NativeEventHandler<TreeViewItemEventData>? ItemExpanded { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<TreeViewItemEventData>? ItemCollapsed { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<TreeViewItemEventData>? ItemExpanding { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<TreeViewItemEventData>? ItemCollapsing { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<TreeViewItemLabelEditEventData>? BeforeItemLabelEdit { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<TreeViewItemLabelEditEventData>? AfterItemLabelEdit { add => throw new Exception(); remove => throw new Exception(); }
    }
}