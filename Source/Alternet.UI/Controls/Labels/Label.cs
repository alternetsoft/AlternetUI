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
    public class Label : Control
    {
        /// <summary>
        /// Routed event declaration for <see cref="TextChanged"/> event.
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent(
                "TextChanged",
                RoutingStrategy.Bubble,
                typeof(TextChangedEventHandler),
                typeof(Label));

        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(Label),
                new FrameworkPropertyMetadata(
                        string.Empty,
                        PropMetadataOption.AffectsLayout | PropMetadataOption.AffectsPaint,
                        new PropertyChangedCallback(OnTextPropertyChanged),
                        new CoerceValueCallback(CoerceText)));

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event TextChangedEventHandler TextChanged
        {
            add
            {
                AddHandler(TextChangedEvent, value);
            }

            remove
            {
                RemoveHandler(TextChangedEvent, value);
            }
        }

        /// <summary>
        /// Gets or sets the text displayed on this label.
        /// </summary>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.Label;

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls
        /// <see cref="OnTextChanged(TextChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public void RaiseTextChanged(TextChangedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
        }

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

        /// <summary>
        /// Called when content in this Control changes.
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateLabelHandler(this);
        }

        private static object CoerceText(DependencyObject d, object value)
            => value ?? string.Empty;

        /// <summary>
        /// Callback for changes to the Text property
        /// </summary>
        private static void OnTextPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Label label = (Label)d;
            label.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void OnTextPropertyChanged(string oldText, string newText)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        }
    }
}