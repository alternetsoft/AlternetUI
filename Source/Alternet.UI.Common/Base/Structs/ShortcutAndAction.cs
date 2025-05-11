using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a combination of a shortcut and its associated action.
    /// </summary>
    public struct ShortcutAndAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShortcutAndAction"/> struct
        /// with the specified shortcut and action.
        /// </summary>
        /// <param name="shortcut">The shortcut information.</param>
        /// <param name="action">The action associated with the shortcut.</param>
        public ShortcutAndAction(ShortcutInfo shortcut, Action action)
        {
            Shortcut = shortcut;
            Action = action;
        }

        /// <summary>
        /// Gets or sets the shortcut information.
        /// </summary>
        public ShortcutInfo Shortcut { get; set; }

        /// <summary>
        /// Gets or sets the action associated with the shortcut.
        /// </summary>
        public Action Action { get; set; }
    }
}