using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements scrollbar drawing.
    /// </summary>
    public class ScrollBarDrawable : BaseDrawable
    {
        /// <summary>
        /// Indicates whether arrows are visible by default in the scroll bar.
        /// </summary>
        /// <remarks>This field determines the default visibility state of arrows.
        /// It can be set to <see langword="true"/> to make arrows visible by default,
        /// or <see langword="false"/> to hide them.</remarks>
        public static bool DefaultArrowsVisible = true;

        /// <summary>
        /// Represents the default margin for an invisible arrow in coordinate units.
        /// </summary>
        /// <remarks>This value is used as a default margin setting for scenarios involving invisible
        /// arrows. The margin is measured in coordinate units.</remarks>
        public static Coord DefaultInvisibleArrowMargin = 1;

        /// <summary>
        /// Gets or sets a value indicating whether scroll bar thumb
        /// should use rounded corners by default.
        /// </summary>
        public static bool DefaultUseThumbRoundCorners = true;

        /// <summary>
        /// Gets or sets the default corner radius for a scroll bar thumb.
        /// This value is used when <see cref="DefaultUseThumbRoundCorners"/>
        /// is set to <see langword="true"/>.
        /// </summary>
        /// <remarks>The default value is 4.</remarks>
        public static Coord DefaultThumbCornerRadius = 4;

        /// <summary>
        /// Gets or sets minimal scrollbar thumb size in dips.
        /// </summary>
        public static Coord MinThumbSize = 10;

        /// <summary>
        /// Gets or sets distance (in dips) between arrow button and arrow.
        /// </summary>
        public EnumArray<VisualControlState, Coord>? ArrowMargin;

        /// <summary>
        /// Gets or sets whether to use arrow width as the thumb width for the vertical scrollbar
        /// and arrow height as the thumb height for the horizontal scrollbar.
        /// </summary>
        public EnumArray<VisualControlState, bool>? UseArrowSizeForThumb;

        /// <summary>
        /// Gets or sets distance (in dips) between thumb and scrollbar bounds.
        /// </summary>
        public EnumArray<VisualControlState, Coord>? ThumbMargin;

        /// <summary>
        /// Gets or sets background element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? Background;

        /// <summary>
        /// Gets or sets scroll thumb element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? Thumb;

        /// <summary>
        /// Gets or sets up button element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? UpButton;

        /// <summary>
        /// Gets or sets down button element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? DownButton;

        /// <summary>
        /// Gets or sets left button element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? LeftButton;

        /// <summary>
        /// Gets or sets right button element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? RightButton;

        /// <summary>
        /// Gets or sets up arrow element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? UpArrow;

        /// <summary>
        /// Gets or sets down arrow element.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? DownArrow;

        /// <summary>
        /// Gets or sets primitive painter for the left arrow.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? LeftArrow;

        /// <summary>
        /// Gets or sets primitive painter for the right arrow.
        /// </summary>
        public ControlStateObjects<RectangleDrawable>? RightArrow;

        private ScrollBar.MetricsInfo? metrics;
        private VisualControlState startButtonState;
        private VisualControlState endButtonState;
        private VisualControlState startArrowState;
        private VisualControlState endArrowState;
        private VisualControlState thumbState;

        /// <summary>
        /// Enumerates possible hit test return values.
        /// </summary>
        public enum HitTestResult
        {
            /// <summary>
            /// Hit is outside of anything.
            /// </summary>
            None,

            /// <summary>
            /// Hit is on the thumb.
            /// </summary>
            Thumb,

            /// <summary>
            /// Hit is on the start button.
            /// </summary>
            StartButton,

            /// <summary>
            /// Hit is on the end button.
            /// </summary>
            EndButton,

            /// <summary>
            /// Hit is before the thumb.
            /// </summary>
            BeforeThumb,

            /// <summary>
            /// Hit is after the thumb.
            /// </summary>
            AfterThumb,
        }

        /// <summary>
        /// Gets of sets whether scrollbar is vertical.
        /// </summary>
        public virtual bool IsVertical { get; set; } = true;

        /// <summary>
        /// Gets or sets scrollbar information.
        /// </summary>
        public virtual ScrollBarInfo Position { get; set; }

        /// <inheritdoc/>
        public override RectD Bounds
        {
            get
            {
                return base.Bounds;
            }

            set
            {
                if (base.Bounds == value)
                    return;
                base.Bounds = value;
            }
        }

        /// <summary>
        /// Gets or sets scrollbar metrics.
        /// </summary>
        public virtual ScrollBar.MetricsInfo? Metrics
        {
            get
            {
                return metrics;
            }

            set
            {
                metrics = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether scroll bar thumb
        /// should use rounded corners. If this property is not set,
        /// <see cref="DefaultUseThumbRoundCorners"/> is used.
        /// </summary>
        public virtual bool? UseThumbRoundCorners { get; set; }

        /// <summary>
        /// Gets or sets the corner radius for a scroll bar thumb.
        /// If this property is not set, <see cref="DefaultThumbCornerRadius"/> is used.
        /// This value is used when <see cref="UseThumbRoundCorners"/>
        /// is set to <see langword="true"/>.
        /// </summary>
        public virtual Coord? ThumbCornerRadius { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether arrows are visible in the scroll bar.
        /// If this property is not set, <see cref="DefaultArrowsVisible"/> is used in order
        /// to determine whether arrows are visible.
        /// </summary>
        public virtual bool? ArrowsVisible { get; set; }

        /// <summary>
        /// Gets or sets the visual state of the start button.
        /// This property doesn't invalidate the control.
        /// </summary>
        public virtual VisualControlState StartButtonState
        {
            get
            {
                if (!Enabled)
                    return VisualControlState.Disabled;
                return startButtonState;
            }

            set => startButtonState = value;
        }

        /// <summary>
        /// Gets or sets the visual state of the end button.
        /// This property doesn't invalidate the control.
        /// </summary>
        public virtual VisualControlState EndButtonState
        {
            get
            {
                if (!Enabled)
                    return VisualControlState.Disabled;
                return endButtonState;
            }

            set => endButtonState = value;
        }

        /// <summary>
        /// Gets or sets the visual state of the start arrow in a control.
        /// This property doesn't invalidate the control.
        /// </summary>
        public virtual VisualControlState StartArrowState
        {
            get
            {
                if (!Enabled)
                    return VisualControlState.Disabled;
                return startArrowState;
            }

            set => startArrowState = value;
        }

        /// <summary>
        /// Gets or sets the visual state of the end arrow in a control.
        /// This property doesn't invalidate the control.
        /// </summary>
        public virtual VisualControlState EndArrowState
        {
            get
            {
                if (!Enabled)
                    return VisualControlState.Disabled;
                return endArrowState;
            }

            set => endArrowState = value;
        }

        /// <summary>
        /// Gets or sets the visual state of the thumb.
        /// This property doesn't invalidate the control.
        /// </summary>
        public virtual VisualControlState ThumbState
        {
            get
            {
                if(!Enabled)
                    return VisualControlState.Disabled;
                return thumbState;
            }

            set => thumbState = value;
        }

        /// <summary>
        /// Returns one of <see cref="HitTestResult"/> constants.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <param name="rectangles">Bounds of the different part of the drawable.</param>
        /// <returns></returns>
        public virtual HitTestResult HitTest(EnumArray<HitTestResult, RectD> rectangles, PointD point)
        {
            if (!Bounds.NotEmptyAndContains(point))
                return HitTestResult.None;

            if (rectangles[HitTestResult.StartButton].NotEmptyAndContains(point))
                return HitTestResult.StartButton;

            if (rectangles[HitTestResult.EndButton].NotEmptyAndContains(point))
                return HitTestResult.EndButton;

            if (rectangles[HitTestResult.Thumb].NotEmptyAndContains(point))
                return HitTestResult.Thumb;

            if (rectangles[HitTestResult.BeforeThumb].NotEmptyAndContains(point))
                return HitTestResult.BeforeThumb;

            if (rectangles[HitTestResult.AfterThumb].NotEmptyAndContains(point))
                return HitTestResult.AfterThumb;

            return HitTestResult.None;
        }

        /// <summary>
        /// Sets value of the <see cref="Position"/>. Implemented for the convenience.
        /// </summary>
        /// <param name="value">New value.</param>
        public void SetPosition(ScrollBarInfo value)
        {
            Position = value;
        }

        /// <summary>
        /// Gets real scrollbar metrics. If <see cref="Metrics"/> is not specified, returns
        /// <see cref="ScrollBar.DefaultMetrics"/>.
        /// </summary>
        /// <returns></returns>
        public virtual ScrollBar.MetricsInfo GetRealMetrics(AbstractControl control)
        {
            return metrics ?? ScrollBar.DefaultMetrics(control);
        }

        /// <summary>
        /// Performs layout of the drawable children and returns calculated bound of the different
        /// parts of the drawable.
        /// </summary>
        /// <param name="scaleFactor">Scale factor used to convert pixels to/from dips.</param>
        /// <returns>Calculated bounds of the different parts of the drawable.</returns>
        public virtual EnumArray<HitTestResult, RectD> GetLayoutRectangles(Coord scaleFactor)
        {
            EnumArray<HitTestResult, RectD> result = new();

            if (!Position.IsVisible)
                return result;

            var startButton = GetVisibleStartButton(VisualControlState.Normal);
            var endButton = GetVisibleEndButton(VisualControlState.Normal);
            var startArrow = GetVisibleStartArrow(VisualControlState.Normal);
            var endArrow = GetVisibleEndArrow(VisualControlState.Normal);

            var arrowsVisible = ArrowsVisible ?? DefaultArrowsVisible;

            var hasStartButton = (startButton is not null || startArrow is not null) && arrowsVisible;
            var hasEndButton = (endButton is not null || endArrow is not null) && arrowsVisible;
            var hasButtons = hasStartButton || hasEndButton;

            var btnSize = IsVertical ? Bounds.Width : Bounds.Height;
            var btnWidth = btnSize;
            var btnHeight = btnSize;

            RectD startButtonBounds;
            RectD endButtonBounds;

            if (hasButtons)
            {
                startButtonBounds = (Bounds.Left, Bounds.Top, btnWidth, btnHeight);
                endButtonBounds = (Bounds.Right - btnWidth, Bounds.Bottom - btnHeight, btnWidth, btnHeight);
                result[HitTestResult.StartButton] = startButtonBounds;
                result[HitTestResult.EndButton] = endButtonBounds;
            }
            else
            {
                startButtonBounds = RectD.Empty;
                endButtonBounds = RectD.Empty;
            }

            if (Position.Range <= 0 || Position.Range <= Position.PageSize || Position.PageSize <= 0)
                return result;

            RectD thumbMaximalBounds;
            RectD thumbBounds = RectD.Empty;
            RectD afterThumbBounds = RectD.Empty;
            RectD beforeThumbBounds = RectD.Empty;

            if (IsVertical)
            {
                Coord startButtonBoundsHeight;
                Coord endButtonBoundsHeight;

                if (hasButtons)
                {
                    startButtonBoundsHeight = startButtonBounds.Height;
                    endButtonBoundsHeight = endButtonBounds.Height;
                }
                else
                {
                    startButtonBoundsHeight = DefaultInvisibleArrowMargin;
                    endButtonBoundsHeight = DefaultInvisibleArrowMargin;
                }

                thumbMaximalBounds = (
                    Bounds.Left,
                    Bounds.Top + startButtonBoundsHeight,
                    Bounds.Width,
                    Bounds.Height - endButtonBoundsHeight - startButtonBoundsHeight);
                var thumbHeight = (Position.PageSize * thumbMaximalBounds.Height) / Position.Range;
                thumbHeight = Math.Max(thumbHeight, MinThumbSize);

                if(thumbHeight >= thumbMaximalBounds.Height)
                {
                }
                else
                {
                    var thumbMaxTop = thumbMaximalBounds.Height - thumbHeight;
                    var thumbTop = (thumbMaxTop * Position.Position)
                        / (Position.Range - Position.PageSize);
                    var vertThumbWidth = Bounds.Width;
                    thumbTop = Math.Min(thumbTop, thumbMaxTop);
                    thumbBounds = (
                        Bounds.Left,
                        thumbMaximalBounds.Top + thumbTop,
                        vertThumbWidth,
                        thumbHeight);
                    beforeThumbBounds = (
                        Bounds.Left,
                        thumbMaximalBounds.Top,
                        vertThumbWidth,
                        thumbTop);
                    afterThumbBounds = (
                        Bounds.Left,
                        thumbBounds.Bottom,
                        vertThumbWidth,
                        thumbMaximalBounds.Bottom - thumbBounds.Bottom);
                }
            }
            else
            {
                Coord startButtonBoundsWidth;
                Coord endButtonBoundsWidth;

                if (hasButtons)
                {
                    startButtonBoundsWidth = startButtonBounds.Width;
                    endButtonBoundsWidth = endButtonBounds.Width;
                }
                else
                {
                    startButtonBoundsWidth = DefaultInvisibleArrowMargin;
                    endButtonBoundsWidth = DefaultInvisibleArrowMargin;
                }

                thumbMaximalBounds = (
                    Bounds.Left + startButtonBoundsWidth,
                    Bounds.Top,
                    Bounds.Width - endButtonBoundsWidth - startButtonBoundsWidth,
                    Bounds.Height);
                var thumbWidth = (Position.PageSize * thumbMaximalBounds.Width) / Position.Range;
                thumbWidth = Math.Max(thumbWidth, MinThumbSize);

                if (thumbWidth >= thumbMaximalBounds.Width)
                {
                }
                else
                {
                    var thumbMaxLeft = thumbMaximalBounds.Width - thumbWidth;
                    var thumbLeft = (thumbMaxLeft * Position.Position)
                        / (Position.Range - Position.PageSize);
                    thumbLeft = Math.Min(thumbLeft, thumbMaxLeft);

                    var horzThumbHeight = Bounds.Height;

                    thumbBounds = (
                        thumbMaximalBounds.Left + thumbLeft,
                        Bounds.Top,
                        thumbWidth,
                        horzThumbHeight);
                    beforeThumbBounds = (
                        thumbMaximalBounds.Left,
                        Bounds.Top,
                        thumbLeft,
                        horzThumbHeight);
                    afterThumbBounds = (
                        thumbBounds.Right,
                        Bounds.Top,
                        thumbMaximalBounds.Right - thumbBounds.Right,
                        horzThumbHeight);
                }
            }

            result[HitTestResult.Thumb] = thumbBounds;
            result[HitTestResult.AfterThumb] = afterThumbBounds;
            result[HitTestResult.BeforeThumb] = beforeThumbBounds;
            return result;
        }

        /// <summary>
        /// Calculates the actual size of the arrow button for the scrollbar,
        /// considering the control's scale factor, margins, and bounds.
        /// </summary>
        /// <remarks>The calculated size ensures that the arrow button fits within the
        /// available bounds while respecting the specified margins. If the computed size
        /// does not align with the button's dimensions, it
        /// is adjusted to maintain consistency.</remarks>
        /// <param name="scaleFactor">The scaling factors.</param>
        /// <param name="metrics">The metrics information for the scrollbar,
        /// providing details such as arrow bitmap size.</param>
        /// <returns>A <see cref="Coord"/> representing the computed size of
        /// the arrow button, adjusted for margins and bounds.</returns>
        public virtual Coord GetRealArrowSize(
            Coord scaleFactor,
            ScrollBar.MetricsInfo metrics)
        {
            var arrowSize = metrics.GetArrowBitmapSize(IsVertical, scaleFactor) - 6;
            arrowSize = SizeD.Max(9, arrowSize);

            var arrowMargin = ArrowMargin?[this.VisualState] ?? 1;
            arrowMargin *= 2;

            var arrowButtonSize = IsVertical ? Bounds.Width : Bounds.Height;

            var realArrowSize =
                MathUtils.Min(
                    arrowSize.Width,
                    arrowSize.Height,
                    Bounds.Width - arrowMargin,
                    Bounds.Height - arrowMargin);

            if (!IntUtils.IsEqualEven((int)arrowButtonSize, (int)realArrowSize))
            {
                realArrowSize--;
            }

            return realArrowSize;
        }

        /// <inheritdoc/>
        public override void Draw(AbstractControl control, Graphics dc)
        {
            if (!Visible || !Position.IsVisible || Bounds.SizeIsEmpty)
                return;

            var scaleFactor = control.ScaleFactor;
            var rectangles = GetLayoutRectangles(scaleFactor);

            var backgroundDrawable = Background?.GetObjectOrNormal(VisualState);

            if (backgroundDrawable is not null)
            {
                backgroundDrawable.Bounds = Bounds;
                backgroundDrawable.Draw(control, dc);
            }

            var metrics = GetRealMetrics(control);

            var startButtonState = StartButtonState;
            var endButtonState = EndButtonState;
            var startArrowState = StartArrowState;
            var endArrowState = EndArrowState;
            var thumbState = ThumbState;

            var startButton = GetVisibleStartButton(startButtonState);
            var endButton = GetVisibleEndButton(endButtonState);
            var startArrow = GetVisibleStartArrow(startArrowState);
            var endArrow = GetVisibleEndArrow(endArrowState);

            var realArrowSize = GetRealArrowSize(scaleFactor, metrics);
            var realArrowSizeI = GraphicsFactory.PixelFromDip(realArrowSize, scaleFactor);

            var thumb = Thumb?.GetObjectOrNormal(thumbState)?.OnlyVisible;

            if (thumb is not null)
            {
                var thumbBounds = rectangles[HitTestResult.Thumb];
                var fillBounds = thumbBounds;

                if (UseArrowSizeForThumb?[thumbState] ?? true)
                {
                    if (IsVertical)
                        fillBounds.Width = realArrowSize;
                    else
                        fillBounds.Height = realArrowSize;

                    thumb.Bounds = fillBounds.CenterIn(thumbBounds, IsVertical, !IsVertical);
                }

                thumb.UseRoundCorners = UseThumbRoundCorners ?? DefaultUseThumbRoundCorners;
                thumb.CornerRadius = ThumbCornerRadius ?? DefaultThumbCornerRadius;
                thumb.CornerRadiusIsPercent = false;
                thumb.OverrideBorderCornerSettings = true;

                thumb.Draw(control, dc);
            }

            if (startButton is not null)
            {
                startButton.Bounds = rectangles[HitTestResult.StartButton];
                if(startButton.Bounds.NotEmpty)
                    startButton.Draw(control, dc);
            }

            if (startArrow is not null)
            {
                startArrow.Bounds = rectangles[HitTestResult.StartButton];
                if (startArrow.Bounds.NotEmpty)
                {
                    startArrow.SetSvgSize(realArrowSizeI);
                    startArrow.Draw(control, dc);
                }
            }

            if (endButton is not null)
            {
                endButton.Bounds = rectangles[HitTestResult.EndButton];
                if(endButton.Bounds.NotEmpty)
                    endButton.Draw(control, dc);
            }

            if (endArrow is not null)
            {
                endArrow.Bounds = rectangles[HitTestResult.EndButton];
                if (endArrow.Bounds.NotEmpty)
                {
                    endArrow.SetSvgSize(realArrowSizeI);
                    endArrow.Draw(control, dc);
                }
            }
        }

        /// <summary>
        /// Gets end arrow with the specified visual state.
        /// </summary>
        /// <param name="state">Visual state for which element is requested.</param>
        /// <returns></returns>
        protected virtual RectangleDrawable? GetVisibleEndArrow(VisualControlState state)
        {
            RectangleDrawable? result;

            if (IsVertical)
                result = DownArrow?.GetObjectOrNormal(state);
            else
                result = RightArrow?.GetObjectOrNormal(state);

            return result?.OnlyVisible;
        }

        /// <summary>
        /// Gets start arrow with the specified visual state.
        /// </summary>
        /// <param name="state">Visual state for which element is requested.</param>
        /// <returns></returns>
        protected virtual RectangleDrawable? GetVisibleStartArrow(VisualControlState state)
        {
            RectangleDrawable? result;

            if (IsVertical)
                result = UpArrow?.GetObjectOrNormal(state);
            else
                result = LeftArrow?.GetObjectOrNormal(state);

            return result?.OnlyVisible;
        }

        /// <summary>
        /// Gets start button with the specified visual state.
        /// </summary>
        /// <param name="state">Visual state for which element is requested.</param>
        /// <returns></returns>
        protected virtual RectangleDrawable? GetVisibleStartButton(VisualControlState state)
        {
            RectangleDrawable? result;

            if (IsVertical)
                result = UpButton?.GetObjectOrNormal(state);
            else
                result = LeftButton?.GetObjectOrNormal(state);

            return result?.OnlyVisible;
        }

        /// <summary>
        /// Gets end button with the specified visual state.
        /// </summary>
        /// <param name="state">Visual state for which element is requested.</param>
        /// <returns></returns>
        protected virtual RectangleDrawable? GetVisibleEndButton(VisualControlState state)
        {
            RectangleDrawable? result;

            if (IsVertical)
                result = DownButton?.GetObjectOrNormal(state);
            else
                result = RightButton?.GetObjectOrNormal(state);

            return result?.OnlyVisible;
        }
    }
}
