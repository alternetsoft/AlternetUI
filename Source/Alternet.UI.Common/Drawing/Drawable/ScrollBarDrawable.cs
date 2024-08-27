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
        /// Initializes this drawable with the specified color settings.
        /// </summary>
        public virtual void SetColors(ScrollBar.ThemeColors colors)
        {
            Background ??= new();
            Background.Normal ??= new();
            Background.Normal.Brush = colors.Background.AsBrush;

            Thumb ??= new();
            Thumb.Normal ??= new();
            Thumb.Normal.Brush = colors.ThumbBackground.AsBrush;
            Thumb.Normal.Border ??= new();
            Thumb.Normal.Border.Color = colors.ThumbBorder;
            Thumb.Normal.Border.Width = 1;

            InitArrowStates(ref UpArrow, KnownSvgImages.ImgTriangleArrowUp);
            InitArrowStates(ref DownArrow, KnownSvgImages.ImgTriangleArrowDown);
            InitArrowStates(ref LeftArrow, KnownSvgImages.ImgTriangleArrowLeft);
            InitArrowStates(ref RightArrow, KnownSvgImages.ImgTriangleArrowRight);

            void InitArrowStates(ref ControlStateObjects<RectangleDrawable>? arrow, SvgImage svgImage)
            {
                arrow ??= new();
                arrow.Normal ??= new();
                arrow.Hovered ??= new();
                arrow.Normal.SvgImage = new(svgImage, colors.Arrow);
                SetStateProps(arrow.Normal);

                arrow.Hovered.SvgImage = new(svgImage, colors.ArrowHovered);
                SetStateProps(arrow.Hovered);

                void SetStateProps(RectangleDrawable d)
                {
                    d.HasImage = true;
                    d.Stretch = false;
                    d.CenterHorz = true;
                    d.CenterVert = true;
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
            var realArrowSize =
                MathUtils.Min(arrowSize.Width, arrowSize.Height, Bounds.Width - 2, Bounds.Height - 2);
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
