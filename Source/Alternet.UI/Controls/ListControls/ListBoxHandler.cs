using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific
    /// <see cref="ListBox"/> behavior and appearance.
    /// </summary>
    internal abstract class ListBoxHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="ListBox"/> this handler provides the
        /// implementation for.
        /// </summary>
        public new ListBox Control => (ListBox)base.Control;

        /*/// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;*/

        /// <summary>
        /// Ensures that the item is visible within the control, scrolling
        /// the contents of the control, if necessary.
        /// </summary>
        public abstract void EnsureVisible(int itemIndex);

        /// <summary>
        /// Returns the zero-based index of the item at the specified coordinates.
        /// </summary>
        public abstract int? HitTest(PointD position);
    }
}