using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific
    /// <see cref="Toolbar"/> behavior and appearance.
    /// </summary>
    internal abstract class ToolbarHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="Toolbar"/> this handler provides the implementation for.
        /// </summary>
        public new Toolbar Control => (Toolbar)base.Control;

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item
        /// text is visible.
        /// </summary>
        public abstract bool ItemTextVisible { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar item
        /// images are visible.
        /// </summary>
        public abstract bool ItemImagesVisible { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar has
        /// horizontal gray line which divides it from other controls.
        /// </summary>
        public abstract bool NoDivider { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether this toolbar
        /// is positioned vertically.
        /// </summary>
        public abstract bool IsVertical { get; set; }

        /// <inheritdoc cref="Toolbar.IsBottom"/>
        public abstract bool IsBottom { get; set; }

        /// <inheritdoc cref="Toolbar.IsRight"/>
        public abstract bool IsRight { get; set; }

        /// <summary>
        /// Gets or sets a value which specifies display modes for toolbar item
        /// image and text.
        /// </summary>
        public abstract ImageToText ImageToText { get; set; }

        /*/// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;*/

        /// <inheritdoc cref="Toolbar.Realize"/>
        public abstract void Realize();
    }
}