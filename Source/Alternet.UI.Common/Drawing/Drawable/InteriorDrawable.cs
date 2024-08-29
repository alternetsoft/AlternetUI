using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements control interior drawing.
    /// This includes border, background, vertical and horizontal scrollbars drawing.
    /// </summary>
    public class InteriorDrawable : BaseDrawable
    {
        /// <summary>
        /// Gets or sets vertical scrollbar element.
        /// </summary>
        public ScrollBarDrawable? VertScrollBar;

        /// <summary>
        /// Gets or sets horizontal scrollbar element.
        /// </summary>
        public ScrollBarDrawable? HorzScrollBar;

        /// <summary>
        /// Gets or sets corner element.
        /// </summary>
        public RectangleDrawable? Corner;

        /// <summary>
        /// Gets or sets background element.
        /// </summary>
        public RectangleDrawable? Background;

        /// <summary>
        /// Gets or sets border element.
        /// </summary>
        public RectangleDrawable? Border;

        private ScrollBar.MetricsInfo? metrics;

        /// <summary>
        /// Gets whether vertical scrollbar is visible.
        /// </summary>
        public bool VertVisible => VertScrollBar?.Visible ?? false;

        /// <summary>
        /// Gets whether horizontal scrollbar is visible.
        /// </summary>
        public bool HorzVisible => HorzScrollBar?.Visible ?? false;

        /// <summary>
        /// Gets whether vertical and horizontal scrollbars are visible.
        /// </summary>
        public bool BothVisible => VertVisible && HorzVisible;

        /// <summary>
        /// Gets whether corner is visible.
        /// </summary>
        public bool CornerVisible => Corner?.Visible ?? false;

        /// <summary>
        /// Gets whether corner and both scrollbars are visible.
        /// </summary>
        public bool HasCorner => BothVisible && CornerVisible;

        /// <summary>
        /// Gets whether interior has border.
        /// </summary>
        public bool HasBorder => Border?.Visible ?? false;

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
                if (VertScrollBar is not null)
                    VertScrollBar.Metrics = value;
                if(HorzScrollBar is not null)
                    HorzScrollBar.Metrics = value;
            }
        }

        /// <summary>
        /// Gets or sets position of the vertical scrollbar if it is created.
        /// </summary>
        public virtual ScrollBar.AltPositionInfo VertPosition
        {
            get
            {
                return VertScrollBar?.AltPosInfo ?? ScrollBar.AltPositionInfo.Default;
            }

            set
            {
                VertScrollBar?.SetAltPosInfo(value);
            }
        }

        /// <summary>
        /// Gets or sets position of the horizontal scrollbar if it is created.
        /// </summary>
        public virtual ScrollBar.AltPositionInfo HorzPosition
        {
            get
            {
                return HorzScrollBar?.AltPosInfo ?? ScrollBar.AltPositionInfo.Default;
            }

            set
            {
                HorzScrollBar?.SetAltPosInfo(value);
            }
        }

        /// <summary>
        /// Gets border widths.
        /// </summary>
        public virtual Thickness BorderWidth
        {
            get
            {
                return Border?.Border?.Width ?? Thickness.Empty;
            }
        }

        /// <summary>
        /// Performs layout of the drawable childs.
        /// </summary>
        /// <param name="scaleFactor">Scale factor used to convert pixels to/from dips.</param>
        /// <param name="clientRect">Client rectangle which equals bounds of the object without the space
        /// occupied by the scrollbars.</param>
        public virtual void PerformLayout(Coord scaleFactor, out RectD clientRect)
        {
            if (HasBorder)
                Border!.Bounds = Bounds;

            var boundsInsideBorder = Bounds;
            var borderWidth = BorderWidth;

            boundsInsideBorder.Left += borderWidth.Left;
            boundsInsideBorder.Top += borderWidth.Top;
            boundsInsideBorder.Width -= borderWidth.Horizontal;
            boundsInsideBorder.Height -= borderWidth.Vertical;

            clientRect = boundsInsideBorder;

            var vertVisible = VertVisible;
            var horzVisible = HorzVisible;
            var bothVisible = vertVisible && horzVisible;
            var cornerVisible = CornerVisible;

            var metrics = GetRealMetrics();

            var vertWidth = metrics.GetPreferredSize(true, scaleFactor).Width;
            var vertHeight = boundsInsideBorder.Height;
            var vertLeft = boundsInsideBorder.Right - vertWidth;
            var vertTop = boundsInsideBorder.Top;
            var vertBounds = new RectD(vertLeft, vertTop, vertWidth, vertHeight);

            var horzHeight = metrics.GetPreferredSize(false, scaleFactor).Height;
            var horzWidth = boundsInsideBorder.Width;
            var horzLeft = boundsInsideBorder.Left;
            var horzTop = boundsInsideBorder.Bottom - horzHeight;
            var horzBounds = new RectD(horzLeft, horzTop, horzWidth, horzHeight);

            SizeD cornerSize = (vertWidth, horzHeight);
            PointD cornerLocation = (
                boundsInsideBorder.Right - cornerSize.Width,
                boundsInsideBorder.Bottom - cornerSize.Height);
            RectD cornerBounds = (cornerLocation, cornerSize);

            if (bothVisible)
            {
                vertBounds.Height -= cornerSize.Height;
                horzBounds.Width -= cornerSize.Width;

                if(cornerVisible)
                    Corner!.Bounds = cornerBounds;
            }

            if (vertVisible)
            {
                VertScrollBar!.Bounds = vertBounds;
                clientRect.Width -= vertWidth;
            }

            if (horzVisible)
            {
                clientRect.Height -= horzHeight;
                HorzScrollBar!.Bounds = horzBounds;
            }

            if(Background is not null)
            {
                Background.Bounds = clientRect;
            }
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            PerformLayout(control.ScaleFactor, out _);

            if(Border is not null && Border.Visible)
            {
                Border.Draw(control, dc);
            }

            if (Background is not null && Background.Visible)
            {
                Background.Draw(control, dc);
            }

            if (HasCorner)
            {
                Corner!.Draw(control, dc);
            }

            if (VertVisible)
            {
                VertScrollBar!.Draw(control, dc);
            }

            if (HorzVisible)
            {
                HorzScrollBar!.Draw(control, dc);
            }
        }

        /// <summary>
        /// Gets real scroll bar metrics. If <see cref="Metrics"/> is not specified, returns
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
        public virtual void SetScrollBarColors(ScrollBar.ThemeMetrics colors)
        {
            VertScrollBar ??= new();
            VertScrollBar.IsVertical = true;
            VertScrollBar.SetThemeMetrics(colors);

            HorzScrollBar ??= new();
            HorzScrollBar.IsVertical = false;
            HorzScrollBar.SetThemeMetrics(colors);

            Corner ??= new();
            Corner.Brush = colors.CornerBackground[VisualControlState.Normal].AsBrush
                ?? colors.Background[VisualControlState.Normal].AsBrush;
        }

        /// <summary>
        /// Initialized this drawable with default settings for the specified color theme.
        /// </summary>
        public virtual void SetScrollBarColors(ScrollBar.KnownTheme theme, bool isDark = false)
        {
            SetScrollBarColors(ScrollBar.ThemeMetrics.GetColors(theme, isDark));
        }
    }
}
