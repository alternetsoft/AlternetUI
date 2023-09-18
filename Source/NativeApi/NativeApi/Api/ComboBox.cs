#pragma warning disable
using System;

namespace NativeApi.Api
{
    public class ComboBox : Control
    {
        public event EventHandler? SelectedItemChanged;
        public event EventHandler? TextChanged;

        public bool HasBorder { get; set; }
        public int ItemsCount { get; }
        public bool IsEditable { get; set; }
        public int SelectedIndex { get; set; }
        public string Text { get; set; }
        public int TextSelectionStart { get; }
        public int TextSelectionLength { get; }

        public IntPtr CreateItemsInsertion() => default;
        public void AddItemToInsertion(IntPtr insertion, string item) { }
        public void CommitItemsInsertion(IntPtr insertion, int index) { }
        public void InsertItem(int index, string value) { }
        public void RemoveItemAt(int index) { }
        public void ClearItems() { }
        public void SelectTextRange(int start, int length) { }
        public void SelectAllText() { }

        public void SetItem(int index, string value) {}
    }
}