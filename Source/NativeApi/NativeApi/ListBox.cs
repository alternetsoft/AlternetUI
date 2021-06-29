using System;

namespace NativeApi.Api
{
    public class ListBox : Control
    {
        public event EventHandler? SelectionChanged { add => throw new Exception(); remove => throw new Exception(); }

        public int ItemsCount { get; }

        public ListBoxSelectionMode SelectionMode { get; set; }

        public int SelectedIndicesCount { get; }

        public void InsertItem(int index, string value) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public void ClearItems() => throw new Exception();

        public int GetSelectedIndexAt(int index) => throw new Exception();

        public void ClearSelection() => throw new Exception();

        public void SetSelected(int index, bool value) => throw new Exception();
    }
}