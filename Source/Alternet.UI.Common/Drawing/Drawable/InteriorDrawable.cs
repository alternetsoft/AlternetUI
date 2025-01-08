﻿using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Implements control interior drawing.
    /// This includes border, background, vertical and horizontal scrollbars drawing.
    /// </summary>
    public partial class InteriorDrawable : BaseDrawable
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
        private InteriorControlActivity? notification;
        private ScrollBar.KnownTheme? scrollBarTheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteriorDrawable"/> class.
        /// </summary>
        public InteriorDrawable()
        {
            SetThemeMetrics(ScrollBar.KnownTheme.MauiAuto, SystemSettings.AppearanceIsDark);
        }

        /// <summary>
        /// Occurs when the interior element is clicked.
        /// </summary>
        public event EventHandler<BaseEventArgs<HitTestsResult>>? ElementClick;

        /// <summary>
        /// Occurs when the corner which is below vertical scrollbar is clicked.
        /// </summary>
        public event EventHandler? CornerClick;

        /// <summary>
        /// Occurs when any of the attached scrollbars change their position.
        /// </summary>
        public event EventHandler<ScrollEventArgs>? Scroll;

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
        /// Gets theme assigned with <see cref="SetThemeMetrics"/>.
        /// </summary>
        public virtual ScrollBar.KnownTheme? ScrollBarTheme
        {
            get
            {
                return scrollBarTheme;
            }
        }

        /// <summary>
        /// Gets whether vertical scrollbar is visible.
        /// </summary>
        public virtual bool VertVisible
        {
            get
            {
                return VertScrollBar?.Visible ?? false;
            }
        }

        /// <summary>
        /// Gets whether horizontal scrollbar is visible.
        /// </summary>
        public virtual bool HorzVisible
        {
            get
            {
                return HorzScrollBar?.Visible ?? false;
            }
        }

        /// <summary>
        /// Gets whether vertical and horizontal scrollbars are visible.
        /// </summary>
        public virtual bool BothVisible
        {
            get
            {
                return VertVisible && HorzVisible;
            }
        }

        /// <summary>
        /// Gets whether corner is visible.
        /// </summary>
        public virtual bool CornerVisible
        {
            get
            {
                return Corner?.Visible ?? false;
            }
        }

        /// <summary>
        /// Gets whether corner and both scrollbars are visible.
        /// </summary>
        public virtual bool HasCorner
        {
            get
            {
                return BothVisible && CornerVisible;
            }
        }

        /// <summary>
        /// Gets whether interior has border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                return Border?.Visible ?? false;
            }

            set
            {
                if (Border is not null)
                    Border.Visible = value;
            }
        }

        /// <summary>
        /// Gets <see cref="InteriorControlActivity"/> attached to this object.
        /// </summary>
        public InteriorControlActivity Notification
        {
            get
            {
                return notification ??= new(this);
            }
        }

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
        public virtual ScrollBar.MetricsInfo? ScrollBarMetrics
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
                if (HorzScrollBar is not null)
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
        /// <param name="control">Control which scale factor
        /// is used to convert pixels to/from dips.</param>
        /// <returns></returns>
        public virtual HitTestsResult HitTests(AbstractControl control, PointD point)
        {
            Coord scaleFactor = control.ScaleFactor;
            var rectangles = GetLayoutRectangles(control);
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
        /// <param name="control">Control which scale factor used to
        /// convert pixels to/from dips.</param>
        /// <returns>Calculated bounds of the different parts of the drawable.</returns>
        public virtual EnumArray<HitTestResult, RectD> GetLayoutRectangles(AbstractControl control)
        {
            Coord scaleFactor = control.ScaleFactor;

            var result = new EnumArray<HitTestResult, RectD>();
            var borderWidth = BorderWidth;

            if (HasBorder)
            {
                var borderSettings = Border!.Border;
                if (borderSettings is not null)
                {
                    result[HitTestResult.TopBorder] = borderSettings.GetTopRectangle(Bounds);
                    result[HitTestResult.BottomBorder] = borderSettings.GetBottomRectangle(Bounds);
                    result[HitTestResult.LeftBorder] = borderSettings.GetLeftRectangle(Bounds);
                    result[HitTestResult.RightBorder] = borderSettings.GetRightRectangle(Bounds);
                }
            }
            else
            {
                borderWidth = 0;
            }

            var boundsInsideBorder = Bounds.DeflatedWithPadding(borderWidth);
            var clientRect = boundsInsideBorder;

            var vertVisible = VertVisible;
            var horzVisible = HorzVisible;
            var bothVisible = vertVisible && horzVisible;
            var cornerVisible = CornerVisible;

            var metrics = GetRealMetrics(control);

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

        /// <summary>
        /// Raises <see cref="Scroll"/> event.
        /// </summary>
        /// <param name="sender">Value to pass as a sender to the event.</param>
        /// <param name="e">Event arguments.</param>
        public void RaiseScroll(object sender, ScrollEventArgs e)
        {
            Scroll?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises <see cref="ElementClick"/> event.
        /// </summary>
        /// <param name="sender">Value to pass as a sender to the event.</param>
        /// <param name="hitTest">Hit test information.</param>
        public void RaiseElementClick(object sender, HitTestsResult hitTest)
        {
            ElementClick?.Invoke(sender, new(hitTest));
        }

        /// <summary>
        /// Raises <see cref="CornerClick"/> event.
        /// </summary>
        /// <param name="sender">Value to pass as a sender to the event.</param>
        public void RaiseCornerClick(object sender)
        {
            CornerClick?.Invoke(sender, EventArgs.Empty);
        }

        /// <inheritdoc/>
        public override void Draw(AbstractControl control, Graphics dc)
        {
            if (!Visible)
                return;

            var rectangles = GetLayoutRectangles(control);

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
        /// Gets real scroll bar metrics. If <see cref="ScrollBarMetrics"/> is not specified, returns
        /// <see cref="ScrollBar.DefaultMetrics"/>.
        /// </summary>
        /// <returns></returns>
        public virtual ScrollBar.MetricsInfo GetRealMetrics(AbstractControl control)
        {
            return metrics ?? ScrollBar.DefaultMetrics(control);
        }

        /// <summary>
        /// Initialized this drawable with default settings for the specified color theme.
        /// </summary>
        public virtual void SetThemeMetrics(ScrollBar.KnownTheme theme, bool isDark = false)
        {
            scrollBarTheme = theme;
            SetDefaultBorder(isDark);
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
            /// Gets whether hit test is on the corner which is below vertical scrollbar.
            /// </summary>
            public bool IsCorner => Interior == InteriorDrawable.HitTestResult.Corner;

            /// <summary>
            /// Gets whether hit test is outside of anything.
            /// </summary>
            public bool IsNone => Interior == InteriorDrawable.HitTestResult.None;

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
