using Alternet.Drawing;
using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class ListView : Control
    {
        public int ItemsCount { get; }

        public ImageList? SmallImageList { get; set; }
        
        public ImageList? LargeImageList { get; set; }

        public void InsertItemAt(int index, string text, int columnIndex, int imageIndex) => throw new Exception();

        public void RemoveItemAt(int index) => throw new Exception();

        public void ClearItems() => throw new Exception();

        public void InsertColumnAt(int index, string header, double width, ListViewColumnWidthMode widthMode) => throw new Exception();
        
        public void RemoveColumnAt(int index) => throw new Exception();

        public ListViewView CurrentView { get; set; }

        public event EventHandler? SelectionChanged { add => throw new Exception(); remove => throw new Exception(); }

        public ListViewSelectionMode SelectionMode { get; set; }

        public int[] SelectedIndices { get => throw new Exception(); }

        public void ClearSelected() => throw new Exception();

        public void SetSelected(int index, bool value) => throw new Exception();

        public bool AllowLabelEdit { get; set; }

        public int TopItemIndex { get; }

        public ListViewGridLinesDisplayMode GridLinesDisplayMode { get; set; }

        public IntPtr ItemHitTest(Point point) => throw new Exception();
        public ListViewHitTestLocations GetHitTestResultLocations(IntPtr hitTestResult) => throw new Exception();
        public int GetHitTestResultItemIndex(IntPtr hitTestResult) => throw new Exception();
        public int GetHitTestResultColumnIndex(IntPtr hitTestResult) => throw new Exception();
        public void FreeHitTestResult(IntPtr hitTestResult) => throw new Exception();

        public void BeginLabelEdit(int itemIndex) => throw new Exception();

        public Rect GetItemBounds(int itemIndex, ListViewItemBoundsPortion portion) => throw new Exception();

        public event NativeEventHandler<CompareListViewItemsEventData>? CompareItemsForCustomSort { add => throw new Exception(); remove => throw new Exception(); }

        public ListViewSortMode SortMode { get; set; }

        public void Clear() => throw new Exception();

        public void EnsureItemVisible(int itemIndex) => throw new Exception();

        public bool ColumnHeaderVisible { get; set; }

        public int FocusedItemIndex { get; set; }

        public event NativeEventHandler<ListViewColumnEventData>? ColumnClick { add => throw new Exception(); remove => throw new Exception(); }

        public event NativeEventHandler<ListViewItemLabelEditEventData>? BeforeItemLabelEdit { add => throw new Exception(); remove => throw new Exception(); }
        public event NativeEventHandler<ListViewItemLabelEditEventData>? AfterItemLabelEdit { add => throw new Exception(); remove => throw new Exception(); }

        public void SetItemText(int itemIndex, int columnIndex, string text) => throw new Exception();
        public void SetItemImageIndex(int itemIndex, int columnIndex, int imageIndex) => throw new Exception();

        public void SetColumnWidth(int columnIndex, double fixedWidth, ListViewColumnWidthMode widthMode) => throw new Exception();
        public void SetColumnTitle(int columnIndex, string text) => throw new Exception();
    }
}