using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class ListBox : Control
    {
        public bool HasBorder { get; set; }
        public static IntPtr CreateEx(long styles) => throw new Exception();

        public event EventHandler? SelectionChanged { add => throw new Exception(); remove => throw new Exception(); }

        public int ItemsCount { get; }

        public ListBoxSelectionMode SelectionMode { get; set; }

        public int[] SelectedIndices { get => throw new Exception(); }

        public void InsertItem(int index, string value) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public void ClearItems() => throw new Exception();

        public void ClearSelected() => throw new Exception();

        public void SetSelected(int index, bool value) => throw new Exception();

        public void EnsureVisible(int itemIndex) => throw new Exception();
        
        public int ItemHitTest(Point position) => throw new Exception();
    }
}