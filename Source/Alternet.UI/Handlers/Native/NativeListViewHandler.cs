using System;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    internal class NativeListViewHandler : NativeControlHandler<ListView, Native.ListView>
    {
        private bool receivingSelection;

        private bool applyingSelection;

        internal override Native.Control CreateNativeControl()
        {
            return new Native.ListView();
        }

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
                NativeControl.InsertItemAt(itemIndex, item.Cells[columnIndex].Text, columnIndex, item.ImageIndex ?? -1);
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
            NativeControl.InsertColumnAt(e.Item.Index ?? throw new Exception(), e.Item.Title);
        }

        private void Columns_ItemRemoved(object? sender, CollectionChangeEventArgs<ListViewColumn> e)
        {
            NativeControl.RemoveColumnAt(e.Item.Index ?? throw new Exception());
        }
    }
}