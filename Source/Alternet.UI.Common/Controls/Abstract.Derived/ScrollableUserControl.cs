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
    public abstract class ScrollableUserControl : UserControl
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

        private readonly bool hasInternalScrollBars;

        private ScrollBarSettings? horizontalScrollBarSettings;
        private ScrollBarSettings? verticalScrollBarSettings;
        private InteriorDrawable? interior;
        private ScrollBarInfo vertScrollBarInfo = new();
        private ScrollBarInfo horzScrollBarInfo = new();

        static ScrollableUserControl()
        {
            DefaultUseInternalScrollBars = App.IsMaui || App.IsLinuxOS;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollableUserControl"/> class.
        /// </summary>
        public ScrollableUserControl()
        {
            hasInternalScrollBars = DefaultUseInternalScrollBars;

            if (!hasInternalScrollBars)
            {
                BorderStyle = DefaultBorderStyle;
                IsScrollable = true;
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
        public override bool HasOwnInterior
        {
            get
            {
                return hasInternalScrollBars;
            }
        }

        /// <summary>
        /// Gets whether the control uses internal scrollbars.
        /// </summary>
        [Browsable(false)]
        public bool HasInternalScrollBars
        {
            get
            {
                return hasInternalScrollBars;
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
        /// Updates the vertical and horizontal scrollbars.
        /// </summary>
        public virtual void UpdateScrollBars(bool refresh)
        {
            if (DisposingOrDisposed)
                return;

            CalcScrollBarInfo(out var horzScrollbar, out var vertScrollbar);
            VertScrollBarInfo = vertScrollbar;
            HorzScrollBarInfo = horzScrollbar;
            if (refresh)
                Refresh();
        }

        /// <summary>
        /// Calculates the position information for the scrollbars based on the number
        /// of visible items
        /// and their total height and maximal width.
        /// </summary>
        public abstract void CalcScrollBarInfo(
            out ScrollBarInfo horzScrollbar,
            out ScrollBarInfo vertScrollbar);

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

        /// <summary>
        /// Scrolls the control horizontally by one char to the left.
        /// </summary>
        public virtual void DoActionScrollCharLeft()
        {
        }

        /// <summary>
        /// Scrolls the control horizontally by one char to the right.
        /// </summary>
        public virtual void DoActionScrollCharRight()
        {
        }

        /// <summary>
        /// Scrolls the control horizontally to the first char.
        /// </summary>
        public virtual void DoActionScrollToFirstChar()
        {
        }

        /// <summary>
        /// Scrolls the control horizontally by one page to the left.
        /// </summary>
        public virtual void DoActionScrollPageLeft()
        {
        }

        /// <summary>
        /// Scrolls the control horizontally by one page to the right.
        /// </summary>
        public virtual void DoActionScrollPageRight()
        {
        }

        /// <summary>
        /// Scrolls the control up by one page.
        /// </summary>
        public virtual void DoActionScrollPageUp()
        {
        }

        /// <summary>
        /// Scrolls the control down by one page.
        /// </summary>
        public virtual void DoActionScrollPageDown()
        {
        }

        /// <summary>
        /// Scrolls the control up by one line.
        /// </summary>
        public virtual void DoActionScrollLineUp()
        {
        }

        /// <summary>
        /// Scrolls the control down by one line.
        /// </summary>
        public virtual void DoActionScrollLineDown()
        {
        }

        /// <summary>
        /// Scrolls to the first line in the control.
        /// </summary>
        public virtual void DoActionScrollToFirstLine()
        {
        }

        /// <summary>
        /// Scrolls to the last line in the control.
        /// </summary>
        public virtual void DoActionScrollToLastLine()
        {
        }

        /// <summary>
        /// Sets vertical scroll offset.
        /// </summary>
        /// <param name="value">Value of the vertical scroll offset.</param>
        public virtual void SetVerticalScrollOffset(Coord value)
        {
        }

        /// <summary>
        /// Sets horizontal scroll offset.
        /// </summary>
        /// <param name="value">Value of the horizontal scroll offset.</param>
        public virtual void SetHorizontalOffset(Coord value)
        {
        }

        /// <summary>
        /// Gets the rectangle that represents the paintable area of the control.
        /// </summary>
        /// <returns>
        /// A <see cref="RectD"/> representing the bounds of the paintable area within the control.
        /// </returns>
        protected virtual RectD GetPaintRectangle()
        {
            var clientR = ClientRectangle;

            if (interior is null)
                return clientR;

            interior.Bounds = clientR;

            var rectangles = interior.GetLayoutRectangles(this);
            var paintRectangle = rectangles[InteriorDrawable.HitTestResult.ClientRect];

            if (HasBorder)
            {
            }

            return paintRectangle;
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
                    DoActionScrollCharRight();
                else
                    DoActionScrollCharLeft();
            }
            else
            {
                if (delta < 0)
                    DoActionScrollLineDown();
                else
                    DoActionScrollLineUp();
            }
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
                        DoActionScrollLineUp();
                        break;
                    case ScrollEventType.SmallIncrement:
                        DoActionScrollLineDown();
                        break;
                    case ScrollEventType.LargeDecrement:
                        DoActionScrollPageUp();
                        break;
                    case ScrollEventType.LargeIncrement:
                        DoActionScrollPageDown();
                        break;
                    case ScrollEventType.ThumbTrack:
                        SetVerticalScrollOffset(e.NewValue);
                        break;
                    case ScrollEventType.First:
                        DoActionScrollToFirstLine();
                        break;
                    case ScrollEventType.Last:
                        DoActionScrollToLastLine();
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
                        DoActionScrollCharLeft();
                        break;
                    case ScrollEventType.SmallIncrement:
                        DoActionScrollCharRight();
                        break;
                    case ScrollEventType.LargeDecrement:
                        DoActionScrollPageLeft();
                        break;
                    case ScrollEventType.LargeIncrement:
                        DoActionScrollPageRight();
                        break;
                    case ScrollEventType.ThumbTrack:
                        SetHorizontalOffset(e.NewValue);
                        break;
                    case ScrollEventType.First:
                        SetHorizontalOffset(0);
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
