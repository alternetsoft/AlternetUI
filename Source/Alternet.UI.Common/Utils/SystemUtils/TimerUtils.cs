﻿using System;
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
        /// Gets or sets default tooltip timeout interval (in msec).
        /// </summary>
        // On Windows some multiple of double-click time is used but we currently have constant
        // constant value by default.
        public static int DefaultToolTipTimeout = 5000;

        private static int clickRepeatInterval = 20;
        private static Timer? clickRepeatTimer;

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
            var distance = DateUtils.GetAbsDistanceWithNow(
                control.LastClickedTimestamp ?? DateTime.Now.Ticks);
            if (distance < TimerUtils.ClickRepeatInterval)
                return true;
            return false;
        }

        private static void ClickRepeatTimerTick(object? sender, EventArgs e)
        {
            ClickRepeatedEvent?.Invoke(sender, e);
        }
    }
}
