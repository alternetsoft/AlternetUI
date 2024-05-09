using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the mouse button events.
    /// </summary>
    internal class ControlMouseButtonEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlMouseButtonEventArgs"/> class.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <param name="e">Mouse button event arguments.</param>
        public ControlMouseButtonEventArgs(Control control, MouseEventArgs e)
        {
            Control = control;
            MouseButtonEventArgs = e;
        }

        /// <summary>
        /// Gets affected control.
        /// </summary>
        public Control Control { get; }

        /// <summary>
        /// Gets mouse button event arguments.
        /// </summary>
        public MouseEventArgs MouseButtonEventArgs { get; }
    }
}
