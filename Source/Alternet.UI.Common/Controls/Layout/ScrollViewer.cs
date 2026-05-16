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
    public partial class ScrollViewer : ScrollableUserControl, IScrollEventRouter
    {
        /// <summary>
        /// Gets or sets default multiplier used in calculation
        /// of total size of the scroll area in the <see cref="ScrollViewer"/>.
        /// This value is assigned to the scrollbar's total size.
        /// </summary>
        public static SizeD DefaultScrollBarTotalSizeMultiplier = 1;

        /// <summary>
        /// Gets or sets default mouse wheel scroll factor. This value is multiplied
        /// with line height and used as an offset when control is scrolled using mouse wheel.
        /// </summary>
        public static SizeD DefaultMouseWheelScrollFactor = (3, 3);

        /// <summary>
        /// Defines the default small change value for scrolling, which determines how much
        /// the content should scroll when a small scroll action is performed (e.g., scrolling by one line or one character).
        /// </summary>
        public static SizeD DefaultScrollSmallChange = new (40, 40);

        private readonly ScrollContainer scrollContainer;

        private bool isScrolledVertically = true;
        private bool isScrolledHorizontally = true;
        private bool insideUpdateInterior;
        private int suspendUpdateInteriorCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        public ScrollViewer()
        {
            scrollContainer = new ScrollContainer();
            scrollContainer.Parent = this;

            SizeChanged += (s, e) =>
            {
                UpdateInterior();
                App.DebugLogIf("ScrollViewer: Size changed, updating interior.", false);
            };

            scrollContainer.LayoutUpdated += (s, e) =>
            {
            };

            scrollContainer.ChildLayoutUpdated += (s, e) =>
            {
                UpdateInterior();
                App.DebugLogIf("ScrollViewer: Child layout updated, updating interior.", false);
            };

            Interior?.Required();

            UpdateInterior();
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
        public AbstractControl Content
        {
            get
            {
                return scrollContainer;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the scroll
        /// viewer is scrolled with the mouse wheel.
        /// </summary>
        public virtual bool IsScrolledWithMouseWheel { get; set; } = true;

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ScrollViewer;

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

        /// <summary>
        /// Creates <see cref="ScrollViewer"/> with the specified child control.
        /// </summary>
        /// <param name="child">Child control.</param>
        /// <returns></returns>
        public static ScrollViewer CreateWithChild(AbstractControl? child)
        {
            ScrollViewer result = new();

            if (child is not null)
            {
                child.Parent = result;
                child.Visible = true;
            }

            return result;
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
        public virtual void ResumeUpdateInterior()
        {
            suspendUpdateInteriorCounter--;

            if (suspendUpdateInteriorCounter < 0)
                throw new InvalidOperationException("ResumeUpdateInterior called more times than SuspendUpdateInterior.");

            if (suspendUpdateInteriorCounter == 0)
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

        /// <inheritdoc/>
        protected override void OnBeforeChildMouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Handled || IgnoreChildMouseWheel(sender as AbstractControl)
                || !IsScrolledWithMouseWheel)
                return;

            var sign = Math.Sign(e.Delta);
            var isVert = !Keyboard.IsShiftPressed;
            var delta = GetDefaultScrollWheelDelta(MeasureCanvas, RealFont, isVert);
            var incValue1 = sign * delta;
            var incValue2 = 0;

            if (isVert)
            {
                incValue2 = incValue1;
                incValue1 = 0;
            };

            DoActionOffsetScroll(new SizeD(incValue1, incValue2));

            e.Handled = true;
        }

        /// <summary>
        /// Updates the interior layout and scroll bars based on the current child controls and their sizes.
        /// </summary>
        protected virtual void UpdateInterior()
        {
            if (insideUpdateInterior || suspendUpdateInteriorCounter > 0)
                return;

            insideUpdateInterior = true;

            try
            {
                UpdateScrollBars(false);
                UpdateInteriorProperties();
                scrollContainer.SuggestedSize = GetPaintRectangle().Size;

                UpdateScrollBars(false);
                UpdateInteriorProperties();

                var maxScrollPosition = GetMaxScrollPosition();
                var scrollPosition = GetScrollPosition();

                DoActionSetScroll(new PointD(
                    Math.Min(scrollPosition.X, maxScrollPosition.X),
                    Math.Min(scrollPosition.Y, maxScrollPosition.Y)));

                Refresh();
            }
            finally
            {
                insideUpdateInterior = false;
            }
        }

        /// <summary>
        /// Represents a container control that is used to hold the child controls within
        /// the scrollable area of the <see cref="ScrollViewer"/>.
        /// </summary>
        public class ScrollContainer : HiddenBorder
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ScrollContainer"/> class,
            /// which serves as the container for child controls within the scrollable area of the <see cref="ScrollViewer"/>.
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
