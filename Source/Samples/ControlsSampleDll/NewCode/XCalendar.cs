using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    public partial class XCalendar : Border
    {
        public const int DayCellCount = 42;

        public const int DayRowCount = 6;

        public const int ColumnCount = 7;

        public static string RowHeightMeasureText = "Wg00";
        public static string DayWidthMeasureText = "00";

        public bool DefaultShowMonthDropDown = true;

        public static LightDarkColor DefaultSurroundDayColor = new(Color.Gray);

        public static DayNamesKind DefaultDayNamesKind = UI.DayNamesKind.Abbreviated;

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

            UpdateDayNames();
            CreateDayItems();

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

        public event EventHandler? ValueChanged;

        public event EventHandler<HeaderClickEventArgs>? HeaderClick;

        public event EventHandler<DayClickEventArgs>? DayClick;

        public CalendarListBox ListBox => listBox;

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
            }
        }

        [Browsable(false)]
        public virtual LightDarkColor? SurroundDayColor
        {
            get => surroundDayColor;
            set
            {
                if (surroundDayColor == value)
                    return;
                surroundDayColor = value;
                UpdateDayItems();
            }
        }

        public virtual BorderSettings? TodayBorder
        {
            get => todayBorder;
            set
            {
                if (todayBorder == value)
                    return;
                todayBorder = value;
                UpdateDayItems();
            }
        }

        public virtual DayNamesKind? DayNamesKind
        {
            get => dayNamesKind;
            set
            {
                if (dayNamesKind == value)
                    return;
                dayNamesKind = value;
                UpdateDayNames();
            }
        }

        public virtual IFormatProvider? FormatProvider
        {
            get => formatProvider;
            set
            {
                if (formatProvider == value) return;
                formatProvider = value;
                header.FormatProvider = value;
                UpdateDayNames();
            }
        }

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

        public virtual float GetTotalWidth(Graphics dc, Font font)
        {
            var totalWidth = GetColumnWidth(dc, font) * ColumnCount;
            return totalWidth;
        }

        public virtual float GetRowHeight(Graphics dc, Font font)
        {
            var rowHeight = dc.GetTextExtent(RowHeightMeasureText, font).Height + 4;
            return rowHeight;
        }

        public virtual float GetTotalHeight(Graphics dc, Font font)
        {
            var totalHeight = GetRowHeight(dc, font) * (DayRowCount + 1);
            return totalHeight;
        }

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

        public CalendarCell GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || rowIndex >= DayRowCount)
                throw new ArgumentOutOfRangeException(nameof(rowIndex));

            if (columnIndex < 0 || columnIndex >= ColumnCount)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            var index = rowIndex * ColumnCount + columnIndex;

            return cells[index];
        }

        public virtual BorderSettings EffectiveTodayBorder()
        {
            return TodayBorder ?? DefaultTodayBorder ?? BorderSettings.Default;
        }

        public virtual Color EffectiveSurroundDayColor(bool isDark)
        {
            return SurroundDayColor?.LightOrDark(isDark) ?? DefaultSurroundDayColor?.LightOrDark(isDark) ?? Color.Gray;
        }

        public virtual DayOfWeek EffectiveDayOfWeek()
        {
            return FirstDayOfWeek ?? DateUtils.SystemFirstDayOfWeek;
        }

        public virtual IFormatProvider EffectiveFormatProvider()
        {
            return FormatProvider ?? CultureInfo.CurrentCulture;
        }

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

        public virtual int GetDayOfWeekIndex(DayOfWeek dayOfWeek)
        {
            var index = DateUtils.GetDayOfWeekIndex(dayOfWeek, EffectiveDayOfWeek());
            return index;
        }

        protected virtual void UpdateColumnWidth()
        {
            var minWidth = GetColumnWidth(listBox.MeasureCanvas, listBox.RealFont);
            foreach (var col in listBox.Columns)
            {
                col.SuggestedWidth = minWidth;
            }
        }

        protected virtual void OnListBoxCellClick(object? sender, ListBoxCellClickEventArgs e)
        {
            if (e.Cell is CalendarHeaderCellItem headerCell)
            {
                HeaderClick?.Invoke(this, new HeaderClickEventArgs(e));
                return;
            }

            if (e.Cell is CalendarCellItem itemCell)
            {
                DayClick?.Invoke(this, new DayClickEventArgs(e));
                return;
            }
        }

        protected virtual void OnHeaderValueChanged(object? sender, EventArgs e)
        {
            if (suspendHeaderEventsCounter > 0) return;
            Value = header.Value;
        }

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

        protected virtual void OnListBoxBackColorChanged(object? sender, EventArgs e)
        {
            BackColor = listBox.BackColor;
        }

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

        protected virtual void UpdateDayItems()
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

            listBox.Invalidate();
        }

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
            UpdateDayItems();
        }

        protected virtual void OnListBoxKeyDown(object? sender, KeyEventArgs e)
        {
            e.Suppressed();
        }

        /// <inheritdoc/>
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            UpdateDayItems();
        }

        /// <inheritdoc/>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateColumnWidth();
            PerformLayout();
        }

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
            }

            for (int i = daysInMonth + index; i < DayCellCount; i++)
            {
                var cell = cells[i];
                cell.Date = lastDayOfMonth.AddDays(i - daysInMonth - index + 1);
                cell.IsCurrentMonth = false;
                cell.IsToday = false;
            }

            for (int i = index - 1; i >= 0; i--)
            {
                var cell = cells[i];
                cell.Date = firstDayOfMonth.AddDays(i - index);
                cell.IsCurrentMonth = false;
                cell.IsToday = false;
            }

            UpdateDayItems();
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

        public event EventHandler? ValueChanged;

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
                OnValueChanged();
            }
        }

        public SpeedButton PrevButton => prevButton;

        public SpeedButton NextButton => nextButton;

        public MonthPicker MonthPicker => monthPicker;

        public SpeedTextButton YearPicker => yearPicker;

        protected virtual void UpdatePickerValues()
        {
            monthPicker.ValueAsInt = Value.Month;
            yearPicker.Text = date.Year.ToString();
        }

        protected virtual void OnPrevButtonClick(object? sender, EventArgs e)
        {
            Value = Value.AddMonths(-1);
        }

        protected virtual void OnNextButtonClick(object? sender, EventArgs e)
        {
            Value = Value.AddMonths(1);
        }

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

        protected virtual void OnYearPickerValueChanged(object? sender, EventArgs e)
        {
            if (suspendCounter > 0)
                return;
        }

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
        public CalendarCell? Data { get; internal set; }
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
        public DayOfWeek DayOfWeek { get; internal set; }
    }

    /// <summary>
    /// Represents a list box used in the calendar control, which displays the days of the month in a grid format.
    /// </summary>
    public class CalendarListBox : VirtualListBox
    {
        public CalendarListBox()
        {
            SelectionMode = ListBoxSelectionMode.None;
        }
    }

    /// <summary>
    /// Represents the event arguments for a day click event in the calendar control,
    /// providing information about the clicked day cell.
    /// </summary>
    public class DayClickEventArgs : BaseEventArgs
    {
        public DayClickEventArgs(ListBoxCellClickEventArgs e)
        {
            OriginalEventArgs = e;
        }

        public ListBoxCellClickEventArgs OriginalEventArgs { get; set; }

        public CalendarCellItem Cell => (CalendarCellItem)OriginalEventArgs.Cell!;
    }

    /// <summary>
    /// Represents the event arguments for a header click event in the calendar control,
    /// </summary>
    public class HeaderClickEventArgs : BaseEventArgs
    {
        public HeaderClickEventArgs(ListBoxCellClickEventArgs e)
        {
            OriginalEventArgs = e;
        }

        public ListBoxCellClickEventArgs OriginalEventArgs { get; set; }

        public CalendarHeaderCellItem Cell => (CalendarHeaderCellItem)OriginalEventArgs.Cell!;
    }

    /// <summary>
    /// Represents a cell in the calendar control, which contains information about a specific day,
    /// including its date, position in the grid, and whether it belongs to the current month or is today.
    /// </summary>
    public class CalendarCell : BaseObject
    {
        internal CalendarCell()
        {
        }

        public int RowIndex { get; internal set; }

        public int ColumnIndex { get; internal set; }

        public DateOnly Date { get; internal set; }

        public bool IsCurrentMonth { get; internal set; }

        public bool IsToday { get; internal set; }
    }
}
