using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    public partial class XCalendar : HiddenBorder
    {
        private const int DayCellCount = 42;

        private const int DayRowCount = 6;

        private const int ColumnCount = 7;

        public static Color DefaultSurroundDayColor = Color.Gray;
        
        public static DayNamesKind DefaultDayNamesKind = UI.DayNamesKind.Abbreviated;
        
        public static BorderSettings? DefaultTodayBorder;

        private readonly CalendarCell[] cells = new CalendarCell[DayCellCount];
        private readonly CalendarListBox listBox = new();
        private readonly CalendarHeader header = new();
        private readonly CalendarHeaderItem headerItem;

        private DateOnly date;
        private int suspendHeaderEventsCounter;
        private Color? surroundDayColor;
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

            MinimumSize = new(400, 400);
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

            header.Parent = this;
            listBox.Parent = this;
        }

        public CalendarListBox ListBox => listBox;

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
            var totalHeight = GetRowHeight(dc, font) * DayRowCount;
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
            if (rowIndex < 0 || rowIndex >= DayRowCount)
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

        public virtual Color? SurroundDayColor
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

        public virtual BorderSettings EffectiveTodayBorder()
        {
            return TodayBorder ?? DefaultTodayBorder ?? BorderSettings.Default;
        }

        public virtual Color EffectiveSurroundDayColor()
        {
            return SurroundDayColor ?? DefaultSurroundDayColor ?? Color.Gray;
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

        protected virtual void UpdateColumnWidth()
        {
            var minWidth = GetColumnWidth(listBox.MeasureCanvas, listBox.RealFont);
            foreach (var col in listBox.Columns)
            {
                col.SuggestedWidth = minWidth;
            }
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
                var cellItem = headerItem.Cells[col];
                cellItem.Text = dayNames[col];
            }
        }

        protected virtual void OnListBoxBackColorChanged(object? sender, EventArgs e)
        {
            BackColor = listBox.BackColor;
        }

        protected virtual void UpdateDayItems()
        {
            for (var row = 1; row < XCalendar.DayRowCount; row++)
            {
                for (var col = 0; col < XCalendar.ColumnCount; col++)
                {
                    var cell = GetCell(row, col);
                    var rowItem = listBox.Items[row];
                    var cellItem = (CalendarCellItem)rowItem.Cells[col];
                    cellItem.Text = cell.Date.Day.ToString();
                    cellItem.ForegroundColor = !cell.IsCurrentMonth ? EffectiveSurroundDayColor() : null;
                    cellItem.Border = cell.IsToday ? EffectiveTodayBorder() : null;
                }
            }
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

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            UpdateColumnWidth();
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

    public class CalendarRowItem : ListControlItem
    {
    }

    public class CalendarCellItem : ListControlItem
    {
    }

    public class CalendarHeaderItem : ListControlItem
    {
    }

    public class CalendarHeaderCellItem : ListControlItem
    {
    }

    public class CalendarListBox : VirtualListBox
    {
        public CalendarListBox()
        {
            SelectionMode = ListBoxSelectionMode.None;
        }
    }

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
