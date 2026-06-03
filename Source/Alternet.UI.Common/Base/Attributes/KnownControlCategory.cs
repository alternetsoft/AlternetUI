using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines known control categories for design-time support.
    /// </summary>
    public static class KnownControlCategory
    {
        /// <summary>
        /// Indicates that the control is hidden and should not be displayed in the designer toolbox.
        /// </summary>
        public const string Hidden = "Hidden";

        /// <summary>
        /// Indicates that the control is for internal use only and should not be displayed in the designer toolbox.
        /// </summary>
        public const string Internal = "Internal";
    }
}
