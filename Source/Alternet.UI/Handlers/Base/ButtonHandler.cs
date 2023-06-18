using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific <see cref="Button"/> behavior and appearance.
    /// </summary>
    public abstract class ButtonHandler : ControlHandler
    {
        /// <inheritdoc/>
        public new Button Control => (Button)base.Control;

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/> is the default button. In a modal dialog,
        /// a user invokes the default button by pressing the ENTER key.
        /// </summary>
        public abstract bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether a <see cref="Button"/> is a Cancel button. In a modal dialog, a
        /// user can activate the Cancel button by pressing the ESC key.
        /// </summary>
        public abstract bool IsCancel { get; set; }

        /// <summary>
        /// Specifies a set of images for different <see cref="Button"/> states.
        /// </summary>
        public abstract ControlStateImages StateImages { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;
    }
}