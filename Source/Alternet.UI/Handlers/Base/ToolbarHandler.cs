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

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item text is visible.
        /// </summary>
        public abstract bool ItemTextVisible { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item images are visible.
        /// </summary>
        public abstract bool ItemImagesVisible { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar has horizontal gray line which divides it from other controls.
        /// </summary>
        public abstract bool NoDivider { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar 
        /// is positioned vertically.
        /// </summary>
        public abstract bool IsVertical { get; set; }

        public abstract void Realize();

        /// <summary>
        /// Gets or sets a value which specifies display modes for toolbar item image and text.
        /// </summary>
        public abstract ToolbarItemImageToTextDisplayMode ImageToTextDisplayMode { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}