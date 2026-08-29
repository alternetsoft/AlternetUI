using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class RepeatPatternPicker
    {
        /// <summary>
        /// Represents an abstract control which serves as a base for date repeat pattern rule pickers,
        /// such as daily, weekly, monthly, and yearly repeat pattern rule pickers.
        /// </summary>
        /// <typeparam name="TValue">The type of the repeat pattern rule.</typeparam>
        [ControlCategory(KnownControlCategory.Date)]
        public abstract partial class DateRepeatPatternRulePicker<TValue> : HiddenBorder
            where TValue : DateRepeatPatternRule
        {
            /// <summary>
            /// Gets or sets the default minimum margin for child controls within the repeat pattern rule picker.
            /// </summary>
            public static Thickness DefaultMinChildMargin = (2, 2, 2, 2);

            /// <summary>
            /// Gets or sets the default padding for the repeat pattern rule picker.
            /// </summary>
            public static Thickness DefaultPadding = 5;

            private readonly TValue data;

            /// <summary>
            /// Initializes a new instance of the <see cref="DateRepeatPatternRulePicker{TValue}"/> class.
            /// </summary>
            public DateRepeatPatternRulePicker(TValue data)
            {
                MinChildMargin = DefaultMinChildMargin;
                Padding = DefaultPadding;
                this.data = data;
            }

            /// <summary>
            /// Occurs when the value of the repeat pattern rule changes.
            /// </summary>
            public event PropertyChangedEventHandler? ValueChanged
            {
                add
                {
                    data.PropertyChanged += value;
                }

                remove
                {
                    data.PropertyChanged -= value;
                }
            }

            /// <summary>
            /// Gets or sets the format provider used for formatting and parsing date and time values.
            /// </summary>
            public virtual IFormatProvider? FormatProvider { get; set; }

            /// <summary>
            /// Gets the value of the date repeat pattern rule.
            /// </summary>
            public virtual TValue Value
            {
                get
                {
                    return data;
                }
            }
        }
    }
}
