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
        /// Initializes a new instance of the <see cref="YearPicker"/> class.
        /// </summary>
        public YearPicker()
        {
            Minimum = DateUtils.MinimumDateTime.Year;
            Maximum = DateUtils.MaximumDateTime.Year;
            Value = DateTime.Now.Year;
        }
    }
}
