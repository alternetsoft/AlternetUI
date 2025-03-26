using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    partial class WxControlHandler
    {
        const int wxHORIZONTAL = 0x0004;
        const int wxVERTICAL = 0x0008;

        public bool IsScrollable
        {
            get => NativeControl.IsScrollable;
            set => NativeControl.IsScrollable = value;
        }

        public ScrollBarInfo VertScrollBarInfo
        {
            get
            {
                return GetScrollBarInfo(true);
            }

            set
            {
                SetScrollBarInfo(true, value);
            }
        }

        public ScrollBarInfo HorzScrollBarInfo
        {
            get
            {
                return GetScrollBarInfo(false);
            }

            set
            {
                SetScrollBarInfo(false, value);
            }
        }

        public ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            ScrollBarOrientation orientation;

            if (isVertical)
            {
                orientation = ScrollBarOrientation.Vertical;
            }
            else
            {
                orientation = ScrollBarOrientation.Horizontal;
            }

            ScrollBarInfo result = new();

            result.Range = NativeControl.GetScrollBarMaximum(orientation);
            result.PageSize = NativeControl.GetScrollBarLargeChange(orientation);
            result.Position = NativeControl.GetScrollBarValue(orientation);

            result.Visibility = NativeControl.IsScrollBarVisible(orientation)
                ? HiddenOrVisible.Auto : HiddenOrVisible.Hidden;

            return result;
        }

        public void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            ScrollBarOrientation orientation;
            if (isVertical)
            {
                orientation = ScrollBarOrientation.Vertical;
            }
            else
            {
                orientation = ScrollBarOrientation.Horizontal;
            }

            NativeControl.SetScrollBar(
                orientation,
                value.IsVisible,
                value.Position,
                value.PageSize,
                value.Range);
        }
    }
}
