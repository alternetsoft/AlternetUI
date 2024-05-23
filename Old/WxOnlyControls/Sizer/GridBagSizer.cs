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

        public SizeI EmptyCellSize
        {
            get
            {
                return Native.GridBagSizer.GetEmptyCellSize(Handle);
            }

            set
            {
                Native.GridBagSizer.SetEmptyCellSize(Handle, value);
            }
        }

        public SizeI GetCellSize(int row, int col)
        {
            return Native.GridBagSizer.GetCellSize(Handle, row, col);
        }

        public ISizerItem FindItem(Control window)
        {
            var itemHandle = Native.GridBagSizer.FindItem(Handle, WxPlatform.WxWidget(window));
            return new SizerItem(itemHandle, false);
        }

        public ISizerItem FindItem(ISizer sizer)
        {
            var itemHandle = Native.GridBagSizer.FindItem2(Handle, ((Sizer)sizer).Handle);
            return new SizerItem(itemHandle, false);
        }

        public PointI GetItemPosition(Control window)
        {
            return Native.GridBagSizer.GetItemPosition(Handle, WxPlatform.WxWidget(window));
        }

        public PointI GetItemPosition(ISizer sizer)
        {
            return Native.GridBagSizer.GetItemPosition2(Handle, ((Sizer)sizer).Handle);
        }

        public PointI GetItemPosition(int index)
        {
            return Native.GridBagSizer.GetItemPosition3(Handle, index);
        }

        public PointI GetItemSpan(Control window)
        {
            return Native.GridBagSizer.GetItemSpan(Handle, WxPlatform.WxWidget(window));
        }

        public PointI GetItemSpan(ISizer sizer)
        {
            return Native.GridBagSizer.GetItemSpan2(Handle, ((Sizer)sizer).Handle);
        }

        public PointI GetItemSpan(int index)
        {
            return Native.GridBagSizer.GetItemSpan3(Handle, index);
        }

        public bool SetItemPosition(Control window, PointI pos)
        {
            return Native.GridBagSizer.SetItemPosition(Handle, WxPlatform.WxWidget(window), pos);
        }

        public bool SetItemPosition(ISizer sizer, PointI pos)
        {
            return Native.GridBagSizer.SetItemPosition2(Handle, ((Sizer)sizer).Handle, pos);
        }

        public bool SetItemPosition(int index, PointI pos)
        {
            return Native.GridBagSizer.SetItemPosition3(Handle, index, pos);
        }

        public bool SetItemSpan(Control window, SizeI span)
        {
            return Native.GridBagSizer.SetItemSpan(Handle, WxPlatform.WxWidget(window), span);
        }

        public bool SetItemSpan(ISizer sizer, SizeI span)
        {
            return Native.GridBagSizer.SetItemSpan2(Handle, ((Sizer)sizer).Handle, span);
        }

        public bool SetItemSpan(int index, SizeI span)
        {
            return Native.GridBagSizer.SetItemSpan3(Handle, index, span);
        }

        public ISizerItem? FindItemAtPoint(PointI pt)
        {
            var result = Native.GridBagSizer.FindItemAtPoint(Handle, pt);
            if (result == IntPtr.Zero)
                return null;
            return new SizerItem(result, false);
        }

        public ISizerItem? FindItemAtPosition(PointI pos)
        {
            var result = Native.GridBagSizer.FindItemAtPosition(Handle, pos);
            if (result == IntPtr.Zero)
                return null;
            return new SizerItem(result, false);
        }

        public ISizerItem Add(
            Control control,
            PointI pos,
            SizeI span,
            SizerFlag flag = 0,
            int border = 0)
        {
            var result = Native.GridBagSizer.Add(
                Handle,
                WxPlatform.WxWidget(control),
                pos,
                span,
                (int)flag,
                border,
                default);
            return new SizerItem(result, false);
        }

        public ISizerItem Add(
            ISizer sizer,
            PointI pos,
            SizeI span,
            SizerFlag flag = 0,
            int border = 0)
        {
            var result = Native.GridBagSizer.Add2(
                Handle,
                ((Sizer)sizer).Handle,
                pos,
                span,
                (int)flag,
                border,
                default);
            return new SizerItem(result, false);
        }

        public ISizerItem Add(
            int width,
            int height,
            PointI pos,
            SizeI span,
            SizerFlag flag = 0,
            int border = 0)
        {
            var result = Native.GridBagSizer.Add4(
                Handle,
                width,
                height,
                pos,
                span,
                (int)flag,
                border,
                default);
            return new SizerItem(result, false);
        }

        public bool CheckForIntersection(ISizerItem item, ISizerItem? excludeItem)
        {
            if(excludeItem is null)
                return Native.GridBagSizer.CheckForIntersection(Handle, ((SizerItem)item).Handle, default);
            return Native.GridBagSizer.CheckForIntersection(
                Handle,
                ((SizerItem)item).Handle,
                ((SizerItem)excludeItem).Handle);
        }

        public bool CheckForIntersection(PointI pos, SizeI span, ISizerItem? excludeItem)
        {
            if (excludeItem is null)
                return Native.GridBagSizer.CheckForIntersection2(Handle, pos, span, default);
            return Native.GridBagSizer.CheckForIntersection2(Handle, pos, span, ((SizerItem)excludeItem).Handle);
        }
    }
}