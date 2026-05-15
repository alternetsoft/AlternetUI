using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class StdScrollViewer : ScrollableUserControl, IScrollEventRouter
    {
        /// <summary>
        /// Defines the default small change value for scrolling, which determines how much
        /// the content should scroll when a small scroll action is performed (e.g., scrolling by one line or one character).
        /// </summary>
        public static SizeD DefaultScrollSmallChange = new (40, 40);

        private readonly ScrollContainer scrollContainer;

        private bool isScrolledVertically = true;
        private bool isScrolledHorizontally = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="StdScrollViewer"/> class.
        /// </summary>
        public StdScrollViewer()
        {
            scrollContainer = new ScrollContainer();
            scrollContainer.Parent = this;

            SizeChanged += (s, e) =>
            {
                UpdateInterior();
                scrollContainer.SuggestedSize = GetPaintRectangle().Size;
            };

            scrollContainer.SizeChanged += (s, e) =>
            {
                UpdateInterior();
            };

            scrollContainer.ChildSizeChanged += (s, e) =>
            {
                UpdateInterior();
            };

            scrollContainer.SuggestedSize = GetPaintRectangle().Size;

            Interior?.Required();
        }

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
        /// Gets the content of the scroll viewer.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? Content
        {
            get
            {
                return scrollContainer;
            }
        }

        /// <summary>
        /// Gets the collection of child controls within the scroll container, which represents the content of the scroll viewer.
        /// </summary>
        [Content]
        [Browsable(false)]
        public BaseCollection<AbstractControl> ContentChildren
        {
            get
            {
                return scrollContainer.Children;
            }
        }

        /// <summary>
        /// Gets the first child control within the scroll container, which can be used
        /// to determine the current scroll position and the layout of the content.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? FirstContentChild
        {
            get
            {
                return scrollContainer?.FirstChild;
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

        new private ControlCollection Children
        {
            get
            {
                return base.Children;
            }
        }

        new private ControlCollection Controls
        {
            get
            {
                return base.Children;
            }
        }

        new private PointD LayoutOffset
        {
            get
            {
                return scrollContainer.LayoutOffset;
            }

            set
            {
                if (LayoutOffset == value)
                    return;

                scrollContainer.LayoutOffset = value;

                UpdateInterior();
            }
        }

        new private SizeD LayoutMaxSize
        {
            get
            {
                return FirstContentChild?.Bounds.Size ?? SizeD.Empty;
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

            if (FirstContentChild is null)
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

            return DefaultScrollSmallChange;
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

        /// <inheritdoc/>
        public override bool IsValidChild(AbstractControl control)
        {
            return control == scrollContainer;
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
        public virtual void DoActionSetScroll(PointD value)
        {
            var newOffset = value.ClampToZero();
            var maxOffset = GetMaxScrollPosition();
            newOffset = newOffset.ClampToMax(maxOffset);
            LayoutOffset = newOffset;
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
        public override bool IsParentPerformLayoutCalled(PerformLayoutParams? layoutParams = null)
        {
            if (layoutParams?.Reason != PerformLayoutReason.SuggestedSizeChanged)
            {
                return false;
            }

            var result = base.IsParentPerformLayoutCalled(layoutParams);

            return result;
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
        protected override void OnLayoutUpdated(EventArgs e)
        {
            base.OnLayoutUpdated(e);
        }

        /// <summary>
        /// Updates the interior layout and scroll bars based on the current child controls and their sizes.
        /// </summary>
        protected virtual void UpdateInterior()
        {
            var scrollbarsUpdated = UpdateScrollBars(false);

            if (scrollbarsUpdated)
            {
                var maxScrollPosition = GetMaxScrollPosition();
                var scrollPosition = GetScrollPosition();

                DoActionSetScroll(new PointD(
                    Math.Min(scrollPosition.X, maxScrollPosition.X),
                    Math.Min(scrollPosition.Y, maxScrollPosition.Y)));
                Refresh();
            }
        }

        /// <summary>
        /// Represents a container control that is used to hold the child controls within
        /// the scrollable area of the <see cref="StdScrollViewer"/>.
        /// </summary>
        public class ScrollContainer : HiddenBorder
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ScrollContainer"/> class,
            /// which serves as the container for child controls within the scrollable area of the <see cref="StdScrollViewer"/>.
            /// </summary>
            public ScrollContainer()
            {
            }

            /// <inheritdoc/>
            public override void OnLayout()
            {
                var control = FirstChild;

                if (control is null)
                    return;

                var childrenLayoutBounds = ChildrenLayoutBounds;

                var boundedPreferredSize = control.GetPreferredSize(new PreferredSizeContext(childrenLayoutBounds.Size));
                var unboundedPreferredSize = control.GetPreferredSize();

                var verticalAlignment = control.VerticalAlignment;
                var horizontalAlignment = control.HorizontalAlignment;

                if (unboundedPreferredSize.Width > childrenLayoutBounds.Width)
                {
                    horizontalAlignment = UI.HorizontalAlignment.Left;
                }

                if (unboundedPreferredSize.Height > childrenLayoutBounds.Height)
                {
                    verticalAlignment = UI.VerticalAlignment.Top;
                }

                var layoutManager = GetLayoutManager();

                var horizontalPosition =
                    layoutManager.AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        horizontalAlignment);
                var verticalPosition =
                    layoutManager.AlignVertical(
                        childrenLayoutBounds,
                        control,
                        boundedPreferredSize,
                        verticalAlignment);

                var layoutOffset = LayoutOffset;

                control.Bounds = new RectD(
                    horizontalPosition.Start - layoutOffset.X,
                    verticalPosition.Start - layoutOffset.Y,
                    horizontalPosition.Length,
                    verticalPosition.Length);
            }
        }
    }
}
