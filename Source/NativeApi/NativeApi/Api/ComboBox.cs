using System;

namespace NativeApi.Api
{
    public class ComboBox : Control
    {
        public event EventHandler? SelectedItemChanged { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler? TextChanged { add => throw new Exception(); remove => throw new Exception(); }

        public int ItemsCount { get; }

        public bool IsEditable { get; set; }

        public int SelectedIndex { get; set; }

        public string Text { get => throw new Exception(); set => throw new Exception(); }

        public void InsertItem(int index, string value) => throw new Exception();

        public IntPtr CreateItemsInsertion() => throw new Exception();
        public void AddItemToInsertion(IntPtr insertion, string item) => throw new Exception();
        public void CommitItemsInsertion(IntPtr insertion, int index) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public void ClearItems() => throw new Exception();

        public int TextSelectionStart { get; }

        public int TextSelectionLength { get; }

        public void SelectTextRange(int start, int length) => throw new Exception();

        public void SelectAllText() => throw new Exception();
    }
}