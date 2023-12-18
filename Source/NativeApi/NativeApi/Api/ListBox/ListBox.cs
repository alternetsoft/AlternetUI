#pragma warning disable
using Alternet.Drawing;
using System;

namespace NativeApi.Api
{
    public class ListBox : Control
    {
        public static IntPtr CreateEx(long styles) => default;

        public event EventHandler? SelectionChanged;
        public event EventHandler? ControlRecreated;

        public bool HasBorder { get; set; }
        public int ItemsCount { get; }
        public ListBoxSelectionMode SelectionMode { get; set; }
        public int[] SelectedIndices { get; }

        public void InsertItem(int index, string value) { }
        public void RemoveItemAt(int index) { }
        public void ClearItems() { }
        public void ClearSelected() { }
        public void SetSelected(int index, bool value) { }
        public void EnsureVisible(int itemIndex) { }
        public int ItemHitTest(PointD position) => default;
        public void SetItem(int index, string value) { }
    }
}