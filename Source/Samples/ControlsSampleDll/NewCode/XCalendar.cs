using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    public partial class XCalendar : HiddenBorder
    {
        public const int CellCount = 42;

        public const int RowCount = 6;

        public const int ColumnCount = 7;

        public static DayNamesKind DefaultDayNamesKind = UI.DayNamesKind.Abbreviated;
        public static BorderSettings DefaultTodayBorder;

        private readonly CalendarCell[] cells = new CalendarCell[CellCount];
        private readonly VirtualListBox listBox = new();
        private readonly CalendarHeader header = new();

        private DateOnly date;
        private int suspendHeaderEventsCounter;

        static XCalendar()
        {
            DefaultTodayBorder = BorderSettings.Default.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XCalendar"/> class.
        /// </summary>
        public XCalendar()
        {
            for (int i = 0; i < CellCount; i++)
            {
                cells[i] = new();
            }

            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColumnCount; col++)
                {
                    var cell = GetCell(row, col);
                    cell.RowIndex = row;
                    cell.ColumnIndex = col;
                }
            }

            date = DateOnly.FromDateTime(DateTime.Now.Date);
            OnValueChanged();

            MinimumSize = new(400, 400);
            Layout = LayoutStyle.Vertical;

            header.Parent = this;
            header.MarginBottom = 5;

            ParentBackColor = false;
            Padding = 10;
            BackColor = listBox.BackColor;

            listBox.VertGridLines = false;
            listBox.HasBorder = false;
            listBox.HorzGridLines = false;
            listBox.VerticalAlignment = VerticalAlignment.Fill;
            listBox.Parent = this;

            listBox.KeyDown += (s, e) =>
            {
                e.Suppressed();
            };

            AssignItemsToListBox(listBox);

            header.ValueChanged += OnHeaderValueChanged;

            listBox.BackColorChanged += (s, e) =>
            {
                BackColor = listBox.BackColor;
            };
        }

        protected virtual void OnHeaderValueChanged(object? sender, EventArgs e)
        {
            if (suspendHeaderEventsCounter > 0) return;
            Value = header.Value;
        }

        public virtual float GetTotalWidth(Graphics dc, Font font)
        {
            var totalWidth = GetColumnWidth(dc, font) * ColumnCount;
            return totalWidth;
        }

        public virtual float GetRowHeight(Graphics dc, Font font)
        {
            var rowHeight = dc.GetTextExtent("Wg00", font).Height + 4;
            return rowHeight;
        }

        public virtual float GetTotalHeight(Graphics dc, Font font)
        {
            var totalHeight = GetRowHeight(dc, font) * RowCount;
            return totalHeight;
        }

        public virtual float GetColumnWidth(Graphics dc, Font font)
        {
            var dayWidth = dc.GetTextExtent("00", font).Width;
            var dayNames = GetDayNames();

            foreach (var day in dayNames)
            {
                var dayNameWidth = dc.GetTextExtent(day, font).Width;
                if (dayNameWidth > dayWidth)
                    dayWidth = dayNameWidth;
            }

            return dayWidth + 4;
        }

        public CalendarCell GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex < 0 || rowIndex >= RowCount)
                throw new ArgumentOutOfRangeException(nameof(rowIndex));

            if (columnIndex < 0 || columnIndex >= ColumnCount)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            var index = rowIndex * ColumnCount + columnIndex;

            return cells[index];
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

                OnValueChanged();
            }
        }

        public BorderSettings? TodayBorder { get; set; }

        public DayNamesKind? DayNamesKind { get; set; }

        public virtual IFormatProvider? FormatProvider { get; set; }

        public virtual DayOfWeek? FirstDayOfWeek { get; set; }

        public virtual BorderSettings EffectiveTodayBorder()
        {
            return TodayBorder ?? DefaultTodayBorder;
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
        public virtual string[] GetDayNames()
        {
            var dayNames = DateUtils.GetDayNames(EffectiveDayNamesKind(), EffectiveFormatProvider());
            var result = new string[dayNames.Length];

            for (var i = 0; i < dayNames.Length; i++)
            {
                var dayOfWeek = (DayOfWeek)i;
                var index = GetDayOfWeekIndex(dayOfWeek);
                result[index] = dayNames[i];
            }

            return result;
        }

        public virtual int GetDayOfWeekIndex(DayOfWeek dayOfWeek)
        {
            var index = DateUtils.GetDayOfWeekIndex(dayOfWeek, EffectiveDayOfWeek());
            return index;
        }

        protected virtual void AssignItemsToListBox(VirtualListBox listBox)
        {
            var newSource = new ListSource<ListControlItem>();

            listBox.Columns.Clear();

            var minWidth = GetColumnWidth(listBox.MeasureCanvas, listBox.RealFont);

            for (var col = 0; col < XCalendar.ColumnCount; col++)
            {
                var column = new ListControlColumn($"Col{col}");
                column.SuggestedWidth = minWidth;
                listBox.Columns.Add(column);
            }

            ListControlItem headerItem = new();
            headerItem.HideSelection = true;

            var dayNames = GetDayNames();

            for (var col = 0; col < XCalendar.ColumnCount; col++)
            {
                var cellItem = headerItem.SafeCell(listBox.Columns[col]);
                cellItem.HorizontalAlignment = HorizontalAlignment.Center;
                cellItem.Text = dayNames[col];
            }

            newSource.Add(headerItem);

            for (var row = 0; row < XCalendar.RowCount; row++)
            {
                ListControlItem rowItem = new();

                for (var col = 0; col < XCalendar.ColumnCount; col++)
                {
                    var cell = GetCell(row, col);
                    var cellItem = rowItem.SafeCell(listBox.Columns[col]);
                    cellItem.HorizontalAlignment = HorizontalAlignment.Center;
                    cellItem.Text = cell.Date.Day.ToString();
                    if (!cell.IsCurrentMonth)
                    {
                        cellItem.ForegroundColor = Color.Gray;
                    }

                    if (cell.IsToday)
                    {
                        cellItem.Border = EffectiveTodayBorder();
                    }
                }

                newSource.Add(rowItem);
            }

            listBox.Items = newSource;
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

            for (int i = daysInMonth + index; i < CellCount; i++)
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

            AssignItemsToListBox(listBox);
        }
    }

    public class CalendarHeader : TransparentPanel
    {
        private readonly MonthSpeedButton monthPicker = new();
        private readonly SpeedTextButton yearPicker = new();

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

            monthPicker.MarginRight = 0;
            monthPicker.ImageVisible = false;

            monthPicker.VerticalAlignment = VerticalAlignment.Stretch;
            monthPicker.HorizontalAlignment = HorizontalAlignment.Center;
            monthPicker.Parent = this;

            yearPicker.HorizontalAlignment = HorizontalAlignment.Center;
            yearPicker.VerticalAlignment = VerticalAlignment.Stretch;
            yearPicker.Parent = this;
        }

        public event EventHandler? ValueChanged;

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

        public MonthPicker MonthPicker => monthPicker;

        public SpeedTextButton YearPicker => yearPicker;

        protected virtual void UpdatePickerValues()
        {
            monthPicker.ValueAsInt = Value.Month;
            yearPicker.Text = date.Year.ToString();
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

        protected virtual void OnPickerValueChanged()
        {
            if (suspendCounter > 0)
                return;
        }

        protected virtual void OnYearPickerValueChanged(object? sender, EventArgs e)
        {
            OnPickerValueChanged();
        }

        protected virtual void OnMonthPickerValueChanged(object? sender, EventArgs e)
        {
            OnPickerValueChanged();
        }
    }

    public class CalendarCell
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
