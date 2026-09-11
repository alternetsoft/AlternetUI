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
            public override ThemedColor? ForegroundColor
            {
                get
                {
                    return base.ForegroundColor ?? Data.DateAttr?.TextColor;
                }

                set
                {
                    base.ForegroundColor = value;
                }
            }

            /// <inheritdoc/>
            public override FontStyle? FontStyle
            {
                get
                {
                    return base.FontStyle ?? Data.DateAttr?.FontStyle;
                }

                set
                {
                    base.FontStyle = value;
                }
            }

            /// <inheritdoc/>
            public override ThemedColor? BackgroundColor
            {
                get
                {
                    return base.BackgroundColor ?? Data.DateAttr?.BackgroundColor;
                }

                set
                {
                    base.BackgroundColor = value;
                }
            }

            /// <inheritdoc/>
            public override BorderSettings? Border
            {
                get => base.Border ?? Data.DateAttr?.Border;
                set => base.Border = value;
            }

            /// <inheritdoc/>
            public override bool IsSelectedCell(ItemCellContext prm)
            {
                return Data.IsCurrent && Data.IsVisible && !Data.IsRestricted && Data.IsCurrentMonth;
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
        /// Represents the event arguments for a day header click event in the calendar control
        /// </summary>
        public class DayHeaderClickEventArgs : BaseEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DayHeaderClickEventArgs"/>
            /// class with the specified list box cell click event arguments.
            /// </summary>
            /// <param name="e">The list box cell click event arguments.</param>
            public DayHeaderClickEventArgs(ListBoxCellClickEventArgs e)
            {
                ClickArgs = e;
            }

            /// <summary>
            /// Gets the day of the week represented by the header cell that was clicked.
            /// </summary>
            public DayOfWeek DayOfWeek { get; set; }

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
        /// Represents the event arguments for querying whether a specific date is a holiday in the calendar control.
        /// </summary>
        public class QueryHolidayEventArgs : BaseEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="QueryHolidayEventArgs"/> class.
            /// </summary>
            public QueryHolidayEventArgs()
            {
            }

            /// <summary>
            /// Gets the calendar cell associated with the queried day.
            /// </summary>
            public DateOnly Date { get; internal set; }

            /// <summary>
            /// Gets or sets a value indicating whether the specified date is a holiday.
            /// </summary>
            public bool IsHoliday { get; set; }
        }

        /// <summary>
        /// Represents the event arguments for querying day attributes in the calendar control.
        /// </summary>
        public class QueryDayAttributesEventArgs : BaseEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="QueryDayAttributesEventArgs"/> class.
            /// </summary>
            public QueryDayAttributesEventArgs()
            {
            }

            /// <summary>
            /// Gets the calendar cell associated with the queried day.
            /// </summary>
            public CalendarCell Cell { get; internal set; } = CalendarCell.Default;

            /// <summary>
            /// Gets or sets the date attributes associated with the queried day,
            /// allowing customization of the day's appearance and behavior.
            /// </summary>
            public IXCalendarDateAttr? DateAttr { get; set; }
        }

        /// <summary>
        /// Represents a cell in the calendar control, which contains information about a specific day,
        /// including its date, position in the grid, and whether it belongs to the current month or is today.
        /// </summary>
        public partial class CalendarCell : BaseObject
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
            public IXCalendarDateAttr? DateAttr { get; internal set; }

            /// <summary>
            /// Gets a value indicating whether the cell represents a day in the current month.
            /// </summary>
            public bool IsCurrentMonth { get; internal set; }

            /// <summary>
            /// Gets a value indicating whether the cell is visible in the calendar grid.
            /// </summary>
            public bool IsVisible { get; internal set; }

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

            /// <summary>
            /// Gets the border settings associated with the cell, allowing customization of the cell's border appearance.
            /// </summary>
            public BorderSettings? Border { get; internal set; }
        }

        /// <summary>
        /// Represents a collection of predefined date attributes for the calendar control,
        /// </summary>
        public partial class CalendarDateAttributes : BaseObject
        {
            private IXCalendarDateAttr? red;
            private IXCalendarDateAttr? blue;
            private IXCalendarDateAttr? green;
            private IXCalendarDateAttr? transparent;

            /// <summary>
            /// Initializes a new instance
            /// of the <see cref="CalendarDateAttributes"/> class with the specified owner calendar control.
            /// </summary>
            internal CalendarDateAttributes()
            {
            }

            /// <summary>
            /// Gets the <see cref="IXCalendarDateAttr"/> attributes with red color of the foreground
            /// used as a highlight for specific dates.
            /// </summary>
            public IXCalendarDateAttr? Red
            {
                get
                {
                    return red ??= Create(ThemedColors.Red);
                }
            }

            /// <summary>
            /// Gets the <see cref="IXCalendarDateAttr"/> attributes with blue color of the foreground
            /// used as a highlight for specific dates.
            /// </summary>
            public IXCalendarDateAttr? Blue
            {
                get
                {
                    return blue ??= Create(ThemedColors.Blue);
                }
            }

            /// <summary>
            /// Gets the <see cref="IXCalendarDateAttr"/> attributes with green color of the foreground
            /// used as a highlight for specific dates.
            /// </summary>
            public IXCalendarDateAttr? Transparent
            {
                get
                {
                    return transparent ??= Create(ExactColors.Transparent);
                }
            }

            /// <summary>
            /// Gets the <see cref="IXCalendarDateAttr"/> attributes with green color of the foreground
            /// used as a highlight for specific dates.
            /// </summary>
            public IXCalendarDateAttr? Green
            {
                get
                {
                    return green ??= Create(ThemedColors.Green);
                }
            }

            /// <summary>
            /// Creates a new instance of <see cref="IXCalendarDateAttr"/> with the specified text color.
            /// </summary>
            /// <param name="textColor">The text color for the calendar date.</param>
            /// <returns>The created <see cref="IXCalendarDateAttr"/> instance.</returns>
            protected virtual IXCalendarDateAttr Create(ThemedColor textColor)
            {
                var result = new XCalendarDateAttr();
                result.TextColor = textColor;
                result.SetImmutable();
                return result;
            }
        }
    }
}
