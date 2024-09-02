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
        public BorderDrawable? Border;

        private ScrollBar.MetricsInfo? metrics;

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
            /// Hit is on the corner.
            /// </summary>
            Corner,

            /// <summary>
            /// Hit is on the top border.
            /// </summary>
            TopBorder,

            /// <summary>
            /// Hit is on the bottom border.
            /// </summary>
            BottomBorder,

            /// <summary>
            /// Hit is on the left border.
            /// </summary>
            LeftBorder,

            /// <summary>
            /// Hit is on the right border.
            /// </summary>
            RightBorder,

            /// <summary>
            /// Hit is on the vertical scrollbar.
            /// </summary>
            VertScrollBar,

            /// <summary>
            /// Hit is on the horizontal scrollbar.
            /// </summary>
            HorzScrollBar,

            /// <summary>
            /// Hit is inside the client rectangle.
            /// </summary>
            ClientRect,
        }

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
        /// Sets default border.
        /// </summary>
        /// <param name="isDarkBackground">Whether to use default border for the dark background
        /// or for the light background.</param>
        public virtual void SetDefaultBorder(bool isDarkBackground)
        {
            Border = new();
            Border.Border = new();
            Border.Border.Color = ColorUtils.GetDefaultBorderColor(isDarkBackground);
        }

        /// <summary>
        /// Returns one of <see cref="HitTestResult"/> constants.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <param name="scaleFactor">Scale factor used to convert pixels to/from dips.</param>
        /// <returns></returns>
        public virtual HitTestResult HitTest(Coord scaleFactor, PointD point)
        {
            if (!Bounds.NotEmptyAndContains(point))
                return HitTestResult.None;

            var rectangles = PerformLayout(scaleFactor);

            if (rectangles[HitTestResult.Corner].NotEmptyAndContains(point))
                return HitTestResult.Corner;

            if (rectangles[HitTestResult.VertScrollBar].NotEmptyAndContains(point))
                return HitTestResult.VertScrollBar;

            if (rectangles[HitTestResult.HorzScrollBar].NotEmptyAndContains(point))
                return HitTestResult.HorzScrollBar;

            if (rectangles[HitTestResult.ClientRect].NotEmptyAndContains(point))
                return HitTestResult.ClientRect;

            if (rectangles[HitTestResult.TopBorder].NotEmptyAndContains(point))
                return HitTestResult.TopBorder;

            if (rectangles[HitTestResult.BottomBorder].NotEmptyAndContains(point))
                return HitTestResult.BottomBorder;

            if (rectangles[HitTestResult.LeftBorder].NotEmptyAndContains(point))
                return HitTestResult.LeftBorder;

            if (rectangles[HitTestResult.RightBorder].NotEmptyAndContains(point))
                return HitTestResult.RightBorder;

            return HitTestResult.None;
        }

        /// <summary>
        /// Performs layout of the drawable childs and returns calculated bound of the different
        /// parts of the drawable.
        /// </summary>
        /// <param name="scaleFactor">Scale factor used to convert pixels to/from dips.</param>
        /// <returns>Calculated bounds of the different parts of the drawable.</returns>
        public virtual EnumArray<HitTestResult, RectD> PerformLayout(Coord scaleFactor)
        {
            var result = new EnumArray<HitTestResult, RectD>();

            if (HasBorder)
            {
                Border!.Bounds = Bounds;
                var borderSettings = Border!.Border;
                if(borderSettings is not null)
                {
                    result[HitTestResult.TopBorder] = borderSettings.GetTopRectangle(Bounds);
                    result[HitTestResult.BottomBorder] = borderSettings.GetBottomRectangle(Bounds);
                    result[HitTestResult.LeftBorder] = borderSettings.GetLeftRectangle(Bounds);
                    result[HitTestResult.RightBorder] = borderSettings.GetRightRectangle(Bounds);
                }
            }
            else
            {
            }

            var boundsInsideBorder = Bounds;
            var borderWidth = BorderWidth;

            boundsInsideBorder.Left += borderWidth.Left;
            boundsInsideBorder.Top += borderWidth.Top;
            boundsInsideBorder.Width -= borderWidth.Horizontal;
            boundsInsideBorder.Height -= borderWidth.Vertical;

            var clientRect = boundsInsideBorder;

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

                if (cornerVisible)
                {
                    Corner!.Bounds = cornerBounds;
                    result[HitTestResult.Corner] = cornerBounds;
                }
                else
                {
                }
            }

            if (vertVisible)
            {
                VertScrollBar!.Bounds = vertBounds;
                clientRect.Width -= vertWidth;
                result[HitTestResult.VertScrollBar] = vertBounds;
            }
            else
            {
            }

            if (horzVisible)
            {
                clientRect.Height -= horzHeight;
                HorzScrollBar!.Bounds = horzBounds;
                result[HitTestResult.HorzScrollBar] = horzBounds;
            }
            else
            {
            }

            if (Background is not null)
            {
                Background.Bounds = Bounds;
            }

            result[HitTestResult.ClientRect] = clientRect;
            return result;
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            PerformLayout(control.ScaleFactor);

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

            if (Border is not null && Border.Visible)
            {
                Border.Draw(control, dc);
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
        /// Initialized this drawable with default settings for the specified color theme.
        /// </summary>
        public void SetThemeMetrics(ScrollBar.KnownTheme theme, bool isDark = false)
        {
            var themeObj = ScrollBar.ThemeMetrics.GetTheme(theme, isDark);
            themeObj.AssignTo(this);
        }
    }
}
