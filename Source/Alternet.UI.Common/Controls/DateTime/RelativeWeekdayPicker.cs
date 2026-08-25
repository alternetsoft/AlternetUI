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
        /// Initializes a new instance of the <see cref="RelativeWeekdayPicker"/> class.
        /// </summary>
        public RelativeWeekdayPicker()
        {
            EnumType = typeof(RelativeWeekday);

            SetDisplayText(RelativeWeekday.First, CommonStrings.Default.RelativeWeekdayFirst);
            SetDisplayText(RelativeWeekday.Second, CommonStrings.Default.RelativeWeekdaySecond);
            SetDisplayText(RelativeWeekday.Third, CommonStrings.Default.RelativeWeekdayThird);
            SetDisplayText(RelativeWeekday.Fourth, CommonStrings.Default.RelativeWeekdayFourth);
            SetDisplayText(RelativeWeekday.Last, CommonStrings.Default.RelativeWeekdayLast);

            Value = RelativeWeekday.First;
        }

        /// <summary>
        /// Gets or sets the selected relative weekday.
        /// </summary>
        public new virtual RelativeWeekday Value
        {
            get => (RelativeWeekday?)base.Value ?? RelativeWeekday.First;
            set => base.Value = value;
        }
    }
}
