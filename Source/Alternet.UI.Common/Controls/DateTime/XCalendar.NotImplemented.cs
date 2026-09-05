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
        /// Gets or sets default value for the <see cref="SequentialMonthSelect"/> property.
        /// Default is True.
        /// </summary>
        internal static readonly bool DefaultSequentialMonthSelect = true;

        /// <summary>
        /// Occurs when the user clicked on the week of the year number
        /// (fired only in generic calendar).
        /// </summary>
        internal event EventHandler? WeekNumberClick;

        /// <summary>
        /// Occurs when the user clicked on the week day header (fired only in generic calendar).
        /// </summary>
        internal event EventHandler? DayHeaderClick;

        /// <summary>
        /// Occurs when a day was double clicked in the calendar.
        /// </summary>
        internal event EventHandler? DayDoubleClick;

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
        /// Gets or sets a value indicating whether to use alternative, more compact,
        /// style for the month and year selection controls.
        /// </summary>
        internal virtual bool SequentialMonthSelect
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
        /// Gets or sets the foreground color currently used for holiday highlighting.
        /// </summary>
        [Browsable(false)]
        internal virtual Color? HolidayColorFg
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the background color currently used for holiday highlighting.
        /// </summary>
        [Browsable(false)]
        internal virtual Color? HolidayColorBg
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the foreground color of the header part of the calendar control.
        /// </summary>
        [Browsable(false)]
        internal virtual Color? HeaderColorFg
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the background color of the header part of the calendar control.
        /// </summary>
        [Browsable(false)]
        internal virtual Color? HeaderColorBg
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the foreground highlight color.
        /// </summary>
        [Browsable(false)]
        internal virtual Color? HighlightColorFg
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the background highlight color.
        /// </summary>
        [Browsable(false)]
        internal virtual Color? HighlightColorBg
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
        /// Clears any attributes associated with the given day.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        internal virtual void ResetAttr(int day)
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
        /// Marks the specified day as being a holiday in the current month.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        internal virtual void SetHoliday(int day)
        {
        }

        /// <summary>
        /// Sets values for <see cref="HighlightColorBg"/> and
        /// <see cref="HighlightColorFg"/> properties.
        /// </summary>
        /// <param name="colorFg">New value of the <see cref="HighlightColorFg"/> property.</param>
        /// <param name="colorBg">New value of the <see cref="HighlightColorBg"/> property.</param>
        internal virtual void SetHighlightColors(Color? colorFg, Color? colorBg)
        {
        }

        /// <summary>
        /// Sets values for <see cref="HolidayColorBg"/> and
        /// <see cref="HolidayColorFg"/> properties.
        /// </summary>
        /// <param name="colorFg">New value of the <see cref="HolidayColorFg"/> property.</param>
        /// <param name="colorBg">New value of the <see cref="HolidayColorBg"/> property.</param>
        internal virtual void SetHolidayColors(Color? colorFg, Color? colorBg)
        {
        }

        /// <summary>
        /// Sets values for <see cref="HeaderColorBg"/> and
        /// <see cref="HeaderColorFg"/> properties.
        /// </summary>
        /// <param name="colorFg">New value of the <see cref="HeaderColorFg"/> property.</param>
        /// <param name="colorBg">New value of the <see cref="HeaderColorBg"/> property.</param>
        internal virtual void SetHeaderColors(Color? colorFg, Color? colorBg)
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
        /// Returns the <see cref="IXCalendarDateAttr"/> attributes for the
        /// given day or <c>null</c>.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        internal virtual IXCalendarDateAttr? GetAttr(int day)
        {
            return default;
        }

        /// <summary>
        /// Raises <see cref="SelectionChanged"/> event and calls
        /// <see cref="OnSelectionChanged"/> method
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void RaiseSelectionChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnSelectionChanged(e);
            /*
            SelectionChanged?.Invoke(this, e);
            */
        }

        /// <summary>
        /// Raises <see cref="PageChanged"/> event and calls
        /// <see cref="OnPageChanged"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void RaisePageChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnPageChanged(e);
            PageChanged?.Invoke(this, e);
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
        /// Sets colors used in the control to the light theme.
        /// </summary>
        internal virtual void SetColorThemeToLight()
        {
        }

        /// <summary>
        /// Sets colors used in the control to the auto theme (takes colors from the
        /// system colors).
        /// </summary>
        internal virtual void SetColorThemeToAuto()
        {
        }

        /// <summary>
        /// Raises <see cref="DayHeaderClick"/> event and calls
        /// <see cref="OnDayHeaderClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void RaiseDayHeaderClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnDayHeaderClick(e);
            DayHeaderClick?.Invoke(this, e);
        }

        /// <summary>
        /// Raises <see cref="DayDoubleClick"/> event and calls
        /// <see cref="OnDayDoubleClick"/> method.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        internal void RaiseDayDoubleClick(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnDayDoubleClick(e);
            DayDoubleClick?.Invoke(this, e);
        }

        /// <summary>
        /// Sets colors used in the control to the dark theme.
        /// </summary>
        internal virtual void SetColorThemeToDark()
        {
        }

        /// <summary>
        /// Called when a day was double clicked in the calendar.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        internal virtual void OnDayDoubleClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the user clicked on the week day header (fired only in generic calendar).
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        internal virtual void OnDayHeaderClick(EventArgs e)
        {
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

        /// <summary>
        /// Called when the selected month (and/or year) changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        internal virtual void OnPageChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the selected date changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        internal virtual void OnSelectionChanged(EventArgs e)
        {
        }
    }
}
