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
            Interior?.Required();
        }

        /// <inheritdoc/>
        public override IScrollEventRouter ScrollEventRouter
        {
            get
            {
                return this;
            }
        }

        /// <inheritdoc/>
        public override RectD ChildrenLayoutBounds
        {
            get
            {
                UpdateScrollBars(false);
                UpdateInteriorProperties();
                return GetPaintRectangle();
            }
        }

        void IScrollEventRouter.CalcScrollBarInfo(out ScrollBarInfo horzScrollbar, out ScrollBarInfo vertScrollbar)
        {
            HiddenOrVisible vertVisibility = HiddenOrVisible.Visible; // need to be Auto
            HiddenOrVisible horzVisibility = HiddenOrVisible.Visible; // need to be Auto

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

            horzScrollbar = new(position: 0, range: 50, pageSize: 15);
            vertScrollbar = new(position: 0, range: 50, pageSize: 15);
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

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            var dc = e.Graphics;

            dc.FillRectangle(RealBackgroundColor.AsBrush, ClientRectangle);

            UpdateInteriorProperties();

            DrawInterior(dc);
        }


        /// <inheritdoc/>
        public override void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            base.SetScrollBarInfo(isVertical, value);
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnChildInserted(int index, AbstractControl childControl)
        {
            base.OnChildInserted(index, childControl);

            childControl.ChildInserted += OnChildOfChildInserted;
            childControl.ChildRemoved += OnChildOfChildRemoved;

            UpdateScrollBars(true);
        }

        /// <inheritdoc/>
        protected override void OnChildRemoved(AbstractControl childControl)
        {
            UpdateScrollBars(true);
            childControl.ChildInserted -= OnChildOfChildInserted;
            childControl.ChildRemoved -= OnChildOfChildRemoved;
            base.OnChildRemoved(childControl);
        }

        private void OnChildOfChildRemoved(object? sender, BaseEventArgs<AbstractControl> e)
        {
        }

        private void OnChildOfChildInserted(object? sender, BaseEventArgs<AbstractControl> e)
        {
        }
    }
}
