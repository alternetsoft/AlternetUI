using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether field or property can be reset automatically.
    /// </summary>
    public class AutoResetAttribute : BaseBoolAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoResetAttribute"/> class.
        /// </summary>
        /// <param name="value">Value</param>
        public AutoResetAttribute(bool value = true)
            : base(value)
        {
        }
    }
}
