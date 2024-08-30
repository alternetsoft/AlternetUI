﻿using System;
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

        /// <summary>
        /// Gets of sets whether scrollbar is vertical.
        /// </summary>
        public bool IsVertical = true;

        private ScrollBar.MetricsInfo? metrics;

        /// <summary>
        /// Gets or sets scrollbar position information.
        /// </summary>
        public ScrollBar.AltPositionInfo? AltPosInfo { get; set; }

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
        /// Sets value of the <see cref="AltPosInfo"/>. Implemented for the convenience.
        /// </summary>
        /// <param name="altPosInfo">New value.</param>
        public void SetAltPosInfo(ScrollBar.AltPositionInfo? altPosInfo)
        {
            AltPosInfo = altPosInfo;
        }

        /// <summary>
        /// Gets real scrollbar metrics. If <see cref="Metrics"/> is not specified, returns
        /// <see cref="ScrollBar.DefaultMetrics"/>.
        /// </summary>
        /// <returns></returns>
        public virtual ScrollBar.MetricsInfo GetRealMetrics()
        {
            return metrics ?? ScrollBar.DefaultMetrics;
        }

        /// <summary>
        /// Initializes this drawable with the specified theme colors and metrics.
        /// </summary>
        public virtual void SetThemeMetrics(ScrollBar.ThemeMetrics themeMetrics)
        {
            ArrowMargin = themeMetrics.ArrowMargin;
            UseArrowSizeForThumb = themeMetrics.UseArrowSizeForThumb;
            ThumbMargin = themeMetrics.ThumbMargin;

            Background = new();
            Background.Normal = CreateBackgroundState(VisualControlState.Normal);
            Background.Hovered = CreateBackgroundState(VisualControlState.Hovered);
            Background.Disabled = CreateBackgroundState(VisualControlState.Disabled);

            Thumb = new();
            Thumb.Normal = CreateThumbState(VisualControlState.Normal);
            Thumb.Hovered = CreateThumbState(VisualControlState.Hovered);
            Thumb.Disabled = CreateThumbState(VisualControlState.Disabled);

            InitArrowStates(ref UpArrow, themeMetrics.UpArrowImage);
            InitArrowStates(ref DownArrow, themeMetrics.DownArrowImage);
            InitArrowStates(ref LeftArrow, themeMetrics.LeftArrowImage);
            InitArrowStates(ref RightArrow, themeMetrics.RightArrowImage);

            ScrollBar.ThemeInitializeArgs e = new(themeMetrics);
            e.ScrollBar = this;
            themeMetrics.RaiseThemeInitialize(e);

            RectangleDrawable? CreateBackgroundState(VisualControlState state)
            {
                var background = themeMetrics.Background[state];

                if (background is null)
                    return null;

                RectangleDrawable result = new();

                result.Brush = background.AsBrush;

                return result;
            }

            RectangleDrawable? CreateThumbState(VisualControlState state)
            {
                var background = themeMetrics.ThumbBackground[state];
                var border = themeMetrics.ThumbBorder[state];

                if (background is null && border is null)
                    return null;

                RectangleDrawable result = new();

                result.Brush = background?.AsBrush;

                if(border is not null)
                {
                    result.Border = new();
                    result.Border.Color = border.AsColor;
                    result.Border.Width = 1;
                }

                return result;
            }

            void InitArrowStates(
                ref ControlStateObjects<RectangleDrawable>? arrow,
                EnumArray<VisualControlState, SvgImage> svgImage)
            {
                arrow ??= new();

                arrow.Normal = CreateStateProps(VisualControlState.Normal);
                arrow.Hovered = CreateStateProps(VisualControlState.Hovered);
                arrow.Disabled = CreateStateProps(VisualControlState.Disabled);

                RectangleDrawable? CreateStateProps(VisualControlState state)
                {
                    var arrow = themeMetrics.Arrow[state];

                    if (arrow is null)
                        return null;

                    RectangleDrawable result = new();

                    result.SvgImage = new(svgImage[state], arrow.AsColor);
                    result.HasImage = true;
                    result.Stretch = false;
                    result.CenterHorz = true;
                    result.CenterVert = true;

                    return result;
                }
            }
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            var backgroundDrawable = Background?.GetObjectOrNormal(VisualState);

            if (backgroundDrawable is not null)
            {
                backgroundDrawable.Bounds = Bounds;
                backgroundDrawable.Draw(control, dc);
            }

            var startButton = GetStartButton();
            var endButton = GetEndButton();
            var startArrow = GetStartArrow();
            var endArrow = GetEndArrow();
            var metrics = GetRealMetrics();
            var scaleFactor = control.ScaleFactor;

            Coord buttonSize = IsVertical ? Bounds.Width : Bounds.Height;
            var arrowSize = metrics.GetArrowBitmapSize(IsVertical, scaleFactor);

            var arrowMargin = ArrowMargin?[this.VisualState] ?? 1;
            arrowMargin *= 2;

            var realArrowSize =
                MathUtils.Min(
                    arrowSize.Width,
                    arrowSize.Height,
                    Bounds.Width - arrowMargin,
                    Bounds.Height - arrowMargin);

            var realArrowSizeI = GraphicsFactory.PixelFromDip(realArrowSize, scaleFactor);

            var startButtonBounds = (Bounds.Left, Bounds.Top, buttonSize, buttonSize);
            var endButtonBounds
                = (Bounds.Right - buttonSize, Bounds.Bottom - buttonSize, buttonSize, buttonSize);

            if (startButton is not null)
            {
                startButton.Bounds = startButtonBounds;
                startButton.Draw(control, dc);
            }

            if (startArrow is not null)
            {
                startArrow.Bounds = startButtonBounds;
                startArrow.SvgImage?.SetSvgSize(realArrowSizeI);
                startArrow.Draw(control, dc);
            }

            if (endButton is not null)
            {
                endButton.Bounds = endButtonBounds;
                endButton.Draw(control, dc);
            }

            if (endArrow is not null)
            {
                endArrow.Bounds = endButtonBounds;
                endArrow.SvgImage?.SetSvgSize(realArrowSizeI);
                endArrow.Draw(control, dc);
            }
        }

        private RectangleDrawable? GetEndArrow()
        {
            if (IsVertical)
                return DownArrow?.GetObjectOrNormal(VisualState);
            return RightArrow?.GetObjectOrNormal(VisualState);
        }

        private RectangleDrawable? GetStartArrow()
        {
            if (IsVertical)
                return UpArrow?.GetObjectOrNormal(VisualState);
            return LeftArrow?.GetObjectOrNormal(VisualState);
        }

        private RectangleDrawable? GetStartButton()
        {
            if (IsVertical)
                return UpButton?.GetObjectOrNormal(VisualState);
            return LeftButton?.GetObjectOrNormal(VisualState);
        }

        private RectangleDrawable? GetEndButton()
        {
            if (IsVertical)
                return DownButton?.GetObjectOrNormal(VisualState);
            return RightButton?.GetObjectOrNormal(VisualState);
        }
    }
}
