using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a month from the Gregorian calendar.
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class MonthPicker : EnumPicker
    {
        /// <summary>
        /// Gets or sets the default text case rule for the displayed labels.
        /// </summary>
        public static TextCaseRule DefaultTextCase = TextCaseRule.SentenceCase;

        private IFormatProvider? formatProvider;
        private MonthNamesKind monthNamesKind = MonthNamesKind.Full;
        private TextCaseRule textCase = DefaultTextCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonthPicker"/> class.
        /// </summary>
        public MonthPicker()
        {
            EnumType = typeof(CalendarMonth);
            UpdateMonthLabels();
            Value = CalendarMonth.January;
        }

        /// <summary>
        /// Gets or sets the kind of month names to display (full or abbreviated).
        /// </summary>
        public virtual MonthNamesKind MonthNamesKind
        {
            get => monthNamesKind;
            set
            {
                if (monthNamesKind != value)
                {
                    monthNamesKind = value;
                    UpdateMonthLabels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text case rule for the displayed labels.
        /// </summary>
        public virtual TextCaseRule TextCase
        {
            get => textCase;
            set
            {
                if (textCase != value)
                {
                    textCase = value;
                    UpdateMonthLabels();
                    ReassignValue();
                }
            }
        }

        /// <summary>
        /// Gets or sets the format provider used to display month names. If not set, the current culture is used by default.
        /// </summary>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider
        {
            get => formatProvider;
            set
            {
                if (formatProvider != value)
                {
                    formatProvider = value;
                    UpdateMonthLabels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected month.
        /// </summary>
        public new virtual CalendarMonth Value
        {
            get => (CalendarMonth?)base.Value ?? CalendarMonth.January;
            set => base.Value = value;
        }

        /// <summary>
        /// Formats the label for a given month.
        /// This method can be overridden to customize the display text for each month.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="monthName">The default name of the month.</param>
        /// <returns>The formatted label for the month.</returns>
        protected virtual string FormatMonthLabel(CalendarMonth month, string monthName)
        {
            return StringUtils.ChangeCase(monthName, textCase) ?? string.Empty;
        }

        /// <summary>
        /// Updates the month labels based on the current format provider and the selected <see cref="MonthNamesKind"/>.
        /// If no format provider is set, the current culture will be used.
        /// This method retrieves the month names from the format provider's DateTimeFormat
        /// and updates the text of each list item accordingly.
        /// </summary>
        protected virtual void UpdateMonthLabels()
        {
            string[] months = DateUtils.GetMonthNames(MonthNamesKind, FormatProvider);

            foreach(var item in ListItems)
            {
                if (item.Value is CalendarMonth month)
                {
                    int monthIndex = (int)month - 1;
                    if (monthIndex >= 0 && monthIndex < months.Length)
                    {
                        item.Text = FormatMonthLabel(month, months[monthIndex]);
                    }
                }
            }

            Invalidate();
        }
    }
}
