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
    [ControlCategory("Other")]
    public partial class Calendar : CustomDateEdit
    {
        static Calendar()
        {
            Application.BeforeNativeLogMessage += Application_BeforeNativeLogMessage;

            static void Application_BeforeNativeLogMessage(object? sender, LogMessageEventArgs e)
            {
                const string s1 = "'MonthCal_SetDayState'";

                if (e.Message?.Contains(s1) ?? false)
                    e.Cancel = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Calendar"/> class.
        /// </summary>
        public Calendar()
        {
            if (Application.IsWindowsOS)
                UserPaint = true;
            BackgroundColor = SystemColors.Window;
        }

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
        /// Possible return values from <see cref="HitTest"/>.
        /// </summary>
        public enum HitTestResult
        {
            /// <summary>
            /// Hit outside of anything.
            /// </summary>
            None,

            /// <summary>
            /// Hit on the header (weekdays).
            /// </summary>
            Header,

            /// <summary>
            /// Hit on a day in the calendar.
            /// </summary>
            Day,

            /// <summary>
            /// Hit on next month arrow (in alternate month selector mode).
            /// </summary>
            IncMonth,

            /// <summary>
            /// Hit on previous month arrow (in alternate month selector mode).
            /// </summary>
            DecMonth,

            /// <summary>
            /// Hit on surrounding week of previous/next month (if shown).
            /// </summary>
            SurroundingWeek,

            /// <summary>
            /// Hit on week of the year number (if shown).
            /// </summary>
            Week,
        }

        /// <summary>
        /// Gets or sets the <see cref="ICalendarDateAttr"/> attributes for the marked days.
        /// </summary>
        public static ICalendarDateAttr? MarkDateAttr
        {
            get
            {
                var result = Native.Calendar.GetMarkDateAttr();
                if (result == default)
                    return null;
                var resultIntf = new CalendarDateAttr(result, true)
                {
                    Immutable = true,
                };
                return resultIntf;
            }

            set
            {
                if (value is null)
                    Native.Calendar.SetMarkDateAttr(default);
                else
                    Native.Calendar.SetMarkDateAttr(value.Handle);
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// When this property is changed from the code and not by the user, events are not fired.
        /// </remarks>
        public override DateTime Value
        {
            get
            {
                return Handler.NativeControl.Value;
            }

            set
            {
                Handler.NativeControl.Value = value;
            }
        }

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
                return Handler.NativeControl.ShowHolidays;
            }

            set
            {
                Handler.NativeControl.EnableHolidayDisplay(value);
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
                return Handler.NativeControl.NoMonthChange;
            }

            set
            {
                Handler.NativeControl.EnableMonthChange(!value);
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
                return Handler.NativeControl.SequentalMonthSelect;
            }

            set
            {
                Handler.NativeControl.SequentalMonthSelect = value;
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
                return Handler.NativeControl.ShowSurroundWeeks;
            }

            set
            {
                Handler.NativeControl.ShowSurroundWeeks = value;
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
                return Handler.NativeControl.ShowWeekNumbers;
            }

            set
            {
                Handler.NativeControl.ShowWeekNumbers = value;
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
                return Handler.NativeControl.UseGeneric;
            }

            set
            {
                Handler.NativeControl.UseGeneric = value;
                SetRange();
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
                return Handler.NativeControl.HasBorder;
            }

            set
            {
                Handler.NativeControl.HasBorder = value;
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
                if (Handler.NativeControl.SundayFirst)
                    return DayOfWeek.Sunday;
                if (Handler.NativeControl.MondayFirst)
                    return DayOfWeek.Monday;
                return null;
            }

            set
            {
                if (FirstDayOfWeek == value)
                    return;

                if (value == DayOfWeek.Sunday)
                {
                    Handler.NativeControl.SundayFirst = true;
                    Handler.NativeControl.MondayFirst = false;
                }
                else
                if (value == DayOfWeek.Monday)
                {
                    Handler.NativeControl.SundayFirst = false;
                    Handler.NativeControl.MondayFirst = true;
                }
                else
                {
                    Handler.NativeControl.SundayFirst = false;
                    Handler.NativeControl.MondayFirst = false;
                }
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Calendar;

        /// <summary>
        /// Gets or sets the foreground color currently used for holiday highlighting.
        /// </summary>
        /// <remarks>
        /// This property is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and always returns
        /// <see cref="Color.Empty"/> in the native versions.
        /// </remarks>
        [Browsable(false)]
        public Color HolidayColorFg
        {
            get => Handler.NativeControl.GetHolidayColorFg();
            set => SetHolidayColors(value, HolidayColorBg);
        }

        /// <summary>
        /// Gets or sets the background color currently used for holiday highlighting.
        /// </summary>
        /// <remarks>
        /// This property is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and always returns
        /// <see cref="Color.Empty"/> in the native versions.
        /// </remarks>
        [Browsable(false)]
        public Color HolidayColorBg
        {
            get => Handler.NativeControl.GetHolidayColorBg();
            set => SetHolidayColors(HolidayColorFg, value);
        }

        /// <summary>
        /// Gets or sets the foreground color of the header part of the calendar control.
        /// </summary>
        /// <remarks>
        /// This property is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and always returns
        /// <see cref="Color.Empty"/> in the native versions.
        /// </remarks>
        [Browsable(false)]
        public Color HeaderColorFg
        {
            get => Handler.NativeControl.GetHeaderColorFg();
            set => SetHeaderColors(value, HeaderColorBg);
        }

        /// <summary>
        /// Gets or sets the background color of the header part of the calendar control.
        /// </summary>
        /// <remarks>
        /// This property is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and always returns
        /// <see cref="Color.Empty"/> in the native versions.
        /// </remarks>
        [Browsable(false)]
        public Color HeaderColorBg
        {
            get => Handler.NativeControl.GetHeaderColorBg();
            set => SetHeaderColors(HeaderColorFg, value);
        }

        /// <summary>
        /// Gets or sets the foreground highlight color.
        /// </summary>
        /// <remarks>
        /// This property is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and always returns
        /// <see cref="Color.Empty"/> in the native versions.
        /// </remarks>
        [Browsable(false)]
        public Color HighlightColorFg
        {
            get => Handler.NativeControl.GetHighlightColorFg();
            set => SetHighlightColors(value, HighlightColorBg);
        }

        /// <summary>
        /// Gets or sets the background highlight color.
        /// </summary>
        /// <remarks>
        /// This property is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and always returns
        /// <see cref="Color.Empty"/> in the native versions.
        /// </remarks>
        [Browsable(false)]
        public Color HighlightColorBg
        {
            get => Handler.NativeControl.GetHighlightColorBg();
            set => SetHighlightColors(HighlightColorFg, value);
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set => base.BackgroundColor = value;
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override Color? ForegroundColor
        {
            get => base.ForegroundColor;
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override Font? Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        /// <inheritdoc/>
        [Browsable(false)]
        public override bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new CalendarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (CalendarHandler)base.Handler;
            }
        }

        internal DayOfWeek FirstDayOfWeekUseGlobalization =>
            System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

        /// <summary>
        /// Creates <see cref="ICalendarDateAttr"/> instance.
        /// </summary>
        /// <param name="border">Date border settings.</param>
        public static ICalendarDateAttr CreateDateAttr(int border = 0)
        {
            var result = Native.Calendar.CreateDateAttr(border);
            return new CalendarDateAttr(result, true);
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
        public virtual void Mark(int day, bool mark = true) => Handler.NativeControl.Mark(day, mark);

        /// <summary>
        /// Clears any attributes associated with the given day.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        /// <remarks>
        /// This method is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and does nothing in the native version.
        /// </remarks>
        public virtual void ResetAttr(int day) => Handler.NativeControl.ResetAttr(day);

        /// <summary>
        /// Marks the specified day as being a holiday in the current month.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        /// <remarks>
        /// This method is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and does nothing in the native version.
        /// </remarks>
        public virtual void SetHoliday(int day) => Handler.NativeControl.SetHoliday(day);

        /// <summary>
        /// Sets values for <see cref="HighlightColorBg"/> and
        /// <see cref="HighlightColorFg"/> properties.
        /// </summary>
        /// <param name="colorFg">New value of the <see cref="HighlightColorFg"/> property.</param>
        /// <param name="colorBg">New value of the <see cref="HighlightColorBg"/> property.</param>
        /// <remarks>
        /// This method is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and does nothing in the native version.
        /// </remarks>
        public virtual void SetHighlightColors(Color colorFg, Color colorBg)
        {
            Handler.NativeControl.SetHighlightColors(colorFg, colorBg);
        }

        /// <summary>
        /// Sets values for <see cref="HolidayColorBg"/> and
        /// <see cref="HolidayColorFg"/> properties.
        /// </summary>
        /// <param name="colorFg">New value of the <see cref="HolidayColorFg"/> property.</param>
        /// <param name="colorBg">New value of the <see cref="HolidayColorBg"/> property.</param>
        /// <remarks>
        /// This method is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and does nothing in the native version.
        /// </remarks>
        public virtual void SetHolidayColors(Color colorFg, Color colorBg)
        {
            Handler.NativeControl.SetHolidayColors(colorFg, colorBg);
        }

        /// <summary>
        /// Sets values for <see cref="HeaderColorBg"/> and
        /// <see cref="HeaderColorFg"/> properties.
        /// </summary>
        /// <param name="colorFg">New value of the <see cref="HeaderColorFg"/> property.</param>
        /// <param name="colorBg">New value of the <see cref="HeaderColorBg"/> property.</param>
        /// <remarks>
        /// This method is only implemented in generic calendar (when
        /// <see cref="UseGeneric"/> is <c>true</c>) and does nothing in the native version.
        /// </remarks>
        public virtual void SetHeaderColors(Color colorFg, Color colorBg)
        {
            Handler.NativeControl.SetHeaderColors(colorFg, colorBg);
        }

        /// <summary>
        /// Changes <see cref="Value"/> property to the today date.
        /// </summary>
        public virtual void SelectToday()
        {
            Value = DateTime.Now.Date;
        }

        /// <summary>
        /// Returns one of <see cref="HitTestResult"/> constants.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <returns></returns>
        /// <remarks>
        /// Not implemented on Linux currently.
        /// </remarks>
        public HitTestResult HitTest(PointD point)
        {
            if (Application.IsLinuxOS)
                return HitTestResult.None;

            var pointi = PixelFromDip(point);
            var result = Handler.NativeControl.HitTest(pointi);
            return (HitTestResult)result;
        }

        /// <summary>
        /// Returns the <see cref="ICalendarDateAttr"/> attributes for the
        /// given day or <c>null</c>.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        public virtual ICalendarDateAttr? GetAttr(int day)
        {
            var result = Handler.NativeControl.GetAttr(day);
            if (result == default)
                return null;
            return new CalendarDateAttr(result, false);
        }

        /// <summary>
        /// Sets the <see cref="ICalendarDateAttr"/> attributes for the given day.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        /// <param name="dateAttr">Day attributes. Pass <c>null</c> to reset attributes.</param>
        public virtual void SetAttr(int day, ICalendarDateAttr? dateAttr)
        {
            if (dateAttr is null)
                Handler.NativeControl.ResetAttr(day);
            else
                Handler.NativeControl.SetAttr(day, dateAttr.Handle);
        }

        internal bool AllowMonthChange() => Handler.NativeControl.AllowMonthChange();

        internal bool EnableMonthChange(bool enable = true) =>
            Handler.NativeControl.EnableMonthChange(enable);

        internal void EnableHolidayDisplay(bool display = true) =>
            Handler.NativeControl.EnableHolidayDisplay(display);

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

        /// <inheritdoc/>
        protected override BaseControlHandler CreateHandler()
        {
            return new CalendarHandler();
        }

        /// <inheritdoc/>
        protected override void SetRange(DateTime min, DateTime max)
        {
            Handler.NativeControl.MinValue = min;
            Handler.NativeControl.MaxValue = max;
            Handler.NativeControl.SetRange(UseMinDate, UseMaxDate);
            Refresh();
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
    }
}