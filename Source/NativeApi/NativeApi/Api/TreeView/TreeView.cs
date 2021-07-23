using System;

namespace NativeApi.Api
{
    public class TreeView : Control
    {
        public ImageList? ImageList { get; set; }

        public IntPtr RootItem { get; }

        public int GetItemCount(IntPtr parentItem) => throw new Exception();

        public void InsertItemAt(IntPtr parentItem, int index, string text, int imageIndex) => throw new Exception();

        public void RemoveItem(IntPtr item) => throw new Exception();

        public void ClearItems(IntPtr parentItem) => throw new Exception();

        public event EventHandler? SelectionChanged { add => throw new Exception(); remove => throw new Exception(); }

        public TreeViewSelectionMode SelectionMode { get; set; }

        public IntPtr[] SelectedItems { get => throw new Exception(); }

        public void ClearSelected() => throw new Exception();

        public void SetSelected(IntPtr item, bool value) => throw new Exception();
    }
}