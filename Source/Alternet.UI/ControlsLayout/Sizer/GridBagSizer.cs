using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GridBagSizer : FlexGridSizer, IGridBagSizer
    {
        public GridBagSizer(int vgap, int hgap, bool disposeHandle)
            : base(
                  Native.GridBagSizer.CreateGridBagSizer(vgap, hgap),
                  disposeHandle)
        {
        }

        public Int32Size GetCellSize(int row, int col)
        {
            return Native.GridBagSizer.GetCellSize(Handle, row, col);
        }

        public Int32Size GetEmptyCellSize()
        {
            return Native.GridBagSizer.GetEmptyCellSize(Handle);
        }

        public void SetEmptyCellSize(Int32Size sz)
        {
            Native.GridBagSizer.SetEmptyCellSize(Handle, sz);
        }

        public ISizer FindItem(Control window)
        {
            var itemHandle = Native.GridBagSizer.FindItem(Handle, window.WxWidget);
            return new Sizer(itemHandle, false);
        }

        public ISizer FindItem(ISizer sizer)
        {
            var itemHandle = Native.GridBagSizer.FindItem2(Handle, sizer.Handle);
            return new Sizer(itemHandle, false);
        }

        public Int32Point GetItemPosition(Control window)
        {
            return Native.GridBagSizer.GetItemPosition(Handle, window.WxWidget);
        }

        public Int32Point GetItemPosition(ISizer sizer)
        {
            return Native.GridBagSizer.GetItemPosition2(Handle, sizer.Handle);
        }

        public Int32Point GetItemPosition(int index)
        {
            return Native.GridBagSizer.GetItemPosition3(Handle, index);
        }

        public Int32Point GetItemSpan(Control window)
        {
            return Native.GridBagSizer.GetItemSpan(Handle, window.WxWidget);
        }

        public Int32Point GetItemSpan(ISizer sizer)
        {
            return Native.GridBagSizer.GetItemSpan2(Handle, sizer.Handle);
        }

        public Int32Point GetItemSpan(int index)
        {
            return Native.GridBagSizer.GetItemSpan3(Handle, index);
        }

        public bool SetItemPosition(Control window, Int32Point pos)
        {
            return Native.GridBagSizer.SetItemPosition(Handle, window.WxWidget, pos);
        }

        public bool SetItemPosition(ISizer sizer, Int32Point pos)
        {
            return Native.GridBagSizer.SetItemPosition2(Handle, sizer.Handle, pos);
        }

        public bool SetItemPosition(int index, Int32Point pos)
        {
            return Native.GridBagSizer.SetItemPosition3(Handle, index, pos);
        }

        public bool SetItemSpan(Control window, Int32Size span)
        {
            return Native.GridBagSizer.SetItemSpan(Handle, window.WxWidget, span);
        }

        public bool SetItemSpan(ISizer sizer, Int32Size span)
        {
            return Native.GridBagSizer.SetItemSpan2(Handle, sizer.Handle, span);
        }

        public bool SetItemSpan(int index, Int32Size span)
        {
            return Native.GridBagSizer.SetItemSpan3(Handle, index, span);
        }
    }
}
