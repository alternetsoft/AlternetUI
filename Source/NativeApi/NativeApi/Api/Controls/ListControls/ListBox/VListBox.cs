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
        public static IntPtr CreateEx(long styles) => default;

        public IntPtr EventDc { get; }
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
    }
}

/*
public int[] SelectedIndices { get; }

public void InsertItem(int index, string value) { }
public void RemoveItemAt(int index) { }
public void EnsureVisible(int itemIndex) { }
public int ItemHitTest(PointD position) => default;
public void SetItem(int index, string value) { }
*/