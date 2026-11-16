using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class XCalendar
    {
        /// <summary>
        /// Gets or sets default value for the <see cref="ShowWeekNumbers"/> property.
        /// Default is False.
        /// </summary>
        internal static bool DefaultShowWeekNumbers = false;

        /// <summary>
        /// Occurs when the user clicked on the week of the year number
        /// (fired only in generic calendar).
        /// </summary>
        internal event EventHandler? WeekNumberClick;

        /// <summary>
        /// Possible return values from <see cref="HitTest"/>.
        /// </summary>
        internal enum HitTestResult
        {
            /// <summary>
            /// Hit is outside of anything.
            /// </summary>
            None,

            /// <summary>
            /// Hit is on the header (weekdays).
            /// </summary>
            Header,

            /// <summary>
            /// Hit is on a day in the calendar.
            /// </summary>
            Day,

            /// <summary>
            /// Hit is on the next month arrow (in alternate month selector mode).
            /// </summary>
            IncMonth,

            /// <summary>
            /// Hit is on the previous month arrow (in alternate month selector mode).
            /// </summary>
            DecMonth,

            /// <summary>
            /// Hit is on the surrounding week of previous/next month (if shown).
            /// </summary>
            SurroundingWeek,

            /// <summary>
            /// Hit is on the week of the year number (if shown).
            /// </summary>
            Week,
        }

        /// <summary>
        /// Gets or sets the <see cref="IXCalendarDateAttr"/> attributes for the marked days.
        /// </summary>
        [Browsable(false)]
        internal virtual IXCalendarDateAttr? MarkDateAttr
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show week numbers on the left side
        /// of the calendar.
        /// </summary>
        internal virtual bool ShowWeekNumbers
        {
            get;

            set;
        }

        /// <summary>
        /// Marks or unmarks the day.
        /// </summary>
        /// <remarks>
        /// This day of month will be marked in every month. Usually marked days
        /// are painted in bold font.
        /// </remarks>
        /// <param name="day">Day (in the range 1...31).</param>
        /// <param name="mark"><c>true</c> to mark the day, <c>false</c> to unmark it.</param>
        internal virtual void Mark(int day, bool mark = true)
        {
        }

        /// <summary>
        /// Marks or unmarks all days in the month.
        /// </summary>
        /// <remarks>
        /// Days will be marked in every month. Usually marked days
        /// are painted in bold font.
        /// </remarks>
        /// <param name="mark"><c>true</c> to mark the days, <c>false</c> to unmark them.</param>
        internal virtual void MarkAll(bool mark = true)
        {
        }

        /// <summary>
        /// Returns one of <see cref="HitTestResult"/> constants.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <returns></returns>
        internal virtual HitTestResult HitTest(PointD point)
        {
            return HitTestResult.None;
        }

        /// <summary>
        /// Raises <see cref="WeekNumberClick"/> event and calls
        /// <see cref="OnWeekNumberClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void RaiseWeekNumberClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnWeekNumberClick(e);
            WeekNumberClick?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the user clicked on the week of the year number
        /// (fired only in generic calendar).
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        internal virtual void OnWeekNumberClick(EventArgs e)
        {
        }
    }
}
