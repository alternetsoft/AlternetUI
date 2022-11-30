using Alternet.Drawing;
using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ToolbarItem"/> behavior and appearance.
    /// </summary>
    public abstract class ToolbarItemHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new ToolbarItem Control => (ToolbarItem)base.Control;

        /// <summary>
        /// Gets or sets a menu used as this toolbar item drop-down.
        /// </summary>
        public abstract Menu? DropDownMenu { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item is checkable.
        /// </summary>
        public abstract bool IsCheckable { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}