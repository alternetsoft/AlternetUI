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
        /// Call this function to force one or both scrollbars to be always shown, even if
        /// the control is big enough to show its entire contents without scrolling.
        /// </summary>
        /// <param name="hflag">Whether the horizontal scroll bar should always be visible.</param>
        /// <param name="vflag">Whether the vertical scroll bar should always be visible.</param>
        /// <remarks>
        /// This function is currently only implemented under Mac/Carbon.
        /// </remarks>
        public virtual void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
            Handler.AlwaysShowScrollbars(hflag, vflag);
        }

        /// <summary>
        /// Sets system scrollbar properties.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <param name="visible">Is scrollbar visible or not.</param>
        /// <param name="value">Thumb position.</param>
        /// <param name="largeChange">Large change value (when scrolls page up or down).</param>
        /// <param name="maximum">Scrollbar Range.</param>
        public virtual void SetScrollBar(
            bool isVertical,
            bool visible,
            int value,
            int largeChange,
            int maximum)
        {
            Handler.SetScrollBar(isVertical, visible, value, largeChange, maximum);
        }

        /// <summary>
        /// Gets whether system scrollbar is visible.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public virtual bool IsScrollBarVisible(bool isVertical)
        {
            return Handler.IsScrollBarVisible(isVertical);
        }

        /// <summary>
        /// Gets system scrollbar thumb position.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public virtual int GetScrollBarValue(bool isVertical)
        {
            return Handler.GetScrollBarValue(isVertical);
        }

        /// <summary>
        /// Gets system scrollbar large change value.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public virtual int GetScrollBarLargeChange(bool isVertical)
        {
            return Handler.GetScrollBarLargeChange(isVertical);
        }

        /// <summary>
        /// Gets system scrollbar max range.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public virtual int GetScrollBarMaximum(bool isVertical)
        {
            return Handler.GetScrollBarMaximum(isVertical);
        }

        /// <summary>
        /// Raises the <see cref="Scroll"/> event and <see cref="OnScroll"/> method.
        /// </summary>
        /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event
        /// data.</param>
        public void RaiseScroll(ScrollEventArgs e)
        {
            OnScroll(e);
            Scroll?.Invoke(this, e);
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
