using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;

namespace Alternet.UI.Native
{
    internal partial class ListView
    {
        public void OnPlatformEventControlRecreated()
        {
            if (Handler is not WxListViewHandler uiHandler)
                return;
            BeginUpdate();
            uiHandler.ApplyColumns();
            uiHandler.ApplyItems();
            uiHandler.ApplySelection();
            EndUpdate();
        }

        public void OnPlatformEventSelectionChanged()
        {
            if (Handler is not WxListViewHandler uiHandler)
                return;

            uiHandler?.NativeControl_SelectionChanged();
        }

        public void OnPlatformEventCompareItemsForCustomSort(
            NativeEventArgs<CompareListViewItemsEventData> ea)
        {

        }

        public void OnPlatformEventColumnClick(NativeEventArgs<ListViewColumnEventData> ea)
        {
            if (UIControl is not UI.ListView uiControl)
                return;
            uiControl.RaiseColumnClick(new ListViewColumnEventArgs(ea.Data.columnIndex));
        }

        public void OnPlatformEventBeforeItemLabelEdit(
            NativeEventArgs<ListViewItemLabelEditEventData> e)
        {
            var ea = new ListViewItemLabelEditEventArgs(
                e.Data.itemIndex,
                e.Data.editCancelled ? null : e.Data.label);
            (UIControl as UI.ListView)?.RaiseBeforeLabelEdit(ea);
            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }

        public void OnPlatformEventAfterItemLabelEdit(
            NativeEventArgs<ListViewItemLabelEditEventData> e)
        {
            if (UIControl is not UI.ListView uiControl)
                return;

            var ea = new ListViewItemLabelEditEventArgs(
                e.Data.itemIndex,
                e.Data.editCancelled ? null : e.Data.label);

            uiControl.RaiseAfterLabelEdit(ea);

            if (!e.Data.editCancelled && !ea.Cancel)
            {
                /*skipSetItemText = true;*/
                uiControl.Items[(int)e.Data.itemIndex].Text = e.Data.label;
                /*skipSetItemText = false;*/
            }

            e.Result = ea.Cancel ? (IntPtr)1 : IntPtr.Zero;
        }
    }
}