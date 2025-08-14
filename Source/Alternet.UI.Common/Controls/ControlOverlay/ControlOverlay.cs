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
        private TimerTickAction timerTickAction = TimerTickAction.None;
        private WeakReferenceValue<UserControl> controlContainer = new();

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
        /// Starts a one-time timer to trigger the removal of an overlay after the specified duration.
        /// </summary>
        /// <remarks>This method configures the timer to execute a single action for overlay removal.
        /// Ensure that the specified duration is a positive value
        /// to avoid unexpected behavior.</remarks>
        /// <param name="milliseconds">The duration, in milliseconds,
        /// after which the overlay will be removed.</param>
        /// <param name="container">The control container from
        /// which the overlay will be removed.</param>
        public virtual void StartRemovalTimer(int milliseconds, UserControl container)
        {
            controlContainer.Value = container;
            timerTickAction = TimerTickAction.RemoveOverlay;
            Timer.Interval = milliseconds;
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
            if (timerTickAction == TimerTickAction.RemoveOverlay)
            {
                var container = controlContainer.Value;
                container?.RemoveOverlay(this);
            }

            timerTickAction = TimerTickAction.None;
            controlContainer.Value = null;
        }
    }
}
