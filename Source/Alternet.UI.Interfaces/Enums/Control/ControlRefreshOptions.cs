using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines how to refresh control after it's visual state is changed.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum ControlRefreshOptions : ulong
    {
        /// <summary>
        /// Do not refresh.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies whether to refresh control after it's visual state is changed.
        /// Control is refreshed only if it has
        /// different borders specified in the state objects.
        /// </summary>
        RefreshOnBorder = 1,

        /// <summary>
        /// Specifies whether to refresh control after it's visual state is changed.
        /// Control is refreshed only if it has
        /// different images specified in the state objects.
        /// </summary>
        RefreshOnImage = 2,

        /// <summary>
        /// Specifies whether to refresh control after it's visual state is changed.
        /// Control is refreshed only if it has
        /// different backgrounds specified in the state objects.
        /// </summary>
        RefreshOnBackground = 4,

        /// <summary>
        /// Specifies whether to refresh control after it's visual state is changed.
        /// Control is refreshed only if it has
        /// different colors specified in the state objects.
        /// </summary>
        RefreshOnColor = 8,

        /// <summary>
        /// Specifies whether to refresh control after it's visual state is changed.
        /// </summary>
        RefreshOnState = 16,
    }
}
