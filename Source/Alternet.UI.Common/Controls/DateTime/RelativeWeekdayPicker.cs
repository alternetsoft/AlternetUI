using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// A control that allows users to select a relative occurrence of a weekday within a month,
    /// such as "first Monday" or "last Friday". Uses the <see cref="RelativeWeekday"/> enum to represent the selected value.
    /// </summary>
    [ControlCategory(KnownControlCategory.Date)]
    public partial class RelativeWeekdayPicker : EnumPicker
    {
        /// <summary>
        /// Gets or sets the default text case rule for the displayed labels.
        /// </summary>
        public static TextCaseRule DefaultTextCase = TextCaseRule.SentenceCase;

        private TextCaseRule textCase = DefaultTextCase;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelativeWeekdayPicker"/> class.
        /// </summary>
        public RelativeWeekdayPicker()
        {
            EnumType = typeof(RelativeWeekday);
            UpdateDayLabels();
            Value = RelativeWeekday.First;
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
                    UpdateDayLabels();
                    ReassignValue();
                }
            }
        }

        /// <summary>
        /// Gets or sets the selected relative weekday.
        /// </summary>
        public new virtual RelativeWeekday Value
        {
            get => (RelativeWeekday?)base.Value ?? RelativeWeekday.First;
            set => base.Value = value;
        }

        /// <summary>
        /// Formats the label for a given relative weekday.
        /// This method can be overridden to customize the display text for each relative weekday.
        /// </summary>
        /// <param name="day">The relative weekday.</param>
        /// <param name="dayName">The default name of the relative weekday.</param>
        /// <returns>The formatted label for the relative weekday.</returns>
        protected virtual string FormatDayLabel(RelativeWeekday day, string dayName)
        {
            return StringUtils.ChangeCase(dayName, textCase) ?? string.Empty;
        }

        /// <summary>
        /// Updates the day labels based on the current settings.
        /// </summary>
        protected virtual void UpdateDayLabels()
        {
            SetDisplayText(RelativeWeekday.First, FormatDayLabel(RelativeWeekday.First, CommonStrings.Default.RelativeWeekdayFirst));
            SetDisplayText(RelativeWeekday.Second, FormatDayLabel(RelativeWeekday.Second, CommonStrings.Default.RelativeWeekdaySecond));
            SetDisplayText(RelativeWeekday.Third, FormatDayLabel(RelativeWeekday.Third, CommonStrings.Default.RelativeWeekdayThird));
            SetDisplayText(RelativeWeekday.Fourth, FormatDayLabel(RelativeWeekday.Fourth, CommonStrings.Default.RelativeWeekdayFourth));
            SetDisplayText(RelativeWeekday.Last, FormatDayLabel(RelativeWeekday.Last, CommonStrings.Default.RelativeWeekdayLast));
        }
    }
}
