using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    internal class NativeTreeViewHandler : NativeControlHandler<TreeView, Native.TreeView>
    {
        private bool receivingSelection;

        private bool applyingSelection;



        internal override Native.Control CreateNativeControl()
        {
            return new Native.TreeView();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyItems();
            ApplyImageList();
            ApplySelectionMode();
            ApplySelection();

            Control.ItemAdded += Control_ItemAdded;
            Control.ItemRemoved += Control_ItemRemoved;
            Control.ImageListChanged += Control_ImageListChanged;
            Control.SelectionModeChanged += Control_SelectionModeChanged;

            Control.SelectionChanged += Control_SelectionChanged;
            NativeControl.SelectionChanged += NativeControl_SelectionChanged;
        }

        private void Control_ImageListChanged(object? sender, EventArgs e)
        {
            ApplyImageList();
        }

        protected override void OnDetach()
        {
            Control.ItemAdded -= Control_ItemAdded;
            Control.ItemRemoved -= Control_ItemRemoved;
            Control.ImageListChanged -= Control_ImageListChanged;
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
            NativeControl.SelectionMode = (Native.TreeViewSelectionMode)Control.SelectionMode;
        }

        IntPtr GetHandleFromItem(TreeViewItem item)
        {
            throw new Exception();
        }

        TreeViewItem GetItemFromHandle(IntPtr handle)
        {
            throw new Exception();
        }

        private void ApplySelection()
        {
            applyingSelection = true;

            try
            {
                var nativeControl = NativeControl;
                nativeControl.ClearSelected();

                var control = Control;
                var handles = control.SelectedItems.Select(GetHandleFromItem);

                foreach (var handle in handles)
                    NativeControl.SetSelected(handle, true);
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
                Control.SelectedItems = NativeControl.SelectedItems.Select(GetItemFromHandle).ToArray();
            }
            finally
            {
                receivingSelection = false;
            }
        }

        private void ApplyImageList()
        {
            NativeControl.ImageList = Control.ImageList?.NativeImageList;
        }

        private void ApplyItems()
        {
            var nativeControl = NativeControl;
            nativeControl.ClearItems(nativeControl.RootItem);

            void Apply(TreeViewItem? parent, IEnumerable<TreeViewItem> items)
            {
                foreach (var item in items)
                {
                    InsertItem(parent, item);
                    Apply(item, item.Items);
                }
            }

            Apply(null, Control.Items);
        }

        private void InsertItem(TreeViewItem? parent, TreeViewItem item)
        {
            NativeControl.InsertItemAt(
                parent == null ? NativeControl.RootItem : GetHandleFromItem(parent),
                item.Index,
                item.Text,
                item.ImageIndex ?? Control.ImageIndex ?? -1);
        }

        private void Control_ItemAdded(object? sender, TreeViewItemEventArgs e)
        {
            InsertItem(e.Item.Parent, e.Item);
        }

        private void Control_ItemRemoved(object? sender, TreeViewItemEventArgs e)
        {
            NativeControl.RemoveItem(GetHandleFromItem(e.Item));
        }
    }
}