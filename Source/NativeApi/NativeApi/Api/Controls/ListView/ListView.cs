using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ListView : Control
    {
        public bool HasBorder { get; set; }

        public long ItemsCount { get; }

        public ImageList? SmallImageList { get; set; }
        
        public ImageList? LargeImageList { get; set; }

        public event EventHandler? ControlRecreated { add => throw new Exception(); remove => throw new Exception(); }

        public void InsertItemAt(long index, string text, long columnIndex, int imageIndex) => throw new Exception();

        public void RemoveItemAt(long index) => throw new Exception();

        public void ClearItems() => throw new Exception();

        public void InsertColumnAt(long index, string header, float width, ListViewColumnWidthMode widthMode) => throw new Exception();
        
        public void RemoveColumnAt(long index) => throw new Exception();

        public ListViewView CurrentView { get; set; }

        public event EventHandler? SelectionChanged { add => throw new Exception(); remove => throw new Exception(); }

        public ListViewSelectionMode SelectionMode { get; set; }

        public long[] SelectedIndices { get => throw new Exception(); }

        public void ClearSelected() => throw new Exception();

        public void SetSelected(long index, bool value) => throw new Exception();

        public bool AllowLabelEdit { get; set; }

        public long TopItemIndex { get; }

        public ListViewGridLinesDisplayMode GridLinesDisplayMode { get; set; }

        public IntPtr ItemHitTest(PointD point) => throw new Exception();
        public ListViewHitTestLocations GetHitTestResultLocations(IntPtr hitTestResult) => throw new Exception();
        public long GetHitTestResultItemIndex(IntPtr hitTestResult) => throw new Exception();
        public long GetHitTestResultColumnIndex(IntPtr hitTestResult) => throw new Exception();
        public void FreeHitTestResult(IntPtr hitTestResult) => throw new Exception();

        public void BeginLabelEdit(long itemIndex) => throw new Exception();

        public RectD GetItemBounds(long itemIndex, ListViewItemBoundsPortion portion) => throw new Exception();

        public event NativeEventHandler<CompareListViewItemsEventData>? CompareItemsForCustomSort { add => throw new Exception(); remove => throw new Exception(); }

        public ListViewSortMode SortMode { get; set; }

        public void Clear() => throw new Exception();

        public void EnsureItemVisible(long itemIndex) => throw new Exception();

        public bool ColumnHeaderVisible { get; set; }

        public long FocusedItemIndex { get; set; }

        public event NativeEventHandler<ListViewColumnEventData>? ColumnClick { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<ListViewItemLabelEditEventData>? BeforeItemLabelEdit { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<ListViewItemLabelEditEventData>? AfterItemLabelEdit { add => throw new Exception(); remove => throw new Exception(); }

        public void SetItemText(long itemIndex, long columnIndex, string text) => throw new Exception();
        public void SetItemImageIndex(long itemIndex, long columnIndex, int imageIndex) => throw new Exception();

        public void SetColumnWidth(long columnIndex, float fixedWidth, ListViewColumnWidthMode widthMode) => throw new Exception();
        public void SetColumnTitle(long columnIndex, string text) => throw new Exception();
    }
}