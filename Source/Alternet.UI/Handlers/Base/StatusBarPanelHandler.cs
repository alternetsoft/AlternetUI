using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="StatusBarPanel"/> behavior and appearance.
    /// </summary>
    public abstract class StatusBarPanelHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new StatusBarPanel Control => (StatusBarPanel)base.Control;

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}