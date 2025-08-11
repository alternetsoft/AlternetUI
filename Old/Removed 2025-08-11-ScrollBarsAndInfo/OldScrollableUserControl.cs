using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class ScrollableUserControl : HiddenBorder
    {
        private ScrollBarsAndInfo scrollBars = new();

        /// <inheritdoc/>
        public override ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            return scrollBars.GetInfo(isVertical);
        }

        /// <inheritdoc/>
        public override void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            var info = GetScrollBarInfo(isVertical);

            if (info == value)
                return;

            Internal();

            void Internal()
            {
                void Initialize(ScrollBar scrollBar)
                {
                    scrollBar.Parent = this;

                    scrollBar.Scroll += (s, e) =>
                    {
                        RaiseScroll(e);
                    };
                }

                scrollBars.SetInfo(isVertical, value);

                if (value.Visibility == HiddenOrVisible.Hidden)
                    return;

                var scrollBar = scrollBars.GetScrollBarSafe(isVertical, Initialize);
                scrollBar.PosInfo = value;
                scrollBars.Layout();
                Invalidate();
            }

            RaiseNotifications((n) => n.AfterSetScrollBarInfo(this, isVertical, value));
        }
    }
}
