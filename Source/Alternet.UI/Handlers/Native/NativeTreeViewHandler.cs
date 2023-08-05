using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeTreeViewHandler : TreeViewHandler
    {
        private readonly Dictionary<TreeViewItem, IntPtr> handlesByItems = new();

        private readonly Dictionary<IntPtr, TreeViewItem> itemsByHandles = new ();

        private bool skipSetItemText;

        private bool receivingSelection;

        private bool applyingSelection;

        /// <inheritdoc cref="TreeView.HideRoot"/>
        public override bool HideRoot
        {
            get
            {
                return NativeControl.HideRoot;
            }

            set
            {
                NativeControl.HideRoot = value;
            }
        }

        /// <inheritdoc cref="TreeView.HasBorder"/>
        public override bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
            }
        }

        /// <inheritdoc cref="TreeView.VariableRowHeight"/>
        public override bool VariableRowHeight
        {
            get
            {
                return NativeControl.VariableRowHeight;
            }

            set
            {
                NativeControl.VariableRowHeight = value;
            }
        }

        /// <inheritdoc cref="TreeView.TwistButtons"/>
        public override bool TwistButtons
        {
            get
            {
                return NativeControl.TwistButtons;
            }

            set
            {
                NativeControl.TwistButtons = value;
            }
        }

        /// <inheritdoc cref="TreeView.StateImageSpacing"/>
        public override uint StateImageSpacing
        {
            get
            {
                return NativeControl.StateImageSpacing;
            }

            set
            {
                NativeControl.StateImageSpacing = value;
            }
        }

        /// <inheritdoc cref="TreeView.Indentation"/>
        public override uint Indentation
        {
            get
            {
                return NativeControl.Indentation;
            }

            set
            {
                NativeControl.Indentation = value;
            }
        }

        /// <inheritdoc cref="TreeView.RowLines"/>
        public override bool RowLines
        {
            get
            {
                return NativeControl.RowLines;
            }

            set
            {
                NativeControl.RowLines = value;
            }
        }

        public new Native.TreeView NativeControl =>
            (Native.TreeView)base.NativeControl!;

        /// <inheritdoc cref="TreeView.ShowLines"/>
        public override bool ShowLines
        {
            get => NativeControl.ShowLines;
            set => NativeControl.ShowLines = value;
        }

        /// <inheritdoc cref="TreeView.ShowRootLines"/>
        public override bool ShowRootLines
        {
            get => NativeControl.ShowRootLines; 
            set => NativeControl.ShowRootLines = value;
        }

        public override bool ShowExpandButtons
        {
            get => NativeControl.ShowExpandButtons;
            set => NativeControl.ShowExpandButtons = value;
        }

        /// <inheritdoc cref="TreeView.TopItem"/>
        public override TreeViewItem? TopItem
        {
            get
            {
                var item = NativeControl.TopItem;
                if (item == IntPtr.Zero)
                    return null;

                return GetItemFromHandle(item);
            }
        }

        /// <inheritdoc cref="TreeView.FullRowSelect"/>
        public override bool FullRowSelect
        {
            get => NativeControl.FullRowSelect;
            set => NativeControl.FullRowSelect = value;
        }

        /// <inheritdoc cref="TreeView.AllowLabelEdit"/>
        public override bool AllowLabelEdit
        {
            get => NativeControl.AllowLabelEdit;
            set => NativeControl.AllowLabelEdit = value;
        }

        public override void ExpandAll() => NativeControl.ExpandAll();

        public override void CollapseAll() => NativeControl.CollapseAll();

        public override TreeViewHitTestInfo HitTest(Point point)
        {
            var result = NativeControl.ItemHitTest(point);
            if (result == IntPtr.Zero)
                throw new Exception();

            try
            {
                var itemHandle = NativeControl.GetHitTestResultItem(result);
                return new TreeViewHitTestInfo(
                    (TreeViewHitTestLocations)NativeControl.
                        GetHitTestResultLocations(result),
                    itemHandle == IntPtr.Zero ? null : GetItemFromHandle(itemHandle));
            }
            finally
            {
                NativeControl.FreeHitTestResult(result);
            }
        }

        public override bool IsItemSelected(TreeViewItem item) =>
            NativeControl.IsItemSelected(GetHandleFromItem(item));

        public override void BeginLabelEdit(TreeViewItem item)
        {
            if (!Control.AllowLabelEdit)
            {
                throw new InvalidOperationException(
                    "Label editing is not allowed.");
            }

            NativeControl.BeginLabelEdit(GetHandleFromItem(item));
        }

        public override void EndLabelEdit(TreeViewItem item, bool cancel) =>
            NativeControl.EndLabelEdit(GetHandleFromItem(item), cancel);

        public override void ExpandAllChildren(TreeViewItem item) =>
            NativeControl.ExpandAllChildren(GetHandleFromItem(item));

        public override void CollapseAllChildren(TreeViewItem item) =>
            NativeControl.CollapseAllChildren(GetHandleFromItem(item));

        public override void EnsureVisible(TreeViewItem item) =>
            NativeControl.EnsureVisible(GetHandleFromItem(item));

        public override void ScrollIntoView(TreeViewItem item) =>
            NativeControl.ScrollIntoView(GetHandleFromItem(item));

        public override void SetFocused(TreeViewItem item, bool value) =>
            NativeControl.SetFocused(GetHandleFromItem(item), value);

        public override bool IsItemFocused(TreeViewItem item) =>
            NativeControl.IsItemFocused(GetHandleFromItem(item));

        public override void SetItemText(TreeViewItem item, string text)
        {
            if (skipSetItemText)
                return;

            NativeControl.SetItemText(GetHandleFromItem(item), text);
        }

        public override void SetItemImageIndex(TreeViewItem item, int? imageIndex) =>
            NativeControl.SetItemImageIndex(GetHandleFromItem(item), imageIndex ?? -1);

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
            NativeControl.BeforeItemLabelEdit += NativeControl_BeforeItemLabelEdit;
            NativeControl.AfterItemLabelEdit += NativeControl_AfterItemLabelEdit;
            NativeControl.ItemExpanding += NativeControl_ItemExpanding;
            NativeControl.ItemCollapsing += NativeControl_ItemCollapsing;
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
            NativeControl.BeforeItemLabelEdit -= NativeControl_BeforeItemLabelEdit;
            NativeControl.AfterItemLabelEdit -= NativeControl_AfterItemLabelEdit;
            NativeControl.ItemExpanding -= NativeControl_ItemExpanding;
            NativeControl.ItemCollapsing -= NativeControl_ItemCollapsing;

            base.OnDetach();
        }

        private void NativeControl_ItemCollapsing(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            var ea = new TreeViewItemExpandedChangingEventArgs(item);
            Control.RaiseBeforeCollapse(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        private void NativeControl_ItemExpanding(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            var ea = new TreeViewItemExpandedChangingEventArgs(item);
            Control.RaiseBeforeExpand(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        private void NativeControl_AfterItemLabelEdit(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemLabelEditEventData> e)
        {
            var ea = new TreeViewItemLabelEditEventArgs(
                GetItemFromHandle(e.Data.item),
                e.Data.editCancelled ? null : e.Data.label);

            Control.RaiseAfterLabelEdit(ea);

            if (!e.Data.editCancelled && !ea.Cancel)
            {
                skipSetItemText = true;
                ea.Item.Text = e.Data.label;
                skipSetItemText = false;
            }

            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        private void NativeControl_BeforeItemLabelEdit(
            object? sender, 
            Native.NativeEventArgs<Native.TreeViewItemLabelEditEventData> e)
        {
            var ea = new TreeViewItemLabelEditEventArgs(
                GetItemFromHandle(e.Data.item),
                e.Data.editCancelled ? null : e.Data.label);
            Control.RaiseBeforeLabelEdit(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        private void NativeControl_ItemCollapsed(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            var ea = new TreeViewItemExpandedChangedEventArgs(item);
            Control.RaiseAfterCollapse(ea);
            Control.RaiseExpandedChanged(ea);
        }

        private void NativeControl_ItemExpanded(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            var ea = new TreeViewItemExpandedChangedEventArgs(item);
            Control.RaiseAfterExpand(ea);
            Control.RaiseExpandedChanged(ea);
        }

        private void NativeControl_ControlRecreated(object? sender, EventArgs e)
        {
            handlesByItems.Clear();
            itemsByHandles.Clear();
            ApplyItems();
            ApplySelection();
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
            NativeControl.SelectionMode =
                (Native.TreeViewSelectionMode)Control.SelectionMode;
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
                Control.SelectedItems =
                    NativeControl.SelectedItems.Select(GetItemFromHandle).ToArray();
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
            var parentCollection = item.Parent == null ?
                Control.Items : item.Parent.Items;
            IntPtr insertAfter = IntPtr.Zero;
            if (item.Index > 0)
            {
                insertAfter = GetHandleFromItem(
                    parentCollection[item.Index.Value - 1]);
            }

            var handle = NativeControl.InsertItem(
                            item.Parent == null ?
                            NativeControl.RootItem : GetHandleFromItem(item.Parent),
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

        private void Control_ItemAdded(
            object? sender,
            TreeViewItemContainmentEventArgs e)
        {
            InsertItemAndChildren(e.Item);
        }

        private void Control_ItemRemoved(
            object? sender,
            TreeViewItemContainmentEventArgs e)
        {
            var item = e.Item;

            NativeControl.RemoveItem(GetHandleFromItem(item));
            RemoveItemAndChildrenFromDictionaries(item);

            void RemoveItemAndChildrenFromDictionaries(TreeViewItem parentItem)
            {
                foreach (var childItem in parentItem.Items)
                    RemoveItemAndChildrenFromDictionaries(childItem);

                var handle = handlesByItems[parentItem];
                handlesByItems.Remove(parentItem);
                itemsByHandles.Remove(handle);
            }
        }
    }
}