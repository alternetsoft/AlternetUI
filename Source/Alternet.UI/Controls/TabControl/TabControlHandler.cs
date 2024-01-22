using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="TabControl"/> behavior and appearance.
    /// </summary>
    internal abstract class TabControlHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="TabControl"/> this handler provides the implementation for.
        /// </summary>
        public new TabControl Control => (TabControl)base.Control;

        /// <summary>
        /// Gets or sets the area of the control (for example, along the top) where the tabs are aligned.
        /// </summary>
        public abstract TabAlignment TabAlignment { get; set; }

        internal new Native.TabControl NativeControl =>
            (Native.TabControl)base.NativeControl!;

        /*/// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;*/
    }
}