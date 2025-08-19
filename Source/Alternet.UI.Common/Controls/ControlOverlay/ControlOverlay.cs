using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a base implementation for overlays that can be drawn on top of controls.
    /// </summary>
    public class ControlOverlay : BaseObjectWithId, IControlOverlay
    {
        private Timer? timer;
        private TimerTickAction timerActionType = TimerTickAction.None;
        private WeakReferenceValue<UserControl> controlContainer = new();
        private ControlOverlayFlags flags;

        /// <summary>
        /// Represents the action to be performed on a timer tick event.
        /// </summary>
        public enum TimerTickAction
        {
            /// <summary>
            /// Do not perform any action on timer tick.
            /// </summary>
            None,

            /// <summary>
            /// Removes the overlay from the control.
            /// </summary>
            RemoveOverlay,
        }

        /// <inheritdoc/>
        public virtual ControlOverlayFlags Flags
        {
            get => flags;
            set => flags = value;
        }

        /// <summary>
        /// Gets or sets the <see cref="UserControl"/> that serves as the container for overlay.
        /// </summary>
        public virtual UserControl? Container
        {
            get => controlContainer.Value;
            set => controlContainer.Value = value;
        }

        /// <summary>
        /// Gets the timer instance associated with this object.
        /// </summary>
        public virtual Timer Timer
        {
            get
            {
                if (timer is null)
                {
                    timer = new();
                    timer.TickAction = OnTimerTick;
                }

                return timer;
            }
        }

        /// <summary>
        /// Gets or sets the action to be executed after each tick of the <see cref="Timer"/>.
        /// </summary>
        /// <remarks>This property allows you to define custom behavior that will
        /// be executed after each tick. Ensure that the assigned action does not throw
        /// exceptions or cause side effects that could disrupt the tick process.</remarks>
        public virtual Action? AfterTickAction { get; set; }

        /// <summary>
        /// Starts a one-time timer to trigger the removal of an overlay after the specified duration.
        /// </summary>
        /// <remarks>This method configures the timer to execute a single action for overlay removal.
        /// Ensure that the specified duration is a positive value
        /// to avoid unexpected behavior.</remarks>
        /// <param name="milliseconds">The duration, in milliseconds,
        /// after which the overlay will be removed. Can be null.
        /// If not specified or not positive,
        /// <see cref="TimerUtils.DefaultToolTipTimeout"/> is used as the interval.</param>
        /// <param name="container">The control container from
        /// which the overlay will be removed.</param>
        public virtual void SetRemovalTimer(int? milliseconds, UserControl container)
        {
            if (milliseconds <= 0 || milliseconds is null)
                milliseconds = TimerUtils.DefaultToolTipTimeout;
            controlContainer.Value = container;
            timerActionType = TimerTickAction.RemoveOverlay;
            Timer.Interval = milliseconds.Value;
            Timer.StartOnce();
        }

        /// <inheritdoc/>
        public virtual void OnPaint(AbstractControl control, PaintEventArgs e)
        {
        }

        /// <summary>
        /// Handles the timer tick event, allowing derived classes
        /// to define custom behavior when the timer elapses.
        /// </summary>
        /// <remarks>This method is called each time the timer elapses.
        /// Override this method in a derived
        /// class to implement specific functionality that should occur on each timer tick.
        /// </remarks>
        protected virtual void OnTimerTick()
        {
            if (timerActionType == TimerTickAction.RemoveOverlay)
            {
                var container = controlContainer.Value;
                container?.RemoveOverlay(this);
            }

            timerActionType = TimerTickAction.None;
            controlContainer.Value = null;
            AfterTickAction?.Invoke();
        }
    }
}
