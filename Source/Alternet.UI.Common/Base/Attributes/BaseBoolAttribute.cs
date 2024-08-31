using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Base attribute with boolean parameter.
    /// </summary>
    public class BaseBoolAttribute : BaseValueAttribute<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBoolAttribute"/> class.
        /// </summary>
        /// <param name="value">Value.</param>
        public BaseBoolAttribute(bool value)
            : base(value)
        {
        }
    }
}
