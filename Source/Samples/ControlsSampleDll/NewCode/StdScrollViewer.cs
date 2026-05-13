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
        public static SizeD DefaultScrollSmallChange = new (16, VirtualListControl.DefaultMinItemHeight);
        
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

        public virtual SizeD? ScrollSmallChange { get; set; }

        public virtual SizeD? ScrollLargeChange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content is allowed to be scrolled vertically.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether the content is allowed to be scrolled horizontally.
        /// </summary>
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

            if (FirstChild is null)
            {
                horzScrollbar = new(horzVisibility);
                vertScrollbar = new(vertVisibility);
                return;
            }

            var paintRectangle = GetPaintRectangle();

            if (FirstChild.LayoutMaxSize is null)
            {
                UpdateInterior();
            }

            var range = FirstChild.LayoutMaxSize ?? paintRectangle.Size;

            horzScrollbar = new(position: (int)FirstChild.LayoutOffset.X, range: (int)range.Width, pageSize: (int)paintRectangle.Width);
            vertScrollbar = new(position: (int)FirstChild.LayoutOffset.Y, range: (int)range.Height, pageSize: (int)paintRectangle.Height);
            horzScrollbar.Visibility = horzVisibility;
            vertScrollbar.Visibility = vertVisibility;
        }

        /// <summary>
        /// Retrieves the width of a single character using the default font.
        /// </summary>
        /// <returns>
        /// A <see cref="Coord"/> representing the width of the character.
        /// </returns>
        public virtual Coord GetCharWidth()
        {
            var result = MeasureCanvas.GetTextExtent("W", RealFont).Width;
            if (result <= 0)
                result = 16;
            return result;
        }

        public virtual Coord GetCharHeight()
        {
            var result = MeasureCanvas.GetTextExtent("W", RealFont).Height;
            if (result <= 0)
                result = VirtualListControl.DefaultMinItemHeight;
            return result;
        }

        public virtual SizeD GetEffectiveScrollSmallChange()
        {
            if (ScrollSmallChange is not null)
            {
                return ScrollSmallChange.Value;
            }

            return DefaultScrollSmallChange;
        }

        public virtual SizeD GetEffectiveScrollLargeChange()
        {
            return ScrollLargeChange ?? GetPaintRectangle().Size;
        }

        public virtual void DoActionScrollCharLeft()
        {
            var value = GetEffectiveScrollSmallChange().Width;
            DoActionOffsetScroll(new SizeD(-value, 0));
        }

        public virtual void DoActionScrollCharRight()
        {
            var value = GetEffectiveScrollSmallChange().Width;
            DoActionOffsetScroll(new SizeD(value, 0));
        }

        public virtual void DoActionScrollLineDown()
        {
            var value = GetEffectiveScrollSmallChange().Height;
            DoActionOffsetScroll(new SizeD(0, value));
        }

        public virtual void DoActionScrollLineUp()
        {
            var value = GetEffectiveScrollSmallChange().Height;
            DoActionOffsetScroll(new SizeD(0, -value));
        }

        public virtual void DoActionScrollPageDown()
        {
            var value = GetEffectiveScrollLargeChange().Height;
            DoActionOffsetScroll(new SizeD(0, value));
        }

        public virtual void DoActionScrollPageUp()
        {
            var value = GetEffectiveScrollLargeChange().Height;
            DoActionOffsetScroll(new SizeD(0, -value));
        }

        public virtual void DoActionScrollPageLeft()
        {
            var value = GetEffectiveScrollLargeChange().Width;
            DoActionOffsetScroll(new SizeD(-value, 0));
        }

        public virtual void DoActionScrollPageRight()
        {
            var value = GetEffectiveScrollLargeChange().Width;
            DoActionOffsetScroll(new SizeD(value, 0));
        }

        public virtual void DoActionScrollToFirstChar()
        {
            DoActionScrollToHorzPos(0);
        }

        public virtual void DoActionScrollToFirstLine()
        {
            DoActionScrollToVertPos(0);
        }

        public virtual void DoActionScrollToHorzPos(int value)
        {
            DoActionSetScroll(new PointD(value, FirstChild?.LayoutOffset.Y ?? 0));
        }

        public virtual SizeD GetScrollRange()
        {
            if (FirstChild is null)
                return SizeD.Empty;
            var paintRectangle = GetPaintRectangle();

            if (FirstChild.LayoutMaxSize is null)
            {
                UpdateInterior();
            }

            if (FirstChild.LayoutMaxSize is null)
                return SizeD.Empty;

            return FirstChild.LayoutMaxSize.Value;
        }

        public virtual PointD GetScrollPosition()
        {
            if (FirstChild is null)
                return PointD.Empty;
            return FirstChild.LayoutOffset;
        }

        public virtual PointD GetMaxScrollPosition()
        {
            if (FirstChild is null)
                return PointD.Empty;
            var scrollRange = GetScrollRange();
            if (scrollRange == SizeD.Empty)
                return PointD.Empty;

            var paintRectangle = GetPaintRectangle();

            var lastHorizontalOffset = Math.Max(scrollRange.Width - paintRectangle.Width, 0);
            var lastVerticalOffset = Math.Max(scrollRange.Height - paintRectangle.Height, 0);
            return new PointD(lastHorizontalOffset, lastVerticalOffset);
        }

        public virtual void DoActionScrollToLastLine()
        {
            if (FirstChild is null)
                return;
            var scrollRange = GetScrollRange();
            if (scrollRange == SizeD.Empty)
                return;

            var lastLineOffset = scrollRange.Height - GetPaintRectangle().Height;
            DoActionSetScroll(new PointD(FirstChild.LayoutOffset.X, lastLineOffset));
        }

        public virtual void DoActionScrollToLastChar()
        {
            if (FirstChild is null)
                return;
            var scrollRange = GetScrollRange();
            if (scrollRange == SizeD.Empty)
                return;

            var lastCharOffset = scrollRange.Width - GetPaintRectangle().Width;
            DoActionSetScroll(new PointD(lastCharOffset, FirstChild.LayoutOffset.Y));
        }

        public virtual void DoActionScrollToVertPos(int value)
        {
            DoActionSetScroll(new PointD(FirstChild?.LayoutOffset.X ?? 0, value));
        }

        public virtual void DoActionSetScroll(PointD value)
        {
            var firstChild = FirstChild;
            if (firstChild is null)
                return;

            var newOffset = value.ClampToZero();

            var maxOffset = GetMaxScrollPosition();

            if (newOffset != firstChild.LayoutOffset)
            {
                firstChild.LayoutOffset = newOffset.ClampToMax(maxOffset);
                UpdateScrollBars(true);
            }
        }

        public virtual void DoActionOffsetScroll(SizeD value)
        {
            if (value.Height == 0 && value.Width == 0)
                return;
            var firstChild = FirstChild;
            if (firstChild is null)
                return;

            var oldOffset = firstChild.LayoutOffset;
            var newOffset = oldOffset + value;
            DoActionSetScroll(newOffset);
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

        protected virtual bool IsScrolledControl(AbstractControl control)
        {
            return control.IsVisible && !control.IgnoreLayout;
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

            var value = firstChild.GetChildrenMaxRightBottom(includeMargins: true);

            value.Width += firstChild.LayoutOffset.X;
            value.Height += firstChild.LayoutOffset.Y;

            firstChild.LayoutMaxSize = value;

            UpdateScrollBars(true);

            var newPaintRectangle = GetPaintRectangle();

            if (newPaintRectangle != paintRectangle)
            {
                PerformLayout(layoutParent: false);
            }
        }

        private void OnChildLayoutUpdated(object? sender, EventArgs e)
        {
            UpdateInterior();

            var maxScrollPosition = GetMaxScrollPosition();
            var scrollPosition = GetScrollPosition();

            DoActionSetScroll(new PointD(
                Math.Min(scrollPosition.X, maxScrollPosition.X),
                Math.Min(scrollPosition.Y, maxScrollPosition.Y)));
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
