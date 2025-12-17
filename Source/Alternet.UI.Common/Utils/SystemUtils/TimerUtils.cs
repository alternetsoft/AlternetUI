using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the timers.
    /// </summary>
    public static class TimerUtils
    {
        /// <summary>
        /// Gets or sets default timeout interval (in msec) for timer that calls
        /// <see cref="AbstractControl.DelayedTextChanged"/> event.
        /// </summary>
        public static int DefaultDelayedTextChangedTimeout = 150;

        /// <summary>
        /// Gets or sets default tooltip timeout interval (in msec).
        /// </summary>
        // On Windows some multiple of double-click time is used but we currently have constant
        // constant value by default.
        public static int DefaultToolTipTimeout = 5000;

        // When interval is 50, in VirtualListBox scroll bar is not working properly.
        private static int clickRepeatInterval = 30;

        private static Timer? clickRepeatTimer;
        private static Timer? hoverTimer;

        /// <summary>
        /// Occurs when the click repeat timer interval has elapsed.
        /// </summary>
        public static event EventHandler? ClickRepeated
        {
            add
            {
                ClickRepeatTimer.Required();
                ClickRepeatedEvent += value;
                ClickRepeatTimer.Enabled = true;
            }

            remove
            {
                ClickRepeatedEvent -= value;
                if(ClickRepeatedEvent is null)
                {
                    ClickRepeatTimer.Enabled = false;
                }
            }
        }

        private static event EventHandler? ClickRepeatedEvent;

        /// <summary>
        /// Gets a value indicating whether the hover timer has been created.
        /// </summary>
        public static bool IsMouseHoverTimerCreated => hoverTimer is not null;

        /// <summary>
        /// Gets the static timer used to track mouse hover events.
        /// </summary>
        public static Timer MouseHoverTimer
        {
            get
            {
                hoverTimer ??= new Timer(SystemSettings.MouseHoverTime, HoverTimerTick);
                return hoverTimer;

                void HoverTimerTick()
                {
                    if (AbstractControl.HoveredControl is not null)
                    {
                        if(AbstractControl.HoveredControl.DisposingOrDisposed)
                            return;

                        AbstractControl.HoveredControl.RaiseMouseHover(EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Timer.Interval"/> for the click repeat timer.
        /// </summary>
        public static int ClickRepeatInterval
        {
            get
            {
                return clickRepeatInterval;
            }

            set
            {
                if (clickRepeatInterval == value)
                    return;
                clickRepeatInterval = value;
                if(clickRepeatTimer is not null)
                {
                    clickRepeatTimer.Interval = value;
                }
            }
        }

        internal static Timer ClickRepeatTimer
        {
            get
            {
                clickRepeatTimer ??= new Timer(clickRepeatInterval, ClickRepeatTimerTick);
                return clickRepeatTimer;
            }
        }

        /// <summary>
        /// Checks whether <see cref="AbstractControl.LastClickedTimestamp"/> is less than
        /// <see cref="ClickRepeatInterval"/>.
        /// </summary>
        /// <param name="control">Control to use.</param>
        /// <returns></returns>
        public static bool LastClickLessThanRepeatInterval(AbstractControl control)
        {
            if(control.LastClickedTimestamp is null)
                return false;

            var distance = DateUtils.GetAbsDistanceWithNow(control.LastClickedTimestamp.Value);
            var msec = DateUtils.TicksToMilliseconds(distance);
            if (msec < TimerUtils.ClickRepeatInterval)
                return true;
            return false;
        }

        /// <summary>
        /// Executes the specified action only once, ensuring it is not run again if already executed.
        /// </summary>
        /// <remarks>If <paramref name="useTimer"/> is set to <see langword="true"/>, the action is
        /// invoked on the timer's tick event. This can be useful to defer execution until after the
        /// current event loop iteration. If <paramref name="useTimer"/> is <see langword="false"/>, the action is
        /// executed immediately. This method is not thread-safe; concurrent calls may result in the action being
        /// executed more than once.</remarks>
        /// <param name="alreadyExecuted">A reference to a Boolean value indicating whether
        /// the action has already been executed. Set to <see
        /// langword="true"/> after the action is run.</param>
        /// <param name="action">The action to execute if it has not already been executed. Cannot be null.</param>
        /// <param name="useTimer">true to execute the action asynchronously using a timer; otherwise,
        /// false to execute the action immediately.</param>
        public static void RunOnce(ref bool alreadyExecuted, Action action, bool useTimer = true)
        {
            if (alreadyExecuted)
                return;

            alreadyExecuted = true;

            if (useTimer)
            {
                var t = new Timer { Interval = 1 };
                t.Tick += (s, ea) =>
                {
                    t.Stop();
                    t.Dispose();
                    action();
                };
                t.Start();
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Runs the specified action using a timer with a 1 millisecond interval.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        public static void RunUsingTimer(Action action)
        {
            var t = new Timer { Interval = 1 };
            t.Tick += (s, ea) =>
            {
                t.Stop();
                t.Dispose();
                action();
            };
            t.Start();
        }

        /// <summary>
        /// Logs execution time of the specified action.
        /// </summary>
        /// <param name="action">Action to measure.</param>
        /// <returns></returns>
        /// <param name="varName">Text to log as prefix.</param>
        public static void LogActionRunTime(string varName, Action action)
        {
            App.LogNameValue(varName, GetActionRunTime(action));
        }

        /// <summary>
        /// Measures execution time of the specified action.
        /// </summary>
        /// <param name="action">Action to measure.</param>
        /// <returns></returns>
        public static long GetActionRunTime(Action action)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            action();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            return elapsedMs;
        }

        private static void ClickRepeatTimerTick(object? sender, EventArgs e)
        {
            ClickRepeatedEvent?.Invoke(sender, e);
        }
    }
}
