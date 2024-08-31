using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether text property is localized or not.
    /// </summary>
    public class IsToolTipLocalizedAttribute : BaseBoolAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsToolTipLocalizedAttribute"/> class.
        /// </summary>
        /// <param name="value">Value</param>
        public IsToolTipLocalizedAttribute(bool value)
            : base(value)
        {
        }
    }
}
