using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Flags used to specify the rounded corner preference for a window.
    /// </summary>
    public enum WindowRoundedCornerPreference
    {
        /// <summary>
        /// Let the system decide whether to round window corners.
        /// </summary>
        Default = 0,

        /// <summary>
        /// No round window corners.
        /// </summary>
        NotRound = 1,

        /// <summary>
        /// Round the corners.
        /// </summary>
        Round = 2,

        /// <summary>
        /// Round the corners if appropriate, with a smaller radius.
        /// </summary>
        RoundSmall = 3,
    }
}