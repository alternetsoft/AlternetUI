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

        public void RemoveItemAt(int index) => throw new Exception();

        public void ClearItems() => throw new Exception();
    }
}