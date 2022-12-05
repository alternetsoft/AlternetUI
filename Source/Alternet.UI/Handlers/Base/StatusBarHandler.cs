using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="StatusBar"/> behavior and appearance.
    /// </summary>
    public abstract class StatusBarHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new StatusBar Control => (StatusBar)base.Control;

        /// <summary>
        /// Gets or sets a value indicating whether a sizing grip is displayed in the lower-right corner of the control.
        /// </summary>
        public abstract bool SizingGripVisible { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}