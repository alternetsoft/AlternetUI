using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace ControlsSample
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    [ControlCategory("Containers")]
    internal partial class StdScrollViewer : ScrollableUserControl, IScrollEventRouter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StdScrollViewer"/> class.
        /// </summary>
        public StdScrollViewer()
        {
        }

        /// <inheritdoc/>
        public override IScrollEventRouter ScrollEventRouter
        {
            get
            {
                return this;
            }
        }

        void IScrollEventRouter.CalcScrollBarInfo(out ScrollBarInfo horzScrollbar, out ScrollBarInfo vertScrollbar)
        {
            HiddenOrVisible vertVisibility = HiddenOrVisible.Auto;
            HiddenOrVisible horzVisibility = HiddenOrVisible.Auto;

            if (HasHorizontalScrollBarSettings)
                horzVisibility = HorizontalScrollBarSettings.SuggestedVisibility;
            if (HasVerticalScrollBarSettings)
                vertVisibility = VerticalScrollBarSettings.SuggestedVisibility;

            if (!HasChildren)
            {
                horzScrollbar = new(horzVisibility);
                vertScrollbar = new(vertVisibility);
                return;
            }

            var paintRectangle = GetPaintRectangle();

            horzScrollbar = new(position: 0, range: 0, pageSize: -1);
            vertScrollbar = new(position: 0, range: 0, pageSize: -1);
            horzScrollbar.Visibility = horzVisibility;
            vertScrollbar.Visibility = vertVisibility;
        }

        void IScrollEventRouter.DoActionScrollCharLeft()
        {
        }

        void IScrollEventRouter.DoActionScrollCharRight()
        {
        }

        void IScrollEventRouter.DoActionScrollLineDown()
        {
        }

        void IScrollEventRouter.DoActionScrollLineUp()
        {
        }

        void IScrollEventRouter.DoActionScrollPageDown()
        {
        }

        void IScrollEventRouter.DoActionScrollPageLeft()
        {
        }

        void IScrollEventRouter.DoActionScrollPageRight()
        {
        }

        void IScrollEventRouter.DoActionScrollPageUp()
        {
        }

        void IScrollEventRouter.DoActionScrollToFirstChar()
        {
        }

        void IScrollEventRouter.DoActionScrollToFirstLine()
        {
        }

        void IScrollEventRouter.DoActionScrollToHorzPos(int value)
        {
        }

        void IScrollEventRouter.DoActionScrollToLastLine()
        {
        }

        void IScrollEventRouter.DoActionScrollToVertPos(int value)
        {
        }
    }
}
