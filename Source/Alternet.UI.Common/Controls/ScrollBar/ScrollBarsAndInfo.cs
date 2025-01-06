using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal struct ScrollBarsAndInfo
    {
        public ScrollBarAndInfo Vertical;

        public ScrollBarAndInfo Horizontal;

        public readonly AbstractControl? Container => Vertical.ScrollBar?.Parent;

        public ScrollBar GetScrollBarSafe(bool isVert, Action<ScrollBar> initialize)
        {
            var result = GetScrollBar(isVert);

            if (result is null)
            {
                result = new();
                result.Visible = false;
                result.IsVertical = isVert;
                result.IgnoreLayout = true;
                SetScrollBar(isVert, result);
                initialize(result);
            }

            return result;
        }

        public readonly void Layout()
        {
            /*
                        var container = Container;
                        if (container.HasBorder)
                        {
                            var borderSettings = Border!.Border;
                            if (borderSettings is not null)
                            {
                                result[HitTestResult.TopBorder] = borderSettings.GetTopRectangle(Bounds);
                                result[HitTestResult.BottomBorder] = borderSettings.GetBottomRectangle(Bounds);
                                result[HitTestResult.LeftBorder] = borderSettings.GetLeftRectangle(Bounds);
                                result[HitTestResult.RightBorder] = borderSettings.GetRightRectangle(Bounds);
                            }
                        }
                        else
                        {
                            borderWidth = 0;
                        }

                        var boundsInsideBorder = Bounds.DeflatedWithPadding(borderWidth);
            */
        }

        public readonly ScrollBarInfo GetInfo(bool isVert)
        {
            if (isVert)
                return Vertical.Info;
            else
                return Horizontal.Info;
        }

        public readonly ScrollBar? GetScrollBar(bool isVert)
        {
            if (isVert)
                return Vertical.ScrollBar;
            else
                return Horizontal.ScrollBar;
        }

        public void SetScrollBar(bool isVert, ScrollBar? value)
        {
            if (isVert)
                Vertical.ScrollBar = value;
            else
                Horizontal.ScrollBar = value;
        }

        public void SetInfo(bool isVert, in ScrollBarInfo info)
        {
            if (isVert)
                Vertical.Info = info;
            else
                Horizontal.Info = info;
        }
    }
}
