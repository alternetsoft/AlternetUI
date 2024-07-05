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
        private Coord? maxTextWidth;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class with specified text.
        /// </summary>
        /// <param name="text">Text displayed on this label.</param>
        public Label(string? text)
            : this()
        {
            Text = text ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label()
        {
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.Label;

        /// <summary>
        /// Gets or sets maximal width of text in the control.
        /// </summary>
        /// <remarks>
        /// Wraps <see cref="Control.Text"/> so that each of its lines becomes at most width
        /// dips wide if possible (the lines are broken at words boundaries so it
        /// might not be the case if words are too long).
        /// </remarks>
        /// <remarks>
        /// If width is negative or null, no wrapping is done. Note that this width is not
        /// necessarily the total width of the control, since a padding for the
        /// border (depending on the controls border style) may be added.
        /// </remarks>
        public double? MaxTextWidth
        {
            get
            {
                return maxTextWidth;
            }

            set
            {
                if (value <= 0)
                    value = null;
                if (maxTextWidth == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    maxTextWidth = value;
                    StateFlags |= ControlFlags.ForceTextChange;
                    Text = Text;
                });
            }
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
        }

        /// <summary>
        /// Wraps <see cref="Control.Text"/> so that each of its lines becomes at most width
        /// dips wide if possible (the lines are broken at words boundaries so it
        /// might not be the case if words are too long).
        /// </summary>
        /// <param name="value">Width in dips.</param>
        /// <remarks>
        /// If width is negative or null, no wrapping is done. Note that this width is not
        /// necessarily the total width of the control, since a padding for the
        /// border (depending on the controls border style) may be added.
        /// </remarks>
        public virtual void Wrap(Coord? value = null)
        {
            MaxTextWidth = value;
        }

        /// <inheritdoc/>
        protected override string CoerceTextForHandler(string s)
        {
            if(maxTextWidth is null)
                return s;
            var result = DrawingUtils.WrapTextToMultipleLines(
                s,
                maxTextWidth.Value,
                Font ?? Control.DefaultFont,
                ScaleFactor);
            return result;
        }

        /// <inheritdoc/>
        protected override void OnHandlerTextChanged()
        {
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateLabelHandler(this);
        }
    }
}