using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class TreeViewHandler : WxControlHandler, ITreeViewHandler
    {
        /// <summary>
        /// Gets a <see cref="TreeView"/> this handler provides the implementation for.
        /// </summary>
        public new TreeView? Control => (TreeView?)base.Control;

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

                return NativeControl.GetItemFromHandle(item);
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
                    item = itemHandle == IntPtr.Zero
                        ? null : NativeControl.GetItemFromHandle(itemHandle);
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
            if (!Control?.AllowLabelEdit ?? true)
            {
                return;
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
            Native.TreeView.SetItemBold(NativeControl.WxWidget, p, isBold);
        }

        public void SetItemBackgroundColor(TreeViewItem item, Color? color)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            if (color is null)
                Native.TreeView.ResetItemBackgroundColor(NativeControl.WxWidget, p);
            else
                Native.TreeView.SetItemBackgroundColor(NativeControl.WxWidget, p, color);
        }

        public void SetItemTextColor(TreeViewItem item, Color? color)
        {
            var p = GetHandleFromItem(item);
            if (p == IntPtr.Zero)
                return;
            if (color is null)
                Native.TreeView.ResetItemTextColor(NativeControl.WxWidget, p);
            else
                Native.TreeView.SetItemTextColor(NativeControl.WxWidget, p, color);
        }

        public void MakeAsListBox()
        {
            NativeControl.MakeAsListBox();
        }

        public void SetItemText(TreeViewItem item, string text)
        {
            if (NativeControl.skipSetItemText)
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

            if (Control is null)
                return;

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
        }

        protected override void OnDetach()
        {
            if (Control is not null)
            {
                Control.ItemAdded -= Control_ItemAdded;
                Control.ItemRemoved -= Control_ItemRemoved;
                Control.ImageListChanged -= Control_ImageListChanged;
                Control.SelectionModeChanged -= Control_SelectionModeChanged;
                Control.SelectionChanged -= Control_SelectionChanged;
            }

            base.OnDetach();
        }

        private void Control_ImageListChanged(object? sender, EventArgs e)
        {
            ApplyImageList();
        }

        private void Control_SelectionChanged(object? sender, EventArgs e)
        {
            if (NativeControl.receivingSelection)
                return;

            ApplySelection();
        }

        private void Control_SelectionModeChanged(object? sender, EventArgs e)
        {
            ApplySelectionMode();
        }

        private void ApplySelectionMode()
        {
            if(Control is not null)
                NativeControl.SelectionMode = Control.SelectionMode;
        }

        private IntPtr GetHandleFromItem(TreeViewItem item)
        {
            return (IntPtr?)item.Handle ?? default;
        }

        internal void ApplySelection()
        {
            NativeControl.applyingSelection = true;

            try
            {
                var nativeControl = NativeControl;
                nativeControl.ClearSelected();

                var control = Control;
                var handles = control?.SelectedItems.Select(GetHandleFromItem) ?? [];

                foreach (var handle in handles)
                {
                    if (handle == IntPtr.Zero)
                        continue;
                    NativeControl.SetSelected(handle, true);
                }
            }
            finally
            {
                NativeControl.applyingSelection = false;
            }
        }

        internal void ReceiveSelection()
        {
            if (Control is null)
                return;

            NativeControl.receivingSelection = true;

            try
            {
                var selected =
                    NativeControl.SelectedItems.Select(NativeControl.GetItemFromHandle).ToArray();

                Control.SelectedItems = selected!;
            }
            finally
            {
                NativeControl.receivingSelection = false;
            }
        }

        private void ApplyImageList()
        {
            NativeControl.ImageList = (UI.Native.ImageList?)Control?.ImageList?.Handler;
        }

        internal void ApplyItems()
        {
            if (Control is null)
                return;
            var nativeControl = NativeControl;
            nativeControl.ClearItems(nativeControl.RootItem);

            foreach (var item in Control.Items)
                InsertItemAndChildren(item);
        }

        private void InsertItem(TreeViewItem item)
        {
            if (Control is null || item.Parent is null)
                return;
            var parentCollection = item.Parent.Items;
            IntPtr insertAfter = IntPtr.Zero;

            if (item.IsLast)
            {
            }
            else
            {
                var itemIndex = item.Index;

                if (itemIndex > 0)
                {
                    insertAfter = GetHandleFromItem(
                        parentCollection[itemIndex.Value - 1]);
                }
            }

            var isRootChild = item.IsRootChild;

            var handle = NativeControl.InsertItem(
                            isRootChild ?
                            NativeControl.RootItem : GetHandleFromItem(item.Parent!),
                            insertAfter,
                            item.Text,
                            item.ImageIndex ?? Control.ImageIndex ?? -1,
                            isRootChild ? false : item.Parent?.IsExpanded ?? false);

            NativeControl.itemsByHandles.Add(handle, item);
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
                NativeControl.itemsByHandles.Remove(handle);
            }
        }

        public void DeleteAllItems()
        {
            if (Control is null)
                return;
            NativeControl.DeleteAllItems();
            NativeControl.itemsByHandles = new();
        }

        public void Expand(TreeViewItem item)
        {
            if (Control is null)
                return;
            NativeControl.ExpandItem(GetHandleFromItem(item));
        }

        public void Collapse(TreeViewItem item)
        {
            if (Control is null)
                return;
            NativeControl.CollapseItem(GetHandleFromItem(item));
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