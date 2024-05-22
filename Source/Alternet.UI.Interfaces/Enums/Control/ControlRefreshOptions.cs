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
    public enum ControlRefreshOptions : ulong
    {
        /// <summary>
        /// No flags are specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies whether to call <see cref="Control.Refresh"/> when
        /// <see cref="Control.CurrentState"/> property is changed, control has
        /// border specified in <see cref="Control.StateObjects"/> and
        /// <see cref="ControlStateSettings.HasOtherBorders"/> returns <c>true</c>.
        /// </summary>
        RefreshOnBorder = 1,

        /// <summary>
        /// Specifies whether to call <see cref="Control.Refresh"/> when
        /// <see cref="Control.CurrentState"/> property is changed, control has
        /// image specified in <see cref="Control.StateObjects"/> and
        /// <see cref="ControlStateSettings.HasOtherImages"/> returns <c>true</c>.
        /// </summary>
        RefreshOnImage = 2,

        /// <summary>
        /// Specifies whether to call <see cref="Control.Refresh"/> when
        /// <see cref="Control.CurrentState"/> property is changed, control has
        /// background specified in <see cref="Control.StateObjects"/> and
        /// <see cref="ControlStateSettings.HasOtherBackgrounds"/> returns <c>true</c>.
        /// </summary>
        RefreshOnBackground = 4,

        /// <summary>
        /// Specifies whether to call <see cref="Control.Refresh"/> when
        /// <see cref="Control.CurrentState"/> property is changed, control has
        /// colors specified in <see cref="Control.StateObjects"/> and
        /// <see cref="ControlStateSettings.HasOtherColors"/> returns <c>true</c>.
        /// </summary>
        RefreshOnColor = 8,

        /// <summary>
        /// Specifies whether to call <see cref="Control.Refresh"/> when
        /// <see cref="Control.CurrentState"/> property is changed.
        /// </summary>
        RefreshOnState = 16,
    }
}
