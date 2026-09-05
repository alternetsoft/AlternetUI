using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class XCalendar
    {
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
            public IXCalendarDateAttr? DateAttr { get; internal set; }

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
            private IXCalendarDateAttr? red;
            private IXCalendarDateAttr? blue;
            private IXCalendarDateAttr? green;

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
                    return red ??= Create(LightDarkColors.Red);
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
                    return blue ??= Create(LightDarkColors.Blue);
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
                    return green ??= Create(LightDarkColors.Green);
                }
            }

            /// <summary>
            /// Creates a new instance of <see cref="IXCalendarDateAttr"/> with the specified text color.
            /// </summary>
            /// <param name="textColor">The text color for the calendar date.</param>
            /// <returns>The created <see cref="IXCalendarDateAttr"/> instance.</returns>
            protected virtual IXCalendarDateAttr Create(Color textColor)
            {
                var result = new XCalendarDateAttr();
                result.TextColor = textColor;
                result.SetImmutable();
                return result;
            }
        }
    }
}
