using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Occurs when the user scrolls through the control contents using scrollbars.
        /// </summary>
        [Category("Action")]
        public event ScrollEventHandler? Scroll;

        /// <summary>
        /// Gets or sets whether controls is scrollable.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsScrollable
        {
            get => Handler.IsScrollable;

            set => Handler.IsScrollable = value;
        }

        /// <summary>
        /// Gets or sets horizontal scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        [Browsable(false)]
        public ScrollBarInfo HorzScrollBarInfo
        {
            get
            {
                return Handler.HorzScrollBarInfo;
            }

            set
            {
                SetScrollBarInfo(false, value);
            }
        }

        /// <summary>
        /// Gets or sets vertical scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        [Browsable(false)]
        public ScrollBarInfo VertScrollBarInfo
        {
            get
            {
                return Handler.VertScrollBarInfo;
            }

            set
            {
                SetScrollBarInfo(true, value);
            }
        }

        /// <summary>
        /// Gets or sets whether scroll events are binded and recveived in the control.
        /// </summary>
        protected virtual bool BindScrollEvents
        {
            get
            {
                return Handler.BindScrollEvents;
            }

            set
            {
                Handler.BindScrollEvents = value;
            }
        }

        /// <summary>
        /// Raises the <see cref="Scroll"/> event and <see cref="OnScroll"/> method.
        /// </summary>
        /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event
        /// data.</param>
        public void RaiseScroll(ScrollEventArgs e)
        {
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            OnScroll(e);
            Scroll?.Invoke(this, e);

            foreach (var n in nn)
            {
                n.AfterScroll(this, e);
            }

            foreach (var n in nn2)
            {
                n.AfterScroll(this, e);
            }
        }

        /// <summary>
        /// Gets vertical or horizontal scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        /// <param name="isVertical">Whether to get position for the vertical or horizontal scrollbar.</param>
        /// <returns></returns>
        public ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            if(isVertical)
                return Handler.VertScrollBarInfo;
            return Handler.HorzScrollBarInfo;
        }

        /// <summary>
        /// Sets vertical or horizontal scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        /// <param name="isVertical">Whether to set position for the vertical or
        /// horizontal scrollbar.</param>
        /// <param name="value">Scrollbar position.</param>
        public void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
            var nn = Notifications;
            var nn2 = GlobalNotifications;

            if (isVertical)
                Handler.VertScrollBarInfo = value;
            else
                Handler.HorzScrollBarInfo = value;

            foreach (var n in nn)
            {
                n.AfterSetScrollBarInfo(this, isVertical, value);
            }

            foreach (var n in nn2)
            {
                n.AfterSetScrollBarInfo(this, isVertical, value);
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
        /// Called when <see cref="Scroll"/> event is raised.
        /// </summary>
        /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnScroll(ScrollEventArgs e)
        {
        }
    }
}
