using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a calendar control that allows users to select a date from a visual calendar interface.
    /// </summary>
    public partial class XCalendar : Border
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
        /// Gets the text used for measuring the height of a single row in the calendar control.
        /// </summary>
        public static readonly string RowHeightMeasureText = "Wg00";

        /// <summary>
        /// Gets the text used for measuring the width of a single day column in the calendar control.
        /// </summary>
        public static readonly string DayWidthMeasureText = "00";

        /// <summary>
        /// Gets or sets a value indicating whether the month dropdown in the calendar header should be displayed by default.
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

        private DateOnly date;
        private int suspendHeaderEventsCounter;
        private LightDarkColor? surroundDayColor;
        private BorderSettings? todayBorder;
        private DayNamesKind? dayNamesKind;
        private IFormatProvider? formatProvider;
        private DayOfWeek? firstDayOfWeek;

        static XCalendar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCalendar"/> class.
        /// </summary>
        public XCalendar()
        {
            listBox = new();
            header = new();

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

            date = DateOnly.FromDateTime(DateTime.Now.Date);

            Layout = LayoutStyle.Vertical;

            header.MarginBottom = 5;

            ParentBackColor = false;
            Padding = 10;
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

            DoInsideLayout(() =>
            {
                header.Parent = this;
                listBox.Parent = this;
            });

            ShowMonthDropDown = DefaultShowMonthDropDown;
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
        /// Occurs when the value of the calendar control changes,
        /// allowing subscribers to handle the event and perform actions based on the new value.
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
        /// Gets or sets a value indicating whether the month dropdown in the calendar header should be displayed,
        /// </summary>
        public virtual bool ShowMonthDropDown
        {
            get
            {
                return header.ShowMonthDropDown;
            }

            set
            {
                header.ShowMonthDropDown = value;
            }
        }

        /// <summary>
        /// Gets or sets the current value of the calendar control as a <see cref="DateTime"/> object,
        /// </summary>
        [Browsable(false)]
        public DateTime AsDateTime
        {
            get => Value.ToDateTime(new TimeOnly(0, 0));
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
                if (this.date == value)
                    return;
                this.date = value;

                suspendHeaderEventsCounter++;
                try
                {
                    header.Value = value;
                }
                finally
                {
                    suspendHeaderEventsCounter--;
                }

                ValueChanged?.Invoke(this, EventArgs.Empty);

                OnValueChanged();

                Invalidate();
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
                UpdateDayItems(true);
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
                UpdateDayItems(true);
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
                UpdateDayNames();
                PerformLayoutAndInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the format provider used for formatting day names in the calendar control.
        /// </summary>
        public virtual IFormatProvider? FormatProvider
        {
            get => formatProvider;
            set
            {
                if (formatProvider == value) return;
                formatProvider = value;
                header.FormatProvider = value;
                UpdateDayNames();
                PerformLayoutAndInvalidate();
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
            return FirstDayOfWeek ?? DateUtils.SystemFirstDayOfWeek;
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

        /// <inheritdoc/>
        protected override SizeD GetPreferredSizeInternal(PreferredSizeContext context)
        {
            if (context.AvailableSize.AnyIsEmptyOrNegative)
                return SizeD.Empty;

            var result = GetDefaultPreferredSize(
                        context.AvailableSize,
                        withPadding: true,
                        (size) =>
                        {
                            var dc = MeasureCanvas;

                            VirtualListBox.MeasureContentSizeResult measureResult = listBox.GetContentSize(
                                dc,
                                fromIndex: null,
                                toIndex: null,
                                prm: null);

                            var width = measureResult.ContentSize.Width;
                            var height = measureResult.ContentSize.Height;
                            var headerHeight = header.GetPreferredSize(context);

                            height += headerHeight.Height + listBox.Margin.Vertical + header.Margin.Vertical;
                            width = Math.Max(width, headerHeight.Width) + listBox.Margin.Horizontal + header.Margin.Horizontal;

                            return new SizeD(width, height);
                        });

            result = result.Ceiling();

            return result;
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
                    cellItem.Text = cell!.Date.Day.ToString();
                    cellItem.ForegroundColor = !cell.IsCurrentMonth ? otherMonthForeColor : null;
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

        /// <inheritdoc/>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateColumnWidth();
            PerformLayoutAndInvalidate();
        }

        /// <summary>
        /// Called when the value of the calendar changes, updating the day items to reflect the new date selection.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            var firstDayOfMonth = DateUtils.GetFirstDateOfMonth(date);
            var lastDayOfMonth = DateUtils.GetLastDateOfMonth(date);
            var daysInMonth = DateUtils.GetDaysInMonth(date);
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
            }

            UpdateDayItems(false);
        }
    }

    /// <summary>
    /// Represents the header of the calendar control, which includes month and year pickers.
    /// </summary>
    public class CalendarHeader : TransparentPanel
    {
        private readonly MonthSpeedButton monthPicker = new();
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
            prevButton.HorizontalAlignment = HorizontalAlignment.Left;
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
            monthPicker.HorizontalAlignment = HorizontalAlignment.Center;
            monthPicker.ValueChanged += OnMonthPickerValueChanged;
            monthPicker.Parent = this;

            yearPicker.HorizontalAlignment = HorizontalAlignment.Center;
            yearPicker.VerticalAlignment = VerticalAlignment.Stretch;
            yearPicker.Parent = this;
        }

        /// <summary>
        /// Gets or sets the current value of the calendar header, representing the selected month and year.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the month
        /// picker in the calendar header should display a dropdown list for selecting months.
        /// </summary>
        public virtual bool ShowMonthDropDown
        {
            get
            {
                return monthPicker.AllowPopupWindow;
            }

            set
            {
                monthPicker.AllowPopupWindow = value;

            }
        }

        /// <summary>
        /// Gets or sets the kind of month names displayed in the month picker, allowing customization of the month name format.
        /// </summary>
        public virtual MonthNamesKind Kind
        {
            get => kind;
            set
            {
                if (value == kind) return;
                kind = value;
                monthPicker.MonthNamesKind = value;
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
                monthPicker.FormatProvider = value;
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
        public MonthPicker MonthPicker => monthPicker;

        /// <summary>
        /// Gets the year picker in the calendar header, which allows users to select a year from a dropdown list.
        /// </summary>
        public GenericControl YearPicker => yearPicker;

        /// <summary>
        /// Updates the values of the month and year pickers based on the current value of the calendar header.
        /// </summary>
        protected virtual void UpdatePickerValues()
        {
            monthPicker.ValueAsInt = Value.Month;
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
        protected virtual void OnYearPickerValueChanged(object? sender, EventArgs e)
        {
            if (suspendCounter > 0)
                return;
        }

        /// <summary>
        /// Handles the event when the value of the month picker changes, updating the calendar's value accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnMonthPickerValueChanged(object? sender, EventArgs e)
        {
            if (suspendCounter > 0)
                return;
            Value = new DateOnly(date.Year, monthPicker.ValueAsInt, date.Day);
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
        public override void DrawCellBackground(in DrawCellParams prm)
        {
            if (!Data.IsCurrent)
                return;

            var container = prm.Container;
            var e = prm.PaintArgs;
            var item = e.Item;
            var rect = prm.Rect;
            var dc = e.Graphics;
            var control = container?.Control;

            var selectionBorder = container?.Defaults.SelectionBorder;

            dc.FillBorderRectangle(
                rect,
                GetSelectedItemBackColor(item, container)?.AsBrush,
                selectionBorder,
                hasBorder: false,
                control);
        }

        /// <inheritdoc/>
        public override void DrawCellForeground(in DrawCellParams prm)
        {
            base.DrawCellForeground(in prm);
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
    }
}
