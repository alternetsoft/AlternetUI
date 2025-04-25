#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class VListBox : Control
    {
        public bool VScrollBarVisible { get; set; }
        public bool HScrollBarVisible { get; set; }

        public RectI GetItemRectI(int index) => default;

        public bool ScrollToRow(int row) => default;

        public bool ScrollRows(int rows) => default;
        public bool ScrollRowPages(int pages) => default;
        public void RefreshRow(int row) { }
        public void RefreshRows(int from, int to) { }

        public int GetVisibleEnd() => default;
        public int GetVisibleBegin() => default;
        public int GetRowHeight(int line) => default;

        public bool IsSelected(int line) => default;
        public bool IsVisible(int line) => default;

        public static IntPtr CreateEx(long styles) => default;

        public IntPtr EventDcHandle { get; }
        public DrawingContext EventDc { get; }
        public RectI EventRect { get; }
        public int EventItem { get; }
        public int EventHeight { get; set; }

        public event EventHandler? SelectionChanged;
        public event EventHandler? MeasureItem;
        public event EventHandler? DrawItem;
        public event EventHandler? DrawItemBackground;
        public event EventHandler? ControlRecreated;

        public bool HasBorder { get; set; }
        public int ItemsCount { get; set; }
        public ListBoxSelectionMode SelectionMode { get; set; }
        public void ClearItems() { }
        public void ClearSelected() { }
        public void SetSelected(int index, bool value) { }

        public int GetFirstSelected() => default;
        public int GetNextSelected() => default;
        public int GetSelectedCount() => default;
        public int GetSelection() => default;
        public void EnsureVisible(int itemIndex) { }
        public int ItemHitTest(PointD position) => default;
        public void SetSelection(int selection) { }
        public void SetSelectionBackground(Color color) { }
        public bool IsCurrent(int current) => default;

        public bool DoSetCurrent(int current) => default;
    }
}