using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether text property is localized or not.
    /// </summary>
    public class IsLocalizedAttribute : BaseBoolAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsLocalizedAttribute"/> class.
        /// </summary>
        /// <param name="value">Value</param>
        public IsLocalizedAttribute(bool value)
            : base(value)
        {
        }
    }
}
