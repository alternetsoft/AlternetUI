using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which are <see cref="Control"/> related.
    /// </summary>
    public static class ControlUtils
    {
        private static Control? empty;

        /// <summary>
        /// Gets an empty control for the debug purposes.
        /// </summary>
        public static Control Empty
        {
            get
            {
                return empty ??= new();
            }
        }
    }
}
