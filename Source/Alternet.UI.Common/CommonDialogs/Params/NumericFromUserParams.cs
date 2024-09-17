using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties which allow to customize dialog box which asks a numeric
    /// value from the user.
    /// </summary>
    public class NumericFromUserParams<T> : ValueFromUserParams<T>
    {
        /// <summary>
        /// Gets or sets minimal value. Makes sense for numbers.
        /// </summary>
        public T? MinValue;

        /// <summary>
        /// Gets or sets maximal value. Makes sense for numbers.
        /// </summary>
        public T? MaxValue;
    }
}
