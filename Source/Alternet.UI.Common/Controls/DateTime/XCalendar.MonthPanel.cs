using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class XCalendar
    {
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

            /// <inheritdoc/>
            public override bool? IsDarkBackgroundOverride
            {
                get => base.IsDarkBackgroundOverride;
                set
                {
                    base.IsDarkBackgroundOverride = value;
                }
            }

            /// <summary>
            /// Gets the rows of the month picker panel, which contain the month buttons for selection.
            /// </summary>
            public IReadOnlyList<TransparentPanel> Rows => rows;

            /// <summary>
            /// Gets the buttons in the month picker panel, which represent the individual months for selection.
            /// </summary>
            public IReadOnlyList<SpeedTextButton> Buttons => buttons;

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
    }
}
