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
    [Flags]
    public enum ControlFlags
    {
        /// <summary>
        /// Indicates that <see cref="Parent"/> was already assigned.
        /// </summary>
        /// <remarks>
        /// This flag is set after <see cref="Parent"/> was changed. It can be used
        /// in the <see cref="ParentChanged"/> event. It allows
        /// to determine whether <see cref="Parent"/> is changed for the first time.
        /// </remarks>
        ParentAssigned = 1,

        /// <summary>
        /// Indicates that start location was applied to the window.
        /// </summary>
        /// <remarks>
        /// Start location is applied only once.
        /// This flag is set after start location was applied.
        /// This flag is used in <see cref="Window"/>.
        /// </remarks>
        StartLocationApplied = 2,
    }
}
