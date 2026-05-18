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
        /// <summary>
        /// Defines the default content size scale factor, which is used to determine the maximum scrollable area based
        /// on the preferred size of the content. This scale factor allows for additional scrolling beyond the calculated
        /// maximum size, which can be useful in scenarios such as accommodating on-screen keyboards or other dynamic content changes.
        /// </summary>
        public static SizeD DefaultContentSizeScale = SizeD.One;

        /// <summary>
        /// Gets or sets default mouse wheel scroll factor. This value is multiplied
        /// with line height and used as an offset when control is scrolled using mouse wheel.
        /// </summary>
        public static SizeD DefaultMouseWheelScrollFactor = (3, 3);

        /// <summary>
        /// Defines the default small change value for scrolling, which determines how much
        /// the content should scroll when a small scroll action is performed (e.g., scrolling by one line or one character).
        /// </summary>
        public static SizeD DefaultScrollSmallChange = new(40, 40);

        private bool insideUpdateInterior;
        private PointD layoutOffset;
        private int suspendUpdateInteriorCounter;
        private SizeD? contentSizeScale;

        /// <summary>
        /// Gets or sets a value indicating whether the content is allowed to be scrolled vertically.
        /// </summary>
        public virtual bool IsScrolledVertically
        {
            get => Interior.VertScrollBar?.Visible ?? true;
            set
            {
                if (IsScrolledVertically == value)
                    return;
                if (Interior.VertScrollBar != null)
                    Interior.VertScrollBar.Visible = value;
                OnScrolledVerticallyChanged();
            }
        }

        /// <summary>
        /// Defines the content size scale factor, which is used to determine the maximum scrollable area based
        /// on the preferred size of the content. This scale factor allows for additional scrolling beyond the calculated
        /// maximum size, which can be useful in scenarios such as accommodating on-screen keyboards or other dynamic content changes.
        /// </summary>
        /// <remarks>
        /// If this property is set to null, the default content size scale factor
        /// defined by <see cref="DefaultContentSizeScale"/> will be used.
        /// </remarks>
        public virtual SizeD? ContentSizeScale
        {
            get => contentSizeScale;
            set => contentSizeScale = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content is allowed to be scrolled horizontally.
        /// </summary>
        public virtual bool IsScrolledHorizontally
        {
            get => Interior.HorzScrollBar?.Visible ?? true;
            set
            {
                if (IsScrolledHorizontally == value)
                    return;
                if (Interior.HorzScrollBar != null)
                    Interior.HorzScrollBar.Visible = value;
                OnScrolledHorizontallyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scroll
        /// viewer is scrolled with the mouse wheel.
        /// </summary>
        public virtual bool IsScrolledWithMouseWheel { get; set; } = true;

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

        /// <inheritdoc/>
        [Browsable(false)]
        public override ScrollBarInfo HorzScrollBarInfo
        {
            get
            {
                var result = GetScrollBarInfo(false);

                if(!IsScrolledHorizontally)
                    result.Visibility = HiddenOrVisible.Hidden;

                return result;
            }

            set
            {
                if (!IsScrolledHorizontally)
                    value.Visibility = HiddenOrVisible.Hidden;

                SetScrollBarInfo(isVertical: false, value);
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override ScrollBarInfo VertScrollBarInfo
        {
            get
            {
                var result = GetScrollBarInfo(true);

                if(!IsScrolledVertically)
                    result.Visibility = HiddenOrVisible.Hidden;

                return result;
            }

            set
            {
                if (!IsScrolledVertically)
                    value.Visibility = HiddenOrVisible.Hidden;

                SetScrollBarInfo(isVertical: true, value);
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
        /// <inheritdoc/>
        protected virtual bool IsContentVisible()
        {
            return !GetContentPreferredSize().AnyIsEmptyOrNegative;
        }

        /// <summary>
        /// Gets default value used to offset scrollbar position when scroll
        /// wheel event is handled.
        /// </summary>
        /// <param name="measureCanvas">The canvas used for text measuring.</param>
        /// <param name="font">The font used for text measuring.</param>
        /// <param name="isVert">Whether to get value for
        /// the vertical of horizontal scrollbar.</param>
        /// <returns></returns>
        public static int GetDefaultScrollWheelDelta(Graphics measureCanvas, Font font, bool isVert)
        {
            if (isVert)
            {
                var h = measureCanvas.GetTextExtent("Wg", font).Height;
                h *= DefaultMouseWheelScrollFactor.Height;
                return (int)h;
            }
            else
            {
                var w = measureCanvas.GetTextExtent("W", font).Width;
                w *= DefaultMouseWheelScrollFactor.Width;
                return (int)w;
            }
        }

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
        /// Suspends the update of the interior layout and scroll bars, allowing for multiple changes to be made to the child controls
        /// without triggering a layout update for each change.
        /// </summary>
        public virtual void SuspendUpdateInterior()
        {
            suspendUpdateInteriorCounter++;
        }

        /// <summary>
        /// Resumes the update of the interior layout and scroll bars after it has been suspended,
        /// allowing any pending layout updates to be processed.
        /// </summary>
        public virtual void ResumeUpdateInterior(bool update = true)
        {
            suspendUpdateInteriorCounter--;

            if (suspendUpdateInteriorCounter < 0)
                throw new InvalidOperationException("ResumeUpdateInterior called more times than SuspendUpdateInterior.");

            if (suspendUpdateInteriorCounter == 0 && update)
                UpdateInterior();
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
        /// Calculates and retrieves the effective content size scale factor, which is used to determine the maximum
        /// scrollable area based on the preferred size of the content. This scale factor allows for additional
        /// scrolling beyond the calculated maximum size, which can be useful in scenarios such as accommodating
        /// on-screen keyboards or other dynamic content changes.
        /// </summary>
        /// <returns>A <see cref="SizeD"/> representing the effective content size scale factor. </returns>
        /// <remarks>By default, it uses the value of <see cref="ContentSizeScale"/> if it is set; otherwise,
        /// falls back to the default content size scale defined by <see cref="DefaultContentSizeScale"/>.</remarks>
        public SizeD GetEffectiveContentSizeScale()
        {
            return ContentSizeScale ?? DefaultContentSizeScale;
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
        public SizeD GetScrollRange()
        {
            return LayoutMaxSize;
        }

        /// <summary>
        /// Retrieves the current scroll position, which is determined by the layout offset of the first child control.
        /// </summary>
        /// <returns>A <see cref="PointD"/> representing the current scroll position.</returns>
        public PointD GetScrollPosition()
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
        /// Determines whether the mouse wheel event should be ignored for
        /// the specified child control.
        /// </summary>
        /// <param name="child">The child control to check.</param>
        /// <returns>True if the mouse wheel event should be ignored; otherwise, false.</returns>
        protected virtual bool IgnoreChildMouseWheel(AbstractControl? child)
        {
            return false;
        }

        /// <inheritdoc/>
        public override bool IsParentPerformLayoutCalled(PerformLayoutParams? layoutParams = null)
        {
            if (layoutParams?.Reason != PerformLayoutReason.SuggestedSizeChanged)
            {
                return false;
            }

            var result = base.IsParentPerformLayoutCalled(layoutParams);

            return result;
        }

        /// <summary>
        /// Called when the <see cref="IsScrolledVertically"/> property changes, allowing for any necessary updates to be performed.
        /// </summary>
        protected virtual void OnScrolledVerticallyChanged()
        {
            UpdateInterior();
        }

        /// <summary>
        /// Called when the <see cref="IsScrolledHorizontally"/> property changes, allowing for any necessary updates to be performed.
        /// </summary>
        protected virtual void OnScrolledHorizontallyChanged()
        {
            UpdateInterior();
        }

        /// <summary>
        /// Calculates and retrieves the preferred content viewport rectangle, which represents the area of the control
        /// that should be used for displaying the content.
        /// </summary>
        /// <returns>The preferred content viewport rectangle.</returns>
        protected virtual RectD GetPreferredContentViewportRect()
        {
            RectD result;

            if (IsContentVisible())
            {
                var contentSize = GetContentPreferredSize();

                var areaRects = GetInteriorScrollableAreaRectangles(IsScrolledHorizontally, IsScrolledVertically);
                result = areaRects.GetPreferredContentViewportRect(contentSize);
            }
            else
            {
                result = ClientRectangle;
            }

            return result;
        }

        /// <summary>
        /// Updates the interior of the control, which includes recalculating the paint rectangle,
        /// updating scroll bars, and refreshing the display.
        /// </summary>
        protected virtual void UpdateInterior(bool refresh = true)
        {
            if (insideUpdateInterior || suspendUpdateInteriorCounter > 0)
                return;

            insideUpdateInterior = true;

            try
            {
                UpdateScrollBars(false);
                UpdateInteriorProperties();

                var maxScrollPosition = GetMaxScrollPosition();
                var scrollPosition = GetScrollPosition();

                DoActionSetScroll(new PointD(
                    Math.Min(scrollPosition.X, maxScrollPosition.X),
                    Math.Min(scrollPosition.Y, maxScrollPosition.Y)));

                if (refresh)
                    Refresh();
            }
            finally
            {
                insideUpdateInterior = false;
            }
        }

        /// <inheritdoc/>
        protected override void UpdateInteriorProperties()
        {
            base.UpdateInteriorProperties();
        }
    }
}
