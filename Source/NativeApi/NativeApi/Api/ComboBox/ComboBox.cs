#pragma warning disable
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class ComboBox : Control
    {
        public event EventHandler? SelectedItemChanged;
        public event EventHandler? TextChanged;
        public event EventHandler? MeasureItem;
        public event EventHandler? MeasureItemWidth;
        public event EventHandler? DrawItem;
        public event EventHandler? DrawItemBackground;

        public string EmptyTextHint { get; set; }
        public bool HasBorder { get; set; }
        public int ItemsCount { get; }
        public bool IsEditable { get; set; }
        public int SelectedIndex { get; set; }
        public string Text { get; set; }
        public int TextSelectionStart { get; }
        public int TextSelectionLength { get; }

        public PointI TextMargins { get; }

        public int OwnerDrawStyle { get; set; }

        public IntPtr PopupWidget { get; }    
        public IntPtr EventDc { get; }
        public RectI EventRect { get; }
        public int EventItem { get; }
        public int EventFlags { get; }
        public int EventResultInt { get; set; }
        public bool EventCalled { get; set; }

        public int DefaultOnMeasureItemWidth() => default;
        public int DefaultOnMeasureItem() => default;
        public void DefaultOnDrawBackground() { }
        public void DefaultOnDrawItem() { }

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