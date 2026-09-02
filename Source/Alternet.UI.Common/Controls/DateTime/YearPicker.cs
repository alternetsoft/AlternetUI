using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a year.
    /// </summary>
    public partial class YearPicker : XIntPicker
    {
        /// <summary>
        /// Gets or sets the format provider used for culture-specific formatting of date and time values.
        /// </summary>
        public virtual IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YearPicker"/> class.
        /// </summary>
        public YearPicker()
        {
            Minimum = DateUtils.MinimumDateTime(FormatProvider).Year;
            Maximum = DateUtils.MaximumDateTime(FormatProvider).Year;
            Value = DateTime.Now.Year;
        }
    }
}
