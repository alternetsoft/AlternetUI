using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an abstract scrollable canvas control that provides functionality for displaying content
    /// that can be scrolled both horizontally and vertically.
    /// </summary>
    public abstract partial class ScrollableCanvasControl : ScrollableUserControl, IScrollEventRouter
    {
        private bool isScrolledHorizontally = true;
        private bool isScrolledVertically = true;
        private PointD layoutOffset;

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
                UpdateInterior();
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
                UpdateInterior();
            }
        }

        /// <summary>
        /// Gets or sets the small change value for scrolling, which determines how much
        /// the content should scroll when a small scroll action is performed (e.g., scrolling by one line or one character).
        /// </summary>
        public virtual SizeD? ScrollSmallChange { get; set; }

        /// <summary>
        /// Gets or sets the large change value for scrolling, which determines how much
        /// the content should scroll when a large scroll action is performed (e.g., scrolling by one page).
        /// </summary>
        public virtual SizeD? ScrollLargeChange { get; set; }

        /// <inheritdoc/>
        [Browsable(false)]
        public override IScrollEventRouter ScrollEventRouter
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets or sets the layout offset, which represents the current scroll position of the content.
        /// </summary>
        new protected virtual PointD LayoutOffset
        {
            get
            {
                return layoutOffset;
            }

            set
            {
                if (layoutOffset == value)
                    return;
                layoutOffset = value;
                UpdateInterior();
            }
        }

        /// <summary>
        /// Gets the maximum size of the layout, which is determined by the preferred size of the content and the padding of the control.
        /// </summary>
        new protected virtual SizeD LayoutMaxSize
        {
            get
            {
                if (!IsContentVisible())
                    return SizeD.Empty;
                var result = GetContentPreferredSize();
                var withPadding = result + Padding.Size;
                return withPadding;
            }
        }

        /// <summary>
        /// Retrieves the preferred size of the content, which is used to determine the layout and scrolling behavior of the control.
        /// </summary>
        /// <returns></returns>
        protected abstract SizeD GetContentPreferredSize();

        /// <summary>
        /// Determines whether the content is currently visible, which can affect the visibility and behavior of scroll bars.
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsContentVisible();

        /// <inheritdoc/>
        public virtual void CalcScrollBarInfo(out ScrollBarInfo horzScrollbar, out ScrollBarInfo vertScrollbar)
        {
            var imageVisible = IsContentVisible();

            HiddenOrVisible vertVisibility = IsScrolledVertically ? HiddenOrVisible.Auto : HiddenOrVisible.Hidden;
            HiddenOrVisible horzVisibility = IsScrolledHorizontally ? HiddenOrVisible.Auto : HiddenOrVisible.Hidden;

            if (HasHorizontalScrollBarSettings)
                horzVisibility = HorizontalScrollBarSettings.SuggestedVisibility;
            if (HasVerticalScrollBarSettings)
                vertVisibility = VerticalScrollBarSettings.SuggestedVisibility;

            if (!imageVisible)
            {
                horzScrollbar = new(horzVisibility);
                vertScrollbar = new(vertVisibility);
                return;
            }

            var paintRectangle = GetPaintRectangle();

            var range = LayoutMaxSize;

            var layoutOffset = LayoutOffset;
            var xOffset = (int)layoutOffset.X;
            var yOffset = (int)layoutOffset.Y;

            horzScrollbar = new(position: xOffset, range: (int)range.Width, pageSize: (int)paintRectangle.Width);
            vertScrollbar = new(position: yOffset, range: (int)range.Height, pageSize: (int)paintRectangle.Height);
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

        /// <summary>
        /// Retrieves the height of a single character using the default font.
        /// </summary>
        /// <returns>
        /// A <see cref="Coord"/> representing the height of the character.
        /// </returns>
        public virtual Coord GetCharHeight()
        {
            var result = MeasureCanvas.GetTextExtent("W", RealFont).Height;
            if (result <= 0)
                result = VirtualListControl.DefaultMinItemHeight;
            return result;
        }

        /// <summary>
        /// Calculates and retrieves the effective small change value for scrolling, which determines how much
        /// the content should scroll when a small scroll action is performed (e.g., scrolling by one line or one character).
        /// </summary>
        /// <returns>A <see cref="SizeD"/> representing the effective small change value for scrolling. </returns>
        public virtual SizeD GetEffectiveScrollSmallChange()
        {
            if (ScrollSmallChange is not null)
            {
                return ScrollSmallChange.Value;
            }

            return ScrollViewer.DefaultScrollSmallChange;
        }

        /// <summary>
        /// Calculates and retrieves the effective large change value for scrolling, which determines how much
        /// the content should scroll when a large scroll action is performed (e.g., scrolling by one page).
        /// </summary>
        /// <returns>A <see cref="SizeD"/> representing the effective large change value for scrolling. </returns>
        public virtual SizeD GetEffectiveScrollLargeChange()
        {
            return ScrollLargeChange ?? GetPaintRectangle().Size;
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollCharLeft()
        {
            var value = GetEffectiveScrollSmallChange().Width;
            DoActionOffsetScroll(new SizeD(-value, 0));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollCharRight()
        {
            var value = GetEffectiveScrollSmallChange().Width;
            DoActionOffsetScroll(new SizeD(value, 0));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollLineDown()
        {
            var value = GetEffectiveScrollSmallChange().Height;
            DoActionOffsetScroll(new SizeD(0, value));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollLineUp()
        {
            var value = GetEffectiveScrollSmallChange().Height;
            DoActionOffsetScroll(new SizeD(0, -value));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageDown()
        {
            var value = GetEffectiveScrollLargeChange().Height;
            DoActionOffsetScroll(new SizeD(0, value));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageUp()
        {
            var value = GetEffectiveScrollLargeChange().Height;
            DoActionOffsetScroll(new SizeD(0, -value));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageLeft()
        {
            var value = GetEffectiveScrollLargeChange().Width;
            DoActionOffsetScroll(new SizeD(-value, 0));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollPageRight()
        {
            var value = GetEffectiveScrollLargeChange().Width;
            DoActionOffsetScroll(new SizeD(value, 0));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToFirstChar()
        {
            DoActionScrollToHorzPos(0);
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToFirstLine()
        {
            DoActionScrollToVertPos(0);
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToHorzPos(int value)
        {
            DoActionSetScroll(new PointD(value, LayoutOffset.Y));
        }

        /// <summary>
        /// Calculates and retrieves the total scrollable area based on the maximum right and bottom positions of the child controls,
        /// </summary>
        /// <returns>A <see cref="SizeD"/> representing the total scrollable area.</returns>
        public virtual SizeD GetScrollRange()
        {
            return LayoutMaxSize;
        }

        /// <summary>
        /// Retrieves the current scroll position, which is determined by the layout offset of the first child control.
        /// </summary>
        /// <returns>A <see cref="PointD"/> representing the current scroll position.</returns>
        public virtual PointD GetScrollPosition()
        {
            return LayoutOffset;
        }

        /// <summary>
        /// Calculates and retrieves the maximum scroll position based on the scroll range and the visible area.
        /// </summary>
        /// <returns>A <see cref="PointD"/> representing the maximum scroll position.</returns>
        public virtual PointD GetMaxScrollPosition()
        {
            var scrollRange = GetScrollRange();
            if (scrollRange == SizeD.Empty)
                return PointD.Empty;

            var paintRectangle = GetPaintRectangle();

            var lastHorizontalOffset = Math.Max(scrollRange.Width - paintRectangle.Width, 0);
            var lastVerticalOffset = Math.Max(scrollRange.Height - paintRectangle.Height, 0);
            return new PointD(lastHorizontalOffset, lastVerticalOffset);
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToLastLine()
        {
            var scrollRange = GetScrollRange();
            if (scrollRange == SizeD.Empty)
                return;

            var lastLineOffset = scrollRange.Height - GetPaintRectangle().Height;
            DoActionSetScroll(new PointD(LayoutOffset.X, lastLineOffset));
        }

        /// <summary>
        /// Scrolls the content to the last character, which is determined by calculating
        /// the maximum horizontal scroll offset based on the scroll range and the visible area.
        /// </summary>
        public virtual void DoActionScrollToLastChar()
        {
            var scrollRange = GetScrollRange();
            if (scrollRange == SizeD.Empty)
                return;

            var lastCharOffset = scrollRange.Width - GetPaintRectangle().Width;
            DoActionSetScroll(new PointD(lastCharOffset, LayoutOffset.Y));
        }

        /// <inheritdoc/>
        public virtual void DoActionScrollToVertPos(int value)
        {
            DoActionSetScroll(new PointD(LayoutOffset.X, value));
        }

        /// <summary>
        /// Scrolls the content to the specified position, ensuring that the new scroll position is within valid bounds.
        /// </summary>
        /// <param name="value">The target scroll position.</param>
        public virtual bool DoActionSetScroll(PointD value)
        {
            var newOffset = value.ClampToZero();
            var maxOffset = GetMaxScrollPosition();
            newOffset = newOffset.ClampToMax(maxOffset);

            var oldOffset = LayoutOffset;

            LayoutOffset = newOffset;

            return newOffset != oldOffset;
        }

        /// <summary>
        /// Scrolls the content by the specified offset.
        /// </summary>
        /// <param name="value">The offset by which to scroll the content.</param>
        public virtual void DoActionOffsetScroll(SizeD value)
        {
            if (value.Height == 0 && value.Width == 0)
                return;

            var oldOffset = LayoutOffset;
            var newOffset = oldOffset + value;
            DoActionSetScroll(newOffset);
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateInterior();
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            UpdateInterior();
        }

        /// <summary>
        /// Updates the interior of the control, which includes recalculating the paint rectangle,
        /// updating scroll bars, and refreshing the display.
        /// </summary>
        protected virtual void UpdateInterior()
        {
            GetPaintRectangle();
            UpdateScrollBars(refresh: false);
            UpdateInteriorProperties();
            Refresh();
        }
    }
}
