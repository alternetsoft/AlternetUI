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
        public virtual ScrollBarInfo VertPosition
        {
            get
            {
                return VertScrollBar?.Position ?? ScrollBarInfo.Default;
            }

            set
            {
                VertScrollBar?.SetPosition(value);
            }
        }

        /// <summary>
        /// Gets or sets position of the horizontal scrollbar if it is created.
        /// </summary>
        public virtual ScrollBarInfo HorzPosition
        {
            get
            {
                return HorzScrollBar?.Position ?? ScrollBarInfo.Default;
            }

            set
            {
                HorzScrollBar?.SetPosition(value);
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
        /// Returns <see cref="HitTestsResult"/> which contains
        /// interior hit test and scrollbar hit test.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <param name="scaleFactor">Scale factor used to convert pixels to/from dips.</param>
        /// <returns></returns>
        public HitTestsResult HitTests(Coord scaleFactor, PointD point)
        {
            var rectangles = GetLayoutRectangles(scaleFactor);
            var hitTest = HitTest(rectangles, point);

            ScrollBarDrawable.HitTestResult scrollHitTest;

            if (hitTest == InteriorDrawable.HitTestResult.HorzScrollBar && HorzScrollBar is not null
                && HorzScrollBar.Visible)
            {
                var horzRectangles = HorzScrollBar.GetLayoutRectangles(scaleFactor);
                scrollHitTest = HorzScrollBar.HitTest(horzRectangles, point);
            }
            else
            if (hitTest == InteriorDrawable.HitTestResult.VertScrollBar && VertScrollBar is not null
                && VertScrollBar.Visible)
            {
                var vertRectangles = VertScrollBar.GetLayoutRectangles(scaleFactor);
                scrollHitTest = VertScrollBar.HitTest(vertRectangles, point);
            }
            else
                scrollHitTest = ScrollBarDrawable.HitTestResult.None;

            return new(hitTest, scrollHitTest);
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

            var cornerRect = rectangles[HitTestResult.Corner];

            if (cornerRect.NotEmptyAndContains(point))
                return HitTestResult.Corner;

            var vertScrollBarRect = rectangles[HitTestResult.VertScrollBar];

            if (vertScrollBarRect.NotEmptyAndContains(point))
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
        public virtual EnumArray<HitTestResult, RectD> GetLayoutRectangles(Coord scaleFactor)
        {
            var result = new EnumArray<HitTestResult, RectD>();

            if (HasBorder)
            {
                var borderSettings = Border!.Border;
                if(borderSettings is not null)
                {
                    result[HitTestResult.TopBorder] = borderSettings.GetTopRectangle(Bounds);
                    result[HitTestResult.BottomBorder] = borderSettings.GetBottomRectangle(Bounds);
                    result[HitTestResult.LeftBorder] = borderSettings.GetLeftRectangle(Bounds);
                    result[HitTestResult.RightBorder] = borderSettings.GetRightRectangle(Bounds);
                }
            }

            var boundsInsideBorder = Bounds.DeflatedWithPadding(BorderWidth);
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
                    result[HitTestResult.Corner] = cornerBounds;
                }
            }

            if (vertVisible)
            {
                clientRect.Width -= vertWidth;
                result[HitTestResult.VertScrollBar] = vertBounds;
            }

            if (horzVisible)
            {
                clientRect.Height -= horzHeight;
                result[HitTestResult.HorzScrollBar] = horzBounds;
            }

            result[HitTestResult.ClientRect] = clientRect;
            return result;
        }

        /// <inheritdoc/>
        public override void Draw(Control control, Graphics dc)
        {
            if (!Visible)
                return;

            var rectangles = GetLayoutRectangles(control.ScaleFactor);

            if (Background is not null && Background.Visible)
            {
                Background.Bounds = Bounds;
                Background.Draw(control, dc);
            }

            if (HasCorner)
            {
                Corner!.Bounds = rectangles[HitTestResult.Corner];
                Corner!.Draw(control, dc);
            }

            if (VertVisible)
            {
                VertScrollBar!.Bounds = rectangles[HitTestResult.VertScrollBar];
                VertScrollBar!.Draw(control, dc);
            }

            if (HorzVisible)
            {
                HorzScrollBar!.Bounds = rectangles[HitTestResult.HorzScrollBar];
                HorzScrollBar!.Draw(control, dc);
            }

            if (Border is not null && Border.Visible)
            {
                Border!.Bounds = Bounds;
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

        /// <summary>
        /// Contains full hit test result, including interior part and scrollbar part.
        /// </summary>
        public readonly struct HitTestsResult
        {
            /// <summary>
            /// Gets or sets interior hit test result.
            /// </summary>
            public readonly InteriorDrawable.HitTestResult Interior;

            /// <summary>
            /// Gets or sets interior scrollbar test result.
            /// Valid if <see cref="Interior"/> hit test contains scrollbars.
            /// </summary>
            public readonly ScrollBarDrawable.HitTestResult ScrollBar;

            /// <summary>
            /// Initializes a new instance of the <see cref="HitTestsResult"/> struct.
            /// </summary>
            /// <param name="interior"><see cref="Interior"/> property value.</param>
            /// <param name="scrollBar"><see cref="ScrollBar"/> property value.</param>
            public HitTestsResult(HitTestResult interior, ScrollBarDrawable.HitTestResult scrollBar)
            {
                Interior = interior;
                ScrollBar = scrollBar;
            }

            /// <summary>
            /// Gets whether hit test is on vertical scrollbar.
            /// </summary>
            public bool IsVertScrollBar => Interior == InteriorDrawable.HitTestResult.VertScrollBar;

            /// <summary>
            /// Gets whether hit test is on horizontal scrollbar.
            /// </summary>
            public bool IsHorzScrollBar => Interior == InteriorDrawable.HitTestResult.HorzScrollBar;

            /// <summary>
            /// Gets whether hit test is on horizontal or vertical scrollbar.
            /// </summary>
            public bool IsScrollBar => IsHorzScrollBar || IsVertScrollBar;

            /// <summary>
            /// Gets horizontal or vertical orientation depending on <see cref="IsVertScrollBar"/>
            /// property value.
            /// </summary>
            public ScrollBarOrientation Orientation => IsVertScrollBar
                ? ScrollBarOrientation.Vertical : ScrollBarOrientation.Horizontal;

            /// <summary>
            /// Returns a string that represents the current object.
            /// </summary>
            /// <returns>A string that represents the current object.</returns>
            public override readonly string ToString()
            {
                string[] names = { nameof(Interior), nameof(ScrollBar) };
                object[] values = { Interior, ScrollBar };

                return StringUtils.ToString(names, values);
            }
        }
    }
}
