using System;
using System.Collections.Generic;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeListViewHandler : ListViewHandler
    {
        private bool receivingSelection;

        private bool applyingSelection;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ListView();
        }

        public new Native.ListView NativeControl => (Native.ListView)base.NativeControl!;

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();
            ApplyView();
            ApplySmallImageList();
            ApplyLargeImageList();
            ApplySelectionMode();
            ApplySelection();

            Control.Items.ItemInserted += Items_ItemInserted;
            Control.Items.ItemRemoved += Items_ItemRemoved;
            Control.Columns.ItemInserted += Columns_ItemInserted;
            Control.Columns.ItemRemoved += Columns_ItemRemoved;
            Control.ViewChanged += Control_ViewChanged;
            Control.SmallImageListChanged += Control_SmallImageListChanged;
            Control.LargeImageListChanged += Control_LargeImageListChanged;
            Control.SelectionModeChanged += Control_SelectionModeChanged;

            Control.SelectionChanged += Control_SelectionChanged;
            NativeControl.SelectionChanged += NativeControl_SelectionChanged;
        }

        /// <inheritdoc/>
        public override bool AllowLabelEdit { get => NativeControl.AllowLabelEdit; set => NativeControl.AllowLabelEdit = value; }

        /// <inheritdoc/>
        public override ListViewItem? TopItem
        {
            get
            {
                var i = NativeControl.TopItemIndex;
                return i == -1 ? null : Control.Items[i];
            }
        }

        /// <inheritdoc/>
        public override ListViewGridLinesDisplayMode GridLinesDisplayMode
        {
            get => (ListViewGridLinesDisplayMode)NativeControl.GridLinesDisplayMode;
            set => NativeControl.GridLinesDisplayMode = (Native.ListViewGridLinesDisplayMode)value;
        }

        /// <inheritdoc/>
        public override ListViewHitTestInfo HitTest(Point point)
        {
            var result = NativeControl.ItemHitTest(point);
            if (result == IntPtr.Zero)
                throw new Exception();

            try
            {
                var itemIndex = NativeControl.GetHitTestResultItemIndex(result);
                var columnIndex = NativeControl.GetHitTestResultColumnIndex(result);
                
                var item = itemIndex == -1 ? null : Control.Items[itemIndex];
                var cell = item == null || columnIndex == -1 ? null : item.Cells[columnIndex];
                var location = (ListViewHitTestLocations)NativeControl.GetHitTestResultLocations(result);

                return new ListViewHitTestInfo(location, item, cell);
            }
            finally
            {
                NativeControl.FreeHitTestResult(result);
            }
        }

        /// <inheritdoc/>
        public override void BeginLabelEdit(int itemIndex) => NativeControl.BeginLabelEdit(itemIndex);

        /// <inheritdoc/>
        public override Rect GetItemBounds(int itemIndex, ListViewItemBoundsPortion portion) =>
            NativeControl.GetItemBounds(itemIndex, (Native.ListViewItemBoundsPortion)portion);

        /// <inheritdoc/>
        public override IComparer<ListViewItem>? CustomItemSortComparer { get; set; }

        /// <inheritdoc/>
        public override ListViewSortMode SortMode
        {
            get => (ListViewSortMode)NativeControl.SortMode;
            set => NativeControl.SortMode = (Native.ListViewSortMode)value;
        }

        /// <inheritdoc/>
        public override void Clear() => NativeControl.Clear();

        /// <inheritdoc/>
        public override bool ColumnHeaderVisible
        {
            get => NativeControl.ColumnHeaderVisible;
            set => NativeControl.ColumnHeaderVisible = value;
        }

        /// <inheritdoc/>
        public override void EnsureItemVisible(int itemIndex) => NativeControl.EnsureItemVisible(itemIndex);

        /// <inheritdoc/>
        public override int? FocusedItemIndex
        {
            get
            {
                int i = NativeControl.FocusedItemIndex;
                return i == -1 ? null : i;
            }
            
            set => NativeControl.FocusedItemIndex = value ?? -1;
        }

        private void Control_ViewChanged(object? sender, EventArgs e)
        {
            ApplyView();
        }

        private void Control_SmallImageListChanged(object? sender, EventArgs e)
        {
            ApplySmallImageList();
        }

        private void Control_LargeImageListChanged(object? sender, EventArgs e)
        {
            ApplyLargeImageList();
        }

        protected override void OnDetach()
        {
            Control.Items.ItemInserted -= Items_ItemInserted;
            Control.Items.ItemRemoved -= Items_ItemRemoved;
            Control.ViewChanged -= Control_ViewChanged;
            Control.Columns.ItemInserted -= Columns_ItemInserted;
            Control.Columns.ItemRemoved -= Columns_ItemRemoved;
            Control.SmallImageListChanged -= Control_SmallImageListChanged;
            Control.LargeImageListChanged -= Control_LargeImageListChanged;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;

            Control.SelectionChanged -= Control_SelectionChanged;
            NativeControl.SelectionChanged -= NativeControl_SelectionChanged;

            base.OnDetach();
        }

        private void NativeControl_SelectionChanged(object? sender, EventArgs e)
        {
            if (applyingSelection)
                return;

            ReceiveSelection();
        }

        private void Control_SelectionChanged(object? sender, EventArgs e)
        {
            if (receivingSelection)
                return;

            ApplySelection();
        }

        private void Control_SelectionModeChanged(object? sender, EventArgs e)
        {
            ApplySelectionMode();
        }

        private void ApplySelectionMode()
        {
            NativeControl.SelectionMode = (Native.ListViewSelectionMode)Control.SelectionMode;
        }

        private void ApplySelection()
        {
            applyingSelection = true;

            try
            {
                var nativeControl = NativeControl;
                nativeControl.ClearSelected();

                var control = Control;
                var indices = control.SelectedIndices;

                for (var i = 0; i < indices.Count; i++)
                    NativeControl.SetSelected(indices[i], true);
            }
            finally
            {
                applyingSelection = false;
            }
        }

        private void ReceiveSelection()
        {
            receivingSelection = true;

            try
            {
                Control.SelectedIndices = NativeControl.SelectedIndices;
            }
            finally
            {
                receivingSelection = false;
            }
        }

        private void ApplyView()
        {
            NativeControl.CurrentView = (Native.ListViewView)Control.View;
        }

        private void ApplySmallImageList()
        {
            NativeControl.SmallImageList = Control.SmallImageList?.NativeImageList;
        }

        private void ApplyLargeImageList()
        {
            NativeControl.LargeImageList = Control.LargeImageList?.NativeImageList;
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems();

            var control = Control;
            var items = control.Items;

            for (var itemIndex = 0; itemIndex < items.Count; itemIndex++)
            {
                var item = control.Items[itemIndex];
                InsertItem(itemIndex, item);
            }
        }

        private void InsertItem(int itemIndex, ListViewItem item)
        {
            for (var columnIndex = 0; columnIndex < item.Cells.Count; columnIndex++)
            {
                var cell = item.Cells[columnIndex];
                NativeControl.InsertItemAt(itemIndex, cell.Text, columnIndex, cell.ImageIndex ?? item.ImageIndex ?? -1);
            }
        }

        private void Items_ItemInserted(object? sender, CollectionChangeEventArgs<ListViewItem> e)
        {
            InsertItem(e.Index, e.Item);
        }

        private void Items_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewItem> e)
        {
            NativeControl.RemoveItemAt(e.Index);
        }

        private void Columns_ItemInserted(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            NativeControl.InsertColumnAt(
                e.Item.Index ?? throw new Exception(),
                e.Item.Title,
                e.Item.Width,
                (Native.ListViewColumnWidthMode)e.Item.WidthMode);
        }

        private void Columns_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            NativeControl.RemoveColumnAt(e.Item.Index ?? throw new Exception());
        }
    }
}