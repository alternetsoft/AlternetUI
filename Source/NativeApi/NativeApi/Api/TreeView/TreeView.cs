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

        public event NativeEventHandler<TreeViewItemEventData>? ItemExpanded { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<TreeViewItemEventData>? ItemCollapsed { add => throw new Exception(); remove => throw new Exception(); }
    }
}