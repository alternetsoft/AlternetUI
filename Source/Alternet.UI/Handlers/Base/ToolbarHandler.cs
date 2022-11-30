using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Toolbar"/> behavior and appearance.
    /// </summary>
    public abstract class ToolbarHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new Toolbar Control => (Toolbar)base.Control;

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}