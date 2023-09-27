using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific
    /// <see cref="Button"/> behavior and appearance.
    /// </summary>
    public abstract class ButtonHandler : ControlHandler
    {
        /// <summary>
        /// Gets a <see cref="Button"/> this handler provides the implementation for.
        /// </summary>
        public new Button Control => (Button)base.Control;

        /// <summary>
        /// Gets or sets a value that indicates whether a
        /// <see cref="Button"/> is the default button. In a modal dialog,
        /// a user invokes the default button by pressing the ENTER key.
        /// </summary>
        public abstract bool IsDefault { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// input or not (i.e. behaves like a static text) and so doesn't need focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        public abstract bool AcceptsFocus { get; set; }

        /// <inheritdoc cref="Button.ExactFit"/>
        public abstract bool ExactFit { get; set; }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// focus from keyboard or not.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        /// <returns>
        /// Return false to indicate that while this control can,
        /// in principle, have focus if the user clicks
        /// it with the mouse, it shouldn't be included
        /// in the TAB traversal chain when using the keyboard.
        /// </returns>
        public abstract bool AcceptsFocusFromKeyboard { get; set; }

        /// <summary>
        /// Indicates whether this control or one of its children accepts focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        public abstract bool AcceptsFocusRecursively { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public abstract bool HasBorder { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether a
        /// <see cref="Button"/> is a Cancel button. In a modal dialog, a
        /// user can activate the Cancel button by pressing the ESC key.
        /// </summary>
        public abstract bool IsCancel { get; set; }

        /// <inheritdoc cref="Button.TextVisible"/>
        public abstract bool TextVisible { get; set; }

        /// <inheritdoc cref="Button.TextAlign"/>
        public abstract GenericDirection TextAlign { get; set; }

        /// <summary>
        /// Specifies a set of images for different <see cref="Button"/> states.
        /// </summary>
        public abstract ControlStateImages StateImages { get; set; }

        /// <inheritdoc/>
        protected override bool VisualChildNeedsNativeControl => true;

        /// <inheritdoc cref="Button.SetImagePosition"/>
        public abstract void SetImagePosition(GenericDirection dir);

        /// <inheritdoc cref="Button.SetImageMargins"/>
        public abstract void SetImageMargins(double x, double y);
    }
}