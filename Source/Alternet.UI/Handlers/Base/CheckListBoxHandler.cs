using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="CheckListBoxHandler"/> behavior and appearance.
    /// </summary>
    public abstract class CheckListBoxHandler : ListBoxHandler
    {
        /// <inheritdoc/>
        public new CheckListBox Control => (CheckListBox)base.Control;
        /// <inheritdoc/>

        protected override bool VisualChildNeedsNativeControl => true;
    }
}