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
        /// Gets or sets the default <see cref="IXCalendarDateAttr"/> attributes for holidays in the calendar control.
        /// You can use <see cref="XCalendarDateAttr"/>
        /// in order to create a new instance of <see cref="IXCalendarDateAttr"/> with the desired attributes.
        /// </summary>
        public static IXCalendarDateAttr? DefaultHolidayAttr;

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
        private IXCalendarDateAttr? holidayAttr;

        static XCalendar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCalendar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public XCalendar(AbstractControl parent)
            : this()
        {
            Parent = parent;
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
        /// Gets or sets the <see cref="IXCalendarDateAttr"/> attributes for holidays in the calendar control,
        /// If not set, the default holiday attributes will be used. You can use <see cref="CreateDateAttr"/>
        /// in order to create a new instance of <see cref="IXCalendarDateAttr"/> with the desired attributes.
        /// </summary>
        [Browsable(false)]
        public virtual IXCalendarDateAttr? HolidayAttr
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
        /// the given <see cref="IXCalendarDateAttr"/> attributes. After current month or year is changed,
        /// this method should be called again to mark dates in the new month.
        /// </summary>
        /// <param name="rule">The repeat pattern rule to match dates.</param>
        /// <param name="attr">The attributes to apply to the matching dates. Pass <c>null</c> to reset attributes.</param>
        /// <param name="invalidate">Indicates whether to invalidate the control after setting the attributes.</param>
        public virtual void MarkWithRule(RepeatPatternRule rule, IXCalendarDateAttr? attr, bool invalidate = true)
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
        /// Sets the <see cref="IXCalendarDateAttr"/> attributes for the given date.
        /// This method can be used to customize the appearance and behavior of current month days.
        /// If the date is not in the current month, the method will return false and no attributes will be set.
        /// After month or year is changed, this method should be called again to set attributes for the new month.
        /// </summary>
        /// <param name="date">The date for which to set the attributes.</param>
        /// <param name="dateAttr">The attributes to set for the date. Pass <c>null</c> to reset attributes.</param>
        /// <param name="invalidate">Indicates whether to invalidate the control after setting the attributes.</param>
        /// <returns><c>true</c> if the attributes were successfully set; otherwise, <c>false</c>.</returns>
        public virtual bool SetAttr(DateOnly date, IXCalendarDateAttr? dateAttr, bool invalidate = true)
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
        /// Sets the <see cref="IXCalendarDateAttr"/> attributes for the given day.
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
        public virtual bool SetAttr(int day, IXCalendarDateAttr? dateAttr, bool invalidate = true)
        {
            if (day < 1 || day > DateTime.DaysInMonth(Value.Year, Value.Month))
                return false;
            var dateToSet = new DateOnly(Value.Year, Value.Month, day);
            return SetAttr(dateToSet, dateAttr, invalidate);
        }

        /// <summary>
        /// Changes <see cref="Value"/> property to the today date.
        /// </summary>
        public virtual void SelectToday()
        {
            Value = DateTime.Now.Date.ToDateOnly();
        }

        /// <summary>
        /// Creates <see cref="IXCalendarDateAttr"/> instance.
        /// </summary>
        public virtual IXCalendarDateAttr CreateDateAttr()
        {
            return new XCalendarDateAttr();
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

        /// <inheritdoc/>
        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
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
                cell.DateAttr = null;
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
                cell.DateAttr = null;
            }

            UpdateDayItems(false);
        }
    }
}
