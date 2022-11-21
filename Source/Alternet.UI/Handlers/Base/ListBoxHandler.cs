using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ListBox"/> behavior and appearance.
    /// </summary>
    public abstract class ListBoxHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new ListBox Control => (ListBox)base.Control;

        /// <summary>
        /// Ensures that the item is visible within the control, scrolling the contents of the control, if necessary.
        /// </summary>
        public abstract void EnsureVisible(int itemIndex);

        /// <summary>
        /// Returns the zero-based index of the item at the specified coordinates.
        /// </summary>
        public abstract int? HitTest(Point position);

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}