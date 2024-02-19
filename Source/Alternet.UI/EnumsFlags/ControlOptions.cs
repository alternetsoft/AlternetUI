using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines different options for the <see cref="Control"/>.
    /// </summary>
    [Flags]
    public enum ControlOptions
    {
        /// <summary>
        /// No flags are specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies whether to call <see cref="Control.Refresh"/> when
        /// <see cref="Control.CurrentState"/> property is changed.
        /// </summary>
        RefreshOnCurrentState = 1,

        /// <summary>
        /// Specifies whether to draw default border, which is specified in
        /// <see cref="Control.Borders"/> property.
        /// </summary>
        DrawDefaultBorder = 2,

        /// <summary>
        /// Specifies whether to draw default background, which is specified
        /// in <see cref="Control.Backgrounds"/> or <see cref="Control.Background"/> properties.
        /// </summary>
        DrawDefaultBackground = 4,
    }
}
