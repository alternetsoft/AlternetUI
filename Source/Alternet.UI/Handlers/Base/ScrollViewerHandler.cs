using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ScrollViewer"/> behavior and appearance.
    /// </summary>
    public abstract class ScrollViewerHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new ScrollViewer Control => (ScrollViewer)base.Control;

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}