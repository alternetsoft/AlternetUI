using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
using System.Collections.Generic;

namespace Alternet.UI.Native
{
    internal partial class TreeView
    {
        internal Dictionary<IntPtr, TreeViewItem> itemsByHandles = new();
        internal bool skipSetItemText;
        internal bool receivingSelection;
        internal bool applyingSelection;

        public void OnPlatformEventSelectionChanged()
        {
            if (Handler is not WxTreeViewHandler handler)
                return;
            if (applyingSelection)
                return;

            handler.ReceiveSelection();
        }

        public void OnPlatformEventControlRecreated()
        {
            if (Handler is not WxTreeViewHandler handler)
                return;
            itemsByHandles.Clear();
            handler.ApplyItems();
            handler.ApplySelection();
        }

        internal TreeViewItem? GetItemFromHandle(IntPtr handle)
        {
            if (itemsByHandles.TryGetValue(handle, out TreeViewItem? result))
                return result;
            return null;
        }

        public void OnPlatformEventItemExpanded(NativeEventArgs<TreeViewItemEventData> e)
        {
            if (UIControl is not UI.TreeView uiControl)
                return;
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewEventArgs(item);
            uiControl.RaiseAfterExpand(ea);
            uiControl.RaiseExpandedChanged(ea);
        }

        public void OnPlatformEventItemCollapsed(NativeEventArgs<TreeViewItemEventData> e)
        {
            if (UIControl is not UI.TreeView uiControl)
                return;
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewEventArgs(item);
            uiControl.RaiseAfterCollapse(ea);
            uiControl.RaiseExpandedChanged(ea);
        }

        public void OnPlatformEventItemExpanding(NativeEventArgs<TreeViewItemEventData> e)
        {
            if (UIControl is not UI.TreeView uiControl)
                return;
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewCancelEventArgs(item);
            uiControl.RaiseBeforeExpand(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        public void OnPlatformEventItemCollapsing(NativeEventArgs<TreeViewItemEventData> e)
        {
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewCancelEventArgs(item);
            (UIControl as UI.TreeView)?.RaiseBeforeCollapse(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        public void OnPlatformEventBeforeItemLabelEdit(
            NativeEventArgs<TreeViewItemLabelEditEventData> e)
        {
            if (UIControl is not UI.TreeView uiControl)
                return;
            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;

            var ea = new TreeViewEditEventArgs(
                item,
                e.Data.editCancelled ? null : e.Data.label);
            uiControl.RaiseBeforeLabelEdit(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        public void OnPlatformEventAfterItemLabelEdit(
            NativeEventArgs<TreeViewItemLabelEditEventData> e)
        {
            if (UIControl is not UI.TreeView uiControl)
                return;

            var item = GetItemFromHandle(e.Data.item);
            if (item == null)
                return;
            var ea = new TreeViewEditEventArgs(
                item,
                e.Data.editCancelled ? null : e.Data.label);

            uiControl.RaiseAfterLabelEdit(ea);

            if (!e.Data.editCancelled && !ea.Cancel)
            {
                skipSetItemText = true;
                ea.Item.Text = e.Data.label;
                skipSetItemText = false;
            }

            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }
    }
}
