using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class TreeViewHandler : WxControlHandler, ITreeViewHandler
    {
        private readonly Dictionary<IntPtr, TreeViewItem> itemsByHandles = new();
        private bool skipSetItemText;
        private bool receivingSelection;
        private bool applyingSelection;

        public TreeViewHandler()
        {
        }

        /// <summary>
        /// Gets a <see cref="TreeView"/> this handler provides the implementation for.
        /// </summary>
        public new TreeView Control => (TreeView)base.Control;

        /// <inheritdoc cref="TreeView.HideRoot"/>
        public bool HideRoot
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
        public bool HasBorder
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
        public bool VariableRowHeight
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
        public bool TwistButtons
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
        public uint StateImageSpacing
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
        public uint Indentation
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
        public bool RowLines
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

        public new NativeTreeView NativeControl =>
            (NativeTreeView)base.NativeControl!;

        /// <inheritdoc cref="TreeView.ShowLines"/>
        public bool ShowLines
        {
            get => NativeControl.ShowLines;
            set => NativeControl.ShowLines = value;
        }

        /// <inheritdoc cref="TreeView.ShowRootLines"/>
        public bool ShowRootLines
        {
            get => NativeControl.ShowRootLines;
            set => NativeControl.ShowRootLines = value;
        }

        public bool ShowExpandButtons
        {
            get => NativeControl.ShowExpandButtons;
            set => NativeControl.ShowExpandButtons = value;
        }

        /// <inheritdoc cref="TreeView.TopItem"/>
        public TreeViewItem? TopItem
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
        public bool FullRowSelect
        {
            get => NativeControl.FullRowSelect;
            set => NativeControl.FullRowSelect = value;
        }

        /// <inheritdoc cref="TreeView.AllowLabelEdit"/>
        public bool AllowLabelEdit
        {
            get => NativeControl.AllowLabelEdit;
            set => NativeControl.AllowLabelEdit = value;
        }

        public void ExpandAll() => NativeControl.ExpandAll();

        public void CollapseAll() => NativeControl.CollapseAll();

        public bool HitTest(
            PointD point,
            out TreeViewItem? item,
            out TreeViewHitTestLocations locations,
            bool needItem = true)
        {
            var result = NativeControl.ItemHitTest(point);
            if (result == IntPtr.Zero)
            {
                item = null;
                locations = 0;
                return false;
            }

            try
            {
                if (needItem)
                {
                    var itemHandle = NativeControl.GetHitTestResultItem(result);
                    item = itemHandle == IntPtr.Zero ? null : GetItemFromHandle(itemHandle);
                }
                else
                {
                    item = null;
                }

                locations = (TreeViewHitTestLocations)NativeControl.GetHitTestResultLocations(result);
                return locations != 0 || item != null;
            }
            finally
            {
                NativeControl.FreeHitTestResult(result);
            }
        }

        public TreeViewHitTestInfo HitTest(PointD point)
        {
            var htResult = HitTest(point, out var item, out var locations);
            if (htResult)
            {
                return new TreeViewHitTestInfo(locations, item);
            }
            else
                return TreeViewHitTestInfo.Empty;
        }

        public bool IsItemSelected(TreeViewItem item)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return false;
            return NativeControl.IsItemSelected(p);
        }

        public void BeginLabelEdit(TreeViewItem item)
        {
            if (!Control.AllowLabelEdit)
            {
                throw new InvalidOperationException(
                    "Label editing is not allowed.");
            }

            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.BeginLabelEdit(p);
        }

        public void EndLabelEdit(TreeViewItem item, bool cancel)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.EndLabelEdit(p, cancel);
        }

        public void ExpandAllChildren(TreeViewItem item)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.ExpandAllChildren(p);
        }

        public void CollapseAllChildren(TreeViewItem item)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.CollapseAllChildren(p);
        }

        public void EnsureVisible(TreeViewItem item)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.EnsureVisible(p);
        }

        public void ScrollIntoView(TreeViewItem item)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.ScrollIntoView(p);
        }

        public void SetFocused(TreeViewItem item, bool value)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.SetFocused(p, value);
        }

        public bool IsItemFocused(TreeViewItem item)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return false;
            return NativeControl.IsItemFocused(p);
        }

        public void SetItemIsBold(TreeViewItem item, bool isBold)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            Native.WxTreeViewFactory.SetItemBold(NativeControl.WxWidget, p, isBold);
        }

        public void SetItemBackgroundColor(TreeViewItem item, Color? color)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            if (color is null)
                Native.WxTreeViewFactory.ResetItemBackgroundColor(NativeControl.WxWidget, p);
            else
                Native.WxTreeViewFactory.SetItemBackgroundColor(NativeControl.WxWidget, p, color);
        }

        public void SetItemTextColor(TreeViewItem item, Color? color)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            if (color is null)
                Native.WxTreeViewFactory.ResetItemTextColor(NativeControl.WxWidget, p);
            else
                Native.WxTreeViewFactory.SetItemTextColor(NativeControl.WxWidget, p, color);
        }

        public void MakeAsListBox()
        {
            NativeControl.MakeAsListBox();
        }

        public void SetItemText(TreeViewItem item, string text)
        {
            if (skipSetItemText)
                return;
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.SetItemText(p, text);
        }

        public void SetItemImageIndex(
            TreeViewItem item,
            int? imageIndex)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            NativeControl.SetItemImageIndex(p, imageIndex ?? -1);
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeTreeView();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (App.IsWindowsOS)
                UserPaint = true;

            bool? hasBorder = AllPlatformDefaults.GetHasBorderOverride(Control.ControlKind);
            if (hasBorder is not null)
                HasBorder = hasBorder.Value;

            ApplyItems();
            ApplyImageList();
            ApplySelectionMode();
            ApplySelection();

            Control.ItemAdded += Control_ItemAdded;
            Control.ItemRemoved += Control_ItemRemoved;
            Control.ImageListChanged += Control_ImageListChanged;
            Control.SelectionModeChanged += Control_SelectionModeChanged;

            Control.SelectionChanged += Control_SelectionChanged;
            NativeControl.SelectionChanged = NativeControl_SelectionChanged;
            NativeControl.ControlRecreated = NativeControl_ControlRecreated;
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
            NativeControl.SelectionChanged = null;
            NativeControl.ControlRecreated = null;
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
            if (item == null)
                return;
            var ea = new TreeViewCancelEventArgs(item);
            Control.RaiseBeforeCollapse(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        private void NativeControl_ItemExpanding(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewCancelEventArgs(item);
            Control.RaiseBeforeExpand(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        private void NativeControl_AfterItemLabelEdit(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemLabelEditEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewEditEventArgs(
                item,
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
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;

            var ea = new TreeViewEditEventArgs(
                item,
                e.Data.editCancelled ? null : e.Data.label);
            Control.RaiseBeforeLabelEdit(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        private void NativeControl_ItemCollapsed(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewEventArgs(item);
            Control.RaiseAfterCollapse(ea);
            Control.RaiseExpandedChanged(ea);
        }

        private void NativeControl_ItemExpanded(
            object? sender,
            Native.NativeEventArgs<Native.TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewEventArgs(item);
            Control.RaiseAfterExpand(ea);
            Control.RaiseExpandedChanged(ea);
        }

        private void NativeControl_ControlRecreated()
        {
            itemsByHandles.Clear();
            ApplyItems();
            ApplySelection();
        }

        private void Control_ImageListChanged(object? sender, EventArgs e)
        {
            ApplyImageList();
        }

        private void NativeControl_SelectionChanged()
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
            return (IntPtr?)item.Handle ?? default;
        }

        private TreeViewItem GetItemFromHandle(IntPtr handle)
        {
            if (itemsByHandles.TryGetValue(handle, out TreeViewItem? result))
                return result;
            return null!;
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
                {
                    if (handle == IntPtr.Zero)
                        continue;
                    NativeControl.SetSelected(handle, true);
                }
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
            NativeControl.ImageList = (UI.Native.ImageList?)Control.ImageList?.Handler;
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
            item.Handle = handle;
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

            if (item.HasItems)
                Apply(item.Items);
        }

        private void Control_ItemAdded(
            object? sender,
            TreeViewEventArgs e)
        {
            InsertItemAndChildren(e.Item);
        }

        private void Control_ItemRemoved(
            object? sender,
            TreeViewEventArgs e)
        {
            var item = e.Item;

            /*Application.DebugLog($"Native TreeViewItem Removed: {item.Text}");*/

            var p = GetHandleFromItem(item);
            RemoveItemAndChildrenFromDictionaries(item);
            if (p != IntPtr.Zero)
            {
                NativeControl.RemoveItem(p);
            }

            void RemoveItemAndChildrenFromDictionaries(TreeViewItem parentItem)
            {
                if (parentItem.HasItems)
                {
                    foreach (var childItem in parentItem.Items)
                        RemoveItemAndChildrenFromDictionaries(childItem);
                }

                var handle = GetHandleFromItem(parentItem);
                parentItem.Handle = default;
                itemsByHandles.Remove(handle);
            }
        }

        public class NativeTreeView : Native.TreeView
        {
            public NativeTreeView()
                : base()
            {
            }
        }
    }
}