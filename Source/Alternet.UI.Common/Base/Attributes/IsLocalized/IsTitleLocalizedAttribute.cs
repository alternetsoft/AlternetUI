using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether text property is localized or not.
    /// </summary>
    public class IsTitleLocalizedAttribute : BaseBoolAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IsTitleLocalizedAttribute"/> class.
        /// </summary>
        /// <param name="value">Value</param>
        public IsTitleLocalizedAttribute(bool value)
            : base(value)
        {
        }
    }
}
