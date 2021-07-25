using System;
using System.Collections.Generic;
using System.Linq;

namespace Alternet.UI
{
    internal class NativeTreeViewHandler : NativeControlHandler<TreeView, Native.TreeView>
    {
        private readonly Dictionary<TreeViewItem, IntPtr> handlesByItems = new Dictionary<TreeViewItem, IntPtr>();
        private readonly Dictionary<IntPtr, TreeViewItem> itemsByHandles = new Dictionary<IntPtr, TreeViewItem>();
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
            NativeControl.ControlRecreated += NativeControl_ControlRecreated;
            NativeControl.ItemExpanded += NativeControl_ItemExpanded;
            NativeControl.ItemCollapsed += NativeControl_ItemCollapsed;
        }

        private void NativeControl_ItemCollapsed(object? sender, Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            Control.RaiseAfterCollapse(new TreeViewItemExpandedChangedEventArgs(item));
            Control.RaiseExpandedChanged(new TreeViewItemExpandedChangedEventArgs(item));
        }

        private void NativeControl_ItemExpanded(object? sender, Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            Control.RaiseAfterExpand(new TreeViewItemExpandedChangedEventArgs(item));
            Control.RaiseExpandedChanged(new TreeViewItemExpandedChangedEventArgs(item));
        }

        private void NativeControl_ControlRecreated(object? sender, EventArgs e)
        {
            handlesByItems.Clear();
            itemsByHandles.Clear();
            ApplyItems();
            ApplySelection();
        }

        protected override void OnDetach()
        {
            Control.ItemAdded -= Control_ItemAdded;
            Control.ItemRemoved -= Control_ItemRemoved;
            Control.ImageListChanged -= Control_ImageListChanged;
            Control.SelectionModeChanged -= Control_SelectionModeChanged;

            Control.SelectionChanged -= Control_SelectionChanged;
            NativeControl.SelectionChanged -= NativeControl_SelectionChanged;
            NativeControl.ControlRecreated -= NativeControl_ControlRecreated;
            NativeControl.ItemExpanded -= NativeControl_ItemExpanded;
            NativeControl.ItemCollapsed -= NativeControl_ItemCollapsed;

            base.OnDetach();
        }

        private void Control_ImageListChanged(object? sender, EventArgs e)
        {
            ApplyImageList();
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

        private IntPtr GetHandleFromItem(TreeViewItem item)
        {
            return handlesByItems[item];
        }

        private TreeViewItem GetItemFromHandle(IntPtr handle)
        {
            return itemsByHandles[handle];
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

            foreach (var item in Control.Items)
                InsertItemAndChildren(item);
        }

        private void InsertItem(TreeViewItem item)
        {
            var parentCollection = item.Parent == null ? Control.Items : item.Parent.Items;
            IntPtr insertAfter = IntPtr.Zero;
            if (item.Index > 0)
                insertAfter = GetHandleFromItem(parentCollection[item.Index.Value - 1]);

            var handle = NativeControl.InsertItem(
                            item.Parent == null ? NativeControl.RootItem : GetHandleFromItem(item.Parent),
                            insertAfter,
                            item.Text,
                            item.ImageIndex ?? Control.ImageIndex ?? -1,
                            item.Parent?.IsExpanded ?? false);

            itemsByHandles.Add(handle, item);
            handlesByItems.Add(item, handle);
        }

        private void InsertItemAndChildren(TreeViewItem item)
        {
            InsertItem(item);
            void Apply(IEnumerable<TreeViewItem> items)
            {
                foreach (var item in items)
                {
                    InsertItem(item);
                    Apply(item.Items);
                }
            }
            Apply(item.Items);
        }

        private void Control_ItemAdded(object? sender, TreeViewItemContainmentEventArgs e)
        {
            InsertItemAndChildren(e.Item);
        }

        private void Control_ItemRemoved(object? sender, TreeViewItemContainmentEventArgs e)
        {
            var item = e.Item;
            var handle = handlesByItems[item];

            NativeControl.RemoveItem(GetHandleFromItem(item));

            handlesByItems.Remove(item);
            itemsByHandles.Remove(handle);
        }
    }
}