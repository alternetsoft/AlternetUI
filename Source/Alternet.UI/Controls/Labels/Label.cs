using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a text label control.
    /// </summary>
    /// <remarks>
    /// <see cref="Label" /> controls are typically used to provide descriptive text
    /// for a control.
    /// For example, you can use a <see cref="Label" /> to add descriptive text for
    /// a <see cref="TextBox"/> control to inform the
    /// user about the type of data expected in the control.
    /// <see cref="Label" /> controls can also be used
    /// to add descriptive text to a <see cref="Window"/> to provide the user
    /// with helpful information.
    /// For example, you can add a <see cref="Label" /> to the top of a
    /// <see cref="Window"/> that provides instructions
    /// to the user on how to input data in the controls on the form.
    /// <see cref="Label" /> controls can be
    /// also used to display run time information on the status of an application.
    /// For example,
    /// you can add a <see cref="Label" /> control to a form to display the status
    /// of each file as a list of files is processed.
    /// </remarks>
    [DefaultProperty("Text")]
    [DefaultBindingProperty("Text")]
    [ControlCategory("Common")]
    public partial class Label : Control
    {
        private string text = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class with specified text.
        /// </summary>
        /// <param name="text">Text displayed on this label.</param>
        public Label(string text)
            : this()
        {
            Text = text ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
        {
            if (Application.IsWindowsOS)
                UserPaint = true;
        }

        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text == value)
                    return;
                value ??= string.Empty;
                text = value;
                OnTextChanged(EventArgs.Empty);
            }
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Label;

        /// <summary>
        /// Wraps <see cref="Text"/> so that each of its lines becomes at most width
        /// pixels wide if possible (the lines are broken at words boundaries so it
        /// might not be the case if words are too long).
        /// </summary>
        /// <param name="value">Width in pixels.</param>
        /// <remarks>
        /// If width is negative, no wrapping is done. Note that this width is not
        /// necessarily the total width of the control, since a few pixels for the
        /// border (depending on the controls border style) may be added.
        /// </remarks>
        public void Wrap(double? value = null)
        {
            double v;
            if (value == null)
                v = Bounds.Width - Padding.Horizontal;
            else
                v = (int)value;

            Native.Label label = (Native.Label)(Handler.NativeControl!);
            label?.Wrap((int)v);
        }

        /// <inheritdoc/>
        internal override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateLabelHandler(this);
        }
    }
}