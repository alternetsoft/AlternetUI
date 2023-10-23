using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// The calendar control allows the user to pick a date.
    /// </summary>
    /// <remarks>
    /// The user can move the current selection using the keyboard and select the date
    /// by pressing "Return" or double clicking it.
    /// </remarks>
    /// Generic calendar has advanced possibilities for the customization of its display,
    /// described below.If you want to use these possibilities on every platform,
    /// set UseGeneric property.
    /// <remarks>
    /// All global settings (such as colours and fonts used) can, of course, be changed.
    /// But also, the display style for each day in the month can be set independently.
    /// </remarks>
    /// <remarks>
    /// An item without custom attributes is drawn with the default colors and font and without
    /// border, but setting custom attributes allows modifying its appearance.
    /// Just create a custom attribute object and set it for the day you want to be
    /// displayed specially.
    /// </remarks>
    /// <remarks>
    /// A day may be marked as being a holiday using the SetHoliday() method.
    /// </remarks>
    /// <remarks>
    /// As the attributes are specified for each day, they may change when the month is changed,
    /// so you will often want to update them in PageChanged event handler.
    /// </remarks>
    /// <remarks>
    /// If <see cref="FirstDayOfWeek"/> is <c>null</c> (default),
    /// the first day of the week is determined from operating system's settings,
    /// if possible.The native Linux calendar chooses the first weekday based on
    /// locale, and these styles have no effect on it.
    /// </remarks>
    public class Calendar : Control
    {
        /// <summary>
        /// Occurs when the selected date changed.
        /// </summary>
        public event EventHandler? SelectionChanged;

        /// <summary>
        /// Occurs when the selected month (and/or year) changed.
        /// </summary>
        public event EventHandler? PageChanged;

        /// <summary>
        /// Occurs when the user clicked on the week of the year number
        /// (fired only in generic calendar).
        /// </summary>
        public event EventHandler? WeekNumberClick;

        /// <summary>
        /// Occurs when the user clicked on the week day header (fired only in generic calendar).
        /// </summary>
        public event EventHandler? DayHeaderClick;

        /// <summary>
        /// Occurs when a day was double clicked in the calendar.
        /// </summary>
        public event EventHandler? DayDoubleClick;

        /// <summary>
        /// Gets or sets a value indicating whether to highlight holidays in the calendar
        /// </summary>
        /// <remarks>
        /// This feature is implemented only in generic calendar. In order to use
        /// generic calendar, set <see cref="UseGeneric"/> property.
        /// </remarks>
        public bool ShowHolidays
        {
            get
            {
                return NativeControl.ShowHolidays;
            }

            set
            {
                NativeControl.ShowHolidays = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the month
        /// (and, implicitly, the year) changing. Not implemented on all platforms.
        /// </summary>
        public bool NoMonthChange
        {
            get
            {
                return NativeControl.NoMonthChange;
            }

            set
            {
                NativeControl.NoMonthChange = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use alternative, more compact,
        /// style for the month and year selection controls.
        /// </summary>
        /// <remarks>
        /// This feature is implemented only in generic calendar. In order to use
        /// generic calendar, set <see cref="UseGeneric"/> property.
        /// </remarks>
        public bool SequentalMonthSelect
        {
            get
            {
                return NativeControl.SequentalMonthSelect;
            }

            set
            {
                NativeControl.SequentalMonthSelect = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the neighbouring weeks in the
        /// previous and next months. (only generic, always on for the native controls)
        /// </summary>
        /// <remarks>
        /// This feature is implemented only in generic calendar. In order to use
        /// generic calendar, set <see cref="UseGeneric"/> property.
        /// </remarks>
        /// <remarks>
        /// In the native calendar neighbouring weeks for the
        /// previous and next months are always shown.
        /// </remarks>
        public bool ShowSurroundWeeks
        {
            get
            {
                return NativeControl.ShowSurroundWeeks;
            }

            set
            {
                NativeControl.ShowSurroundWeeks = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show week numbers on the left side
        /// of the calendar.
        /// </summary>
        /// <remarks>
        /// This feature is implemented only in the native calendar. In order to use
        /// native calendar, set <see cref="UseGeneric"/> property to <c>false</c>.
        /// </remarks>
        public bool ShowWeekNumbers
        {
            get
            {
                return NativeControl.ShowWeekNumbers;
            }

            set
            {
                NativeControl.ShowWeekNumbers = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use generic calendar or native calendar.
        /// </summary>
        public bool UseGeneric
        {
            get
            {
                return NativeControl.UseGeneric;
            }

            set
            {
                NativeControl.UseGeneric = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                return NativeControl.HasBorder;
            }

            set
            {
                NativeControl.HasBorder = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the day that is considered the beginning of the week.
        /// </summary>
        /// <returns>A <see cref="DayOfWeek"/> that represents the beginning of the week.
        /// The default is <c>null</c> that is determined by the operating system.</returns>
        /// <remarks>
        /// <see cref="Calendar"/> supports only <see cref="DayOfWeek.Sunday"/>,
        /// <see cref="DayOfWeek.Monday"/> or <c>null</c> values for this property.
        /// </remarks>
        public DayOfWeek? FirstDayOfWeek
        {
            get
            {
                if (NativeControl.SundayFirst)
                    return DayOfWeek.Sunday;
                if (NativeControl.MondayFirst)
                    return DayOfWeek.Monday;
                return null;
            }

            set
            {
                if (FirstDayOfWeek == value)
                    return;

                if (value == DayOfWeek.Sunday)
                {
                    NativeControl.SundayFirst = true;
                    NativeControl.MondayFirst = false;
                }
                else
                if (value == DayOfWeek.Monday)
                {
                    NativeControl.SundayFirst = false;
                    NativeControl.MondayFirst = true;
                }
                else
                {
                    NativeControl.SundayFirst = false;
                    NativeControl.MondayFirst = false;
                }
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Calendar;

        [Browsable(false)]
        internal new NativeCalendarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeCalendarHandler)base.Handler;
            }
        }

        internal Native.Calendar NativeControl => Handler.NativeControl;

        internal DayOfWeek FirstDayOfWeekUseGlobalization =>
            System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

        public Color HolidayColorFg => NativeControl.GetHolidayColorFg();

        public Color HolidayColorBg => NativeControl.GetHolidayColorBg();

        public Color HeaderColorFg => NativeControl.GetHeaderColorFg();

        public Color HeaderColorBg => NativeControl.GetHeaderColorBg();

        public Color HighlightColorFg => NativeControl.GetHighlightColorFg();

        public Color HighlightColorBg => NativeControl.GetHighlightColorBg();

        public bool AllowMonthChange() => NativeControl.AllowMonthChange();

        public bool EnableMonthChange(bool enable = true) => NativeControl.EnableMonthChange(enable);

        public void Mark(int day, bool mark) => NativeControl.Mark(day, mark);

        public void ResetAttr(int day) => NativeControl.ResetAttr(day);

        public void EnableHolidayDisplay(bool display = true) => NativeControl.EnableHolidayDisplay(display);

        public void SetHoliday(int day) => NativeControl.SetHoliday(day);

        public void SetHighlightColors(Color? colorFg, Color? colorBg)
        {
            colorFg ??= Color.Empty;
            colorBg ??= Color.Empty;
            NativeControl.SetHighlightColors(colorFg.Value, colorBg.Value);
        }

        public void SetHolidayColors(Color? colorFg, Color? colorBg)
        {
            colorFg ??= Color.Empty;
            colorBg ??= Color.Empty;
            NativeControl.SetHolidayColors(colorFg.Value, colorBg.Value);
        }

        public void SetHeaderColors(Color? colorFg, Color? colorBg)
        {
            colorFg ??= Color.Empty;
            colorBg ??= Color.Empty;
            NativeControl.SetHeaderColors(colorFg.Value, colorBg.Value);
        }

        public static ICalendarDateAttr? GetMarkDateAttr()
        {
            var result = Native.Calendar.GetMarkDateAttr();
            if (result == default)
                return null;
            return new CalendarDateAttr(result, false);
        }

        public ICalendarDateAttr? GetAttr(int day)
        {
            var result = NativeControl.GetAttr(day);
            if (result == default)
                return null;
            return new CalendarDateAttr(result, false);
        }

        public void SetAttr(int day, ICalendarDateAttr? dateAttr)
        {
            if (dateAttr is null)
                NativeControl.SetAttr(day, default);
            else
                NativeControl.SetAttr(day, dateAttr.Handle);
        }

        public static void SetMarkDateAttr(ICalendarDateAttr? dateAttr)
        {
            if (dateAttr is null)
                Native.Calendar.SetMarkDateAttr(default);
            else
                Native.Calendar.SetMarkDateAttr(dateAttr.Handle);
        }

        public static ICalendarDateAttr CreateDateAttr(int border = 0)
        {
            var result = Native.Calendar.CreateDateAttr(border);
            return new CalendarDateAttr(result, false);
        }

        internal void RaiseSelectionChanged(EventArgs e)
        {
            OnSelectionChanged(e);
            SelectionChanged?.Invoke(this, e);
        }

        internal void RaisePageChanged(EventArgs e)
        {
            OnPageChanged(e);
            PageChanged?.Invoke(this, e);
        }

        internal void RaiseWeekNumberClick(EventArgs e)
        {
            OnWeekNumberClick(e);
            WeekNumberClick?.Invoke(this, e);
        }

        internal void RaiseDayHeaderClick(EventArgs e)
        {
            OnDayHeaderClick(e);
            DayHeaderClick?.Invoke(this, e);
        }

        internal void RaiseDayDoubleClick(EventArgs e)
        {
            OnDayDoubleClick(e);
            DayDoubleClick?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a day was double clicked in the calendar.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnDayDoubleClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the user clicked on the week day header (fired only in generic calendar).
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnDayHeaderClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the user clicked on the week of the year number
        /// (fired only in generic calendar).
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnWeekNumberClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the selected month (and/or year) changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnPageChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the selected date changed.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnSelectionChanged(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeCalendarHandler();
        }
    }
}