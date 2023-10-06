using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ScrollViewer"/> behavior and appearance.
    /// </summary>
    public abstract class ScrollViewerHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="ScrollViewer"/> this handler provides the implementation for.
        /// </summary>
        public new ScrollViewer Control => (ScrollViewer)base.Control;

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}