using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Possible values for SystemSettings.HasFeature parameter.
    /// </summary>
    public enum SystemSettingsFeature
    {
        /// <summary>
        /// System feature identifier.
        /// </summary>
        CanDrawFrameDecorations = 1,

        /// <summary>
        /// System feature identifier.
        /// </summary>
        CanIconizeFrame,

        /// <summary>
        /// System feature identifier.
        /// </summary>
        TabletPresent,
    }
}
