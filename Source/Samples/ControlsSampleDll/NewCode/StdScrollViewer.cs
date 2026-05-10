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
        private bool isScrolledVertically = true;
        private bool isScrolledHorizontally = true;

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

        public virtual bool IsScrolledVertically
        {
            get => isScrolledVertically;
            set
            {
                if (isScrolledVertically == value)
                    return;
                isScrolledVertically = value;
            }
        }

        public virtual bool IsScrolledHorizontally
        {
            get => isScrolledHorizontally;
            set
            {
                if (isScrolledHorizontally == value)
                    return;
                isScrolledHorizontally = value;
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
            HiddenOrVisible vertVisibility = IsScrolledVertically ? HiddenOrVisible.Auto : HiddenOrVisible.Hidden;
            HiddenOrVisible horzVisibility = IsScrolledHorizontally ? HiddenOrVisible.Auto : HiddenOrVisible.Hidden;

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

            if (LayoutMaxSize is null)
            {
                UpdateInterior();
            }

            var range = LayoutMaxSize ?? paintRectangle.Size;

            horzScrollbar = new(position: (int)LayoutOffset.X, range: (int)range.Width, pageSize: (int)paintRectangle.Width);
            vertScrollbar = new(position: (int)LayoutOffset.Y, range: (int)range.Height, pageSize: (int)paintRectangle.Height);
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
            UpdateInterior();
        }

        protected virtual void UpdateInterior()
        {
            var firstChild = FirstChild;

            if (firstChild is null)
                return;

            var availableSize = SizeD.PositiveInfinity;

            var paintRectangle = GetPaintRectangle();

            if (!IsScrolledVertically)
                availableSize.Height = paintRectangle.Height;
            if (!IsScrolledHorizontally)
                availableSize.Width = paintRectangle.Width;

            PreferredSizeContext context = new(availableSize);

            LayoutMaxSize = firstChild.GetChildrenMaxRightBottom(includeMargins: true);

            UpdateScrollBars(true);
        }

        /// <inheritdoc/>
        protected override void OnChildInserted(int index, AbstractControl childControl)
        {
            base.OnChildInserted(index, childControl);

            if (IsScrolledControl(childControl))
            {
                childControl.LayoutUpdated += OnChildLayoutUpdated;
                UpdateInterior();
            }
        }

        /// <inheritdoc/>
        protected override void OnChildRemoved(AbstractControl childControl)
        {
            childControl.LayoutUpdated -= OnChildLayoutUpdated;
            base.OnChildRemoved(childControl);

            if (IsScrolledControl(childControl))
            {
                UpdateInterior();
            }
        }

        private void OnChildLayoutUpdated(object? sender, EventArgs e)
        {
            UpdateInterior();
        }

        protected virtual bool IsScrolledControl(AbstractControl control)
        {
            return control.IsVisible && !control.IgnoreLayout;
        }

        private void OnChildOfChildRemoved(object? sender, BaseEventArgs<AbstractControl> e)
        {
            if(!IsScrolledControl(e.Value))
                return;
            UpdateInterior();
        }

        private void OnChildOfChildInserted(object? sender, BaseEventArgs<AbstractControl> e)
        {
            if (!IsScrolledControl(e.Value))
                return;
            UpdateInterior();
        }
    }
}
