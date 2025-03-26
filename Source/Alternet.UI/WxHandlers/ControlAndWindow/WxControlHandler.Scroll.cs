using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    partial class WxControlHandler
    {
        const int wxHORIZONTAL = 0x0004;
        const int wxVERTICAL = 0x0008;

        private ScrollBarInfo vertScrollBarInfo = ScrollBarInfo.Default;
        private ScrollBarInfo horzScrollBarInfo = ScrollBarInfo.Default;
        private bool vertScrollBarInfoAssigned;
        private bool horzScrollBarInfoAssigned;

        public bool IsScrollable
        {
            get => NativeControl.IsScrollable;
            set => NativeControl.IsScrollable = value;
        }

        public bool ScrollBarAlwaysVisible
        {
            get => NativeControl.ScrollBarAlwaysVisible;

            set
            {
                NativeControl.ScrollBarAlwaysVisible = value;
            }
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
            ScrollBarInfo defaultResult;
            bool scrollbarAssigned;

            if (isVertical)
            {
                orientation = ScrollBarOrientation.Vertical;
                scrollbarAssigned = vertScrollBarInfoAssigned;
                defaultResult = vertScrollBarInfo;
            }
            else
            {
                orientation = ScrollBarOrientation.Horizontal;
                scrollbarAssigned = horzScrollBarInfoAssigned;
                defaultResult = horzScrollBarInfo;
            }

            if (!scrollbarAssigned)
            {
                return defaultResult;
            }

            if (!defaultResult.IsVisible)
                return defaultResult;

            bool canScroll = NativeControl.CanScroll(isVertical ? wxVERTICAL : wxHORIZONTAL);

            if (!canScroll)
                return defaultResult;

            /*
            bool hasScrollBar =
                NativeControl.HasScrollbar(isVertical ? wxVERTICAL : wxHORIZONTAL);

            if (!hasScrollBar)
            {
                return ScrollBarInfo.Default;
            }
            */

            ScrollBarInfo result = new();

            result.Range = NativeControl.GetScrollBarMaximum(orientation);
            result.PageSize = NativeControl.GetScrollBarLargeChange(orientation);
            result.Position = NativeControl.GetScrollBarValue(orientation);

            if (ScrollBarAlwaysVisible)
            {
                result.Visibility = HiddenOrVisible.Visible;
            }
            else
            {
                result.Visibility = NativeControl.IsScrollBarVisible(orientation)
                    ? HiddenOrVisible.Auto : HiddenOrVisible.Hidden;
            }

            if (isVertical)
                vertScrollBarInfo = result;
            else
                horzScrollBarInfo = result;

            return result;
        }

        public void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            ScrollBarOrientation orientation;
            if (isVertical)
            {
                orientation = ScrollBarOrientation.Vertical;
                vertScrollBarInfo = value;
                if (value.IsVisible)
                {
                    vertScrollBarInfoAssigned = true;
                }
            }
            else
            {
                orientation = ScrollBarOrientation.Horizontal;
                horzScrollBarInfo = value;
                if (value.IsVisible)
                {
                    horzScrollBarInfoAssigned = true;
                }
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
