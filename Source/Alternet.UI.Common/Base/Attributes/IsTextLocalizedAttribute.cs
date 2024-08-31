using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether text property is localized or not.
    /// </summary>
    public class IsTextLocalizedAttribute : BaseBoolAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsTextLocalizedAttribute"/> class.
        /// </summary>
        /// <param name="value">Value</param>
        public IsTextLocalizedAttribute(bool value)
            : base(value)
        {
        }
    }
}
