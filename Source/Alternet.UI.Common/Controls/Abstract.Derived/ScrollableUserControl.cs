using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="UserControl"/> to provide scrollable functionality.
    /// </summary>
    public class ScrollableUserControl : UserControl
    {
        /// <summary>
        /// Indicates whether the list box controls use internal scrollbars.
        /// </summary>
        public static bool DefaultUseInternalScrollBars;

        /// <summary>
        /// Specifies the default border style for controls.
        /// By default, it equals <see cref="ControlBorderStyle.Theme"/>.
        /// </summary>
        public static ControlBorderStyle DefaultBorderStyle = ControlBorderStyle.Theme;

        /// <summary>
        /// Defines the default increment for horizontal scrollbar position.
        /// Value is specified in characters. Default is 4.
        /// </summary>
        public static int DefaultHorizontalScrollBarLargeIncrement = 4;

        private bool hasInternalScrollBars;
        private ScrollBarSettings? horizontalScrollBarSettings;
        private ScrollBarSettings? verticalScrollBarSettings;
        private InteriorDrawable? interior;
        private ScrollBarInfo vertScrollBarInfo = new();
        private ScrollBarInfo horzScrollBarInfo = new();
        private bool internalScrollBarsImmutable;

        static ScrollableUserControl()
        {
            // We use internal scrollbars:
            // 1. On MAUI because native scrollbars are not supported.
            // 2. On Linux because native scrollbars are not good enough.
            // 3. Everywhere else by default because we want to have the same behavior on all platforms.
            // 4. On Windows with OpenGL because native scrollbars do not work with OpenGL.
            // 5. On Windows because native scrollbars do not change it's color when
            // system color theme changes.
            DefaultUseInternalScrollBars = App.IsMaui || App.IsLinuxOS || true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollableUserControl"/> class.
        /// </summary>
        public ScrollableUserControl()
        {
            var useOpenGL = RenderingFlags.HasFlag(ControlRenderingFlags.UseOpenGL);
            hasInternalScrollBars = useOpenGL || DefaultUseInternalScrollBars;

            OnHasInternalScrollBarsChanged();
        }

        /// <summary>
        /// Gets the router responsible for handling scroll events.
        /// </summary>
        [Browsable(false)]
        public virtual IScrollEventRouter ScrollEventRouter
        {
            get
            {
                return DummyScrollEventRouter.Default;
            }
        }

        /// <inheritdoc/>
        public override ControlBorderStyle BorderStyle
        {
            get => base.BorderStyle;
            set
            {
                if (BorderStyle == value)
                    return;
                base.BorderStyle = value;
                UpdateScrollBars(true);
            }
        }

        /// <summary>
        /// Gets control interior element (border and scrollbars).
        /// Interior is created on demand. So if you need to check whether
        /// the control has interior, use <see cref="HasInterior"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual InteriorDrawable Interior
        {
            get
            {
                if (interior is null)
                {
                    interior = new(IsDarkBackground);
                    AddNotification(interior.Notification);
                    Invalidate();
                }

                return interior;
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool HasOwnInterior
        {
            get
            {
                return UseInternalScrollBars;
            }
        }

        /// <summary>
        /// Gets or sets whether the control uses internal scrollbars.
        /// </summary>
        public virtual bool UseInternalScrollBars
        {
            get
            {
                return hasInternalScrollBars;
            }

            set
            {
                var useOpenGL = RenderingFlags.HasFlag(ControlRenderingFlags.UseOpenGL);
                if (useOpenGL)
                    value = true;

                if (hasInternalScrollBars == value)
                    return;
                if (internalScrollBarsImmutable)
                    return;

                hasInternalScrollBars = value;
                OnHasInternalScrollBarsChanged();
                UpdateScrollBars(true);
            }
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override ControlRenderingFlags RenderingFlags
        {
            get => base.RenderingFlags;
            set
            {
                if (RenderingFlags == value)
                    return;

                var newOpenGL = value.HasFlag(ControlRenderingFlags.UseOpenGL);

                if (newOpenGL)
                {
                    UseInternalScrollBars = true;
                }

                base.RenderingFlags = value;
            }
        }

        /// <inheritdoc/>
        [Browsable(true)]
        public override bool HasBorder
        {
            get
            {
                if (interior is null)
                    return BorderStyle != ControlBorderStyle.None;
                return interior.HasBorder;
            }

            set
            {
                if (HasBorder == value)
                    return;

                if (interior is null)
                {
                    base.HasBorder = value;
                    if (value)
                        BorderStyle = DefaultBorderStyle;
                    else
                        BorderStyle = ControlBorderStyle.None;
                }
                else
                {
                    interior.HasBorder = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets whether the control has initialized <see cref="Interior"/>.
        /// </summary>
        [Browsable(false)]
        public bool HasInterior => interior is not null;

        /// <summary>
        /// Indicates whether horizontal scrollbar settings are defined.
        /// </summary>
        [Browsable(false)]
        public bool HasHorizontalScrollBarSettings => horizontalScrollBarSettings is not null;

        /// <summary>
        /// Indicates whether vertical scrollbar settings are defined.
        /// </summary>
        [Browsable(false)]
        public bool HasVerticalScrollBarSettings => verticalScrollBarSettings is not null;

        /// <summary>
        /// Gets the horizontal scrollbar settings. Initializes them if required.
        /// </summary>
        [Browsable(false)]
        public virtual ScrollBarSettings HorizontalScrollBarSettings
        {
            get
            {
                if (horizontalScrollBarSettings is null)
                {
                    horizontalScrollBarSettings = new();
                    horizontalScrollBarSettings.PropertyChangedAction = (e) =>
                    {
                        UpdateScrollBars(true);
                    };
                }

                return horizontalScrollBarSettings;
            }
        }

        /// <inheritdoc/>
        public override Cursor? Cursor
        {
            get
            {
                return base.Cursor;
            }

            set
            {
                if (IsMouseOverScrollBar())
                {
                    value = Cursors.Default;
                }

                base.Cursor = value;
            }
        }

        /// <summary>
        /// Gets the vertical scrollbar settings. Initializes them if required.
        /// </summary>
        [Browsable(false)]
        public virtual ScrollBarSettings VerticalScrollBarSettings
        {
            get
            {
                if (verticalScrollBarSettings is null)
                {
                    verticalScrollBarSettings = new();
                    verticalScrollBarSettings.PropertyChangedAction = (e) =>
                    {
                        UpdateScrollBars(true);
                    };
                }

                return verticalScrollBarSettings;
            }
        }

        /// <summary>
        /// Performs additional operations after the vertical scroll bar has been painted.
        /// </summary>
        /// <remarks>This method is intended to be overridden in derived classes
        /// to customize behavior after the vertical scroll bar painting is complete.
        /// The default implementation does nothing.</remarks>
        /// <param name="prm">A reference to the parameters used during the painting operation.
        /// This parameter provides context about the
        /// painting process, such as the graphics surface and clipping region.</param>
        public virtual void AfterVertScrollBarPaint(
            ref Alternet.Drawing.BaseDrawable.PaintDelegateParams prm)
        {
        }

        /// <summary>
        /// Updates the vertical and horizontal scrollbars.
        /// </summary>
        public virtual void UpdateScrollBars(bool refresh)
        {
            if (DisposingOrDisposed)
                return;

            ScrollEventRouter.CalcScrollBarInfo(out var horzScrollbar, out var vertScrollbar);
            VertScrollBarInfo = vertScrollbar;
            HorzScrollBarInfo = horzScrollbar;
            if (refresh)
                Refresh();
        }

        /// <inheritdoc/>
        public override ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            if (interior is null)
                return base.GetScrollBarInfo(isVertical);
            if (isVertical)
            {
                return vertScrollBarInfo;
            }
            else
            {
                return horzScrollBarInfo;
            }
        }

        /// <summary>
        /// Marks the <see cref="UseInternalScrollBars"/> property as immutable,
        /// preventing further modifications.
        /// </summary>
        /// <remarks>Once this method is called, the <see cref="UseInternalScrollBars"/> property
        /// is set to an immutable state
        /// and cannot be altered. This operation is irreversible.</remarks>
        public virtual void SetInternalScrollBarsImmutable()
        {
            internalScrollBarsImmutable = true;
        }

        /// <inheritdoc/>
        public override void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            if (interior is null)
            {
                base.SetScrollBarInfo(isVertical, value);
                return;
            }

            if (isVertical)
            {
                vertScrollBarInfo = value;
            }
            else
            {
                horzScrollBarInfo = value;
            }

            RaiseNotifications((n) => n.AfterSetScrollBarInfo(this, isVertical, value));
        }

        /// <inheritdoc/>
        public override RectD GetOverlayRectangle() => GetPaintRectangle();

        /// <summary>
        /// Retrieves the bounding rectangle of the vertical scroll bar, if present.
        /// </summary>
        /// <returns>A <see cref="RectD"/> representing the bounds of the vertical scroll bar, or <see langword="null"/> if the
        /// scroll bar is not available.</returns>
        protected RectD? GetVertScrollBarRectangle()
        {
            return GetInteriorRectangle(InteriorDrawable.HitTestResult.VertScrollBar);
        }

        /// <summary>
        /// Retrieves the bounding rectangle of the horizontal scroll bar, if present.
        /// </summary>
        /// <returns>A <see cref="RectD"/> representing the bounds of the horizontal scroll bar, or <see langword="null"/> if the
        /// scroll bar is not available.</returns>
        protected virtual RectD? GetHorzScrollBarRectangle()
        {
            return GetInteriorRectangle(InteriorDrawable.HitTestResult.HorzScrollBar);
        }

        /// <summary>
        /// Retrieves the interior rectangle corresponding to the specified hit test value, if available.
        /// </summary>
        /// <param name="hitTest">The hit test value that identifies which interior rectangle to retrieve.</param>
        /// <returns>A <see cref="RectD"/> representing the interior rectangle for the given hit test result, or
        /// <see langword="null"/> if no interior is available.</returns>
        protected virtual RectD? GetInteriorRectangle(InteriorDrawable.HitTestResult hitTest)
        {
            if (interior is null)
                return null;

            var clientR = ClientRectangle;

            interior.Bounds = clientR;

            var rectangles = interior.GetLayoutRectangles(this);
            var result = rectangles[hitTest];

            return result;
        }

        /// <summary>
        /// Gets the rectangle that represents the paintable area of the control.
        /// </summary>
        /// <returns>
        /// A <see cref="RectD"/> representing the bounds of the paintable area within the control.
        /// </returns>
        protected virtual RectD GetPaintRectangle()
        {
            var result = GetInteriorRectangle(InteriorDrawable.HitTestResult.ClientRect) ?? ClientRectangle;
            return result;
        }

        /// <inheritdoc/>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateScrollBars(false);
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            GetPaintRectangle();

            UpdateScrollBars(false);
        }

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);

            if (HasInterior)
                Interior.UpdateThemeMetrics();
        }

        /// <summary>
        /// Draws the interior of the control, including borders and scrollbars.
        /// </summary>
        /// <param name="dc"></param>
        protected virtual void DrawInterior(Graphics dc)
        {
            if (interior is null)
                return;

            interior.Draw(this, dc);
        }

        /// <summary>
        /// Removes the interior which draws and manages the scrollbars.
        /// </summary>
        protected virtual void RemoveInterior()
        {
            if (interior is not null)
            {
                RemoveNotification(interior.Notification);
                interior = null;
            }
        }

        /// <summary>
        /// Updates the interior properties such as theme metrics and scrollbar positions.
        /// </summary>
        protected virtual void UpdateInteriorProperties()
        {
            if (interior is null)
                return;

            interior.UpdateThemeMetrics(IsDarkBackground);
            interior.VertPosition = VertScrollBarInfo;
            interior.HorzPosition = HorzScrollBarInfo;
        }

        /// <inheritdoc/>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            var delta = e.Delta;
            if (delta > 0)
            {
                delta = 1;
            }
            else
            if (delta < 0)
            {
                delta = -1;
            }
            else
                return;

            if (Keyboard.IsShiftPressed)
            {
                if (delta < 0)
                    ScrollEventRouter.DoActionScrollCharRight();
                else
                    ScrollEventRouter.DoActionScrollCharLeft();
            }
            else
            {
                if (delta < 0)
                    ScrollEventRouter.DoActionScrollLineDown();
                else
                    ScrollEventRouter.DoActionScrollLineUp();
            }
        }

        /// <summary>
        /// Called when the <see cref="UseInternalScrollBars"/> property changes.
        /// </summary>
        protected virtual void OnHasInternalScrollBarsChanged()
        {
            var oldHasBorder = HasBorder;

            if (UseInternalScrollBars)
            {
                Interior.Required();
                BorderStyle = ControlBorderStyle.None;
                IsScrollable = false;
                Interior.HasBorder = oldHasBorder;
            }
            else
            {
                RemoveInterior();
                HasBorder = oldHasBorder;
                IsScrollable = true;
            }
        }

        /// <summary>
        /// Determines whether the mouse pointer is currently positioned
        /// over a scroll bar or thumb within the control.
        /// </summary>
        /// <remarks>This method checks if the control has its own interior and performs a hit test to
        /// determine whether the mouse pointer is over a scroll bar or thumb. Override this method to customize the
        /// behavior for detecting mouse interactions with scrollable elements.</remarks>
        /// <returns><see langword="true"/> if the mouse pointer is over a scroll bar or thumb; otherwise, <see
        /// langword="false"/>.</returns>
        protected virtual bool IsMouseOverScrollBar()
        {
            if (!HasOwnInterior)
                return false;
            var pt = Mouse.GetPosition(this);
            var hitTest = Interior.HitTests(this, pt);
            return hitTest.IsScrollBar || hitTest.IsThumb;
        }

        /// <inheritdoc/>
        protected override void OnCursorRequested(EventArgs e)
        {
            base.OnCursorRequested(e);
        }

        /// <inheritdoc/>
        protected override void OnScroll(ScrollEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            base.OnScroll(e);

            if (e.IsVertical)
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallDecrement:
                        ScrollEventRouter.DoActionScrollLineUp();
                        break;
                    case ScrollEventType.SmallIncrement:
                        ScrollEventRouter.DoActionScrollLineDown();
                        break;
                    case ScrollEventType.LargeDecrement:
                        ScrollEventRouter.DoActionScrollPageUp();
                        break;
                    case ScrollEventType.LargeIncrement:
                        ScrollEventRouter.DoActionScrollPageDown();
                        break;
                    case ScrollEventType.ThumbTrack:
                        ScrollEventRouter.DoActionScrollToVertPos(e.NewValue);
                        break;
                    case ScrollEventType.First:
                        ScrollEventRouter.DoActionScrollToFirstLine();
                        break;
                    case ScrollEventType.Last:
                        ScrollEventRouter.DoActionScrollToLastLine();
                        break;
                    case ScrollEventType.ThumbPosition:
                    case ScrollEventType.EndScroll:
                    default:
                        break;
                }
            }
            else
            {
                switch (e.Type)
                {
                    case ScrollEventType.SmallDecrement:
                        ScrollEventRouter.DoActionScrollCharLeft();
                        break;
                    case ScrollEventType.SmallIncrement:
                        ScrollEventRouter.DoActionScrollCharRight();
                        break;
                    case ScrollEventType.LargeDecrement:
                        ScrollEventRouter.DoActionScrollPageLeft();
                        break;
                    case ScrollEventType.LargeIncrement:
                        ScrollEventRouter.DoActionScrollPageRight();
                        break;
                    case ScrollEventType.ThumbTrack:
                        ScrollEventRouter.DoActionScrollToHorzPos(e.NewValue);
                        break;
                    case ScrollEventType.First:
                        ScrollEventRouter.DoActionScrollToHorzPos(0);
                        break;
                    case ScrollEventType.Last:
                        break;
                    case ScrollEventType.ThumbPosition:
                    case ScrollEventType.EndScroll:
                    default:
                        break;
                }
            }
        }
    }
}
