using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly
    /// from your code.
    /// </summary>
    public class UsableDuringInitializationAttribute : BaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsableDuringInitializationAttribute"/> class.
        /// </summary>
        public UsableDuringInitializationAttribute(bool usable)
        {
        }
    }
}
