using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Internal control flags.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum ControlFlags
    {
        /// <summary>
        /// Indicates that parent was assigned to the control.
        /// </summary>
        /// <remarks>
        /// This flag is set after parent of the control was changed.
        /// </remarks>
        ParentAssigned = 1,

        /// <summary>
        /// Indicates that start location was applied to the window.
        /// </summary>
        /// <remarks>
        /// Start location is applied only once.
        /// This flag is set after start location was applied.
        /// </remarks>
        StartLocationApplied = 2,

        /// <summary>
        /// Indicates that control is forced to update handler's text next time Text property is changed.
        /// </summary>
        ForceTextChange = 4,
    }
}
