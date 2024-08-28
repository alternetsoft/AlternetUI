using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements vertical and horizontal scrollbars drawing.
    /// </summary>
    public class ScrollBarsDrawable : BaseDrawable
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
        /// Performs layout of the drawable childs.
        /// </summary>
        /// <param name="scaleFactor">Scale factor used to convert pixels to/from dips.</param>
        public virtual void PerformLayout(Coord scaleFactor)
        {
            var vertVisible = VertVisible;
            var horzVisible = HorzVisible;
            var bothVisible = vertVisible && horzVisible;
            var cornerVisible = CornerVisible;

            var metrics = GetRealMetrics();

            var vertWidth = metrics.GetPreferredSize(true, scaleFactor).Width;
            var vertHeight = Bounds.Height;
            var vertLeft = Bounds.Right - vertWidth;
            var vertTop = Bounds.Top;
            var vertBounds = new RectD(vertLeft, vertTop, vertWidth, vertHeight);

            var horzHeight = metrics.GetPreferredSize(false, scaleFactor).Height;
            var horzWidth = Bounds.Width;
            var horzLeft = Bounds.Left;
            var horzTop = Bounds.Bottom - horzHeight;
            var horzBounds = new RectD(horzLeft, horzTop, horzWidth, horzHeight);

            SizeD cornerSize = (vertWidth, horzHeight);
            PointD cornerLocation = (Bounds.Right - cornerSize.Width, Bounds.Bottom - cornerSize.Height);
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
            }

            if (horzVisible)
            {
                HorzScrollBar!.Bounds = horzBounds;
            }
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            PerformLayout(control.ScaleFactor);

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
        public virtual void SetColors(ScrollBar.ThemeMetrics colors)
        {
            VertScrollBar ??= new();
            VertScrollBar.IsVertical = true;
            VertScrollBar.SetColors(colors);

            HorzScrollBar ??= new();
            HorzScrollBar.IsVertical = false;
            HorzScrollBar.SetColors(colors);

            Corner ??= new();
            Corner.Brush = colors.CornerBackground[VisualControlState.Normal].AsBrush
                ?? colors.Background[VisualControlState.Normal].AsBrush;
        }

        /// <summary>
        /// Initialized this drawable with default settings for the specified color theme.
        /// </summary>
        public virtual void SetColors(ScrollBar.KnownTheme theme, bool isDark = false)
        {
            SetColors(ScrollBar.ThemeMetrics.GetColors(theme, isDark));
        }
    }
}
