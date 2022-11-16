using Alternet.Drawing;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="ListView"/> behavior and appearance.
    /// </summary>
    public abstract class ListViewHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new ListView Control => (ListView)base.Control;

        /// <summary>
        /// Gets or sets a value indicating whether the label text of the items can be edited.
        /// </summary>
        public abstract bool AllowLabelEdit { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}