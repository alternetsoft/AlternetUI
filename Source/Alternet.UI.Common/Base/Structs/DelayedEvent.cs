using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements delayed event.
    /// </summary>
    /// <typeparam name="TArgs">Type of the event argument.</typeparam>
    public struct DelayedEvent<TArgs>
    {
        private Timer? timer;

        /// <summary>
        /// Occurs when delayed event handlers are notified.
        /// </summary>
        public event EventHandler<TArgs>? Delayed;

        /// <summary>
        /// Gets or sets default timeout interval (in msec) for timer that calls
        /// <see cref="Delayed"/> event. If not specified,
        /// <see cref="TimerUtils.DefaultDelayedTextChangedTimeout"/> is used.
        /// </summary>
        public int? Interval { get; set; }

        /// <summary>
        /// Adds event handler.
        /// </summary>
        /// <param name="evt">Event handler.</param>
        public void Add(EventHandler<TArgs> evt)
        {
            Delayed += evt;
        }

        /// <summary>
        /// Removes event handler.
        /// </summary>
        /// <param name="evt">Event handler.</param>
        public void Remove(EventHandler<TArgs> evt)
        {
            Delayed -= evt;
        }

        /// <summary>
        /// Raises <see cref="Delayed"/> event immediately.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <param name="isSuspended">Whether event is suspended (will not be called).</param>
        public readonly void RaiseWithoutDelay(
            object? sender,
            TArgs e,
            Func<bool>? isSuspended = null)
        {
            if (isSuspended?.Invoke() ?? false)
                return;
            Delayed?.Invoke(sender, e);
        }

        /// <summary>
        /// Raises delayed event.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        /// <param name="isSuspended">Whether event is suspended (will not be called).</param>
        public void Raise(object? sender, TArgs e, Func<bool> isSuspended)
        {
            if (Delayed is not null)
            {
                var self = this;
                timer ??= new();
                timer.Stop();
                timer.Interval = Interval ?? TimerUtils.DefaultDelayedTextChangedTimeout;
                timer.TickAction = () =>
                {
                    if (isSuspended())
                        return;
                    self.Delayed?.Invoke(sender, e);
                };
                timer.StartOnce();
            }
        }

        /// <summary>
        /// Resets object state, disposes event timer.
        /// </summary>
        public void Reset()
        {
            BaseObject.SafeDispose(ref timer);
            Delayed = null;
        }
    }
}
