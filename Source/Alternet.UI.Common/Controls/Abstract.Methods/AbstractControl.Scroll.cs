using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Occurs when the user scrolls through the control contents using scrollbars.
        /// </summary>
        [Category("Action")]
        public event ScrollEventHandler? Scroll;

        /// <summary>
        /// Gets or sets whether controls is scrollable.
        /// This makes sense only for <see cref="UserControl"/> descendants.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsScrollable
        {
            get
            {
                return GetIsScrollable(true) && GetIsScrollable(false);
            }

            set
            {
                SetScrollable(true, value);
                SetScrollable(false, value);
            }
        }

        /// <summary>
        /// Gets or sets whether horizontal scrollbar is visible in the control.
        /// This makes sense only for <see cref="UserControl"/> descendants.
        /// </summary>
        [Browsable(false)]
        public virtual bool HorizontalScrollbar
        {
            get => GetIsScrollable(false);

            set
            {
                if (this is not UserControl)
                    return;
                SetScrollable(false, value);
            }
        }

        /// <summary>
        /// Gets or sets whether vertical scrollbar is visible in the control.
        /// This makes sense only for <see cref="UserControl"/> descendants.
        /// </summary>
        [Browsable(false)]
        public virtual bool VerticalScrollbar
        {
            get => GetIsScrollable(true);

            set
            {
                if (this is not UserControl)
                    return;
                SetScrollable(true, value);
            }
        }

        /// <summary>
        /// Gets or sets horizontal scrollbar position as <see cref="ScrollBarInfo"/>.
        /// This makes sense only for <see cref="UserControl"/> descendants.
        /// </summary>
        [Browsable(false)]
        public virtual ScrollBarInfo HorzScrollBarInfo
        {
            get
            {
                return GetScrollBarInfo(false);
            }

            set
            {
                SetScrollBarInfo(false, value);
            }
        }

        /// <summary>
        /// Gets or sets vertical scrollbar position as <see cref="ScrollBarInfo"/>.
        /// This makes sense only for <see cref="UserControl"/> descendants.
        /// </summary>
        [Browsable(false)]
        public virtual ScrollBarInfo VertScrollBarInfo
        {
            get
            {
                return GetScrollBarInfo(true);
            }

            set
            {
                SetScrollBarInfo(true, value);
            }
        }

        /// <summary>
        /// Raises the <see cref="Scroll"/> event and <see cref="OnScroll"/> method
        /// with the specified parameters.
        /// </summary>
        /// <param name="orientation">Scroll bar orientation.</param>
        /// <param name="eventType">Type of the scroll event.</param>
        public void RaiseScroll(
            ScrollBarOrientation orientation,
            ScrollEventType eventType)
        {
            ScrollEventArgs scrollArgs = new();
            scrollArgs.ScrollOrientation = orientation;
            scrollArgs.Type = eventType;
            RaiseScroll(scrollArgs);
        }

        /// <summary>
        /// Raises scroll event which scrolls page down.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollPageDown()
        {
            RaiseScroll(ScrollBarOrientation.Vertical, ScrollEventType.LargeIncrement);
        }

        /// <summary>
        /// Raises scroll event which scrolls page up.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollPageUp()
        {
            RaiseScroll(ScrollBarOrientation.Vertical, ScrollEventType.LargeDecrement);
        }

        /// <summary>
        /// Raises scroll event which scrolls line down.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollLineDown()
        {
            RaiseScroll(ScrollBarOrientation.Vertical, ScrollEventType.SmallIncrement);
        }

        /// <summary>
        /// Raises scroll event which scrolls line up.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollLineUp()
        {
            RaiseScroll(ScrollBarOrientation.Vertical, ScrollEventType.SmallDecrement);
        }

        /// <summary>
        /// Raises scroll event which scrolls char right.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollCharRight()
        {
            RaiseScroll(ScrollBarOrientation.Horizontal, ScrollEventType.LargeIncrement);
        }

        /// <summary>
        /// Raises scroll event which scrolls char left.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollCharLeft()
        {
            RaiseScroll(ScrollBarOrientation.Horizontal, ScrollEventType.SmallDecrement);
        }

        /// <summary>
        /// Raises scroll event which scrolls page right.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollPageRight()
        {
            RaiseScroll(ScrollBarOrientation.Horizontal, ScrollEventType.LargeIncrement);
        }

        /// <summary>
        /// Raises scroll event which scrolls page left.
        /// </summary>
        [Browsable(false)]
        public void RaiseScrollPageLeft()
        {
            RaiseScroll(ScrollBarOrientation.Horizontal, ScrollEventType.LargeDecrement);
        }

        /// <summary>
        /// Raises the <see cref="Scroll"/> event and <see cref="OnScroll"/> method.
        /// </summary>
        /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event
        /// data.</param>
        public void RaiseScroll(ScrollEventArgs e)
        {
            OnScroll(e);

            if(RealLayout == LayoutStyle.Scroll && IsScrollable)
            {
                var offset = GetScrollBarInfo(e.IsVertical).Position;

                if (e.IsVertical)
                    LayoutOffset = new PointD(LayoutOffset.X, -offset);
                else
                    LayoutOffset = new PointD(-offset, LayoutOffset.Y);
            }

            Scroll?.Invoke(this, e);

            RaiseNotifications((n) => n.AfterScroll(this, e));

            ForEachVisibleChild(e, (control, e) => control.OnAfterParentScroll(this, e));
        }

        /// <summary>
        /// Gets vertical or horizontal scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        /// <param name="isVertical">Whether to get position for the vertical
        /// or horizontal scrollbar.</param>
        /// <returns></returns>
        public virtual ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            return ScrollBarInfo.Default;
        }

        /// <summary>
        /// Sets vertical or horizontal scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        /// <param name="isVertical">Whether to set position for the vertical or
        /// horizontal scrollbar.</param>
        /// <param name="value">Scrollbar position.</param>
        public virtual void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            RaiseNotifications((n) => n.AfterSetScrollBarInfo(this, isVertical, value));
        }

        /// <summary>
        /// Sets vertical and horizontal scrollbar positions using page
        /// and total size parameters.
        /// </summary>
        /// <param name="pageSize">Page size.</param>
        /// <param name="totalSize">Total size.</param>
        /// <param name="visibilityHorz">Horizontal scrollbar visibility.</param>
        /// <param name="visibilityVert">Vertical scrollbar visibility.</param>
        public virtual void SetScrollBarInfo(
            SizeD pageSize,
            SizeD totalSize,
            HiddenOrVisible? visibilityHorz = null,
            HiddenOrVisible? visibilityVert = null)
        {
            if (totalSize.Width <= pageSize.Width)
            {
                HorzScrollBarInfo
                    = HorzScrollBarInfo.WithVisibility(visibilityHorz ?? HiddenOrVisible.Hidden);
            }
            else
            {
                ScrollBarInfo horz = new()
                {
                    Visibility = visibilityHorz ?? HiddenOrVisible.Auto,
                    Range = (int)totalSize.Width,
                    PageSize = (int)pageSize.Width,
                    Position = GetScrollBarValue(false),
                };

                HorzScrollBarInfo = horz;
            }

            if (totalSize.Height <= pageSize.Height)
            {
                VertScrollBarInfo
                    = VertScrollBarInfo.WithVisibility(visibilityVert ?? HiddenOrVisible.Hidden);
            }
            else
            {
                ScrollBarInfo vert = new()
                {
                    Visibility = visibilityVert ?? HiddenOrVisible.Auto,
                    Range = (int)totalSize.Height,
                    PageSize = (int)pageSize.Height,
                    Position = GetScrollBarValue(true),
                };

                VertScrollBarInfo = vert;
            }
        }

        /// <summary>
        /// Sets system scrollbar properties.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <param name="visibility">Scrollbar visibility mode.</param>
        /// <param name="value">Thumb position.</param>
        /// <param name="largeChange">Large change value (when scrolls page up or down).</param>
        /// <param name="maximum">Scrollbar Range.</param>
        public void SetScrollBar(
            bool isVertical,
            HiddenOrVisible visibility,
            int value,
            int largeChange,
            int maximum)
        {
            ScrollBarInfo info = new(value, maximum, largeChange);
            info.Visibility = visibility;

            if (isVertical)
                VertScrollBarInfo = info;
            else
                HorzScrollBarInfo = info;
        }

        /// <summary>
        /// Gets whether system scrollbar is visible.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public HiddenOrVisible GetScrollBarVisibility(bool isVertical)
        {
            return GetScrollBarInfo(isVertical).Visibility;
        }

        /// <summary>
        /// Gets system scrollbar thumb position.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public int GetScrollBarValue(bool isVertical)
        {
            return GetScrollBarInfo(isVertical).Position;
        }

        /// <summary>
        /// Gets system scrollbar large change value.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public int GetScrollBarLargeChange(bool isVertical)
        {
            return GetScrollBarInfo(isVertical).PageSize;
        }

        /// <summary>
        /// Gets system scrollbar max range.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public int GetScrollBarMaximum(bool isVertical)
        {
            return GetScrollBarInfo(isVertical).Range;
        }

        /// <summary>
        /// Called after <see cref="Scroll"/> event of the <see cref="Parent"/> is raised.
        /// </summary>
        /// <param name="e">A <see cref="ScrollEventArgs"/> that
        /// contains the event data.</param>
        /// <param name="sender">The source of the event.</param>
        protected virtual void OnAfterParentScroll(object? sender, ScrollEventArgs e)
        {
        }

        /// <summary>
        /// Called when <see cref="Scroll"/> event is raised.
        /// </summary>
        /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnScroll(ScrollEventArgs e)
        {
        }

        private bool GetIsScrollable(bool isVert)
        {
            var visible = GetScrollBarInfo(isVert).IsVisible;
            return visible;
        }

        private void SetScrollable(bool isVert, bool value)
        {
            var info = GetScrollBarInfo(isVert);
            if (info.IsVisible == value)
                return;
            info.Visibility = HiddenOrVisible.Auto;
            SetScrollBarInfo(isVert, info);
        }
    }
}
