using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class CalendarCell
    {
        internal CalendarCell()
        {
        }

        public int RowIndex { get; internal set; }

        public int ColumnIndex { get; internal set; }

        public DateOnly Date { get; internal set; }
    }

    public class CalendarCells
    {
        public const int CellCount = 42;

        public const int RowCount = 6;

        public const int ColumnCount = 7;

        private DateOnly date;
        private readonly CalendarCell[] cells = new CalendarCell[CellCount];

        public CalendarCells()
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

        public DateOnly Value
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

        protected virtual void OnValueChanged()
        {
            var firstDayOfMonth = DateUtils.GetFirstDateOfMonth(date);
            var lastDayOfMonth = DateUtils.GetLastDateOfMonth(date);
            var daysInMonth = DateUtils.GetDaysInMonth(date);
            var dayOfWeek = firstDayOfMonth.DayOfWeek;
            var index = DateUtils.GetDayOfWeekIndex(dayOfWeek, DateUtils.SystemFirstDayOfWeek);

            for (int i = 0; i < daysInMonth; i++)
            {
                var cell = cells[index + i];
                var d = firstDayOfMonth.AddDays(i);
                cell.Date = d;
            }

            for (int i = daysInMonth + index; i < CellCount; i++)
            {
                var cell = cells[i];
                cell.Date = lastDayOfMonth.AddDays(i - daysInMonth - index + 1);
            }
        }
    }
}
