using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether text property is localized or not.
    /// </summary>
    public class IsCsLocalizedAttribute : BaseBoolAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsCsLocalizedAttribute"/> class.
        /// </summary>
        /// <param name="value">Value</param>
        public IsCsLocalizedAttribute(bool value)
            : base(value)
        {
        }
    }
}