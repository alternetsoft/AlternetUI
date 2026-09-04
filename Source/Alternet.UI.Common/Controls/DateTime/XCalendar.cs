using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a calendar control that allows users to select a date from a visual calendar interface.
    /// </summary>
    public partial class XCalendar : ScrollViewer
    {
        /// <summary>
        /// Gets the total number of day cells in the calendar control, which is 42 (6 rows x 7 columns).
        /// </summary>
        public const int DayCellCount = 42;

        /// <summary>
        /// Gets the number of rows in the calendar control, which is 6,
        /// representing the maximum number of weeks that can be displayed in a month view.
        /// </summary>
        public const int DayRowCount = 6;

        /// <summary>
        /// Gets the number of columns in the calendar control, which is 7, 
        /// representing the days of the week.
        /// </summary>
        public const int ColumnCount = 7;

        /// <summary>
        /// Gets or sets the default <see cref="ICalendarDateAttr"/> attributes for holidays in the calendar control.
        /// You can use <see cref="PlessCalendarDateAttr"/>
        /// in order to create a new instance of <see cref="ICalendarDateAttr"/> with the desired attributes.
        /// </summary>
        public static ICalendarDateAttr? DefaultHolidayAttr;

        /// <summary>
        /// Gets the text used for measuring the height of a single row in the calendar control.
        /// </summary>
        public static readonly string RowHeightMeasureText = "Wg00";

        /// <summary>
        /// Gets the text used for measuring the width of a single day column in the calendar control.
        /// </summary>
        public static readonly string DayWidthMeasureText = "00";

        /// <summary>
        /// Gets or sets a value indicating whether the year dropdown in the calendar header should be displayed by default
        /// when the year picker is clicked, allowing users to select a specific year.
        /// </summary>
        public bool DefaultShowYearDropDown = true;

        /// <summary>
        /// Gets or sets a value indicating whether the month dropdown in the calendar header should be displayed by default
        /// when the month picker is clicked, allowing users to select a specific month.
        /// </summary>
        public bool DefaultShowMonthDropDown = true;

        /// <summary>
        /// Gets or sets the default color used for surrounding days (days not in the current month) in the calendar control,
        /// </summary>
        public static LightDarkColor DefaultSurroundDayColor = new(Color.Gray);

        /// <summary>
        /// Gets or sets the default kind of day names to be used in the calendar control,
        /// </summary>
        public static DayNamesKind DefaultDayNamesKind = UI.DayNamesKind.Abbreviated;

        /// <summary>
        /// Gets or sets the default border settings used to highlight today's date in the calendar control,
        /// </summary>
        public static BorderSettings? DefaultTodayBorder;

        private readonly CalendarCell[] cells = new CalendarCell[DayCellCount];
        private readonly CalendarListBox listBox;
        private readonly CalendarHeader header;
        private readonly CalendarHeaderItem headerItem;
        private readonly YearPicker popupYearPicker;
        private readonly TransparentPanel popupYearPickerPanel;
        private readonly MonthPickerPanel popupMonthPicker;
        private readonly CalendarContainer container;

        private RestrictedDate restrictedDate;
        private DateOnly date;
        private int suspendHeaderEventsCounter;
        private LightDarkColor? surroundDayColor;
        private BorderSettings? todayBorder;
        private DayNamesKind? dayNamesKind;
        private DayOfWeek? firstDayOfWeek;
        private ICalendarDateAttr? holidayAttr;

        static XCalendar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCalendar"/> class.
        /// </summary>
        public XCalendar()
        {
            SetScrollBarVisible(isVert: true, isVisible: false);
            SetScrollBarVisible(isVert: false, isVisible: false);

            restrictedDate = new(
                () => date,
                v => date = v,
                (min, max) =>
                {
                });

            popupYearPicker = new();
            popupYearPickerPanel = new();
            listBox = new();
            header = new();
            popupMonthPicker = new();
            container = new();

            container.Layout = LayoutStyle.Vertical;

            popupMonthPicker.Visible = false;

            popupYearPickerPanel.Visible = false;
            popupYearPicker.Parent = popupYearPickerPanel;

            CreateColumns();
            headerItem = new();
            headerItem.HideSelection = true;

            for (var col = 0; col < XCalendar.ColumnCount; col++)
            {
                var cellItem = headerItem.AddCell<CalendarHeaderCellItem>(listBox.Columns[col]);
                cellItem.HorizontalAlignment = HorizontalAlignment.Center;
            }

            for (int i = 0; i < DayCellCount; i++)
            {
                cells[i] = new();
            }

            for (int row = 0; row < DayRowCount; row++)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    var cell = GetCell(row, col);
                    cell.RowIndex = row;
                    cell.ColumnIndex = col;
                }
            }

            restrictedDate.Value = DateTime.Now.Date.ToDateOnly();

            popupYearPickerPanel.MarginBottom = 5;
            header.MarginBottom = 5;

            ParentBackColor = false;
            BackColor = listBox.BackColor;

            listBox.VertGridLines = false;
            listBox.HasBorder = false;
            listBox.HorzGridLines = false;
            listBox.VerticalAlignment = VerticalAlignment.Fill;

            listBox.KeyDown += OnListBoxKeyDown;

            CreateDayItems();
            UpdateDayNames();
            OnValueChanged();

            header.ValueChanged += OnHeaderValueChanged;

            listBox.BackColorChanged += OnListBoxBackColorChanged;
            listBox.CellClick += OnListBoxCellClick;

            header.Parent = container;
            popupYearPickerPanel.Parent = container;
            popupMonthPicker.Parent = container;
            listBox.Parent = container;

            container.Parent = this.Content;

            ShowMonthDropDown = DefaultShowMonthDropDown;
            ShowYearDropDown = DefaultShowYearDropDown;

            popupYearPickerPanel.VisibleChanged += OnPopupYearPickerVisibleChanged;
            popupMonthPicker.VisibleChanged += OnPopupMonthPickerVisibleChanged;

            popupYearPicker.TextPicker.EnterPressed += OnPopupYearPickerEnterPressed;
            popupYearPicker.TextPicker.EscapePressed += OnPopupYearPickerEscapePressed;
            popupYearPicker.ValueChanged += OnPopupYearPickerValueChanged;
            HeaderYearClick += OnHeaderYearClick;
            HeaderMonthClick += OnHeaderMonthClick;

            popupMonthPicker.MonthClick += (s, e) =>
            {
                popupMonthPicker.Visible = false;
                Value = new DateOnly(Value.Year, (int)e.Value, Value.Day);
            };

            UpdateListBoxSize();
        }

        /// <summary>
        /// Called when the month picker in the calendar header is clicked, toggling the visibility of the month dropdown panel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnHeaderMonthClick(object? sender, EventArgs e)
        {
            if (!ShowMonthDropDown)
                return;
            popupMonthPicker.Visible = !popupMonthPicker.Visible;
        }

        /// <summary>
        /// Occurs when the year picker in the calendar header is clicked, 
        /// allowing subscribers to handle the event and perform actions based on the year selection.
        /// </summary>
        public event EventHandler? HeaderYearClick
        {
            add
            {
                header.YearPicker.Click += value;
            }

            remove
            {
                header.YearPicker.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the month picker in the calendar header is clicked,
        /// allowing subscribers to handle the event and perform actions based on the month selection.
        /// </summary>
        public event EventHandler? HeaderMonthClick
        {
            add
            {
                header.MonthPicker.Click += value;
            }

            remove
            {
                header.MonthPicker.Click -= value;
            }
        }

        /// <summary>
        /// Occurs when the selected date changed. This is the same as the <see cref="ValueChanged"/> event
        /// and is provided for compatibility with other calendar controls that use the SelectionChanged event.
        /// Control is invalidated automatically after this event handler executes.
        /// </summary>
        public event EventHandler? SelectionChanged
        {
            add
            {
                ValueChanged += value;
            }

            remove
            {
                ValueChanged -= value;
            }
        }

        /// <summary>
        /// Occurs when the selected month (and/or year) changed.
        /// In the event handler, you can assign day attributes for the new month
        /// as they are cleared each time the month or year changes.
        /// You can set attributes without invalidating the control as it is invalidated
        /// automatically after this event handler executes.
        /// </summary>
        public event EventHandler? PageChanged;

        /// <summary>
        /// Occurs when the value of the calendar control changes,
        /// allowing subscribers to handle the event and perform actions based on the new value.
        /// Control is invalidated automatically after this event handler executes.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs when the header of the calendar control is clicked, 
        /// allowing subscribers to handle the event and perform actions based on the header click.
        /// </summary>
        public event EventHandler<HeaderClickEventArgs>? HeaderClick;

        /// <summary>
        /// Occurs when a day cell in the calendar control is clicked,
        /// allowing subscribers to handle the event and perform actions based on the selected date.
        /// You can cancel the event by setting the <see cref="CancelEventArgs.Cancel"/> property to true,
        /// in this case the new day will not be selected and the <see cref="Value"/> property will not be changed.
        /// </summary>
        public event EventHandler<DayClickEventArgs>? DayClick;

        /// <summary>
        /// Gets the inner list box used in the calendar control,
        /// which displays the days of the month in a grid format.
        /// </summary>
        public CalendarListBox ListBox => listBox;

        /// <summary>
        /// Gets the popup panel that contains the year picker.
        /// </summary>
        public TransparentPanel PopupYearPickerPanel => popupYearPickerPanel;

        /// <summary>
        /// Gets the year picker used in the popup panel.
        /// </summary>
        public YearPicker PopupYearPicker => popupYearPicker;

        /// <summary>
        /// Gets or sets a value indicating whether the month dropdown in the calendar header should be displayed,
        /// when the month picker is clicked, allowing users to select a specific month.
        /// </summary>
        public virtual bool ShowMonthDropDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the year dropdown in the calendar header should be displayed
        /// when the year picker is clicked, allowing users to select a specific year.
        /// </summary>
        public virtual bool ShowYearDropDown { get; set; }

        /// <summary>
        /// Gets the collection of custom attributes for specific dates in the calendar control,
        /// allowing you to customize the appearance and behavior of individual dates.
        /// </summary>
        [Browsable(false)]
        public virtual CalendarDateAttributes DateAttributes { get; } = new CalendarDateAttributes();

        /// <summary>
        /// Gets or sets the <see cref="ICalendarDateAttr"/> attributes for holidays in the calendar control,
        /// If not set, the default holiday attributes will be used. You can use <see cref="CreateDateAttr"/>
        /// in order to create a new instance of <see cref="ICalendarDateAttr"/> with the desired attributes.
        /// </summary>
        [Browsable(false)]
        public virtual ICalendarDateAttr? HolidayAttr
        {
            get => holidayAttr;

            set
            {
                if (value == holidayAttr)
                    return;
                holidayAttr = value;
                Invalidate();
            }
        }

        /// <summary>Gets or sets the minimum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The minimum date and time that can be selected in the
        /// control. The default is <see cref="DateUtils.MinDateTime"/>.
        /// </returns>
        public virtual DateOnly MinDate
        {
            get
            {
                return restrictedDate.MinDate;
            }

            set
            {
                if (restrictedDate.MinDate == value)
                    return;
                restrictedDate.MinDate = value;
                OnValueChanged();
                Invalidate();
            }
        }

        /// <summary>Gets or sets the maximum date and time that can be
        /// selected in the control.</summary>
        /// <returns>The maximum date and time that can be selected
        /// in the control. The default is determined as the minimum of the
        /// CurrentCulture's Calendar's
        /// <see cref="System.Globalization.Calendar.MaxSupportedDateTime" />
        /// property and <see cref="DateUtils.MaxDateTime"/>.</returns>
        public virtual DateOnly MaxDate
        {
            get
            {
                return restrictedDate.MaxDate;
            }

            set
            {
                if (restrictedDate.MaxDate == value)
                    return;
                restrictedDate.MaxDate = value;
                OnValueChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="MinDate"/> for the date range limitation.
        /// </summary>
        public virtual bool UseMinDate
        {
            get
            {
                return restrictedDate.UseMinDate;
            }

            set
            {
                if (restrictedDate.UseMinDate == value)
                    return;
                restrictedDate.UseMinDate = value;
                OnValueChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="MaxDate"/> and
        /// <see cref="MinDate"/> for the date range limitation.
        /// </summary>
        [Browsable(false)]
        public virtual bool UseMinMaxDate
        {
            get
            {
                return UseMinDate && UseMaxDate;
            }

            set
            {
                if (UseMinDate == value && UseMaxDate == value)
                    return;
                UseMinDate = value;
                UseMaxDate = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to use <see cref="MaxDate"/> for the date range limitation.
        /// </summary>
        public virtual bool UseMaxDate
        {
            get
            {
                return restrictedDate.UseMaxDate;
            }

            set
            {
                if (restrictedDate.UseMaxDate == value)
                    return;
                restrictedDate.UseMaxDate = value;
                OnValueChanged();
                Invalidate();
            }
        }


        /// <summary>
        /// Gets or sets the current value of the calendar control as a <see cref="DateTime"/> object,
        /// </summary>
        [Browsable(false)]
        public DateTime AsDateTime
        {
            get => Value.ToDateTime();
            set => Value = DateOnly.FromDateTime(value);
        }

        /// <summary>
        /// Gets or sets the currently selected date in the calendar control,
        /// allowing users to select a specific date from the calendar interface.
        /// </summary>
        public virtual DateOnly Value
        {
            get
            {
                return date;
            }

            set
            {
                if (Value == value)
                    return;

                var oldValue = restrictedDate.Value;

                restrictedDate.Value = value;

                var newValue = restrictedDate.Value;

                suspendHeaderEventsCounter++;
                try
                {
                    header.Value = newValue;
                    popupYearPicker.Value = newValue.Year;
                }
                finally
                {
                    suspendHeaderEventsCounter--;
                }

                var pageChanged = oldValue.Year != newValue.Year || oldValue.Month != newValue.Month;

                if(pageChanged)
                {
                    ResetAttrAll(invalidate: false);
                }

                OnValueChanged();

                ValueChanged?.Invoke(this, EventArgs.Empty);

                if (pageChanged)
                {
                    PageChanged?.Invoke(this, EventArgs.Empty);
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets the first date of the currently displayed month in the calendar.
        /// </summary>
        [Browsable(false)]
        public DateOnly FirstDateOfMonth
        {
            get
            {
                return DateUtils.GetFirstDateOfMonth(Value);
            }
        }

        /// <summary>
        /// Gets the last date of the currently displayed month in the calendar.
        /// </summary>
        [Browsable(false)]
        public DateOnly LastDateOfMonth
        {
            get
            {
                return DateUtils.GetLastDateOfMonth(Value);
            }
        }

        /// <summary>
        /// Gets or sets the color used for surrounding days (days not in the current month) in the calendar control,
        /// </summary>
        [Browsable(false)]
        public virtual LightDarkColor? SurroundDayColor
        {
            get => surroundDayColor;
            set
            {
                if (surroundDayColor == value)
                    return;
                surroundDayColor = value;
                OnValueChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border settings used to highlight today's date in the calendar control,
        /// allowing customization of the appearance of the current date.
        /// </summary>
        public virtual BorderSettings? TodayBorder
        {
            get => todayBorder;
            set
            {
                if (todayBorder == value)
                    return;
                todayBorder = value;
                OnValueChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the kind of day names to be used in the calendar control, allowing customization of the day name format.
        /// </summary>
        public virtual DayNamesKind? DayNamesKind
        {
            get => dayNamesKind;
            set
            {
                if (dayNamesKind == value)
                    return;
                dayNamesKind = value;
                PerformLayoutAndInvalidate(() =>
                {
                    UpdateDayNames();
                    UpdateColumnWidth();
                    UpdateListBoxSize();
                    OnValueChanged();
                });
            }
        }

        /// <summary>
        /// Gets or sets the format provider used for formatting day names in the calendar control.
        /// </summary>
        public virtual IFormatProvider? FormatProvider
        {
            get => restrictedDate.FormatProvider;
            set
            {
                if (restrictedDate.FormatProvider == value) return;
                restrictedDate.FormatProvider = value;
                header.FormatProvider = value;
                UpdateDayNames();
                UpdateColumnWidth();
                PerformLayoutAndInvalidate(() =>
                {
                    UpdateListBoxSize();
                });
            }
        }

        /// <summary>
        /// Gets or sets the first day of the week to be used in the calendar control.
        /// </summary>
        public virtual DayOfWeek? FirstDayOfWeek
        {
            get => firstDayOfWeek;
            set
            {
                if (firstDayOfWeek == value) return;
                firstDayOfWeek = value;
                UpdateDayNames();
                UpdateDayItems(true);
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets the width of a single column in the calendar control based
        /// on the measured width of the day names and other properties.
        /// </summary>
        /// <param name="dc">The graphics context used for measuring text.</param>
        /// <param name="font">The font used for measuring text.</param>
        /// <returns>The width of a single column in the calendar control.</returns>
        public virtual float GetColumnWidth(Graphics dc, Font font)
        {
            var dayWidth = dc.GetTextExtent(DayWidthMeasureText, font).Width;
            var dayNames = GetDayNames();

            foreach (var day in dayNames)
            {
                var dayNameWidth = dc.GetTextExtent(day.Text, font).Width;
                if (dayNameWidth > dayWidth)
                    dayWidth = dayNameWidth;
            }

            return dayWidth + 4;
        }

        /// <summary>
        /// Marks all weekends as holidays. This method updates attributes only for the weekends of the currently
        /// displayed month in the calendar control, attributes for other days are not affected. 
        /// After current month or year is changed,
        /// this method should be called again to mark weekends in the new month.
        /// </summary>
        public virtual void MarkWeekendsAsHolidays(bool invalidate = true)
        {
            var weekEnds = DateUtils.GetWeekendsOfMonth(Value);

            var needInvalidate = false;

            foreach (var date in weekEnds)
            {
                needInvalidate |= SetHoliday(date.Day, invalidate: false);
            }

            if (invalidate && needInvalidate)
                Invalidate();    
        }

        /// <summary>
        /// Marks all days that match the given <see cref="RepeatPatternRule"/> with
        /// the given <see cref="ICalendarDateAttr"/> attributes. After current month or year is changed,
        /// this method should be called again to mark dates in the new month.
        /// </summary>
        /// <param name="rule">The repeat pattern rule to match dates.</param>
        /// <param name="attr">The attributes to apply to the matching dates. Pass <c>null</c> to reset attributes.</param>
        /// <param name="invalidate">Indicates whether to invalidate the control after setting the attributes.</param>
        public virtual void MarkWithRule(RepeatPatternRule rule, ICalendarDateAttr? attr, bool invalidate = true)
        {
            IDateRepeatPatternRule.RuleGetDatesParams prm = new();
            prm.MinDate = FirstDateOfMonth;
            prm.MaxDate = LastDateOfMonth;

            var result = rule.GetDates(prm).Dates;

            var needInvalidate = false;

            foreach (var date in result)
            {
                needInvalidate |= SetAttr(date.Day, attr, invalidate: false);
            }

            if (invalidate && needInvalidate)
                Invalidate();
        }

        /// <summary>
        /// Marks the specified day as being a holiday in the current month.
        /// After current month or year is changed,
        /// this method should be called again to mark dates in the new month.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        /// <param name="invalidate">Indicates whether to invalidate the control after setting the holiday attribute.</param>
        public virtual bool SetHoliday(int day, bool invalidate = true)
        {
            return SetAttr(day, HolidayAttr ?? DefaultHolidayAttr ?? DateAttributes.Red, invalidate);
        }

        /// <summary>
        /// Sets the <see cref="ICalendarDateAttr"/> attributes for the given date.
        /// This method can be used to customize the appearance and behavior of current month days.
        /// If the date is not in the current month, the method will return false and no attributes will be set.
        /// After month or year is changed, this method should be called again to set attributes for the new month.
        /// </summary>
        /// <param name="date">The date for which to set the attributes.</param>
        /// <param name="dateAttr">The attributes to set for the date. Pass <c>null</c> to reset attributes.</param>
        /// <param name="invalidate">Indicates whether to invalidate the control after setting the attributes.</param>
        /// <returns><c>true</c> if the attributes were successfully set; otherwise, <c>false</c>.</returns>
        public virtual bool SetAttr(DateOnly date, ICalendarDateAttr? dateAttr, bool invalidate = true)
        {
            foreach (var cell in cells)
            {
                if (cell.Date == date)
                {
                    cell.DateAttr = dateAttr;
                    if (invalidate)
                        Invalidate();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the <see cref="ICalendarDateAttr"/> attributes for the given day.
        /// This method can be used to customize the appearance and behavior of current month days.
        /// If the date is not in the current month, the method will return false and no attributes will be set.
        /// After month or year is changed, this method should be called again to set attributes for the new month.
        /// </summary>
        /// <param name="day">Day (in the range 1...31).</param>
        /// <param name="dateAttr">Day attributes. Pass <c>null</c> to reset attributes.</param>
        /// <param name="invalidate">Indicates whether to invalidate the control after setting the attributes.</param>
        /// <remarks>
        /// After current page is changed, this method should be called again to set attributes for the new month.
        /// </remarks>
        public virtual bool SetAttr(int day, ICalendarDateAttr? dateAttr, bool invalidate = true)
        {
            if (day < 1 || day > DateTime.DaysInMonth(Value.Year, Value.Month))
                return false;
            var dateToSet = new DateOnly(Value.Year, Value.Month, day);
            return SetAttr(dateToSet, dateAttr, invalidate);
        }

        /// <summary>
        /// Creates <see cref="ICalendarDateAttr"/> instance.
        /// </summary>
        /// <param name="border">Date border settings.</param>
        public virtual ICalendarDateAttr CreateDateAttr(CalendarDateBorder border = 0)
        {
            return new PlessCalendarDateAttr(border);
        }

        /// <summary>
        /// Gets the <see cref="CalendarCell"/> at the specified row and column indices in the calendar grid.
        /// </summary>
        /// <param name="rowIndex">The zero-based row index of the cell.</param>
        /// <param name="columnIndex">The zero-based column index of the cell.</param>
        /// <returns>The <see cref="CalendarCell"/> at the specified row and column indices.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the row or column index is out of range.</exception>
        public CalendarCell GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || rowIndex >= DayRowCount)
                throw new ArgumentOutOfRangeException(nameof(rowIndex));

            if (columnIndex < 0 || columnIndex >= ColumnCount)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            var index = rowIndex * ColumnCount + columnIndex;

            return cells[index];
        }

        /// <summary>
        /// Gets the effective border settings to be used for highlighting today's date in the calendar control,
        /// considering the specified <see cref="TodayBorder"/> property and the default value.
        /// </summary>
        /// <returns>The effective border settings for highlighting today's date.</returns>
        public virtual BorderSettings EffectiveTodayBorder()
        {
            return TodayBorder ?? DefaultTodayBorder ?? BorderSettings.Default;
        }

        /// <summary>
        /// Gets the effective color to be used for surrounding days (days not in the current month) in the calendar control,
        /// considering the specified <see cref="SurroundDayColor"/> property and the default value.
        /// </summary>
        /// <param name="isDark">Indicates whether the color should be adjusted for a dark theme.</param>
        /// <returns>The effective color for surrounding days.</returns>
        public virtual Color EffectiveSurroundDayColor(bool isDark)
        {
            return SurroundDayColor?.LightOrDark(isDark) ?? DefaultSurroundDayColor?.LightOrDark(isDark) ?? Color.Gray;
        }

        /// <summary>
        /// Gets the effective first day of the week to be used in the calendar control,
        /// considering the specified <see cref="FirstDayOfWeek"/> property and the system default.
        /// </summary>
        /// <returns>The effective first day of the week.</returns>
        public virtual DayOfWeek EffectiveDayOfWeek()
        {
            return FirstDayOfWeek ?? DateUtils.GetFirstDayOfWeek(EffectiveFormatProvider());
        }

        /// <summary>
        /// Gets the effective format provider to be used in the calendar control, 
        /// considering the specified <see cref="FormatProvider"/> property and the current culture.
        /// </summary>
        /// <returns>The effective format provider.</returns>
        public virtual IFormatProvider EffectiveFormatProvider()
        {
            return FormatProvider ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets the effective kind of day names to be used in the calendar control,
        /// considering the specified <see cref="DayNamesKind"/> property and the default value.
        /// </summary>
        /// <returns>The effective kind of day names.</returns>
        public virtual DayNamesKind EffectiveDayNamesKind()
        {
            return DayNamesKind ?? DefaultDayNamesKind;
        }

        /// <summary>
        /// Gets the array of day names representing the days of the week.
        /// The first day of the week is determined by the <see cref="EffectiveDayOfWeek"/> method.
        /// </summary>
        /// <returns>An array of day names.</returns>
        public virtual (string Text, DayOfWeek DayOfWeek)[] GetDayNames()
        {
            var dayNames = DateUtils.GetDayNames(EffectiveDayNamesKind(), EffectiveFormatProvider());
            var result = new (string Text, DayOfWeek DayOfWeek)[dayNames.Length];

            for (var i = 0; i < dayNames.Length; i++)
            {
                var dayOfWeek = (DayOfWeek)i;
                var index = GetDayOfWeekIndex(dayOfWeek);
                result[index] = (dayNames[i], dayOfWeek);
            }

            return result;
        }

        /// <summary>
        /// Resets the attributes of all cells in the calendar control,
        /// clearing any custom date attributes and restoring the default appearance of the cells.
        /// </summary>
        public virtual void ResetAttrAll(bool invalidate = true)
        {
            var needInvalidate = false;

            foreach (var cell in cells)
            {
                if (cell.DateAttr is not null)
                    needInvalidate = true;

                cell.DateAttr = null;
            }

            if (needInvalidate && invalidate)
                Invalidate();
        }

        /// <summary>
        /// Gets the index of the specified day of the week based on the effective first day of the week.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week.</param>
        /// <returns>The index of the day of the week.</returns>
        public virtual int GetDayOfWeekIndex(DayOfWeek dayOfWeek)
        {
            var index = DateUtils.GetDayOfWeekIndex(dayOfWeek, EffectiveDayOfWeek());
            return index;
        }

        /// <summary>
        /// Gets the height of a single row in the calendar control based
        /// on the measured height of the day names and other properties.
        /// </summary>
        /// <param name="dc">The graphics context used for measuring text.</param>
        /// <param name="font">The font used for measuring text.</param>
        /// <returns>The height of a single row in the calendar control.</returns>  
        internal virtual float GetRowHeight(Graphics dc, Font font)
        {
            var rowHeight = dc.GetTextExtent(RowHeightMeasureText, font).Height + 4;
            return rowHeight;
        }

        /// <summary>
        /// Updates the width of the columns in the calendar control
        /// based on the measured width of the day names and other properties.
        /// </summary>
        protected virtual void UpdateColumnWidth()
        {
            var minWidth = GetColumnWidth(listBox.MeasureCanvas, listBox.RealFont);
            foreach (var col in listBox.Columns)
            {
                col.SuggestedWidth = minWidth;
            }
        }

        /// <summary>
        /// Called when a cell in the list box is clicked, handling the click event for both header and day cells.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A ListBoxCellClickEventArgs that contains the event data.</param>
        protected virtual void OnListBoxCellClick(object? sender, ListBoxCellClickEventArgs e)
        {
            if (e.Cell is CalendarHeaderCellItem headerCell)
            {
                if (HeaderClick is not null)
                {
                    var args = new HeaderClickEventArgs(e);
                    HeaderClick?.Invoke(this, args);

                    if (args.Cancel)
                        return;
                }

                return;
            }

            if (e.Cell is CalendarCellItem itemCell)
            {
                if (DayClick is not null)
                {
                    var args = new DayClickEventArgs(e);
                    DayClick?.Invoke(this, args);

                    if (args.Cancel)
                        return;

                }

                Value = itemCell.Data.Date;

                return;
            }
        }

        /// <summary>
        /// Called when the value of the calendar header changes, updating the calendar's value accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnHeaderValueChanged(object? sender, EventArgs e)
        {
            if (suspendHeaderEventsCounter > 0) return;
            Value = header.Value;
        }

        /// <summary>
        /// Creates the columns in the calendar control, initializing the list box with the appropriate number
        /// of columns for displaying days of the week.
        /// </summary>
        protected virtual void CreateColumns()
        {
            listBox.Columns.Clear();

            for (var col = 0; col < XCalendar.ColumnCount; col++)
            {
                var column = new ListControlColumn($"Col{col}");
                listBox.Columns.Add(column);
            }

            UpdateColumnWidth();
        }

        /// <summary>
        /// Updates the day names displayed in the calendar header, refreshing the text of each header cell.
        /// </summary>
        protected virtual void UpdateDayNames()
        {
            var dayNames = GetDayNames();

            for (var col = 0; col < XCalendar.ColumnCount; col++)
            {
                var cellItem = (CalendarHeaderCellItem)headerItem.Cells[col];
                cellItem.Text = dayNames[col].Text;
                cellItem.DayOfWeek = dayNames[col].DayOfWeek;
            }
        }

        /// <summary>
        /// Called when the back color of the inner list box changes,
        /// updating the back color of the calendar control to match the inner list box's back color.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        protected virtual void OnListBoxBackColorChanged(object? sender, EventArgs e)
        {
            BackColor = listBox.BackColor;
        }

        /// <summary>
        /// Updates the size of the inner list box based on the measured content size,
        /// ensuring it meets the minimum total height requirement.
        /// </summary>
        protected virtual void UpdateListBoxSize()
        {
            VirtualListBox.MeasureContentSizeResult measureResult = listBox.GetContentSize(MeasureCanvas);

            var minTotalHeight = listBox.MinItemHeight * 7;

            var w = measureResult.ContentSize.Width;
            var h = Math.Max(measureResult.ContentSize.Height, minTotalHeight);

            listBox.SuggestedSize = new(w, h);
        }

        /// <inheritdoc/>
        protected override SizeD GetPreferredSizeInternal(PreferredSizeContext context)
        {
            return base.GetPreferredSizeInternal(context);
        }

        /// <summary>
        /// Updates the day items in the calendar control, refreshing the display of each day cell
        /// based on the current date selection and other properties.
        /// </summary>
        /// <param name="invalidate">A boolean value indicating whether to invalidate
        /// the list box and trigger a repaint.</param>
        protected virtual void UpdateDayItems(bool invalidate)
        {
            if (listBox.Items.Count == 0)
                return;

            var isDark = IsDarkBackground;
            var otherMonthForeColor = EffectiveSurroundDayColor(isDark);

            for (var row = 1; row <= XCalendar.DayRowCount; row++)
            {
                for (var col = 0; col < XCalendar.ColumnCount; col++)
                {
                    var rowItem = listBox.Items[row];
                    var cellItem = (CalendarCellItem)rowItem.Cells[col];

                    var cell = cellItem.Data;
                    var isGrey = cell.IsRestricted || !cell.IsCurrentMonth;

                    cellItem.Text = cell!.Date.Day.ToString();
                    cellItem.ForegroundColor = isGrey ? otherMonthForeColor : null;
                    cellItem.Border = cell.IsToday ? EffectiveTodayBorder() : null;
                }
            }

            if (invalidate)
                listBox.Invalidate();
        }

        /// <summary>
        /// Creates the day items in the calendar control, populating the list box with header
        /// and day cells for each week of the month.
        /// </summary>
        protected virtual void CreateDayItems()
        {
            var newSource = new ListSource<ListControlItem>();
            newSource.Add(headerItem);

            for (var row = 0; row < XCalendar.DayRowCount; row++)
            {
                CalendarRowItem rowItem = new();

                for (var col = 0; col < XCalendar.ColumnCount; col++)
                {
                    var cellItem = rowItem.AddCell<CalendarCellItem>(listBox.Columns[col]);
                    var cell = GetCell(row, col);
                    cellItem.Data = cell;
                    cellItem.HorizontalAlignment = HorizontalAlignment.Center;
                }

                newSource.Add(rowItem);
            }

            listBox.Items = newSource;
        }

        /// <summary>
        /// Called when a key is pressed while the list box has focus,
        /// suppressing the default key handling behavior to prevent unintended actions in the calendar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="KeyEventArgs"/> that contains the event data.</param>
        protected virtual void OnListBoxKeyDown(object? sender, KeyEventArgs e)
        {
            e.Suppressed();
        }

        /// <inheritdoc/>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            UpdateDayItems(true);
        }

        /// <summary>
        /// Called when the visibility of the year picker popup panel changes,
        /// updating the sticky state of the year picker in the calendar header accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPopupYearPickerVisibleChanged(object? sender, EventArgs e)
        {
            header.YearPicker.Sticky = popupYearPickerPanel.Visible;
        }

        /// <summary>
        /// Called when the visibility of the month picker popup panel changes,
        /// updating the sticky state of the month picker in the calendar header accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPopupMonthPickerVisibleChanged(object? sender, EventArgs e)
        {
            header.MonthPicker.Sticky = popupMonthPicker.Visible;
        }

        /// <summary>
        /// Called when the enter key is pressed in the year picker, canceling the edit and hiding the year dropdown panel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPopupYearPickerEnterPressed(object? sender, EventArgs e)
        {
            popupYearPicker.TextPicker.CancelEdit();
            popupYearPickerPanel.Visible = false;
        }

        /// <summary>
        /// Called when the escape key is pressed in the year picker, canceling the edit and hiding the year dropdown panel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPopupYearPickerEscapePressed(object? sender, EventArgs e)
        {
            popupYearPicker.TextPicker.CancelEdit();
            popupYearPickerPanel.Visible = false;
        }

        /// <summary>
        /// Called when the value of the year picker in the calendar header changes, updating the calendar's value accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPopupYearPickerValueChanged(object? sender, EventArgs e)
        {
            if (suspendHeaderEventsCounter > 0)
                return;
            Value = new DateOnly(popupYearPicker.Value, Value.Month, Value.Day);
        }

        /// <summary>
        /// Called when the year picker in the calendar header is clicked, toggling the visibility of the year dropdown panel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnHeaderYearClick(object? sender, EventArgs e)
        {
            if (!ShowYearDropDown)
                return;
            popupYearPickerPanel.Visible = !popupYearPickerPanel.Visible;
        }

        /// <inheritdoc/>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateColumnWidth();
            PerformLayoutAndInvalidate(() =>
            {
                UpdateListBoxSize();
            });
        }

        /// <summary>
        /// Called when the value of the calendar changes, updating the day items to reflect the new date selection.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            var firstDayOfMonth = DateUtils.GetFirstDateOfMonth(Value);
            var lastDayOfMonth = DateUtils.GetLastDateOfMonth(Value);
            var daysInMonth = DateUtils.GetDaysInMonth(Value);
            var dayOfWeek = firstDayOfMonth.DayOfWeek;
            var index = GetDayOfWeekIndex(dayOfWeek);

            var today = DateOnly.FromDateTime(DateTime.Now.Date);

            for (int i = 0; i < daysInMonth; i++)
            {
                var cell = cells[index + i];
                var d = firstDayOfMonth.AddDays(i);
                cell.Date = d;
                cell.IsCurrentMonth = true;
                cell.IsToday = d == today;
                cell.IsCurrent = d == Value;
                cell.IsSelected = cell.IsCurrent;
                cell.IsRestricted = restrictedDate.IsRestricted(d);
            }

            for (int i = daysInMonth + index; i < DayCellCount; i++)
            {
                var cell = cells[i];
                var d = lastDayOfMonth.AddDays(i - daysInMonth - index + 1);
                cell.Date = d;
                cell.IsCurrentMonth = false;
                cell.IsToday = false;
                cell.IsCurrent = d == Value;
                cell.IsSelected = cell.IsCurrent;
                cell.IsRestricted = restrictedDate.IsRestricted(d);
            }

            for (int i = index - 1; i >= 0; i--)
            {
                var cell = cells[i];
                var d = firstDayOfMonth.AddDays(i - index);
                cell.Date = d;
                cell.IsCurrentMonth = false;
                cell.IsToday = false;
                cell.IsCurrent = d == Value;
                cell.IsSelected = cell.IsCurrent;
                cell.IsRestricted = restrictedDate.IsRestricted(d);
            }

            UpdateDayItems(false);
        }

        /// <summary>
        /// Represents the header of the calendar control, which includes month and year pickers.
        /// </summary>
        public class CalendarHeader : TransparentPanel
        {
            /// <summary>
            /// Gets or sets the default horizontal alignment for the left button in the calendar header.
            /// </summary>
            public static HorizontalAlignment DefaultLeftButtonHorzAlignment = HorizontalAlignment.Right;

            /// <summary>
            /// Gets or sets the default horizontal alignment for the month picker in the calendar header.
            /// </summary>
            public static HorizontalAlignment DefaultMonthHorzAlignment = HorizontalAlignment.Left;

            /// <summary>
            /// Gets or sets the default horizontal alignment for the year picker in the calendar header.
            /// </summary>
            public static HorizontalAlignment DefaultYearHorzAlignment = HorizontalAlignment.Left;

            private readonly SpeedTextButton monthPicker = new();
            private readonly SpeedTextButton yearPicker = new();
            private readonly SpeedButton prevButton = new();
            private readonly SpeedButton nextButton = new();

            private DateOnly date = DateOnly.FromDateTime(DateTime.Now);
            private int suspendCounter;
            private MonthNamesKind kind = MonthNamesKind.Full;
            private IFormatProvider? formatProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="CalendarHeader"/> class.
            /// </summary>
            public CalendarHeader()
            {
                Layout = LayoutStyle.Horizontal;

                OnValueChanged();

                prevButton.VerticalAlignment = VerticalAlignment.Center;
                prevButton.SvgImage = KnownSvgImages.ImgAngleLeft;

                prevButton.HorizontalAlignment = DefaultLeftButtonHorzAlignment;
                prevButton.Parent = this;
                prevButton.Click += OnPrevButtonClick;

                nextButton.VerticalAlignment = VerticalAlignment.Center;
                nextButton.HorizontalAlignment = HorizontalAlignment.Right;
                nextButton.SvgImage = KnownSvgImages.ImgAngleRight;
                nextButton.Parent = this;
                nextButton.Click += OnNextButtonClick;

                monthPicker.MarginRight = 0;
                monthPicker.ImageVisible = false;
                monthPicker.VerticalAlignment = VerticalAlignment.Stretch;
                monthPicker.HorizontalAlignment = DefaultMonthHorzAlignment;
                monthPicker.Parent = this;

                yearPicker.HorizontalAlignment = DefaultYearHorzAlignment;
                yearPicker.VerticalAlignment = VerticalAlignment.Stretch;
                yearPicker.Parent = this;
            }

            /// <summary>
            /// Gets or sets the current value of the calendar header, representing the selected month and year.
            /// </summary>
            public event EventHandler? ValueChanged;

            /// <summary>
            /// Gets or sets the kind of month names displayed in the month picker,
            /// allowing customization of the month name format.
            /// </summary>
            public virtual MonthNamesKind Kind
            {
                get => kind;
                set
                {
                    if (value == kind) return;
                    kind = value;
                    UpdatePickerValues();
                }
            }

            /// <summary>
            /// Gets or sets the format provider used for formatting month and year values in the calendar header.
            /// </summary>
            public virtual IFormatProvider? FormatProvider
            {
                get => formatProvider;
                set
                {
                    if (value == formatProvider) return;
                    formatProvider = value;
                    UpdatePickerValues();
                }
            }

            /// <summary>
            /// Gets or sets the current value of the calendar header, representing the selected month and year.
            /// </summary>
            public virtual DateOnly Value
            {
                get
                {
                    return date;
                }

                set
                {
                    value = new(value.Year, value.Month, 1);
                    if (this.date == value)
                        return;
                    this.date = value;
                    OnValueChanged();
                    Invalidate();
                }
            }

            /// <summary>
            /// Gets the previous button in the calendar header, which allows users to navigate to the previous month.
            /// </summary>
            public SpeedButton PrevButton => prevButton;

            /// <summary>
            /// Gets the next button in the calendar header, which allows users to navigate to the next month.
            /// </summary>
            public SpeedButton NextButton => nextButton;

            /// <summary>
            /// Gets the month picker in the calendar header, which allows users to select a month from a dropdown list.
            /// </summary>
            public SpeedButton MonthPicker => monthPicker;

            /// <summary>
            /// Gets the year picker in the calendar header, which allows users to select a year from a dropdown list.
            /// </summary>
            public SpeedButton YearPicker => yearPicker;

            /// <summary>
            /// Updates the values of the month and year pickers based on the current value of the calendar header.
            /// </summary>
            protected virtual void UpdatePickerValues()
            {
                monthPicker.Text = DateUtils.GetMonthName((CalendarMonth)date.Month, Kind, FormatProvider);
                yearPicker.Text = date.Year.ToString();
            }

            /// <summary>
            /// Handles the event when the previous button is clicked, updating the calendar's value to the previous month.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnPrevButtonClick(object? sender, EventArgs e)
            {
                Value = Value.AddMonths(-1);
            }

            /// <summary>
            /// Handles the event when the next button is clicked, updating the calendar's value to the next month.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            protected virtual void OnNextButtonClick(object? sender, EventArgs e)
            {
                Value = Value.AddMonths(1);
            }

            /// <summary>
            /// Called when the value of the calendar header changes, updating the month and year pickers
            /// accordingly and raising the <see cref="ValueChanged"/> event.
            /// </summary>
            protected virtual void OnValueChanged()
            {
                suspendCounter++;
                try
                {
                    UpdatePickerValues();
                }
                finally
                {
                    suspendCounter--;
                }

                ValueChanged?.Invoke(this, EventArgs.Empty);
            }

            /// <summary>
            /// Handles the event when the value of the year picker changes, updating the calendar's value accordingly.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            private void OnYearPickerValueChanged(object? sender, EventArgs e)
            {
                if (suspendCounter > 0)
                    return;
            }

            /// <summary>
            /// Handles the event when the value of the month picker changes, updating the calendar's value accordingly.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The event arguments.</param>
            private void OnMonthPickerValueChanged(object? sender, EventArgs e)
            {
                if (suspendCounter > 0)
                    return;
            }
        }

        /// <summary>
        /// Represents a row item in the calendar control, which contains cells for each day of the week.
        /// </summary>
        public class CalendarRowItem : ListControlItem
        {
        }

        /// <summary>
        /// Represents a cell item in the calendar control, which corresponds to a specific day in the calendar.
        /// </summary>
        public class CalendarCellItem : ListControlItem
        {
            /// <summary>
            /// Gets the data associated with the calendar cell.
            /// </summary>
            public CalendarCell Data { get; internal set; } = CalendarCell.Default;

            /// <inheritdoc/>
            public override Color? ForegroundColor
            {
                get
                {
                    return Data.DateAttr?.TextColor ?? base.ForegroundColor;
                }

                set
                {
                    base.ForegroundColor = value;
                }
            }

            /// <inheritdoc/>
            public override Color? BackgroundColor
            {
                get
                {
                    return Data.DateAttr?.BackgroundColor ?? base.BackgroundColor;
                }

                set
                {
                    base.BackgroundColor = value;
                }
            }

            /// <inheritdoc/>
            public override bool IsSelectedCell(IListControlItemContainer? container)
            {
                return Data.IsCurrent;
            }
        }

        /// <summary>
        /// Represents the header item in the calendar control, which contains cells for the day names.
        /// </summary>
        public class CalendarHeaderItem : ListControlItem
        {
        }

        /// <summary>
        /// Represents a header cell item in the calendar control, which corresponds to a specific day of the week.
        /// </summary>
        public class CalendarHeaderCellItem : ListControlItem
        {
            /// <summary>
            /// Gets the day of the week represented by the header cell,
            /// providing information about the specific day in the calendar.
            /// </summary>
            public DayOfWeek DayOfWeek { get; internal set; }
        }

        /// <summary>
        /// Represents a list box used in the calendar control, which displays the days of the month in a grid format.
        /// </summary>
        public class CalendarListBox : VirtualListBox
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CalendarListBox"/> class.
            /// </summary>
            public CalendarListBox()
            {
                SelectionMode = ListBoxSelectionMode.None;
            }
        }

        /// <summary>
        /// Represents a container panel used in the calendar control, which provides a layout for its child controls.
        /// </summary>
        public partial class CalendarContainer : HiddenBorder
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CalendarContainer"/> class,
            /// </summary>
            public CalendarContainer()
            {
                Padding = 10;
                Layout = LayoutStyle.Vertical;
            }

            /// <inheritdoc/>
            protected override SizeD GetPreferredSizeInternal(PreferredSizeContext context)
            {
                return base.GetPreferredSizeInternal(context);
            }
        }

        /// <summary>
        /// Represents the event arguments for a day click event in the calendar control,
        /// providing information about the clicked day cell.
        /// </summary>
        public class DayClickEventArgs : BaseCancelEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DayClickEventArgs"/>
            /// class with the specified list box cell click event arguments.
            /// </summary>
            /// <param name="e">The list box cell click event arguments.</param>
            public DayClickEventArgs(ListBoxCellClickEventArgs e)
            {
                ClickArgs = e;
            }

            /// <summary>
            /// Gets the original list box cell click event arguments that triggered the day click event,
            /// </summary>
            public ListBoxCellClickEventArgs ClickArgs { get; set; }

            /// <summary>
            /// Gets the calendar cell item that was clicked, providing information about the specific day.
            /// </summary>
            public CalendarCellItem Cell => (CalendarCellItem)ClickArgs.Cell!;
        }

        /// <summary>
        /// Represents the event arguments for a header click event in the calendar control,
        /// </summary>
        public class HeaderClickEventArgs : BaseCancelEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HeaderClickEventArgs"/>
            /// class with the specified list box cell click event arguments.
            /// </summary>
            /// <param name="e">The list box cell click event arguments.</param>
            public HeaderClickEventArgs(ListBoxCellClickEventArgs e)
            {
                ClickArgs = e;
            }

            /// <summary>
            /// Gets the original list box cell click event arguments that triggered the header click event,
            /// </summary>
            public ListBoxCellClickEventArgs ClickArgs { get; set; }

            /// <summary>
            /// Gets the header cell item that was clicked, providing information about the specific day of the week.
            /// </summary>
            public CalendarHeaderCellItem Cell => (CalendarHeaderCellItem)ClickArgs.Cell!;
        }

        /// <summary>
        /// Represents a cell in the calendar control, which contains information about a specific day,
        /// including its date, position in the grid, and whether it belongs to the current month or is today.
        /// </summary>
        public class CalendarCell : BaseObject
        {
            /// <summary>
            /// Gets the default instance of the <see cref="CalendarCell"/> class,
            /// which can be used as a placeholder or default value.
            /// </summary>
            public static CalendarCell Default = new();

            internal CalendarCell()
            {
            }

            /// <summary>
            /// Gets the row index of the cell in the calendar grid.
            /// </summary>
            public int RowIndex { get; internal set; }

            /// <summary>
            /// Gets the column index of the cell in the calendar grid.
            /// </summary>
            public int ColumnIndex { get; internal set; }

            /// <summary>
            /// Gets the date represented by the cell in the calendar.
            /// </summary>
            public DateOnly Date { get; internal set; }

            /// <summary>
            /// Gets the date attributes associated with the cell, providing additional information
            /// about the date's characteristics.
            /// </summary>
            public ICalendarDateAttr? DateAttr { get; internal set; }

            /// <summary>
            /// Gets a value indicating whether the cell represents a day in the current month.
            /// </summary>
            public bool IsCurrentMonth { get; internal set; }

            /// <summary>
            /// Gets a value indicating whether the cell represents today's date.
            /// </summary>
            public bool IsToday { get; internal set; }

            /// <summary>
            /// Gets a value indicating whether the cell represents the currently selected date in the calendar.
            /// </summary>
            public bool IsCurrent { get; internal set; }

            /// <summary>
            /// Gets a value indicating whether the cell is selected in the calendar.
            /// </summary>
            public bool IsSelected { get; internal set; }

            /// <summary>
            /// Gets a value indicating whether the cell's date is restricted based on the calendar's date range limitations.
            /// </summary>
            public bool IsRestricted { get; internal set; }
        }

        /// <summary>
        /// Represents a panel used as the month picker in the calendar control,
        /// allowing users to select a month.
        /// </summary>
        public partial class MonthPickerPanel : HiddenGenericBorder
        {
            private readonly TransparentPanel[] rows;
            private readonly List<SpeedTextButton> buttons = new(12);
            private IFormatProvider? formatProvider;
            private MonthNamesKind kind = MonthNamesKind.Abbreviated;
            private CalendarMonth data = CalendarMonth.January;

            /// <summary>
            /// Initializes a new instance of the <see cref="MonthPickerPanel"/> class.
            /// </summary>
            public MonthPickerPanel()
            {
                this.RoundCorners();
                Padding = 10;
                HasBorder = true;

                var row1 = CreateRow([CalendarMonth.January, CalendarMonth.February, CalendarMonth.March, CalendarMonth.April]);
                var row2 = CreateRow([CalendarMonth.May, CalendarMonth.June, CalendarMonth.July, CalendarMonth.August]);
                var row3 = CreateRow([CalendarMonth.September, CalendarMonth.October, CalendarMonth.November, CalendarMonth.December]);

                rows = [row1, row2, row3];

                Layout = LayoutStyle.Vertical;

                row1.Parent = this;
                row2.Parent = this;
                row3.Parent = this;

                UpdateMonthNames();
            }

            /// <summary>
            /// Occurs when a month button is clicked in the month picker panel,
            /// allowing subscribers to respond to the month selection.
            /// </summary>
            public event EventHandler<BaseEventArgs<CalendarMonth>>? MonthClick;

            /// <summary>
            /// Occurs when the selected month value changes, allowing subscribers to respond to the change in selection.
            /// </summary>
            public event EventHandler? ValueChanged;

            /// <summary>
            /// Gets or sets the kind of month names displayed in the month picker,
            /// allowing customization of the month name format.
            /// </summary>
            public virtual MonthNamesKind MonthNamesKind
            {
                get => kind;
                set
                {
                    if (value == kind) return;
                    kind = value;
                    UpdateMonthNames();
                }
            }

            /// <summary>
            /// Gets or sets the currently selected month in the month picker, allowing users to select a specific month.
            /// </summary>
            public virtual CalendarMonth Value
            {
                get
                {
                    return data;
                }

                set
                {
                    if (value == data) return;
                    data = value;
                    UpdateSelectedMonth();
                    ValueChanged?.Invoke(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Gets or sets the format provider used for formatting month values.
            /// </summary>
            public virtual IFormatProvider? FormatProvider
            {
                get => formatProvider;
                set
                {
                    if (value == formatProvider) return;
                    formatProvider = value;
                    UpdateMonthNames();
                }
            }

            /// <summary>
            /// Updates the values of the month buttons in the month picker panel based on the current kind and format provider.
            /// </summary>
            /// <param name="months">The months to include in the row.</param>
            /// <returns>The created <see cref="TransparentPanel"/> instance representing the row.</returns>
            protected virtual TransparentPanel CreateRow(CalendarMonth[] months)
            {
                var result = new TransparentPanel();
                result.Layout = LayoutStyle.Horizontal;

                foreach (var m in months)
                {
                    var button = CreateButton(m);
                    button.Parent = result;
                }

                return result;
            }

            /// <summary>
            /// Updates the month names displayed on the buttons in the month picker panel
            /// based on the current kind and format provider.
            /// </summary>
            protected virtual void UpdateMonthNames()
            {
                var monthNames = DateUtils.GetMonthNames(MonthNamesKind, formatProvider);
                var width = GetMaxWidth(MeasureCanvas, RealFont);

                foreach (var button in buttons)
                {
                    if (button.Tag is CalendarMonth month)
                    {
                        button.Text = monthNames[(int)month - 1];
                        button.MinWidth = width;
                    }
                }

                float GetMaxWidth(Graphics dc, Font font)
                {
                    var result = 0f;

                    foreach (var month in monthNames)
                    {
                        var monthNameWidth = dc.GetTextExtent(month, font).Width;
                        if (monthNameWidth > result)
                            result = monthNameWidth;
                    }

                    return result + 12;
                }
            }

            /// <summary>
            /// Updates the selected month in the month picker panel, setting the sticky state of the corresponding button.
            /// </summary>
            protected virtual void UpdateSelectedMonth()
            {
                foreach (var button in buttons)
                {
                    if (button.Tag is CalendarMonth month)
                    {
                        button.Sticky = month == data;
                    }
                }
            }

            /// <inheritdoc/>
            protected override void OnFontChanged(EventArgs e)
            {
                base.OnFontChanged(e);
                UpdateMonthNames();
            }

            /// <summary>
            /// Creates a button for a specific month in the month picker panel, initializing its properties and click action.
            /// </summary>
            /// <param name="month">The month for which to create the button.</param>
            /// <returns>The created <see cref="SpeedTextButton"/> instance.</returns>
            protected virtual SpeedTextButton CreateButton(CalendarMonth month)
            {
                var result = new SpeedTextButton();
                result.Tag = month;
                result.Text = DateUtils.GetMonthName(month, MonthNamesKind, FormatProvider);
                result.HorizontalAlignment = HorizontalAlignment.Center;
                result.VerticalAlignment = VerticalAlignment.Center;
                result.Sticky = data == month;
                buttons.Add(result);

                result.ClickAction = () =>
                {
                    Value = month;
                    MonthClick?.Invoke(this, new BaseEventArgs<CalendarMonth>(month));
                };

                return result;
            }
        }

        /// <summary>
        /// Represents a collection of predefined date attributes for the calendar control,
        /// </summary>
        public partial class CalendarDateAttributes : BaseObject
        {
            private ICalendarDateAttr? red;
            private ICalendarDateAttr? blue;
            private ICalendarDateAttr? green;

            /// <summary>
            /// Initializes a new instance
            /// of the <see cref="CalendarDateAttributes"/> class with the specified owner calendar control.
            /// </summary>
            internal CalendarDateAttributes()
            {
            }

            /// <summary>
            /// Gets the <see cref="ICalendarDateAttr"/> attributes with red color of the foreground
            /// used as a highlight for specific dates.
            /// </summary>
            public ICalendarDateAttr? Red
            {
                get
                {
                    return red ??= Create(LightDarkColors.Red);
                }
            }

            /// <summary>
            /// Gets the <see cref="ICalendarDateAttr"/> attributes with blue color of the foreground
            /// used as a highlight for specific dates.
            /// </summary>
            public ICalendarDateAttr? Blue
            {
                get
                {
                    return blue ??= Create(LightDarkColors.Blue);
                }
            }

            /// <summary>
            /// Gets the <see cref="ICalendarDateAttr"/> attributes with green color of the foreground
            /// used as a highlight for specific dates.
            /// </summary>
            public ICalendarDateAttr? Green
            {
                get
                {
                    return green ??= Create(LightDarkColors.Green);
                }
            }

            /// <summary>
            /// Creates a new instance of <see cref="ICalendarDateAttr"/> with the specified text color.
            /// </summary>
            /// <param name="textColor">The text color for the calendar date.</param>
            /// <returns>The created <see cref="ICalendarDateAttr"/> instance.</returns>
            protected virtual ICalendarDateAttr Create(Color textColor)
            {
                var result = new PlessCalendarDateAttr();
                result.TextColor = textColor;
                result.SetImmutable();
                return result;
            }
        }
    }
}
